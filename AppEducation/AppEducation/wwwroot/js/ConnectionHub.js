'use strict';
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
        width: 1080,
        height: 720
    },
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
cameraStream.onaddtrack = async e => { await callbackOnaddtrackCamera(e); };
var remoteAudio = new MediaStream();
let localcamera, localscreen, localaudio;
let isCaller = false;
var connections = {};
var localDataChannels = {};
var remoteDataChannels = {};
const initialize = async () => {
    console.log("Start connection for hub!")
    wsconn.start() // when this succeeds, fulfilling the returned promise
        .then(async () => {
            await wsconn.invoke("Join", username, classid)
                .catch((err) => {
                    console.log("Join Fail | " + err);
                });
        })
        .catch(err => console.log(err));
};
document.addEventListener("DOMContentLoaded", async () => {
    console.log("Ready for Hub");
    await initialize();
});
const callbackDisplayMediaSuccess = async (stream) => {
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

//=============== Condition of get user divices=========================================
const callbackUserMediaSuccess = async (stream) => {
    console.log("WebRTC: got camera media stream");
    var camera = document.querySelector("#camera");
    cameraStream = stream;
    camera.srcObject = cameraStream;
    localcamera = new MediaStream(stream.getTracks());
}
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
const initializeDevices = async (UserCall) => {
    console.log(JSON.stringify(UserCall));
    console.log('WebRTC: InitializeUserMedia: ');
    if (isCaller) {
        await navigator.mediaDevices.getDisplayMedia(screenConstraints)
            .then((stream) => callbackDisplayMediaSuccess(stream))
            .catch(err => console.log(err));
        await navigator.mediaDevices.getUserMedia(cameraConstraints)
            .then((stream) => callbackUserMediaSuccess(stream))
            .catch(err => console.log(err));
        await navigator.mediaDevices.getUserMedia(audioConstraints)
            .then(stream => callbackAudioMediaSuccess(stream))
            .catch(err => console.log(err));
    }
    else {
        localcamera = new MediaStream();
        localscreen = new MediaStream();
        await navigator.mediaDevices.getUserMedia(audioConstraints)
            .then(stream => callbackAudioMediaSuccess(stream))
            .catch(err => console.log(err));
    }
}

const receivedCandidateSignal = async (connection, partnerClientId, candidate) => {
    console.log('WebRTC: adding full candidate');
    connection.addIceCandidate(new RTCIceCandidate(candidate), () => console.log("WebRTC: added candidate successfully"), () => console.log("WebRTC: cannot add candidate"));
}

//Process a newly received SDP signal
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

// Hand off a new signal from the signaler to the connection
const newSignal = async (partnerClientId, data) => {
    console.log('WebRTC: called newSignal');
    var signal = JSON.parse(data);
    var connection = getConnection(partnerClientId);
    console.log("connection: ", connection);

    // Route signal based on type
    if (signal.sdp) {
        console.log('WebRTC: sdp signal');
        await receivedSdpSignal(connection, partnerClientId, signal.sdp);
    } else if (signal.candidate) {
        console.log('WebRTC: candidate signal');
        await receivedCandidateSignal(connection, partnerClientId, signal.candidate);
    } else {
        console.log('WebRTC: adding null candidate');
        await connection.addIceCandidate(null, async () => console.log("WebRTC: added null candidate successfully"), () => console.log("WebRTC: cannot add null candidate"));
    }
}

// Close the connection between myself and the given partner
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

const getConnection = (partnerClientId) => {
    console.log("WebRTC: called getConnection");
    if (connections[partnerClientId]) {
        console.log("WebRTC: connections partner client exist");
        return connections[partnerClientId];
    }
    else {
        console.log("WebRTC: initialize new connection");
        return initializeConnection(partnerClientId)
    }
}
// gui tin hieu di 
const sendHubSignal = async (candidate, partnerClientId) => {
    console.log('candidate', candidate);
    console.log('SignalR: called sendhubsignal ');
    await wsconn.invoke('sendSignal', candidate, partnerClientId).catch(errorHandler);
};
// goi ham them luong du lieu vao ket noi
const callbackAddStream = (connection, evt) => {
    console.log('WebRTC: called callbackAddStream');
    attachMediaStream(evt);
}
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
// goi ham tao ung vien cho ket noi
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
        remotedataChannel.onmessage = (e) => {
            var data = JSON.parse(e.data);
            console.log(data.message.length);
            if (data.type == "particular") {
                receiveMessage(data.message, document.querySelector("#chat-particular").querySelector("#chatconversation"))
            }
            else if (data.type == "public") {
                receiveMessage(data.message, document.querySelector("#chat-public").querySelector("#chatconversation"));
            }
            else if (data.type == "group") {
                receiveMessage(data.message, document.querySelector("#chat-group").querySelector("#chatconversation"));
            }
        }
        remoteDataChannels[partnerClientId] = remotedataChannel;
    });
    localDataChannels[partnerClientId] = LocaldataChannel;
    connection.onicecandidate = evt => callbackIceCandidate(evt, connection, partnerClientId);
    connection.ontrack = async e => { await callbackAddTrack(connection, e) };
    //connection.onremovestream = evt => callbackRemoveStream(connection, evt); // Remove stream handler callback
    connections[partnerClientId] = connection; // Store away the connection based on username
    return connection;
}

const initiateOffer = async (partnerClientId) => {
    console.log('WebRTC: called initiateoffer: ');
    var connection = getConnection(partnerClientId); // // get a connection for the given partner
    //console.log('initiate Offer stream: ', stream);
    console.log("offer connection: ", connection);
    console.log("WebRTC: Added local stream");
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
    else {
        for (var i = 0; i < localaudio.getTracks().length; i++) {
            console.log(localaudio.getTracks()[i]);
            connection.addTrack(localaudio.getTracks()[i]);
        }
    }
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

wsconn.on("initDevices", async (UserCall) => {
    isCaller = UserCall.isCaller;
    await initializeDevices(UserCall);
});
// initial something

wsconn.on('updateUserList', (userList) => {
    console.log("update list users " + JSON.stringify(userList));
    var listUserTag = document.querySelector("#chat-userlist");
    var strTmp = ""
    userList.forEach(user => {
        if (user.isCaller) {
            strTmp += "<a id=\"part-chat\" class=\"nav-link\" data-toggle=\"tab\" data-cid=\"" + user.connectionID + "\" href=\"#chat-particular\" onclick=addEvent(\"" + user.connectionID + "\"); >\
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
            strTmp += "<a class=\"nav-link\" data-toggle=\"tab\" datacid=\"" + user.connectionID + "\" href=\"#chat-particular\" onclick=addEvent(\"" + user.connectionID  + "\"); >\
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
    listUserTag.innerHTML = strTmp;
});
wsconn.on("NotifyNewMember",async (newMember) => {
    console.log("New member !");
    await wsconn.invoke("CallUser",newMember)
        .catch(err => console.log(err));
});
// cuoc goi toi
wsconn.on('incomingCall', async (callingUser) => {
    console.log('SignalR: incoming call from: ' + JSON.stringify(callingUser));
    await wsconn.invoke('AnswerCall', true, callingUser).catch(err => console.log(err));
});
// Hub Callback: Call Accepted
wsconn.on('callAccepted',async (acceptingUser) => {
    console.log('SignalR: call accepted from: ' + JSON.stringify(acceptingUser) + '.  Initiating WebRTC call and offering my stream up...');
    await initiateOffer(acceptingUser.connectionID);
});
// hub : receive signal
wsconn.on('receiveSignal', async (signalingUser, signal) => {
    console.log('WebRTC: receive signal ');
    await newSignal(signalingUser.connectionID, signal);
});
//Hub inform : call decline
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

// bat mic tren client duoc goi
document.querySelector("#scrn").addEventListener("click",async () => {
    //console.log("turn on/off screen");
    //var screenStream = document.querySelector("#screen").srcObject;
    //var screenTrack = screenStream.getVideoTracks()[0];
    //screenTrack.enabled = !screenTrack.enabled;
});



// *********************** Implement with chat ********************************** //
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