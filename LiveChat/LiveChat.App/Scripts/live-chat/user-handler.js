var userHandler;

$(function() {
    userHandler = $.connection.userHandlerHub;

    userHandler.client.addInformation = function (message) {
        var $informationPanel = $('#information-panel');
        $informationPanel.html(message);
        $informationPanel.show('slow').delay(5000).hide('slow');
    };

    userHandler.client.setConnectionStatus = function (userId, isConnected) {
        var $control = $('#' + userId + '-user-list-element > .badge');

        if (isConnected) {
            $control.fadeIn('slow');
        } else {
            $control.fadeOut('slow');
        }
    };

    userHandler.client.newMessageAlert = function (authorName, url) {
        if (document.URL != url) {
            var $informationPanel = $('#information-panel');
            $informationPanel.html('New message from <strong>' + authorName + '</strong>.<a href="' + url + '">Click to read</a>');
            $informationPanel.show('slow').delay(7654).hide('slow');
        }
    };

    $.connection.hub.start();
});