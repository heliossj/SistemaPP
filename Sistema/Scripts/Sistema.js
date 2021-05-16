$(function () {

    // formata input datepicker
    $.fn.datepicker.dates['pt-BR'] = {
        days: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado", "Domingo"],
        daysShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb", "Dom"],
        daysMin: ["Do", "Se", "Te", "Qu", "Qu", "Se", "Sa", "Do"],
        months: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthsShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        today: "Hoje",
        clear: "Limpar",
        titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
        weekStart: 0
    };

    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        lacale: 'pt-BR',
        clearBtn: true,
    });

    $('.phone').addEventListener('blur', function (e) {
        var x = e.target.value.replace(/\D/g, '').match(/(\d{3})(\d{3})(\d{4})/);
        e.target.value = '(' + x[1] + ') ' + x[2] + '-' + x[3];
    });

    //$(document).ready(function () {

    //});

    //$('input[type="numeric"]').each(function () {
    //    var val = $(this).val().replace('.', ',');
    //    $(this).val(val);
    //});

    //$("input").on("change", function () {
    //    $(this).val(parseFloat($(this).val()).toFixed(2));
    //});




});
