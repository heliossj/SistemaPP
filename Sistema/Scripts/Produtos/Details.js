﻿$(function () {
    if ($("#unidade").val() == "M") {
        $("#divLargura").show()
    } else if ($("#unidade").val() == "U") {
        $("#largura").val("");
        $("#divLargura").hide()
    }
});
