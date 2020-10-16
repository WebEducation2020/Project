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

};
