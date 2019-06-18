    
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


//jQuery plugin
$(document).ready(function() {
    var settings = {
        MessageAreaText: "No files selected.",
        MessageAreaTextWithFiles: "File List:",
        DefaultErrorMessage: "Unable to open this file.",
        BadTypeErrorMessage: "We cannot accept this file type at this time.",
        acceptedFileTypes: ['pdf', 'jpg', 'gif', 'jpeg']
    };

    var uploadId = 1;
    //update the messaging 
    $('.file-uploader__message-area p').text(settings.MessageAreaText);

    //when choosing a file, add the name to the list and copy the file input into the hidden inputs
    $('.file-chooser__input').on('change',
        function() {
            var file = $(this).val();
            var fileName = (file.match(/([^\\\/]+)$/)[0]);

            //clear any error condition
            $('.file-chooser').removeClass('error');
            $('.error-message').remove();

            //validate the file
            var check = checkFile(fileName);
            if (check === "valid") {
                $('.file-chooser').append($($(this).parent('.input-file-container')).clone({ withDataAndEvents: true }));
                $(this).parent('.input-file-container').replaceWith("");
                // move the 'real' one to hidden list 
                $('.hidden-inputs').append($(this));

                //add the name and a remove button to the file-list
                $('.file-list').append('<li style="display: none;"><span class="file-list__name">' +
                    fileName +
                    '</span><button class="removal-button" data-uploadid="' +
                    uploadId +
                    '"></button></li>');
                $('.file-list').find("li:last").show(800);

                //removal button handler
                $('.removal-button').on('click',
                    function(e) {
                        e.preventDefault();

                        //remove the corresponding hidden input
                        $('.hidden-inputs input[data-uploadid="' + $(this).data('uploadid') + '"]').remove();

                        //remove the name from file-list that corresponds to the button clicked
                        $(this).parent().hide("puff").delay(10).queue(function() { $(this).remove(); });

                        //if the list is now empty, change the text back 
                        if ($('.file-list li').length === 0) {
                            $('.file-uploader__message-area').text(settings.MessageAreaText);
                        }
                    });

                //so the event handler works on the new "real" one
                $('.hidden-inputs .file-chooser__input').removeClass('file-chooser__input')
                    .attr('data-uploadId', uploadId);

                //update the message area
                $('.file-uploader__message-area')
                    .text(settings.MessageAreaTextWithFiles);

                uploadId++;

            } else {
                //indicate that the file is not ok
                $('.file-chooser').addClass("error");
                var errorText = settings.DefaultErrorMessage;

                if (check === "badFileName") {
                    errorText = settings.BadTypeErrorMessage;
                }

                $('.file-chooser__input').after('<p class="error-message">' + errorText + '</p>');
            }
        });

    var checkFile = function(fileName) {
        var accepted = "invalid",
            acceptedFileTypes = this.acceptedFileTypes || settings.acceptedFileTypes,
            regex;

        for (var i = 0; i < acceptedFileTypes.length; i++) {
            regex = new RegExp("\\." + acceptedFileTypes[i] + "$", "i");

            if (regex.test(fileName)) {
                accepted = "valid";
                break;
            } else {
                accepted = "badFileName";
            }
        }

        return accepted;
    }
});