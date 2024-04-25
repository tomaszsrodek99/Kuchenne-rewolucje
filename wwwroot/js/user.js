$(document).ready(function () {
    $('#change-image-button').on('click', function() {
        $('#change-image-form').prop('hidden', false)
    });
    $('#go-back').on('click', function () {
        $('#change-image-form').prop('hidden', true)
    });
})
