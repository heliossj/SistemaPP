$(function () {
    vlTotalVenda = 0;
    var venda = new Venda();
    venda.init();

    $("#divAddProduto").hide();
});

Venda = function () {
    self = this;
    dtProdutos = null;

    this.init = function () {

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
                            let result = "";
                            if (data.unidade == "M")
                                result = "METRO";
                            if (data.unidade == "U")
                                result = "UNIDADE"
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
                            return data.vlVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
                            let vlTotalVenda = (data.txDesconto * data.vlVenda) / 100;
                            vlTotalVenda = (data.vlVenda - vlTotalVenda) * data.qtProduto;
                            return vlTotalVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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

    //Produto
    
    self.calcTotalProduto = function () {
        let total = 0;
        if (dtProdutos.length && dtProdutos.length > 0) {
            for (var i = 0; i < dtProdutos.length; i++) {
                let vlVendaDesconto = (dtProdutos.data[i].vlVenda * dtProdutos.data[i].txDesconto) / 100;
                vlVendaDesconto = dtProdutos.data[i].vlVenda - vlVendaDesconto;
                let totalProduto = vlVendaDesconto * dtProdutos.data[i].qtProduto;
                total += totalProduto;
            }
            $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
            $("#CondicaoPagamento_btn-localizar").show();
        } else {
            $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
            $("#CondicaoPagamento_btn-localizar").hide();
            $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true)
        }
        vlTotalVenda = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        $("#vlTotal").val(total)
    }

    self.calcTotalItem = function (vlVenda) {

        let qtProduto = $("#Produto_qtProduto").val();
        let txDesconto = $("#Produto_txDesconto").val()

        if (!IsNullOrEmpty(qtProduto)) {
            qtProduto = qtProduto.replace(".", "").replace(",", ".");
            qtProduto = parseFloat(qtProduto)
            let total = 0;
            if (!IsNullOrEmpty(txDesconto)) {
                txDesconto = txDesconto.replace(".", "").replace(",", ".");
                let desc = parseFloat(txDesconto);
                let txDesc = (desc * vlVenda) / 100
                vlVenda = vlVenda - txDesc;
            }
            total = vlVenda * qtProduto;
            $("#Produto_vlTotal").val(total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }))
        } else {
            $("#Produto_vlTotal").val("");
        }
    }

    //self.datatable.atualizarItens();
    //self.datatable.atualizarGrid();

}
