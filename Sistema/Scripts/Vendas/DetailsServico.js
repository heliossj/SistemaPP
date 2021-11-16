$(function () {
    var venda = new Venda();
    venda.init();
});

Venda = function () {
    self = this;
    dtProdutos = null;
    dtServicos = null;
    dtParcelas = null;

    this.init = function () {

        dtServicos = new tDataTable({
            table: {
                jsItem: "jsServicos",
                name: "tblServicos",
                remove: false,
                edit: false,
                order: [[1, "asc"]],
                columns: [
                    { data: "nomeExecutante" },
                    { data: "nomeServico" },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.qtServico.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.vlServico.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            let total = data.qtServico * data.vlServico;
                            total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                            return total;
                        }
                    },
                ]
            },
        });
        self.calcTotalServico();

        dtParcelas = new tDataTable({
            table: {
                jsItem: "jsParcelas",
                name: "tblParcelas",
                order: [[0, "asc"]],
                columns: [
                    { data: "nrParcela" },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.vlParcela.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            var dtVencimento = data.dtVencimento;
                            dtVencimento = JSONDate(dtVencimento);
                            return dtVencimento;
                        }
                    },
                    { data: "nmFormaPagamento" },
                ]
            },
        });
    }

    //Serviço
    
    self.calcTotalServico = function () {
        let total = 0;
        if (dtServicos.length && dtServicos.length > 0) {
            for (var i = 0; i < dtServicos.length; i++) {
                let totalServico = dtServicos.data[i].vlServico * dtServicos.data[i].qtServico;
                total += totalServico;
            }
            $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
            $("#CondicaoPagamento_btn-localizar").show();
        } else {
            $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
            $("#CondicaoPagamento_btn-localizar").hide();
        }
        vlTotalServicos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#fts").text("Total: " + total);
        self.calcTotal();
    }

    self.calcTotal = function () {
        vlTotalOS = vlTotalServicos;
        vlTotalOS = vlTotalOS.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });


        $("#vlTotal").val(vlTotalOS)
    }

    //self.datatable.atualizarItens();
    //self.datatable.atualizarGrid();

}
