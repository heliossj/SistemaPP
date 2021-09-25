$(function () {

    var compra = new Compra();
    compra.init();

    $("#divAddProduto").hide()
});


Compra = function () {
    self = this;
    dtProdutos = null;
    dtParcelas = null;

    this.init = function () {
        dtProdutos = new tDataTable({
            table: {
                jsItem: "jsProdutos",
                name: "tblProduto",
                order: [[0, "asc"]],
                columns: [
                    { data: "codProduto" },
                    { data: "nomeProduto" },
                    {
                        data: null,
                        mRender: function (data) {
                            let resut = "";
                            if (data.unidade == "M")
                                result = "METRO";

                            return result;
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
                            let vlCompraDesconto = data.vlCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                            return vlCompraDesconto;
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.txDesconto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            let vlTotalCompra = (data.txDesconto * data.vlCompra) / 100;
                            vlTotalCompra = (data.vlCompra - vlTotalCompra) * data.qtProduto;
                            return vlTotalCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
                    {
                        data: null,
                        mRender: function (data) {
                            return JSONDate(data.dtVencimento);
                        }
                    },
                    { data: "nmFormaPagamento" },
                    { data: "situacao"}
                ]
            },
        });
    }

    self.calcTotalProduto = function () {
        let total = 0;
        if (dtProdutos.length && dtProdutos.length > 0) {
            for (var i = 0; i < dtProdutos.length; i++) {
                let vlCompraDesconto = (dtProdutos.data[i].vlCompra * dtProdutos.data[i].txDesconto) / 100;
                vlCompraDesconto = dtProdutos.data[i].vlCompra - vlCompraDesconto;
                let totalProduto = vlCompraDesconto * dtProdutos.data[i].qtProduto;
                total += totalProduto;
            }
        }
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        $("#vlTotal").val(total);
    }



}