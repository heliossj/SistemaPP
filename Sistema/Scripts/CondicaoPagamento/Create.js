$(function () {
    var cond = new CondicaoPagamento;
    cond.init();
    $("#addCondPagamento").click(function () {
        cond.addItem();
    });

    $("#removeAll").click(function (e) {
        cond.removerTudo();
        e.preventDefault();
    });

    $(document).on("tblCondicaoOpenEdit", cond.openEdit);
    $(document).on("tblCondicaoCancelEdit", cond.clear);

});


CondicaoPagamento = function () {
    self = this;
    //let nr = 1;
    var dtCondicao = null;
    var nrParcelaAux = 0;
    this.init = function () {
        dtCondicao = new tDataTable({
            table: {
                jsItem: "jsItens",
                name: "tblCondicao",
                remove: false,
                edit: true,
                order: [[1, "asc"]],
                columns: [
                    { data: "nrParcela" },
                    { data: "qtDias" },
                    {
                        data: null,
                        mRender: function (data) {
                            let result = "";
                            if (data) {
                                result = data.txPercentual.toFixed(2).replace(".", ",");
                            }
                            return result;
                        }
                    },

                    { data: "nomeFormaPagamento" },
                ]
            },
        });

        self.AtualizaTaxa(dtCondicao);
    }

    self.valid = function () {
        let valid = true;

        if (IsNullOrEmpty($("#qtDias").val())) {
            $("#qtDias").blink({ msg: "Informe a quantidade de dias" });
            valid = false;
        }

        if ($("#qtDias").val() == 0 || $("#qtDias").val() == "") {
            $("#qtDias").blink({ msg: "Informe uma quantidade de dias válido" });
            valid = false;
        }

        //dias parcelas
        let maior = 0;
        if (dtCondicao.data != null && dtCondicao.data.length) {
            for (var i = 0; i < dtCondicao.data.length; i++) {
                //console.log(dtCondicao.data[i].qtDias)
                if (dtCondicao.data[i].qtDias > maior) {
                    maior = dtCondicao.data[i].qtDias;
                }
            }
            //console.log(maior);
            //console.log(dtCondicao.dataSelected)
            if ($("#qtDias").val() <= maior) {//&& dtCondicao.dataSelected.item.nrParcela
                $("#qtDias").blink({ msg: "Não é permitido adicionar uma parcela menor ou igual, verifique!" });
                valid = false;
            }
        }

        if (IsNullOrEmpty($("#txPercentual").val())) {
            $("#txPercentual").blink({ msg: "Informe o percentual" });
            valid = false;
        }

        if (IsNullOrEmpty($("#FormaPagamento_id").val())) {
            $("#FormaPagamento_id").blink({ msg: "Informe a condição de pagamento" });
            valid = false;
        }

        //taxa total
        let txTotal = $("#txPercentualTotal").val();
        let txPercentual = $("#txPercentual").val();

        if (!IsNullOrEmpty(txPercentual)) {
            if (!IsNullOrEmpty(txTotal)) {
                txTotal = parseFloat(txTotal);
            }
            txPercentual = txPercentual.replace(",", ".");
            txPercentual = parseFloat(txPercentual);
            txTotal = parseFloat(txTotal);

            let total = 0;

            //console.log(dtCondicao.data)
            if (dtCondicao.isEdit) {
                total = txTotal - dtCondicao.dataSelected.item.txPercentual + txPercentual;
                if (total > 100) {
                    $("#txPercentualTotal").blink({ msg: "O valor total deve ser equivalente a 100%, verifique!" });
                    valid = false;
                } else {
                    if (valid == "true") {
                        $("#txPercentualTotal").val(total);
                        $("#txPercentualTotalAux").val(total);
                    }
                }
            } else {
                total = txTotal + txPercentual;
                if (total > 100) {
                    $("#txPercentualTotal").blink({ msg: "O valor total deve ser equivalente a 100%, verifique!" });
                    valid = false;
                } else {
                    if (valid == "true") {
                        $("#txPercentualTotal").val(total);
                        $("#txPercentualTotalAux").val(total);
                    }
                }
            }
        }

        return valid;
    }

    self.getModel = function () {
        let taxa = $("#txPercentual").val().replace(",", ".");
        taxa = parseFloat(taxa)
        var model = {
            codFormaPagamento: $("#FormaPagamento_id").val(),
            nomeFormaPagamento: $("#FormaPagamento_text").val(),
            qtDias: $("#qtDias").val(),
            txPercentual: taxa,
        }
        return model;

    }

    self.clear = function () {
        $("#FormaPagamento_id").val('');
        $("#FormaPagamento_text").val('');
        $("#qtDias").val('');
        $("#txPercentual").val('');
        //$('input[name="qtDias"]').prop('disabled', false)
    }

    self.addItem = function () {
        if (self.valid()) {
            var model = self.getModel();
            let item = {
                nrParcela: dtCondicao.isEdit ? dtCondicao.dataSelected.item.nrParcela : dtCondicao.length + 1, //nr
                codFormaPagamento: model.codFormaPagamento,
                nomeFormaPagamento: model.nomeFormaPagamento,
                qtDias: parseFloat(model.qtDias),
                txPercentual: model.txPercentual,
            }
            //nr++;
            self.save(item)
            //dtCondicao.addItem(item)

            self.clear();
            self.AtualizaTaxa(dtCondicao);
        }
    }

    self.AtualizaTaxa = function (data) {

        let taxaTotal = 0;
        let dt = data.data;
        let aux = "";
        for (var i = 0; i < dt.length; i++) {
            if (typeof (dt[i].txPercentual) == "string") {
                aux = dt[i].txPercentual.replace(",", ".");
                aux = parseFloat(aux);
            } else {
                aux = dt[i].txPercentual;
            }
            taxaTotal += aux;
        }
        $("#txPercentualTotal").val(taxaTotal.toFixed(2).replace(".", ","));
        $("#txPercentualTotalAux").val(taxaTotal);
    }

    self.removerTudo = function () {
        if (dtCondicao.data == null || !dtCondicao.data.length) {
            $.notify({ message: "Não existem parcelas para serem removidas", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000, });
        } else {
            dtCondicao.data = null;
            $("#txPercentualTotal").val(0);
            $("#txPercentualTotalAux").val(0);
        }
    }

    self.openEdit = function (e, data) {
        let item = dtCondicao.dataSelected.item;
        $("#FormaPagamento_id").val(item.codFormaPagamento);
        $("#FormaPagamento_text").val(item.nomeFormaPagamento);
        $("#qtDias").val(item.qtDias);
        $("#txPercentual").val(item.txPercentual.toFixed(2).replace(".", ","));
        //$('input[name="qtDias"]').prop('disabled', true)
    };

    self.save = function (data) {
        if (dtCondicao.isEdit) {
            dtCondicao.editItem(data);
        } else {
            dtCondicao.addItem(data);
        }
    };


}
