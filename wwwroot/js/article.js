$(document).ready(function () {
    $('.partial-view').hide();
    $('#go-back').on('click', function () {
        $('.partial-view').slideUp(1000, function () {
            $('.editor-container').slideDown(1000);
            document.getElementById('add-article-header').textContent = 'Dodaj nowy przepis';
        });
    });

    $('#add-category-button').on('click', function () {
        $('.editor-container').slideUp(1000, function () {
            $('.partial-view').slideDown(1000);
            document.getElementById('add-article-header').textContent = 'Dodaj nową kategorię';
        });
    });

    $('#category-name').on('blur', function () {
        var name = $('#category-name').val();
        if (name.length > 0) {
            $.ajax({
                //url: 'http://localhost:7240/Category/CategoryExists?name=' + encodeURIComponent(name),
                url: 'http://singulaar-001-site1.ltempurl.com/Category/CategoryExists?name=' + encodeURIComponent(name),
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

function blur() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');
}

function CloseForm() {
    var elements = document.getElementsByClassName("rating-partial-view");

    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        element.style.visibility = "hidden";
    }

    $('#blurOverlay').hide();
    $('body').removeClass('blur-overlay-visible');
}

$('#create-rating-button').on('click', function () {
    var ratingPartialView = document.getElementById("rating-partial-view");
    ratingPartialView.style.visibility = "visible";
    $('#blurOverlay').show();
});
