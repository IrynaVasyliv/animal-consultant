$(".like").on("click", function() {
    event.preventDefault();
    var $this = $(this);
    var parent = $this.parent(".likes");

    var counter = parent.find(".likes-count");

    if (parent.find(".dislike").hasClass("active")) {
        parent.find(".dislike").removeClass("active");
        var dislikes = parent.find(".dislike").find(".dislikes-count");
        dislikes.html(parseInt(dislikes.html()) - 1);
    }
   
    if($this.hasClass("active")) {
        $this.removeClass("active");
        counter.html(parseInt(counter.html()) - 1);
    }
    else {
        $this.addClass("active");
        counter.html(parseInt(counter.html()) + 1);
    }
});

$(".dislike").on("click", function() {
    event.preventDefault();
    var $this = $(this);
    var parent = $this.parent(".likes");

    if (parent.find(".like").hasClass("active")) {
        parent.find(".like").removeClass("active");
        var likes = parent.find(".likes-count");
        likes.html(parseInt(likes.html()) - 1);
    }

    var counter = parent.find(".dislikes-count");
    if($this.hasClass("active")) {
        $this.removeClass("active");
        counter.html(parseInt(counter.html()) - 1);
    }
    else {
        $this.addClass("active");
        counter.html(parseInt(counter.html()) + 1);
    }    
});