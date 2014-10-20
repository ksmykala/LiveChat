var userHandler;

$(function() {
    userHandler = $.connection.userHandlerHub;

    userHandler.client.addInformation = function (message) {
        var $informationPanel = $('#information-panel');
        $informationPanel.html(message);
        $informationPanel.show('slow').delay(5000).hide('slow');
    };

    $.connection.hub.start();
});