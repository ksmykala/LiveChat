function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

function scrollToEnd(selector) {
    $(selector).scrollTop($(selector)[0].scrollHeight);
}