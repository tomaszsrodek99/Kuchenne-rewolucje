$(document).ready(function () {
    $('.category-partial-view').hide();
    $('#go-back').on('click', function () {
        $('.category-partial-view').slideUp(1000, function () {
            $('#category-list-table').slideDown(1000);
        });
    });

    $('#add-category-button').on('click', function () {
        $('#category-list-table').slideUp(1000, function () {
            $('.category-partial-view').slideDown(1000);
        });
    });

    $('#category-name').on('blur', function () {
        var name = $('#category-name').val();
        if (name.length > 0) {
            $.ajax({
                url: 'http://localhost:7240/Category/CategoryExists?name=' + encodeURIComponent(name),
                type: "POST",
                success: function (result) {
                    if (result.success === true) {
                        $('#uniqueNameError').text('');
                    } else {
                        $('#uniqueNIPError').text('Kategoria o podanej nazwie istnieje.');
                    }
                }
            });
        }
    });
});