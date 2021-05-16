$(function () {
    ////    $.fn.datepicker.dates['en'] = {
    ////        days: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo"],
    ////        daysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
    ////        daysMin: ["Do", "Se", "Te", "Qu", "Qu", "Se", "Sa", "Do"],
    ////        months: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
    ////        monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
    ////        today: "Hoje",
    ////        clear: "Limpar",
    ////        titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
    ////        weekStart: 0
    ////    };

    ////    $('.datepicker').datepicker({
    ////        format: 'dd/mm/yyyy',
    ////        lacale: 'en',
    ////        clearBtn: true,
    ////    });

    //$('.datepicker').datepicker({
    //    format: 'dd/mm/yyyy',
    //    lacale: 'en',
    //    clearBtn: true,
    //});

    //$(document).ready(function () {

    //});

    //$('input[type="numeric"]').each(function () {
    //    var val = $(this).val().replace('.', ',');
    //    $(this).val(val);
    //});

    //$("input").on("change", function () {
    //    $(this).val(parseFloat($(this).val()).toFixed(2));
    //});



    //$(".phone").inputmask({ "mask": "(999) 999-9999" });

    $(".phone").mask("(99) 9999-9999?9").focusout(function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unmask();
            if (phone.length > 10) {
                element.mask("(99) 99999-999?9");
            } else {
                element.mask("(99) 9999-9999?9");
            }

     });
});
