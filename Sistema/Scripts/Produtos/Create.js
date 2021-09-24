$(function () {
    if ($("#unidade").val() == "M") {
        $("#divLargura").show()
    } else if ($("#unidade").val() == "U") {
        $("#largura").val("");
        $("#divLargura").hide()
    }

    $("#unidade").change(function () {
        if ($(this).val() == "M") {
            $("#divLargura").show()
        } else if ($(this).val() == "U") {
            $("#largura").val("");
            $("#divLargura").hide()
        }
    })



});
