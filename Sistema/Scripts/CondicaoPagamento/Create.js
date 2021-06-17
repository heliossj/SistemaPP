$(function () {

    var cond = new CondicaoPagamento;
    cond.init();
    //$(document).ready(function () {

    //});



    $("#addCondPagamento").click(function () {
        //alert();
        cond.addItem();

    })


});


CondicaoPagamento = function () {
    self = this;
    var dt = null;
    this.init = function () {
        dt = $('#tbcondicao').DataTable({
            language: {
                //url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/Portuguese-Brasil.json',
            },
            //data: "ListCondicaoPagamento",
            searching: false,
        });
    }
    console.log(dt)

    self.valid = function () {
        let valid = true;
        if (IsNullOrEmpty($("#qtDias").val())) {
            $("#qtDias").blink({ msg: "Informe a quantidade de dias" });
            valid = false;
        }

        if (IsNullOrEmpty($("#txPercentual").val())) {
            $("#txPercentual").blink({ msg: "Informe o percentual" });
            valid = false;
        }

        if (IsNullOrEmpty($("#codForma").val())) {
            $("#codForma").blink({ msg: "Informe a condição de pagamento" });
            $("#nomeForma").blink({ msg: "Informe a condição de pagamento" });
            valid = false;
        }

        return valid;
    }

    self.getModel = function () {
        var model = {
            codFormaPagamento: $("#codForma").val(),
            nomeFormaPagamento: $("#nomeForma").val(),
            qtDias: $("#qtDias").val(),
            txPercentual: $("#txPercentual").val()
        }
        return model;

    }

    self.clear = function () {
        $("#codForma").val('');
        $("#nomeForma").val('');
        $("#qtDias").val('');
        $("#txPercentual").val('');
    }

    self.addItem = function () {
        if (self.valid()) {
            var model = self.getModel();
            console.log(model);
            alert("addItem");


            let nr = 0;
            
            dt.row.add([
                nr+= 1,
                model.qtDias,
                model.txPercentual,
                model.nomeFormaPagamento,
            ]).draw(true);
            self.clear();
            console.log(dt.rows().data());
            
        }
    }



}
