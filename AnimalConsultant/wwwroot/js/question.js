$(document).on('submit', "#create-question", function (e) {
    var $this = $(this);
    var formData = new FormData(this);
    var filesInputs = $this.find(".input-file.btn.btn-tan-hide:not('.file-chooser__input')");

    for (var i = 0; i < filesInputs.length; i++) {
        //formData.append(`Images[${i}]`, filesInputs[i].files[0]);
        $(filesInputs[i]).attr("name", `Images[${i}]`);
    }
    return true;
});

$(document).on('click', '.animal-filter', function() {
    console.log(window.location.href);
    var url = new URL(window.location.href);
    var animalsValues = url.searchParams.getAll("animalTypeId");

    var currentAnimalId = $(this).data("value").toString();

    if (animalsValues.includes(currentAnimalId)) {
        animalsValues = animalsValues.filter(function (value, index, arr) {
            return value !== currentAnimalId;
        });
        url.searchParams.delete("animalTypeId");
        animalsValues.forEach(function (item, i, arr) {
            url.searchParams.append("animalTypeId", item);
        });
    } else {
        url.searchParams.append("animalTypeId", currentAnimalId);
    }
    console.log(url.toString());
    window.location.href = url;
});

$(document).on('click', '.category-filter', function () {
    console.log(window.location.href);
    var url = new URL(window.location.href);
    var categoriesValues = url.searchParams.getAll("categoryId");

    var currentCategoryId = $(this).data("value").toString();

    if (categoriesValues.includes(currentCategoryId)) {
        categoriesValues = categoriesValues.filter(function (value, index, arr) {
            return value !== currentCategoryId;
        });
        url.searchParams.delete("categoryId");
        categoriesValues.forEach(function (item, i, arr) {
            url.searchParams.append("categoryId", item);
        });
    } else {
        url.searchParams.append("categoryId", currentCategoryId);
    }
    console.log(url.toString());
    window.location.href = url;
});

$(document).on('submit', '.rd-search', function (e) {
    e.preventDefault();
    console.log(window.location.href);
    var url = new URL(window.location.href);
    url.searchParams.delete("q");
    url.searchParams.append("q", $("#rd-search-form-input").val());
    console.log(url.toString());
    window.location.href = url;
});

$(document).on('submit', '.add-comment-form', function(e) {
    e.preventDefault();
    var formData = new FormData(this);
    $.ajax({
        url: "/account/AddComment",
        type: "POST",
        data: formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            $(".blog-main-item").append(data);
            $("#comment-message").val("");
        }
    });
});

$(document).on('click', '.like-reaction', function (e) {
    var url = $(this).data("url");
    var $this = $(this);
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            if (data.length > 0)
                window.location.href = "/login";
            else {
                if ($this.hasClass("like")) {
                    var parent = $this.parent(".likes");

                    var counter = parent.find(".likes-count");

                    if (parent.find(".dislike").hasClass("active")) {
                        parent.find(".dislike").removeClass("active");
                        var dislikes = parent.find(".dislike").find(".dislikes-count");
                        dislikes.html(parseInt(dislikes.html()) - 1);
                    }

                    if ($this.hasClass("active")) {
                        $this.removeClass("active");
                        counter.html(parseInt(counter.html()) - 1);
                    }
                    else {
                        $this.addClass("active");
                        counter.html(parseInt(counter.html()) + 1);
                    }
                }
                if ($this.hasClass("dislike")) {
                    var parent2 = $this.parent(".likes");

                    if (parent2.find(".like").hasClass("active")) {
                        parent2.find(".like").removeClass("active");
                        var likes = parent2.find(".likes-count");
                        likes.html(parseInt(likes.html()) - 1);
                    }

                    var counter2 = parent2.find(".dislikes-count");
                    if ($this.hasClass("active")) {
                        $this.removeClass("active");
                        counter2.html(parseInt(counter2.html()) - 1);
                    }
                    else {
                        $this.addClass("active");
                        counter2.html(parseInt(counter2.html()) + 1);
                    }
                }
            }
           
        }
    });
});