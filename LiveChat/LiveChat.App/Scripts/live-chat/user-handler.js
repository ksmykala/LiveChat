var userHandler;

$(function() {
    userHandler = $.connection.userHandlerHub;

    userHandler.client.addInformation = function (message) {
        var $informationPanel = $('#information-panel');
        $informationPanel.html(message);
        $informationPanel.show('slow').delay(5000).hide('slow');
    };

    userHandler.client.setConnectionStatus = function(userId, isConnected) {
        var $control = $('#' + userId + '-user-list-element > .badge');

        if (isConnected) {
            $control.fadeIn('slow');
        } else {
            $control.fadeOut('slow');
        }
    };

    $.connection.hub.start();
});