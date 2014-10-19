$(function () {


    $('.remove-user').click(function () {
        var userId = $(this).data('user-id');
        var userName = $(this).data('user-name');

        var confirmed = confirm('User ' + userName + ' will be remove - are you sure?');

        if (confirmed) {
            var elementToRemove = $(this).parent().parent();
            var url = $('#role-url-container').data('url-removeuser');
            $.ajax({
                url: url,
                data: { userId: userId }
            }).complete(function () {
                elementToRemove.slideUp('slow', function () {
                    elementToRemove.remove();
                });
            });
        }
    });

    $('.remove-role').click(function () {
        var roleId = $(this).data('role-id');
        var roleName = $(this).data('role-name');
        var userId = $(this).data('user-id');
        var userName = $(this).data('user-name');

        var confirmed = confirm('Role ' + roleName + ' will be unpined from user ' + userName + ' - are you sure?');

        if (confirmed) {
            var elementToRemove = $(this);
            var url = $('#role-url-container').data('url-removeuserrole');
            $.ajax({
                url: url,
                data: { userId: userId, roleId: roleId }
            }).complete(function (role) {
                elementToRemove.hide('slow', function () {
                    $(role.responseText).appendTo('#users-roles').show('slow');
                    elementToRemove.remove();
                });
            });
        }
    });

    $('.add-role').click(function () {
        var roleId = $(this).data('role-id');
        var userId = $(this).data('user-id');
        var url = $('#role-url-container').data('url-adduserrole');

        var elementToRemove = $(this);
        $.ajax({
            url: url,
            data: { userId: userId, roleId: roleId }
        }).complete(function (role) {
            elementToRemove.hide('slow', function () {
                $(role.responseText).appendTo('#users-roles').show('slow');
                elementToRemove.remove();
            });
        });
    });

    function updateUserRoleCounter(userId) {
        var url = $('#role-url-container').data('url-getuserrolescount');
        $.ajax({
            url: url,
            data: { userId: userId }
        }).success(function (count) {
            $('#' + userId + '-roles > .badge').html(count);
        });
    }
});