﻿'use strict';

const isDebugging = true;

var hubUrl = document.location.pathname + 'ConnectionHub';
// Thiết lập tham số RTCConfiguration cho hàm RTCPeerConnection 
// đối tượng này xác định cách thiết lập kết nối ngang hàng và nên chứa thông tin về các máy chủ ICE sử dụng
// iceServers: Information about ICE servers - Use your own!
var peerConnectionConfig = { "iceServers": [{ "urls": "stun:stun.l.google.com:19302" }] };
// Create connection to Hub
var wsconn = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();
//Tạo đối tượng video track -  cho phép show màn hình chính
const screenConstraints = {
    video: {
        width: 1280,
        height:720,
        displaySurface: "monitor"
    }
}
// tạo đối tượng video track - camera of caller
const cameraConstraints = {
    video: {
        width: 480,
        height: 360
    }
}
// we want an audio track 
const audioConstraints = {
    audio: true
}
/** A stream of media content. A stream consists of several tracks such as video or audio tracks. Each track is specified as an instance of MediaStreamTrack. */
var screenStream = new MediaStream();

//  adds a new media track to the set of tracks which will be transmitted to the other peer.
screenStream.onaddtrack = async e => { await callbackOnaddtrackScreen(e);}

var cameraStream = new MediaStream();
var remoteAudio = new MediaStream();
let localcamera, localscreen, localaudio;
let isCaller = false;
var connections = {};
var localDataChannels = {};
var remoteDataChannels = {};
var isChatPublic = true;
var isChatPrivate = false;
let PartnerChatConnectionID; 
let myConnectionID;
var fileInput = document.querySelector("input#fileInput");
var sendFileButton = document.querySelector("button#sendFile");
// khởi chạy hub với hàm start(), khi client kết nối sẽ gọi tới hàm Join Trong connectionHub.cs vs tham sô là username 
// và classid 
const Join = () => {
    console.log("Start connection for hub!")
    wsconn.start() // when this succeeds, fulfilling the returned promise
        .then(() => {
            wsconn.invoke("Join", username, classid, isCaller)
                .catch((err) => {
                    console.log("Join Fail | " + err);
                });
            wsconn.invoke("getConnectionID")
                .catch(err => console.log(err));
        })
        .catch(err => console.log(err));
};
// khi page load xong, gọi tới hàm initialize để khởi chạy Hub
document.addEventListener("DOMContentLoaded", () => {
    if (isteacher == "True") {
        isCaller = true;
    }
    initializeDevices();
});
// hàm này gọi khi luồng media từ Màn hình đc lấy ra và đính vào thẻ video để hiển thị nội dung chia sẻ
const callbackDisplayMediaSuccess = (stream) => {
    
    console.log("WebRTC: got screen media stream");
    var screen = document.querySelector("#screen");
    screenStream = stream;
    screen.srcObject = screenStream;
    localscreen = new MediaStream(stream.getTracks());
}

//=============== Add Track =========================================
const callbackOnaddtrackCamera = async (e) => {
    console.log("call back add camera")
    var camera = document.querySelector("#camera");
    // we attach the incoming stream to the local preview <video> element by 
    // setting the element's srcObject property. Since the element is configured
    // to automatically play incoming video, the stream begins playing in our
    // local preview box.
    camera.srcObject = cameraStream;
}
const callbackOnaddtrackScreen = async (e) => {
    console.log("call back add screen");
    var screen = document.querySelector("#screen");
    screen.srcObject = cameraStream;
}

// tương tự với luồng từ camera 

const callbackUserMediaSuccess = (stream) => {
    console.log("WebRTC: got camera media stream");
    var camera = document.querySelector("#camera");
    cameraStream = stream;
    camera.srcObject = cameraStream;
    localcamera = new MediaStream(stream.getTracks());
}
//tuong tu vơi audio
const callbackAudioMediaSuccess = (stream) => {
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
const initializeDevices = () => {
    console.log('WebRTC: InitializeUserMedia: ');
    if (isCaller) { // nếu là giáo viên sẽ chia sẻ cả camera và màn hình
        navigator.mediaDevices.getDisplayMedia({ video: true }) //screen
            .then((stream) => callbackDisplayMediaSuccess(stream))
            .catch(err => console.log(err));
        navigator.mediaDevices.getUserMedia(cameraConstraints)    //camera
            .then((stream) => callbackUserMediaSuccess(stream))
            .catch(err => console.log(err));
        navigator.mediaDevices.getUserMedia(audioConstraints)
            .then(stream => {
                callbackAudioMediaSuccess(stream);
                Join();
            })
            .catch(err => console.log(err));
    }
    else { // học sinh
        localcamera = new MediaStream();
        localscreen = new MediaStream();
        navigator.mediaDevices.getUserMedia(audioConstraints)     //audio
            .then(stream => {
                callbackAudioMediaSuccess(stream);
                Join();
            })
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
    var LocaldataChannel = connection.createDataChannel("chat");
    LocaldataChannel.binaryType = "arraybuffer";
    LocaldataChannel.addEventListener('open', event => {
        console.log("open datachanel");
    });
    LocaldataChannel.addEventListener('close', event => {
        console.log("close datachanel");
    });
    connection.addEventListener('datachannel', (event) => {
        var remotedataChannel = event.channel;
        console.log(remotedataChannel);
        // sự kiện khi bên kia nhận được tin nhắn 
        if (remotedataChannel.label == "chat") {
            remotedataChannel.binaryType = "arraybuffer";
            remotedataChannel.onmessage = (e) => {
                var data = JSON.parse(e.data);
                console.log(data);
                console.log(data.connectionID);
                if (data.type == "particular") { // nếu là nhắn tin riêng
                    if (!document.querySelector("#P" + data.connectionID)) {
                        makeNewBoxChatPrivate(data.connectionID);
                    }
                    receiveMessage(data.message, document.querySelector("#P" + data.connectionID).querySelector("#chatconversation"),data.user)
                }
                else if (data.type == "public") { // nếu là nhắn tin cho tất cả mọi người trong lớp học 
                    receiveMessage(data.message, document.querySelector("#chat-public").querySelector("#chatconversation"),data.user);
                }
                else if (data.type == "group") { // nhắn tin trong nhóm 
                    receiveMessage(data.message, document.querySelector("#chat-group").querySelector("#chatconversation"),data.user);
                }
            }
        }
        else if (remotedataChannel.label == "send-file" || remotedataChannel == "send-file-private") {
            console.log("here");
            let receiveSize = 0;
            let receiveBuffer = [];
            if (remotedataChannel.label == "send-file")
                receiveChannelCallBack(remotedataChannel, receiveSize, receiveBuffer, true);
            else {
                receiveChannelCallBack(remotedataChannel, receiveSize, receiveBuffer, false);
            }

        }
        remotedataChannel.onclose = (e) => {
            console.log(`Closed data channel with label: ${receiveChannel.label}`);
        }
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
wsconn.on("getConnectionID", (callingUser) => {
    console.log("My connection ID : " + callingUser.connectionID);
    myConnectionID = callingUser.connectionID;
});
// update lại danh sách thành viên trong lớp 
wsconn.on('updateUserList', (UserCalls) => {
    console.log("update list users " + JSON.stringify(UserCalls));
    var listUserTag = document.querySelector("#chat-userlist");
    var strTmp1 = "";
    var strTmp2 = "";
    var strTmp3 = "";
    UserCalls.forEach(user => {
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
                                <span class=\"user-name\" title=\""+ user.fullName +"\">" + user.fullName + "</span>\
                            </div>\
                        </li>";
            if (user.fullName != username) {
                strTmp2 += "<a id=\"part-chat\" class=\"nav-link\" data-toggle=\"tab\" data-cid=\"#chat-particular\" href=\"#P" + user.connectionID + "\" onclick=addEvent(\"" + user.connectionID + "\"); >\
            <li class=\"contact-list-item\">\
                <div class=\"group-icon-device\">\
                    <div class=\"box-icon-device\">\
                        <div class=\"icon-user icon-teacher\">\
                            <svg class=\"icon icon-px_ic_teacher\"></svg>\
                        </div>\
                    </div>\
                 </div>\
            <div class=\"contact-list-item-name\">\
                <span title=\"TEACHER\" class=\"user-role\">\
                    <span class=\"role-device\">TEACHER</span><br></span>\
                    <span class=\"user-name\" title=\""+ user.fullName + "\">" + user.fullName + "</span >\
            </div>\
            <div class=\"box-number-noti\"></div></li></a>";
            }
        }
        else {
            strTmp3 += "<li class=\"contact-list-item\">\
                            <div class=\"group-icon-device\">\
                                <div class=\"box-icon-device\">\
                                    <div class=\"icon-network connect-good\"></div>\
                                    <div class=\"icon-user icon-teacher\">\
                                            <svg class=\"icon icon-px_ic__Device__Website\"></svg>\
                                    </div>\
                                </div>\
                            </div>\
                            <div class=\"contact-list-item-name\">\
                                <span data-title=\"student\" class=\"user-role\">\
                                <span class=\"role-device\">student</span><br>\
                                </span><span class=\"user-name\" title=\""+ user.fullName + "\"> " + user.fullName + "</span>\
                                  <i id=\"micStudent\" class=\"fa fa - microphone\"></i>\
                            </div>\
                        </li>";
            if (user.fullName != username) {
                strTmp2 += "<a class=\"nav-link\" data-toggle=\"tab\" datacid=\"" + user.connectionID + "\" href=\"#chat-particular\" onclick=addEvent(\"" + user.connectionID + "\"); >\
            <li class=\"contact-list-item\">\
                <div class=\"group-icon-device\">\
                    <div class=\"box-icon-device\">\
                        <div class=\"icon-user icon-teacher\">\
                            <svg class=\"icon icon-px_ic_teacher\"></svg>\
                        </div>\
                    </div>\
                 </div>\
            <div class=\"contact-list-item-name\">\
                <span title=\"STUDENT\" class=\"user-role\">\
                    <span class=\"role-device\">STUDENT</span><br></span>\
                    <span class=\"user-name\" title=\""+ user.fullName + "\">" + user.fullName + "</span >\
                    <i id=\"micStudent\" class=\"fa fa - microphone\"></i>\
            </div>\
            <div class=\"box-number-noti\"></div></li></a>";
            }
        }
    });
    document.querySelector("#student-part").querySelector("span.user-count").innerHTML = countStudent(UserCalls);
    document.querySelector("#contacts-teacher").innerHTML = strTmp1;
    document.querySelector("#contacts-student").innerHTML = strTmp3;
    listUserTag.innerHTML = strTmp2;
});
const makeNewBoxChatPrivate = (connectionID) => {
    var tmp = "<div id=\"P" + connectionID + "\"><div id=\"sideToolbarContainerChat\">\
            <div id=\"chat_container\" data-userid-private=\"0\" class=\"sideToolbarContainer__inner\">\
                <div id=\"chatconversation\">\
                </div></div></div></div>";
    document.querySelector("#chat-particular").innerHTML += tmp;
}
// khi có một người mới vào phòng 
wsconn.on("NotifyNewMember", (newMember) => {
    console.log("New member !");
    if (localaudio && localcamera) {
        wsconn.invoke("CallUser", newMember) // thực thi các cuộc goi kết nối tới người mới 
            .catch(err => console.log(err));
    }
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
    var localaudiotrack = localaudio.getAudioTracks()[0];
    localaudiotrack.enabled = !localaudiotrack.enabled;
});

document.querySelector("#cam").addEventListener("click", () => {
    console.log("turn on/off cam");
    var cameraStream = document.querySelector("#camera").srcObject;
    var cameraTrack = cameraStream.getVideoTracks()[0];
    cameraTrack.enabled = !cameraTrack.enabled;
});
document.querySelector("#speaker").addEventListener("click", () => {
    console.log("turn on/off speaker");
    var remotetracks = remoteAudio.getAudioTracks();
    remotetracks.forEach(track => {
        track.enabled = !track.enabled;
    });
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
            <div class=\"timestamp\">"+ Date(Date.now()).toString().split(" ")[4] +"</div>\
            <div class=\"usermessage\"><p class=\"userMessageContent\" >" + message +"</p></div>\
        </div></div>";
}
const receiveMessage = (message, elementTag,name) => {
    elementTag.innerHTML += "\
        <div class=\"box-content-chat\">\
            <div class=\"chatmessage chatmessageReceived\">\
                <div class=\"username remoteuser\">" + name  +"</div>\
                <div class=\"timestamp\">"+ Date(Date.now()).toString().split(" ")[4] + "</div>\
                <div class=\"usermessage\"><p class=\"userMessageContentReceived\">" + message +"</p></div>\
            </div>\
        </div>";
}




const getLocalDataChannel = (connectionID) => {
    return localDataChannels[connectionID] != null ? localDataChannels[connectionID] : null;
}
const getRemoteDataChannel = (connectionID) => {
    return remoteDataChannels[connectionID] != null ? remoteDataChannels[connectionID] : null;
}



const addEvent = (connectionID) => {
    PartnerChatConnectionID = connectionID;
    isChatPrivate = true;
    isChatPublic = false;
    document.querySelector("#a_sf").style.display = "block";
    document.querySelector("textarea.text-area").disabled = false;
    document.querySelector("#chat-group").style.display = "none";
    document.querySelector("#chat-particular").style.display = "block";
}
document.querySelector("li#public").addEventListener("click", e => {
    isChatPublic = true;
    isChatPrivate = false;
    document.querySelector("textarea.text-area").disabled = false;
    document.querySelector("#a_sf").style.display = "block";
});
document.querySelector("li#private").addEventListener("click", e => {
    isChatPublic = false;
    isChatPrivate = false;
    document.querySelector("#a_sf").style.display = "none";
    document.querySelector("textarea.text-area").disabled = true;
    document.querySelector("#chat-group").style.display = "block";
    document.querySelector("a#bk-icon").onclick = turnbackGroup;
});
const turnbackGroup = () => {
    isChatPublic = false;
    isChatPrivate = false;
    document.querySelector("textarea.text-area").disabled = true;
    document.querySelector("#chat-group").style.display = "block";
    document.querySelector("#chat-particular").style.display = "none";
};
document.querySelector("textarea.text-area").addEventListener('keypress', (e) => {
    if ("nav-item active" == document.querySelector("li#public").getAttribute("class")) {
        if (e.key == "Enter") {
            var messageTextArea = document.querySelector("textarea.text-area");
            const message = messageTextArea.value;
            if (message != "") {
                for (const [cntionID, localdtChannel] of Object.entries(localDataChannels)) {
                    localdtChannel.send(JSON.stringify({ "message": message, "type": "public", "user": username }));
                };
                addnewMessageForMe(message, document.querySelector("#chat-public").querySelector("#chatconversation"));
                messageTextArea.value = "";
                if (e.preventDefault) e.preventDefault();
                return false;
            }
        }
    }
    else if ("nav-item active" == document.querySelector("li#private").getAttribute("class") && isChatPrivate && !isChatPublic) {
        if (e.key == "Enter") {
            var localDtChannel = getLocalDataChannel(PartnerChatConnectionID);
            var messageTextArea = document.querySelector("textarea.text-area");
            const message = messageTextArea.value;
            if (message != "") {
                localDtChannel.send(JSON.stringify({ "message": message, "type": "particular", "connectionID": myConnectionID, "user": username }));
                wsconn.invoke("Message", message, "particular", PartnerChatConnectionID);
                if (!document.querySelector("#P" + PartnerChatConnectionID)) {
                    makeNewBoxChatPrivate(PartnerChatConnectionID);
                }
                addnewMessageForMe(message, document.querySelector("#P" + PartnerChatConnectionID).querySelector("#chatconversation"));
                messageTextArea.value = "";
                if (e.preventDefault) e.preventDefault();
                return false;
            }
        }
    }
});

// ******************************* Send File *********************************//

var handleFileInputChange = async () => {
    const file = fileInput.files[0];
    if (!file) {
        console.log("NO file chosen !");
    } else {
        sendFileButton.disabled = false;
    }
};
var closeDataChannels = (dataChannel) => {
    console.log('Closing data channels');
    dataChannel.close();
    console.log(`Closed data channel with label: ${dataChannel.label}`);
};
var addNewFile = (filename) => {
    if (!document.querySelector("#P" + PartnerChatConnectionID)) {
        makeNewBoxChatPrivate(PartnerChatConnectionID);
    }
    var tmp = "<div class=\"box-content-chat\">\
        <div class=\"chatmessage\">\
            <div class=\"username localuser\"></div>\
            <div class=\"timestamp\">"+ Date(Date.now()).toString().split(" ")[4] + "</div>\
            <div class=\"usermessage\"><p class=\"userMessageContent\" ><a download href=\"\"><div><i class=\"fas fa-file-word\"></i></div>\
                    <div><span>" + filename + "</span></div></a></p></div>\
        </div></div>";
    document.querySelector("#P" + PartnerChatConnectionID).querySelector("#chatconversation").innerHTML += tmp;
}
var sendData = (sendChannel, isPrivate) => {
    console.log("hi");
    const file = fileInput.files[0];
    console.log(`File is ${[file.name, file.size, file.type, file.lastModified].join(' ')}`);
    if (file.size === 0) {
        //statusMessage.textContent = 'File is empty, please select a non-empty file';
        closeDataChannels(sendChannel);
        return;

    }
    //sendProgress.max = file.size;
    const chunkSize = 16384;
    let offset = 0;
    var fileReader = new FileReader();
    fileReader.addEventListener('error', error => console.error('Error reading file:', error));
    fileReader.addEventListener('abort', event => console.log('File reading aborted:', event));
    fileReader.addEventListener('load', e => {
        console.log('FileRead.onload ', e);
        sendChannel.send(e.target.result);
        offset += e.target.result.byteLength;
        //sendProgress.value = offset;
        if (offset == file.size) {
            if (isPrivate) {
                sendChannel.send(JSON.stringify({ "type": "done", "filename": file.name, "connectID": myConnectionID }));
                callbackSendFileSuccess();
            }
            else {
                sendChannel.send(JSON.stringify({ "type": "done", "filename": file.name }));
                callbackSendFileSuccess();
            }
        }
    });
    const readSlice = o => {
        console.log('readSlice ', o);
        const slice = file.slice(offset, o + chunkSize);
        fileReader.readAsArrayBuffer(slice);
    };
    readSlice(0);
    addNewFile(file.name);
}

var addNewFileRemotePublic = (received, filename) => {
    var tmp = "<div class=\"box-content-chat\">\
        <div class=\"chatmessage\">\
            <div class=\"username localuser\"></div>\
            <div class=\"timestamp\">"+ Date(Date.now()).toString().split(" ")[4] + "</div>\
            <div class=\"usermessage\"><p class=\"userMessageContent\" ><a id=\"download\"><div><i class=\"fas fa-file-word\"></i></div>\
                    <div><span>" + filename + "</span></div></a></p></div>\
        </div></div>";
    document.querySelector("#chat-public").querySelector("#chatconversation").innerHTML += tmp;
    var downloadAnchor = document.querySelector("a#download");
    downloadAnchor.href = URL.createObjectURL(received);
    downloadAnchor.download = filename;
    downloadAnchor.textContent = filename;
    downloadAnchor.style.display = 'block';
};
var addNewFileRemotePrivate = (received, filename, connectID) => {
    if (!document.querySelector("#P" + connectionID)) {
        makeNewBoxChatPrivate(data.connectionID);
    }
    var tmp = "<div class=\"box-content-chat\">\
        <div class=\"chatmessage\">\
            <div class=\"username localuser\"></div>\
            <div class=\"timestamp\">"+ Date(Date.now()).toString().split(" ")[4] + "</div>\
            <div class=\"usermessage\"><p class=\"userMessageContent\" ><a id=\"download\"><div><i class=\"fas fa-file-word\"></i></div>\
                    <div><span>" + filename + "</span></div></a></p></div>\
        </div></div>";
    document.querySelector("#P" + connectID).querySelector("#chatconversation").innerHTML += tmp;
    var downloadAnchor = document.querySelector("a#download");
    downloadAnchor.href = URL.createObjectURL(received);
    downloadAnchor.download = filename;
    downloadAnchor.textContent = filename;
    downloadAnchor.style.display = 'block';
}

function onReceiveMessageCallback(event, receiveSize, receiveBuffer, receiveChannel,isPublic) {
    console.log(event.data);

    if (event.data.byteLength) {
        console.log("Received Message :" + event.data.byteLength );
        receiveBuffer.push(event.data);
        receiveSize += event.data.byteLength;
        
    }
    else{
        var data = JSON.parse(event.data);
        console.log(data);
        const received = new Blob(receiveBuffer);
        receiveBuffer = [];
        if(isPublic)
            addNewFileRemotePublic(received,data.filename);
        else
            addNewFileRemotePrivate(received, data.filename);
        closeDataChannels(receiveChannel);
    }
};
function onReceiveChannelStateChange(receiveChannel) {
    const readyState = receiveChannel.readyState;
    console.log(`Receive channel state is: ${readyState}`);
}
var receiveChannelCallBack = (receiveChannel,receiveSize,receiveBuffer,isPublic) => {
    console.log('Receive Channel Callback');
    receiveChannel.binaryType = "arraybuffer";
    receiveChannel.onmessage = e => onReceiveMessageCallback(e,receiveSize,receiveBuffer,receiveChannel,isPublic);
    receiveChannel.onopen = onReceiveChannelStateChange(receiveChannel);
    receiveChannel.onclose = onReceiveChannelStateChange(receiveChannel);
    receiveSize = 0;
    downloadAnchor.textContent = '';
    downloadAnchor.removeAttribute('download');
    if (downloadAnchor.href) {
        URL.revokeObjectURL(downloadAnchor.href);
        downloadAnchor.removeAttribute('href');
    }
};
var createDataChannelsPublic = () => {
    console.log("Create Data Channels public !!!");
    for (var conn in connections) {
        console.log(connections[conn]);
        var fileDtChannel = connections[conn].createDataChannel("send-file");
        fileDtChannel.binaryType = "arraybuffer";
        fileDtChannel.addEventListener("open", (e) => {
            const readyState = fileDtChannel.readyState;
            console.log(`Send channel state is: ${readyState}`);
            if (readyState === 'open') {
                sendData(fileDtChannel,false);
            }
        });
        fileDtChannel.addEventListener('close', (e) => {
            const readyState = fileDtChannel.readyState;
            console.log(`Send channel state is: ${readyState}`);
        });
        fileDtChannel.addEventListener('error', error => console.error('Error in sendChannel:', error));
        fileDtChannel.onmessage = e => {
            console.log(e.data);
        };
    }
}
var createDataChannelPrivate = (connectID) => {
    console.log("Create Data Channels private !!!");
    var fileDtChannel = connections[connectID].createDataChannel("send-file-private");
        fileDtChannel.binaryType = "arraybuffer";
        fileDtChannel.addEventListener("open", (e) => {
            const readyState = fileDtChannel.readyState;
            console.log(`Send channel state is: ${readyState}`);
            if (readyState === 'open') {
                sendData(fileDtChannel,true);
            }
        });
        fileDtChannel.addEventListener('close', (e) => {
            const readyState = fileDtChannel.readyState;
            console.log(`Send channel state is: ${readyState}`);
        });
        fileDtChannel.addEventListener('error', error => console.error('Error in sendChannel:', error));
        fileDtChannel.onmessage = e => {
            console.log(e.data);
        }
}
var SendFile = () => {
    if ("nav-item active" == document.querySelector("li#public").getAttribute("class")) {
        createDataChannelsPublic();
    }
    else if ("nav-item active" == document.querySelector("li#private").getAttribute("class") && isChatPrivate && !isChatPublic) {
        createDataChannelPrivate(PartnerChatConnectionID);
    }
}

fileInput.addEventListener("change", handleFileInputChange, false);
sendFileButton.addEventListener("click", SendFile);

const callbackSendFileSuccess = () => {
    alert("Gui file thanh cong");
}

// ************************** Zoom ******************************//

document.querySelector("#zoom").addEventListener("click", () => {
    var video_element = document.querySelector("#screen");
    if (video_element.requestFullscreen) {
        video_element.requestFullscreen();
    }
});