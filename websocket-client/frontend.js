$(function () {
    "use strict";

    var input = $('#input');
    var status = $('#status');
    var button = $('#button');
    var result = $('#result');

    var connection = new WebSocket('wss://g49fepw5h8.execute-api.us-west-2.amazonaws.com/Test?ContractId=A100');

    connection.onopen = function() {
        console.log("WebSocket opened successfully");
        input.removeAttr('disabled');
        status.html('Enter the message, for example: <br /> { "action": "message", "ContractId": "A100" } <br />{ "action": "freezeMoney", "ContractId": "A100" } <br />');
        connection.send('{ "action": "message", "ContractId": "A100" }');
    }

    connection.onerror = function(error) {
        console.log("Something happened :(");
        console.log(error);
    }

    connection.onmessage = function(message) {
        console.log("Message received");
        var data = JSON.parse(message.data);

        result.html("<p>ContractId: " + data.ContractId.Value 
            + "<br />Instrument Id: " + data.InstrumentId.Value + "</p>");
    }

    button.click(function(e) {
        connection.send(input.val());
    })
});