$(function () {

    if ($("#juros").val() == "N") {
        $("#divJuros").hide();
    } else {
        $("#divJuros").show();
    }

    $("#juros").change(function () {
        if ($("#juros").val() == "N") {
            $("#divJuros").slideUp();
        } else {
            $("#divJuros").slideDown();
        }
    })

    if ($("#parcela").val() == "N") {
        $("#divParcela").hide();
    } else {
        $("#divParcela").show();
    }

    $("#parcela").change(function () {
        if ($("#parcela").val() == "N") {
            $("#divParcela").slideUp();
        } else {
            $("#divParcela").slideDown();
        }
    })



});
