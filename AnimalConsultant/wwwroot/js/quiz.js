let questionIndex = 1;

$(document).on('click', '.quiz-answer', function () {
    $.ajax({
        url: `/home/GetQuizQuestion?index=${questionIndex}`,
        type: "Get",
        data: { index: questionIndex},
        success: function (data) {
            questionIndex = questionIndex + 1;
            $('.quiz-container').fadeOut("slow");
            $(".quiz-container").html(data);
            $(".quiz-container").fadeIn("slow" );
        }
    });
});