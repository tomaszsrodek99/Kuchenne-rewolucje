document.querySelectorAll('.category-element a').forEach(function (link) {
    link.addEventListener('click', function (event) {
        var categoryId = this.getAttribute('data-categoryid');
        var currentPath = window.location.pathname;

        if (currentPath !== '/Article/Index') {
            return;
        }

        event.preventDefault();
        filterByCategory(categoryId);

        var activeLink = document.querySelector('.category-element a.active');
        if (activeLink) {
            activeLink.classList.remove('active');
        }
        this.classList.add('active');
    });
});

function goBack() {
    history.back();
}

function searchByName() {
    var input, filter, articles, article, title, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    articles = document.getElementsByClassName("box-shadow");

    for (i = 0; i < articles.length; i++) {
        article = articles[i];
        title = article.getElementsByTagName("a")[0];
        txtValue = title.textContent || title.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
            article.style.display = "";
        } else {
            article.style.display = "none";
        }
    }
}

function filterByCategory(categoryId) {
    var articles = document.querySelectorAll('.box-shadow');
    articles.forEach(function (article) {
        var articleCategoryId = article.getAttribute('data-categoryid');
        if (categoryId !== articleCategoryId) {
            article.setAttribute('hidden', true);
        } else {
            article.removeAttribute('hidden');
        }
    });
}

function AddArticleToFavourites(articleId, element) {
    $.ajax({
        url: 'http://localhost:7240/Article/AddFavouriteArticle?id=' + articleId,
        type: "POST",
        success: function (result) {
            if (element.classList.contains('favourites')) {
                element.classList.remove('favourites');
            } else {
                element.classList.add('favourites');
            }
        }
    });
}

function hideArticlesWithoutFavourites() {
    var articles = document.querySelectorAll('.col-3.box-shadow');
    var favouriteIcon = document.querySelector('#favourites');

    var showAllArticles = favouriteIcon.classList.toggle('favourites');

    articles.forEach(function (article) {
        var favouriteIcon = article.querySelector('.fa-solid.fa-star.favourites');
        article.hidden = showAllArticles && !favouriteIcon;
    });
}

