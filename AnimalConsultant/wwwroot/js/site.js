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
      
$(document).on("keydown", ".input-file-trigger", function( event ) {  
    if ( event.keyCode == 13 || event.keyCode == 32 ) {  
        $(this).parent().find(".input-file").focus();
    }  
});

$(document).on("click", ".input-file-trigger", function( event ) {
    $(this).parent().find(".input-file").focus();
   return false;
});  

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.file-input-replace').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$(document).on("change", ".input-file", function (event) {
    $(this).parent().find(".input-file-trigger").html(this.value.substr(0, 60) + "...");
    if ($(this).hasClass("change-picture-input-file")) {
        readURL(this);
    }
});

$('.input-value').keyup(function() {
    var $this = $(this);
    console.log($('.' + $this.attr('name')));
    console.log($this.val());
    $('.' + $this.attr('name')).html($this.val());
});


$(document).on("click", '.dropdown .dropdown-item', function (e) {

    var selText = $(this).text();
    var valText = $(this).data("value");

    var element = $(this).parents('.dropdown').find('.dropdown-toggle');
    var elementInput = $(this).parents('.dropdown').find(".input-hidden");

    elementInput.val(valText);
    var elementSpan = $(this).parents('.dropdown').find('.dropdown-toggle span');

    elementSpan.html(selText);
    element.data("value", valText);
    element.next().val(valText);

});