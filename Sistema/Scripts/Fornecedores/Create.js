$(function () {

    if ($("#tipo").val() == "F") {
        $(".fisica").show();
        $(".juridica").hide();
    } else {
        $(".fisica").hide();
        $(".juridica").show();
    }

    $("#tipo").change(function () {
        if ($("#tipo").val() == "F") {
            $(".fisica").slideDown();
            $(".juridica").slideUp();
        } else {
            $(".fisica").slideUp();
            $(".juridica").slideDown();
        }

    });

});
