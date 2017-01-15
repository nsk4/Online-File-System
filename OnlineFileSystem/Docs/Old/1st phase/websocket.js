document.addEventListener("DOMContentLoaded", function() {
	//document.getElementById("syncButton").addEventListener("click", initWebSocket);
	initWebSocket();
});

var tempSocket;
var host = "127.0.0.1:1234"; // vedi streznik v LUSY
function initWebSocket(event) {
	tempSocket = new WebSocket("ws://"+host);
	tempSocket.binaryType = 'arraybuffer';
	tempSocket.addEventListener("message", dataHandler);
	tempSocket.addEventListener("error", errorHandler);
}

function dataHandler(event) {
	serverStatus = JSON.parse(event.data);
	console.log(serverStatus);
	if(serverStatus.state>8)
	{
		document.getElementById("serverStatus").innerHTML = "Strong";
		document.getElementById("serverStatus").style.color = "green";
	}
	else if(serverStatus.state>4)
	{
		document.getElementById("serverStatus").innerHTML = "Moderate";
		document.getElementById("serverStatus").style.color = "orange";
	}
	else
	{
		document.getElementById("serverStatus").innerHTML = "Weak";
		document.getElementById("serverStatus").style.color = "red";
	}
	
	//document.getElementById("serverStatus").value = serverStatus.state;
}

function errorHandler(event) {
	//document.getElementById("serverStatus").innerHTML = "ERROR in communication with server";
	alert("There is a problem with connection to the server");
	//tempSocket.removeEventListener("message", dataHandler);
	//tempSocket.removeEventListener("error", errorHandler);
}