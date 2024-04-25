$(document).ready(function () {
    $('#login-container').hide();
    $('.no-list-style').on('click', function () {
        var username = $(this).data('username');
        $('#username').val(username);
        $('#username').prop('readonly', true);
        $('#list-profiles').slideUp(1000, function () {
            $('#login-container').slideDown(1000);
        });
    });
    
    $('#go-back').on('click', function () {
        $('#login-container').slideUp(1000, function () {
            $('#list-profiles').slideDown(1000);
        });
    });
});

