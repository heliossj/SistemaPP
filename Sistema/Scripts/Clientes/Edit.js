$(function () {
    document.getElementById("cpf").readOnly = true;
    document.getElementById("cnpj").readOnly = true;

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

    //$("#cpf").on('change', function () {
    //    if (!ValidCPF($("#cpf").val())) {
    //        $.notify({ message: "Informe um CPF válido!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000, });
    //        $("#cpf").val("");
    //    } else {
    //        $.notify({ message: "CPF válido!", icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000, });
    //    }
    //});

    //$("#cnpj").on('change', function () {
    //    if (!ValidCNPJ($("#cnpj").val())) {
    //        $.notify({ message: "Informe um CNPJ válido!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000, });
    //        $("#cnpj").val("");
    //    } else {
    //        $.notify({ message: "CNPJ válido!", icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000, });
    //    }
    //})

});
