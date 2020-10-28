'use strict';
const isDebugging = true;

var hubUrl = document.location.pathname + 'ConnectionHub';
var peerConnectionConfig = { "iceServers": [{ "urls": "stun:stun.l.google.com:19302" }] };
// Create connection to Hub
var wsconn = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();
// initial valuable !
const screenConstraints = {
    video: {
        width: 1080,
        height: 720
    },
}
const cameraConstraints = {
    video: {
        width: 480,
        height: 360
    }
}
const audioConstraints = {
    audio: true
}
var screenStream = new MediaStream();
var cameraStream = new MediaStream();
var remoteAudio = new MediaStream();
let localcamera, localscreen, localaudio;
let isCaller = false;
var connections = {};
var localDataChannels = {};
var remoteDataChannels = {};

// khởi chạy hub với hàm start(), khi client kết nối sẽ gọi tới hàm Join Trong connectionHub.cs vs tham sô là username 
// và classid 
const initialize = async () => {
    console.log("Start connection for hub!")
    wsconn.start()
        .then(async () => {
            await wsconn.invoke("Join", username, classid)
                .catch((err) => {
                    console.log("Join Fail | " + err);
                });
        })
        .catch(err => console.log(err));
};
// khi page load xong, gọi tới hàm initialize để khởi chạy Hub
document.addEventListener("DOMContentLoaded", async () => {
    console.log("Ready for Hub");
    await initialize();
});
// hàm này gọi khi luồng media từ Màn hình đc lấy ra và đính vào thẻ video để hiển thị nội dung chia sẻ
const callbackDisplayMediaSuccess = async (stream) => {
    console.log("WebRTC: got screen media stream");
    var screen = document.querySelector("#screen");
    screenStream = stream;
    screen.srcObject = screenStream;
    localscreen = new MediaStream(stream.getTracks());
}
// tương tự với luồng từ camera 
const callbackUserMediaSuccess = async (stream) => {
    console.log("WebRTC: got camera media stream");
    var camera = document.querySelector("#camera");
    cameraStream = stream;
    camera.srcObject = cameraStream;
    localcamera = new MediaStream(stream.getTracks());
}
//tuong tu vơi audio
const callbackAudioMediaSuccess = async (stream) => {
    console.log("WebRTC: got audio media stream");
    var camera = document.querySelector("#camera");
    localaudio = new MediaStream(stream.getTracks());
    var tracks = stream.getTracks();
    for (var i = 0; i < tracks.length ; i++)
    {
        console.log("cameraStream add Track : " + JSON.stringify(tracks[i]));
        cameraStream.addTrack(tracks[i]);
    }
}
// hàm này thực hiện khởi tạo các luồn video,screen, audio.
const initializeDevices = async (UserCall) => {
    console.log(JSON.stringify(UserCall));
    console.log('WebRTC: InitializeUserMedia: ');
    if (isCaller) { // nếu là giáo viên sẽ chia sẻ cả camera và màn hình
        await navigator.mediaDevices.getDisplayMedia(screenConstraints) //screen
            .then((stream) => callbackDisplayMediaSuccess(stream))
            .catch(err => console.log(err));
        await navigator.mediaDevices.getUserMedia(cameraConstraints)    //camera
            .then((stream) => callbackUserMediaSuccess(stream))
            .catch(err => console.log(err));
        await navigator.mediaDevices.getUserMedia(audioConstraints)
            .then(stream => callbackAudioMediaSuccess(stream))
            .catch(err => console.log(err));
    }
    else { // học sinh
        localcamera = new MediaStream();
        localscreen = new MediaStream();
        await navigator.mediaDevices.getUserMedia(audioConstraints)     //audio
            .then(stream => callbackAudioMediaSuccess(stream))
            .catch(err => console.log(err));
    }
}

// cấu hình ICE candidate (cái này mô tả các giao thức và định tuyến cần thiết lập cho webrtc để kế nối
// ngang hành) cho kết nối
const receivedCandidateSignal = async (connection, partnerClientId, candidate) => {
    console.log('WebRTC: adding full candidate');
    connection.addIceCandidate(new RTCIceCandidate(candidate), () => console.log("WebRTC: added candidate successfully"), () => console.log("WebRTC: cannot add candidate"));
}

//Process a newly received SDP signal - thực hiện khi nhận một tín hiệu session description protocol mới
// có các hàm setRemoteDescription , createAnswer 
const receivedSdpSignal = async (connection, partnerClientId, sdp) => {
    console.log('connection: ', connection);
    console.log('sdp', sdp);
    console.log('WebRTC: called receivedSdpSignal');
    console.log('WebRTC: processing sdp signal');
    connection.setRemoteDescription(new RTCSessionDescription(sdp), async () => {
        console.log('WebRTC: set Remote Description');
        if (connection.remoteDescription.type == "offer") {
            console.log('WebRTC: remote Description type offer');
            connection.addTrack(localaudio.getTracks()[0]);
            connection.createAnswer().then((desc) => {
                console.log('WebRTC: create Answer...');
                connection.setLocalDescription(desc, async () => {
                    console.log('WebRTC: set Local Description...');
                    console.log('connection.localDescription: ', connection.localDescription);
                    //setTimeout(() => {
                    await sendHubSignal(JSON.stringify({ "sdp": connection.localDescription }), partnerClientId);
                    //}, 1000);
                }, errorHandler);
            }, errorHandler);
        } else if (connection.remoteDescription.type == "answer") {
            console.log('WebRTC: remote Description type answer');
        }
    }, errorHandler);
}

// chuyển tín hiệu mới để cài đặt kết nối 
const newSignal = async (partnerClientId, data) => {
    console.log('WebRTC: called newSignal');
    var signal = JSON.parse(data);
    var connection = getConnection(partnerClientId);
    console.log("connection: ", connection);

    // Route signal based on type
    if (signal.sdp) { // nếu là sdp 
        console.log('WebRTC: sdp signal');
        await receivedSdpSignal(connection, partnerClientId, signal.sdp);
    } else if (signal.candidate) { // neu la candidate
        console.log('WebRTC: candidate signal');
        await receivedCandidateSignal(connection, partnerClientId, signal.candidate);
    } else { // con k 
        console.log('WebRTC: adding null candidate');
        await connection.addIceCandidate(null, async () => console.log("WebRTC: added null candidate successfully"), () => console.log("WebRTC: cannot add null candidate"));
    }
}

// ngat ket noi ngang hang với client có partnerClientId
const closeConnection = async (partnerClientId) => {
    console.log("WebRTC: called closeConnection ");
    var connection = connections[partnerClientId];
    if (connection) {
        connection.close();
        delete connections[partnerClientId]; // Remove the property
    }
}
// Close all of our connections
const closeAllConnections = async () => {
    console.log("WebRTC: call closeAllConnections ");
    for (var connectionID in connections) {
        await closeConnection(connectionID);
    }
}
// lấy ra connecction kết nối với partnerClientId
const getConnection = (partnerClientId) => {
    console.log("WebRTC: called getConnection");
    if (connections[partnerClientId]) {
        console.log("WebRTC: connections partner client exist");
        return connections[partnerClientId];
    }
    else {
        console.log("WebRTC: initialize new connection");
        return initializeConnection(partnerClientId) // tạo mới connection 
    }
}
// gui tin hieu di thống qua hub đến partnerClientId 
const sendHubSignal = async (candidate, partnerClientId) => {
    console.log('candidate', candidate);
    console.log('SignalR: called sendhubsignal ');
    await wsconn.invoke('sendSignal', candidate, partnerClientId).catch(errorHandler);
};

// khi có một track được attach vào kế nối, bên kia kết nối sẽ gọi hàm này
// hàm này thực hiện thêm cách track vào các thẻ để hiển thị video , screeen, hay audio 
const callbackAddTrack = async (connection, e) => {
    if (e.streams && e.streams[0]) {
        console.log('add Track 1');
        console.log(e.streams);
    } else {
        console.log("add Track 2");
        if (e.track.kind == "audio") {
            console.log("add track : " + e.track);
            remoteAudio.addTrack(e.track);
            cameraStream.addTrack(e.track);
            document.querySelector("#camera").srcObject = cameraStream;
        }
        else {
            console.log("add track video")
            if (localcamera.getVideoTracks().length == 0) {
                var camera = document.querySelector("#camera");
                cameraStream.addTrack(e.track);
                localcamera.addTrack(e.track);
                document.querySelector("#camera").srcObject = cameraStream;
            }
            else {
                localscreen.addTrack(e.track);
                var screen = document.querySelector("#screen");
                screenStream.addTrack(e.track);
                localscreen.addTrack(e.track);
                document.querySelector("#screen").srcObject = screenStream;
            }
        }
    }
}
// khi có ICE candidate được thêm vào kết nối sẽ gọi hàm này 
const callbackIceCandidate = (evt, connection, partnerClientId) => {
    console.log("WebRTC: Ice Candidate callback");
    //console.log("evt.candidate: ", evt.candidate);
    if (evt.candidate) {// Found a new candidate
        console.log('WebRTC: new ICE candidate');
        //console.log("evt.candidate: ", evt.candidate);
        sendHubSignal(JSON.stringify({ "candidate": evt.candidate }), partnerClientId);
    } else {
        // Null candidate means we are done collecting candidates.
        console.log('WebRTC: ICE candidate gathering complete');
        sendHubSignal(JSON.stringify({ "candidate": null }), partnerClientId);
    }
}
// khoi tao ket noi
const initializeConnection = (partnerClientId) => {
    console.log('WebRTC: Initializing connection...');
    var connection = new RTCPeerConnection(peerConnectionConfig);
    // tạo Kênh dữ liệu để chat 
    var LocaldataChannel = connection.createDataChannel("chat-publish");
    LocaldataChannel.binaryType = "arraybuffer";
    LocaldataChannel.addEventListener('open', event => {
        console.log("open datachanel");
    });
    LocaldataChannel.addEventListener('close', event => {
        console.log("close datachanel");
    });
    connection.addEventListener('datachannel', (event) => {
        var remotedataChannel = event.channel;
        remotedataChannel.binaryType = "arraybuffer";
        // sự kiện khi bên kia nhận được tin nhắn 
        remotedataChannel.onmessage = (e) => {
            var data = JSON.parse(e.data);
            console.log(data.message.length);
            if (data.type == "particular") { // nếu là nhắn tin riêng
                receiveMessage(data.message, document.querySelector("#chat-particular").querySelector("#chatconversation"))
            }
            else if (data.type == "public") { // nếu là nhắn tin cho tất cả mọi người trong lớp học 
                receiveMessage(data.message, document.querySelector("#chat-public").querySelector("#chatconversation"));
            }
            else if (data.type == "group") { // nhắn tin trong nhóm 
                receiveMessage(data.message, document.querySelector("#chat-group").querySelector("#chatconversation"));
            }
        }
        remoteDataChannels[partnerClientId] = remotedataChannel;
    });
    localDataChannels[partnerClientId] = LocaldataChannel;
    //
    connection.onicecandidate = evt => callbackIceCandidate(evt, connection, partnerClientId); // hàm callback cài đặt cho sư kiên onicecandidate
    connection.ontrack = async e => { await callbackAddTrack(connection, e) }; // hàm callback cho sư kiện khi có track đươcj attach vào kết nối
    //connection.onremovestream = evt => callbackRemoveStream(connection, evt); // Remove stream handler callback
    connections[partnerClientId] = connection; // Store away the connection based on username
    return connection;
}
// khởi tạo offer cho kết nối 
const initiateOffer = async (partnerClientId) => {
    console.log('WebRTC: called initiateoffer: ');
    var connection = getConnection(partnerClientId); // // get a connection for the given partner
    //console.log('initiate Offer stream: ', stream);
    console.log("offer connection: ", connection);
    console.log("WebRTC: Added local stream");
    // nếu là người gọi thì thêm các track video, screen, audio vào kết nối để truyền đi 
    if (isCaller) {
        for (var i = 0; i < localcamera.getTracks().length; i++) {
            console.log(localcamera.getTracks()[i]);
            connection.addTrack(localcamera.getTracks()[i]);
        }
        for (var i = 0; i < localscreen.getTracks().length; i++) {
            console.log(localscreen.getTracks()[i]);
            connection.addTrack(localscreen.getTracks()[i]);
        }
        for (var i = 0; i < localaudio.getTracks().length; i++) {
            console.log(localaudio.getTracks()[i]);
            connection.addTrack(localaudio.getTracks()[i]);
        }
    }
    // nếu là học sinh thì thêm track audio ( mic nói ) vào kết nối để truyền đi
    else {
        for (var i = 0; i < localaudio.getTracks().length; i++) {
            console.log(localaudio.getTracks()[i]);
            connection.addTrack(localaudio.getTracks()[i]);
        }
    }
    // tạo offer cho kết nối 
    connection.createOffer().then(offer => {
        console.log('WebRTC: created Offer: ');
        console.log('WebRTC: Description after offer: ', offer);
        connection.setLocalDescription(offer).then( async () => {
            console.log('WebRTC: set Local Description: ');
            console.log('connection before sending offer ', connection);
            setTimeout(async () => {
                await sendHubSignal(JSON.stringify({ "sdp": connection.localDescription }), partnerClientId);
            }, 1000);
        }).catch(err => console.error('WebRTC: Error while setting local description', err));
    }).catch(err => console.error('WebRTC: Error while creating offer', err));
}
// khi hub yêu càu khời tạo devices cho client 
wsconn.on("initDevices", async (UserCall) => {
    isCaller = UserCall.isCaller; // nếu là giáo viên iscaller = true , nếu là học sinh iscaller = false
    await initializeDevices(UserCall);
});
//đếm giáo viên - k cần thiêt :) 
const countTeacher = (userList) => {
    var result = 0;
    userList.forEach(u => {
        if (u.isCaller) result++;
    })
    return result;
}
// đếm học sinh 
const countStudent = (userList) => {
    var result = 0;
    userList.forEach(u => {
        if (!u.isCaller) result++;
    })
    return result;
}
// update lại danh sách thành viên trong lớp 
wsconn.on('updateUserList', (userList) => {
    console.log("update list users " + JSON.stringify(userList));
    var listUserTag = document.querySelector("#chat-userlist");
    var strTmp1 = "";
    var strTmp2 = "";
    var strTmp3 = "";
    userList.forEach(user => {
        if (user.isCaller) {
            strTmp1 += "<li class=\"contact-list-item\">\
                            <div class=\"group-icon-device\">\
                                <div class=\"box-icon-device\">\
                                    <div class=\"icon-network connect-good\"></div>\
                                    <div class=\"icon-user icon-teacher\">\
                                            <svg class=\"icon icon-px_ic_teacher\">\
                                            </svg>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class=\"contact-list-item-name\">\
                                <span data-title=\"teacher\" class=\"user-role\">\
                                <span class=\"role-device\">teacher</span><br></span>\
                                <span class=\"user-name\" title=\""+ user.userName +"\">" + user.userName + "</span>\
                            </div>\
                        </li>";

            strTmp2 += "<a id=\"part-chat\" class=\"nav-link\" data-toggle=\"tab\" data-cid=\"" + user.connectionID + "\" href=\"#chat-particular\" onclick=addEvent(\"" + user.connectionID + "\"); >\
            <li class=\"contact-list-item\">\
                <div class=\"group-icon-device\">\
                    <div class=\"box-icon-device\">\
                        <div class=\"icon-user icon-teacher\">\
                            <svg class=\"icon icon-px_ic_teacher\"><use xlink:href=\"#icon-px_ic_teacher\"></use></svg>\
                        </div>\
                    </div>\
                 </div>\
            <div class=\"contact-list-item-name\">\
                <span title=\"TEACHER\" class=\"user-role\">\
                    <span class=\"role-device\">TEACHER</span><br></span>\
                    <span class=\"user-name\" title=\""+ user.username + "\">" + user.userName + "</span >\
            </div>\
            <div class=\"box-number-noti\"></div></li></a>";
        }
        else {
            strTmp3 += "<li class=\"contact-list-item\">\
                            <div class=\"group-icon-device\">\
                                <div class=\"box-icon-device\">\
                                    <div class=\"icon-network connect-good\"></div>\
                                    <div class=\"icon-user icon-teacher\">\
                                            <svg class=\"icon icon-px_ic__Device__Website\">\
                                                <use xlink:href=\"#icon-px_ic__Device__Website\"></use>\
                                            </svg>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class=\"contact-list-item-name\">\
                                <span data-title=\"student\" class=\"user-role\">\
                                <span class=\"role-device\">student</span><br>\
                                </span><span class=\"user-name\" title=\""+ user.userName + "\"> "+ user.userName+ "</span>\
                            </div>\
                        </li>";
            strTmp2 += "<a class=\"nav-link\" data-toggle=\"tab\" datacid=\"" + user.connectionID + "\" href=\"#chat-particular\" onclick=addEvent(\"" + user.connectionID  + "\"); >\
            <li class=\"contact-list-item\">\
                <div class=\"group-icon-device\">\
                    <div class=\"box-icon-device\">\
                        <div class=\"icon-user icon-teacher\">\
                            <svg class=\"icon icon-px_ic_teacher\"><use xlink:href=\"#icon-px_ic_teacher\"></use></svg>\
                        </div>\
                    </div>\
                 </div>\
            <div class=\"contact-list-item-name\">\
                <span title=\"STUDENT\" class=\"user-role\">\
                    <span class=\"role-device\">STUDENT</span><br></span>\
                    <span class=\"user-name\" title=\""+ user.username + "\">" + user.userName + "</span >\
            </div>\
            <div class=\"box-number-noti\"></div></li></a>";
        }
    });
    document.querySelector("#student-part").querySelector("span.user-count").innerHTML = countStudent(userList);
    document.querySelector("#teacher-part").querySelector("#contacts").innerHTML = strTmp1;
    document.querySelector("#student-part").querySelector("#contacts").innerHTML = strTmp3;
    listUserTag.innerHTML = strTmp2;
});
// khi có một người mới vào phòng 
wsconn.on("NotifyNewMember",async (newMember) => {
    console.log("New member !");
    await wsconn.invoke("CallUser",newMember) // thực thi các cuộc goi kết nối tới người mới 
        .catch(err => console.log(err));
});
// cuoc goi toi từ phía callingUser
wsconn.on('incomingCall', async (callingUser) => {
    console.log('SignalR: incoming call from: ' + JSON.stringify(callingUser));
    await wsconn.invoke('AnswerCall', true, callingUser).catch(err => console.log(err));
});
// cuộc gọi được chấp nhận 
wsconn.on('callAccepted',async (acceptingUser) => {
    console.log('SignalR: call accepted from: ' + JSON.stringify(acceptingUser) + '.  Initiating WebRTC call and offering my stream up...');
    await initiateOffer(acceptingUser.connectionID);
});
// nhận được tín hiệu gửi từ signalingUser
wsconn.on('receiveSignal', async (signalingUser, signal) => {
    console.log('WebRTC: receive signal ');
    await newSignal(signalingUser.connectionID, signal);
});
// cuộc gọi bị từ chốii 
wsconn.on('callDeclined', async (decliningUser, reason) => {
    console.log('SignalR: call declined from: ' + decliningUser.connectionID);
    console.log(reason);
});

// Hub Callback: Call Ended
wsconn.on('callEnded', async (signalingUser, signal) => {
    console.log('SignalR: call with ' + signalingUser.connectionID + ' has ended: ' + signal);

    // Let the user know why the server says the call is over
    console.log(signal);

    // Close the WebRTC connection
    await closeConnection(signalingUser.connectionID);
});
wsconn.onclose(e => {
    if (e) {
        console.log("SignalR: closed with error.");
        console.log(e);
    }
    else {
        console.log("Disconnected");
    }
});
const errorHandler = (error) => {
    if (error.message)
        console.log('Error Occurred - Error Info: ' + JSON.stringify(error.message));
    else
        console.log('Error Occurred - Error Info: ' + JSON.stringify(error));

    consoleLogger(error);
};

const consoleLogger = (val) => {
    if (isDebugging) {
        console.log(val);
    }
};

// 3 sự kiện tắt bật mic, cam, screen
document.querySelector("#mic").addEventListener("click", () => {
    console.log("turn on/off mic");
    var cameraStream = document.querySelector("#camera").srcObject;
    var AudioTrack = cameraStream.getAudioTracks()[0];
    AudioTrack.enabled = !AudioTrack.enabled;
});

document.querySelector("#cam").addEventListener("click", () => {
    console.log("turn on/off cam");
    var cameraStream = document.querySelector("#camera").srcObject;
    var cameraTrack = cameraStream.getVideoTracks()[0];
    cameraTrack.enabled = !cameraTrack.enabled;
});
async function getConnectedDevices(type) {
    const devices = await navigator.mediaDevices.enumerateDevices();
    return devices.filter(device => device.kind === type)
}


// *********************** Implement with chat ********************************** //
// thêm nôi dung tin nhắn 
const addnewMessageForMe = (message, elementTag) => {
    elementTag.innerHTML += "<div class=\"box-content-chat\">\
        <div class=\"chatmessage\">\
            <div class=\"username localuser\"></div>\
            <div class=\"timestamp\">14:54:44</div>\
            <div class=\"usermessage\"><p class=\"userMessageContent\" >" + message +"</p></div>\
        </div></div>";
}
const receiveMessage = (message, elementTag) => {
    elementTag.innerHTML += "\
        <div class=\"box-content-chat\">\
            <div class=\"chatmessage chatmessageReceived\">\
                <div class=\"username remoteuser\"></div>\
                <div class=\"timestamp\">19:10:01</div>\
                <div class=\"usermessage\"><p class=\"userMessageContentReceived\">" + message +"</p></div>\
            </div>\
        </div>";
}


document.querySelector("div#chat-public").querySelector("textarea.text-area").addEventListener('keypress', (e) => {
    if (e.key == "Enter") {
        var messageTextArea = document.querySelector("div#chat-public").querySelector("textarea.text-area");
        const message = messageTextArea.value;
        for (const [cntionID, localdtChannel] of Object.entries(localDataChannels)) {
            localdtChannel.send(JSON.stringify({ "message": message, "type": "public" }));
        };
        messageTextArea.value = "";
        addnewMessageForMe(message, document.querySelector("div#chatconversation"));
    }
});
const getLocalDataChannel = (connectionID) => {
    return localDataChannels[connectionID] != null ? localDataChannels[connectionID] : null;
}
const getRemoteDataChannel = (connectionID) => {
    return remoteDataChannels[connectionID] != null ? remoteDataChannels[connectionID] : null;
}
const addEvent = (connectionID) => {
    var localDtChannel = getLocalDataChannel(connectionID);
    var remoteDtChannel = getRemoteDataChannel(connectionID);
    var messageBox = document.querySelector("#chat-particular").querySelector("textarea.text-area");
    messageBox.addEventListener("keypress", function sendMessage(e){
        if (e.key == "Enter") {
            var messageBox = document.querySelector("#chat-particular").querySelector("textarea.text-area").value;
            localDtChannel.send(JSON.stringify({ "message": messageBox.value, "type": "particular" }));

            addnewMessageForMe(messageBox.value, document.querySelector("#chat-particular").querySelector("#chatconversation"));
            messageBox.value = "";
        }
    });
}