﻿$(document).on('submit', "#create-question", function (e) {
    var $this = $(this);
    var formData = new FormData(document.getElementById("create-question"));
    var filesInputs = $this.find(".input-file.btn.btn-tan-hide:not('.file-chooser__input')");

    for (var i = 0; i < filesInputs.length; i++) {
        formData.append(`Images[${i}]`, filesInputs[i].files[0]);
    }
    return true;
});