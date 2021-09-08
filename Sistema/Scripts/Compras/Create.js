$(function () {
    vlTotalCompra = 0;
    date = null;
    var compra = new Compra();
    compra.init();

    $("#addProduto").click(function () {
        compra.addProduto();
    });

    $(document).on("tblProdutoAfterDelete", function () {
        compra.calcTotalProduto();
        compra.clearProduto();
    });

    $(document).on("tblProdutoOpenEdit", compra.openEditProduto);
    $(document).on("tblProdutoCancelEdit", compra.clearProduto);


    if (!$("#flFinalizar").is(":checked")) {
        $("#divFinaliza").slideUp();
        $("#vlTotal").val("");
    } else {
        $("#divFinaliza").slideDown();
    }

    $("#flFinalizar").click(function () {
        if (!dtProdutos.length || IsNullOrEmpty($("#dtEmissao").val())) {
            let msg = "";
            if (!dtProdutos.length) {
                msg += "- Informe ao menos um produto para finalizar<br/>";
            }
            if (IsNullOrEmpty($("#dtEmissao").val())){
                msg += "- Informe a data de emissão"
            }
            $.notify({ message: msg , icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
            $("#flFinalizar").prop("checked", false)
        } else {
            if ($(this).is(":checked")) {
                $('input[name="dtEmissao"]').prop('disabled', true)
                compra.calcTotalProduto();
                $("#divParcelas").hide();
                $("#divFinaliza").slideDown();
                let total = vlTotalCompra;
                let totalFormat = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                $("#vlTotal").val(totalFormat);
                dtProdutos.atualizarGrid();
                $("#divAddProduto").slideUp();
            } else {
                $("#divFinaliza").slideUp();

                dtProdutos.atualizarGrid();
                dtParcelas.clear();
                $("#divAddProduto").slideDown();
                $("#CondicaoPagamento_id").val("")
                $("#CondicaoPagamento_text").val("")
                $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true);
                $('input[name="dtEmissao"]').prop('disabled', false)
            }
        }
    });

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        compra.getparcelas();
    });

    $(document).on('tblProdutoRowCallback', function (e, data) {
        if ($("#flFinalizar").is(":checked")) {
            let btn = $('td a[data-event=remove]', data.nRow);
            btn.attr('title', "Indisponível para alteração!");
            btn.attr('data-event', false);
            btn.removeClass().addClass("btn btn-secondary btn-sm");
            btn.find("i").removeClass().addClass("fa fa-info");
            btn.on('click', function (e) {
                e.preventDefault();
            })

            let btnEdit = $('td a[data-event=edit]', data.nRow);
            btnEdit.attr('title', "Indisponível para alteração!");
            btnEdit.attr('data-event', false);
            btnEdit.removeClass().addClass("btn btn-secondary btn-sm").css("width", "29px");
            btnEdit.find("i").removeClass().addClass("fa fa-info");
            btnEdit.click(function (e) {
                e.preventDefault();
            });
        }
        return false;
    });
    let dtAux = $("#dtEmissao")
    dtAux.change(function () {
        let dtString = dtAux.val();
        let dayArray = dtString.split("/");
        let day = dayArray[0];
        let month = (parseFloat(dayArray[1]) - 1);
        let year = dayArray[2];
        let dtTeste = new Date(year, month, day)
        date = new Date(year, month, day).toJSON();
        console.log(date)
    });
});

Compra = function () {
    self = this;
    dtProdutos = null;

    this.init = function () {

        dtProdutos = new tDataTable({
            table: {
                jsItem: "jsProdutos",
                name: "tblProduto",
                remove: true,
                edit: true,
                order: [[1, "asc"]],
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
                            let vlCompraDesconto = (data.txDesconto * data.vlCompra) / 100;
                            vlCompraDesconto = data.vlCompra - vlCompraDesconto;
                            return vlCompraDesconto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
                    { data: "dtVencimento" },
                    { data: "nmFormaPagamento" },
                ]
            },
        });
    }

    //Produto
    self.getModelProduto = function () {
        let vlVendaProduto = $("#Produto_vlVenda").val().replace(".", "").replace(",", ".");
        let vlVendaProdutoAux = parseFloat(vlVendaProduto);

        let vlCompraProduto = $("#vlCompra").val().replace(".", "").replace(",", ".");
        let vlCompraProdutoAux = parseFloat(vlCompraProduto);

        let qtProdutoAux = $("#qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);


        let txDesconto = $("#txDesconto").val().replace(".", "").replace(",", ".");
        let txDescontoAux = 0;

        if (!IsNullOrEmpty(txDesconto)) {
            txDescontoAux += parseFloat(txDesconto);
            //let calcDesc = (txDescontoAux * vlCompraProdutoAux) / 100;
            //vlCompraProdutoAux = vlCompraProdutoAux - calcDesc;
        }
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
            unidade: $("#unidade").val(),
            vlVenda: vlVendaProdutoAux,
            vlCompra: vlCompraProdutoAux,
            qtProduto: qtProdutoAux,
            vlTotal: vlCompraProdutoAux * qtProdutoAux,
            txDesconto: txDescontoAux,
        };
        return model;
    }

    self.validProduto = function () {
        let valid = true;

        if (IsNullOrEmpty($("#Produto_id").val()) || $("#Produto_id").val() == "") {
            $("#Produto_id").blink({ msg: "Informe o produto" });
            $("#Produto_id").focus();
            valid = false;
        }


        else if (IsNullOrEmpty($("#qtProduto").val()) || $("#qtProduto").val() == "" || $("#qtProduto").val() == 0) {
            $("#qtProduto").blink({ msg: "Informe a quantidade" });
            $("#qtProduto").focus();
            valid = false;
        }

        else if (IsNullOrEmpty($("#vlCompra").val()) || $("#vlCompra").val() == 0) {
            $("#vlCompra").blink({ msg: "Informe o valor de compra" });
            $("#vlCompra").focus();
            valid = false;
        }

        if (!dtProdutos.isEdit) {
            if (dtProdutos.exists("codProduto", $("#Produto_id").val())) {
                $("#Produto_id").blink({ msg: "Produto já informado, verifique!" });
                valid = false;
            }
        }

        return valid;
    }

    self.clearProduto = function () {
        $("#Produto_id").val("");
        $("#Produto_text").val("");
        $("#Produto_vlVenda").val("");
        $("#unidade").val("M");
        $("#qtProduto").val("");
        $("#vlCompra").val("");
        $("#txDesconto").val("");
        $("#Produto_vlVenda").val("");
        $('input[name="Produto.id"]').prop('disabled', false)
    }

    self.addProduto = function () {
        if (self.validProduto()) {
            let model = self.getModelProduto();
            let item = {
                codProduto: model.codProduto,
                nomeProduto: model.nomeProduto,
                unidade: model.unidade,
                qtProduto: model.qtProduto,
                vlVenda: model.vlVenda,
                vlCompra: model.vlCompra,
                txDesconto: model.txDesconto,
                vlTotal: model.vlTotal
            }
            self.saveProduto(item);
            self.clearProduto();
            self.calcTotalProduto();
        }
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
        vlTotalCompra = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
    }

    self.openEditProduto = function (e, data) {
        let item = dtProdutos.dataSelected.item;
        $("#Produto_id").val(item.codProduto);
        $("#Produto_text").val(item.nomeProduto);
        $("#Produto_vlVenda").val(item.vlVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#unidade").val(item.unidade);
        $("#qtProduto").val(item.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#vlCompra").val(item.vlCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#txDesconto").val(item.txDesconto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('input[name="Produto.id"]').prop('disabled', true)
    }

    self.saveProduto = function (data) {
        if (dtProdutos.isEdit) {
            dtProdutos.editItem(data);
        } else {
            dtProdutos.addItem(data)
        }
    }

    self.getparcelas = function (dtInicio) {
        if (!dtParcelas.length) {
            let totalF = vlTotalCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamento_id").val(), vlTotal: totalF, dtIiniParcela: date },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelas(data);
                },
                error: function (request) {
                    alert("Erro ao buscar registro");
                }
            });
        } else {
            $.notify({ message: "Já foram geradas parcelas para esta Compra, verifique!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
        }
    }

    self.setParcelas = function (data) {
        let itens = data.parcelas;
        for (var i = 0; i < itens.length; i++) {
            let dtParcela = JSONDate(itens[i].dtVencimento,)
            let item = {
                idFormaPagamento: itens[i].idFormaPagamento,
                nmFormaPagamento: itens[i].nmFormaPagamento,
                flSituacao: itens[i].flSituacao,
                dtVencimento: dtParcela,
                vlParcela: itens[i].vlParcela,
                nrParcela: itens[i].nrParcela
            }
            dtParcelas.addItem(item);
        }
        $("#divParcelas").slideDown();
    }





}
