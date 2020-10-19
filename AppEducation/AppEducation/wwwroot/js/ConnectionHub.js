'use strict';
const isDebugging = true;

var hubUrl = document.location.pathname + 'ConnectionHub';

// Create connection to Hub
var wsconn = new signalR.HubConnectionBuilder().withUrl("/ConnectionHub").build();

document.addEventListener("DOMContentLoaded", () => {
    console.log("Ready for Hub");
    initialize();
});

// initial something
const initialize= ()=>{
    console.log("Start connection for hub!")
    wsconn.start()
        .then(() => {
            wsconn.invoke("Join", username, classid)
                .catch((err) => {
                    console.log(err);
                });
        })
        .catch(err => console.log(err));
};
wsconn.on('updateUserList', (userList) => {
    console.log("update list users " + JSON.stringify(userList));
});
wsconn.on("NotifyNewMember", (newMember) => {
    console.log(" new member");
    wsconn.invoke("CallUser")
        .catch(err => console.log(err));
});
wsconn.on('incomingCall', (callingUser) => {
    console.log('SignalR: incoming call from: ' + JSON.stringify(callingUser));
    wsconn.invoke('AnswerCall', true, callingUser).catch(err => console.log(err));
});
    