$(function () {
    var cond = new CondicaoPagamento;
    cond.init();
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
                edit: false,
                order: [[1, "asc"]],
                columns: [
                    { data: "nrParcela" },
                    { data: "qtDias" },
                    {
                        data: null,
                        mRender: function (data) {
                            let k = "";
                            if (data) {
                                k = data.txPercentual.toFixed(2).replace(".", ",");
                            }
                            return k;
                            //return data;
                        }
                    },

                    { data: "nomeFormaPagamento" },
                ]
            },
        });
        self.AtualizaTaxa(dtCondicao);
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

}
