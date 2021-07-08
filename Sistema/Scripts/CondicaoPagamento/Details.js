$(function () {
    var cond = new CondicaoPagamento;
    cond.init();
    $("#addCondPagamento").click(function () {
        cond.addItem();
    })







});


CondicaoPagamento = function () {
    self = this;
    let nr = 1;
    var dtCondicao = null;
    this.init = function () {
        dtCondicao = new tDataTable({
            table: {
                jsItem: "jsItens",
                name: "tblCondicao",
                remove: true,
                order: [[1, "asc"]],
                columns: [
                    { data: "nrParcela" },
                    { data: "qtDias" },
                    { data: "txPercentual" },
                    { data: "nomeFormaPagamento" },
                ]
            },
        });

        $(document).on('tblCondicaoAfterDelete', function (e, data) {
            self.AtualizaTaxa(dtCondicao);
        })

    }

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

        if (IsNullOrEmpty($("#FormaPagamento_id").val())) {
            $("#FormaPagamento_id").blink({ msg: "Informe a condição de pagamento" });
            valid = false;
        }

        return valid;
    }

    self.getModel = function () {
        var model = {
            codFormaPagamento: $("#FormaPagamento_id").val(),
            nomeFormaPagamento: $("#FormaPagamento_text").val(),
            qtDias: $("#qtDias").val(),
            txPercentual: $("#txPercentual").val()
        }
        return model;

    }

    self.clear = function () {
        $("#FormaPagamento_id").val('');
        $("#FormaPagamento_text").val('');
        $("#qtDias").val('');
        $("#txPercentual").val('');
    }

    self.addItem = function () {
        if (self.valid()) {
            var model = self.getModel();
            let item = {
                nrParcela: nr,
                codFormaPagamento: model.codFormaPagamento,
                nomeFormaPagamento: model.nomeFormaPagamento,
                qtDias: model.qtDias,
                txPercentual: model.txPercentual,
            }
            nr++;
            dtCondicao.addItem(item)
            self.clear();
            self.AtualizaTaxa(dtCondicao);


        }
    }

    self.AtualizaTaxa = function (data) {
        let taxaTotal = 0;
        let dt = data.data;
        for (var i = 0; i < dt.length; i++) {
            //console.log(dt[i].txPercentual);
            //console.log(parseFloat(dt[i].txPercentual))
            taxaTotal += parseFloat(dt[i].txPercentual);
        }
        $("#txPercentualTotal").val(taxaTotal);
    }


}
