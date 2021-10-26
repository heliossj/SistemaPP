$(function () {
    vlTotalServicos = 0;
    vlTotalProdutos = 0;
    vlTotalOS = 0;

    var OS = new OrdemServico();
    OS.init();

    $("#divAddServico").hide();
    $("#divAddProduto").hide();
});


OrdemServico = function () {
    self = this;
    dtServicos = null;
    dtProdutos = null;
    dtParcelas = null;

    this.init = function () {
        dtServicos = new tDataTable({
            table: {
                jsItem: "jsServicos",
                name: "tblServico",
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

        dtProdutos = new tDataTable({
            table: {
                jsItem: "jsProdutos",
                name: "tblProduto",
                remove: false,
                edit: false,
                order: [[1, "asc"]],
                columns: [
                    { data: "codProduto" },
                    { data: "nomeProduto" },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.unidade == "M" ? "METRO" : "UNIDADE"
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.vlProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            let total = data.vlProduto * data.qtProduto;
                            return total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                ]
            },
        });
        self.calcTotalProduto();

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
                    { data: "dtVencimento" },
                    { data: "nmFormaPagamento" },
                ]
            },
        });
    }

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

    self.calcTotalProduto = function () {
        let total = 0;
        if (dtProdutos.length && dtProdutos.length > 0) {
            for (var i = 0; i < dtProdutos.length; i++) {
                let totalProduto = dtProdutos.data[i].vlProduto * dtProdutos.data[i].qtProduto;
                total += totalProduto;
            }
        }
        vlTotalProdutos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        self.calcTotal();
    }

    self.calcTotal = function () {
        vlTotalOS = vlTotalServicos + vlTotalProdutos;
        vlTotalOS = vlTotalOS.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });


        $("#vlTotal").val(vlTotalOS)
    }
}

