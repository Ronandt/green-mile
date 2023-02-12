"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNotification", function (notification) {
  // var noNotif = document.getElementById("noNotification");
  // noNotif.remove();

  console.log(notification);
  let { id, message, system, date, read } = notification;
  var tableBody = document.getElementById('tableBody');
  var row = document.createElement('tr');
  tableBody.appendChild(row);
  row.id = "notification_" + id;
  var sysCol = document.createElement('td');
  row.appendChild(sysCol);
  sysCol.textContent = system;
  var msgCol = document.createElement('td');
  row.appendChild(msgCol);
  msgCol.textContent = message;
  var readCol = document.createElement('td');
  row.appendChild(readCol);
  readCol.textContent = read ? "Read" : "Unread";
  var dateCol = document.createElement('td');
  row.appendChild(dateCol);
  dateCol.textContent = date;
  var actionCol = document.createElement('td');
  row.appendChild(actionCol);
  actionCol.textContent = "-";

  var total = document.getElementById('totalNotifications')
  var tnum = parseInt(total.innerText)
  total.innerText = tnum + 1
  var unread = document.getElementById('unreadNotifications')
  var unum = parseInt(unread.innerText)
  unread.innerText = unum + 1
});

connection.start()
  .then(() => console.log("Okay"))
  .catch(err => console.log(err));

function deleteNotification(e) {
  var row = e.parentElement.parentElement;
  let { id } = row;
  var notificationId = id.split("_")[1]
  connection.invoke("DeleteNotification", parseInt(notificationId)).catch(err => console.log(err));
  row.remove();
  var total = document.getElementById('totalNotifications')
  var tnum = parseInt(total.innerText)
  total.innerText = tnum - 1
}