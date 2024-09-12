const connection = new signalR.HubConnectionBuilder()
    .withUrl('/ChatHub')
    .build();

connection.start()
    .catch(error => { console.error(error.message) }).then(result =>
    {
        addClientToGroup();
    });

connection.on('receiveMessage', (username, message, createdAt, path) => {

    addMessageToChat(username, message, createdAt, path);
    chat.scrollTop = chat.scrollHeight;

});


connection.on('test', () => {
    alert("test");
});


function sendMessageToHub(message) {

    connection.invoke("sendMessage", message);
} 

function addClientToGroup()
{
    if (!groupId.textContent) return;

    connection.invoke("addToGroup", groupId.textContent);
}

const textInput = document.getElementById('messageText');
const chat = document.getElementById('chat-container');
const groupId = document.getElementById('groupId');

const messageQueue = [];



document.getElementById('submitButton').addEventListener('click', () => {
    var currentDate = new Date();
    date = currentDate;
    clearInputField();
    sendMessage();
});

document.onkeydown = function () {
    if (window.event.keyCode == '13') {
        clearInputField();
        sendMessage();    }
}


chat.scrollTop = chat.scrollHeight;

function clearInputField() {
    messageQueue.push(textInput.value);
    textInput.value = "";
}

function sendMessage() {
    let text = messageQueue.shift() || "";
    if (text.trim() === "") return;

    let date = new Date();

    sendMessageToHub(text);
}

function addMessageToChat(senderName, message, date, path) {

    console.log("sendind message");
    let container = document.createElement('div');
    container.className = 'w-100 d-flex align-items-start p-2';

    let iconPfp = document.createElement('img');
    iconPfp.className = 'icon-pfp m-2';
    iconPfp.width = 128;
    iconPfp.alt = path;
    iconPfp.src = path;
    container.appendChild(iconPfp);

    let contentContainer = document.createElement('div');
    contentContainer.className = 'w-75';

    let headerContainer = document.createElement('div');
    headerContainer.className = 'w-50 d-flex align-items-center text-white';

    let displayName = document.createElement('div');
    displayName.textContent = senderName;
    headerContainer.appendChild(displayName);

    let sentAt = document.createElement('div');
    sentAt.className = 'text-secondary m-2';
    sentAt.style.fontSize = '10px';
    sentAt.textContent = date;

    headerContainer.appendChild(sentAt);
    contentContainer.appendChild(headerContainer);

    let messageContent = document.createElement('div');
    messageContent.className = 'text-white w-100';
    messageContent.textContent = message;

    contentContainer.appendChild(messageContent);
    container.appendChild(contentContainer);

    chat.appendChild(container);

    chat.scrollTop = chat.scrollHeight;


}