$(function () {
    vlTotalVenda = 0;
    var venda = new Venda();
    venda.init();

    $("#addProduto").click(function () {
        venda.addProduto();
    });

    $(document).on("tblProdutoAfterDelete", function () {
        venda.calcTotalProduto();
        venda.clearProduto();
    });

    $(document).on("tblProdutoOpenEdit", venda.openEditProduto);
    $(document).on("tblProdutoCancelEdit", venda.clearProduto);

    $("#btnSalvar").attr("disabled", true);
    $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
    $("#CondicaoPagamento_btn-localizar").hide();
    //$("#divAddProduto").hide();

    $("#CondicaoPagamento_id").change(function () {
        dtParcelas.clear();
        let idCondicao = $("#CondicaoPagamento_id").val()
        if (IsNullOrEmpty(idCondicao)) {
            $("#divAddProduto").show();
            $('input[name="dtEmissao"]').prop('disabled', false);
            $("#flTblProdutos").val("");
            $("#btnSalvar").attr("disabled", true);
        } else {
            $("#divAddProduto").hide();
        }
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    })

    $(document).on('AfterLoad_CondicaoPagamento', function (e, data) {
        $("#flTblProdutos").val("S")
        $("#divAddProduto").hide();
        console.log("CarregaCondicao");
        dtParcelas.clear();
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    });

    $("#Produto_qtProduto").change(function () {
        let vlVenda = $("#Produto_vlVenda").val()
        if (!IsNullOrEmpty(vlVenda)) {
            vlVenda = vlVenda.replace(".", "").replace(",", ".");
            vlVenda = parseFloat(vlVenda);
            console.log(vlVenda)
            venda.calcTotalItem(vlVenda);
        } else {
            $("#Produto_vlTotal").val("")
        }
    })

    $("#Produto_txDesconto").change(function () {
        let vlVenda = $("#Produto_vlVenda").val()
        if (!IsNullOrEmpty(vlVenda)) {
            vlVenda = vlVenda.replace(".", "").replace(",", ".");
            vlVenda = parseFloat(vlVenda);
            console.log(vlVenda)
            venda.calcTotalItem(vlVenda);
        } else {
            $("#Produto_vlTotal").val("")
        }
    })

    $(document).on('AfterLoad_Produto', function (e, data) {
        venda.calcTotalItem(data.vlVenda);
    })

    //calcTotalItem

    //if (!$("#flFinalizar").is(":checked")) {
    //    $("#divFinaliza").slideUp();
    //    $("#vlTotal").val("");
    //} else {
    //    $("#divFinaliza").slideDown();
    //}

    //$("#flFinalizar").click(function () {
    //    if (!dtProdutos.length) {
    //        $.notify({ message: "Informe ao menos um produto para finalizar", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
    //        $("#flFinalizar").prop("checked", false)
    //    } else {
    //        if ($(this).is(":checked")) {
    //            $('input[name="dtEmissao"]').prop('disabled', true)
    //            venda.calcTotalProduto();
    //            $("#divParcelas").hide();
    //            $("#divFinaliza").slideDown();
    //            let total = vlTotalVenda;
    //            let totalFormat = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
    //            $("#vlTotal").val(totalFormat);
    //            dtProdutos.atualizarGrid();
    //            $("#divAddProduto").slideUp();
    //        } else {
    //            $("#divFinaliza").slideUp();

    //            dtProdutos.atualizarGrid();
    //            dtParcelas.clear();
    //            $("#divAddProduto").slideDown();
    //            $("#CondicaoPagamento_id").val("")
    //            $("#CondicaoPagamento_text").val("")
    //            $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true);
    //            $('input[name="dtEmissao"]').prop('disabled', false)
    //        }
    //    }
    //});

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        venda.getparcelas();
    });

    $(document).on('tblProdutoRowCallback', function (e, data) {
        let flTblProdutos = $("#flTblProdutos").val()
        console.log(flTblProdutos)
        if (flTblProdutos == "S") {
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

    if (dtProdutos.length > 0) {
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
    }

    //load
    let idCond = $("#CondicaoPagamento_id").val()
    if (!IsNullOrEmpty(idCond)) {
        $("#flTblProdutos").val("S");
        $('input[name="dtEmissao"]').prop('disabled', true)
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
        $("#CondicaoPagamento_btnGerarParcela").attr('disabled', false)

        let dtString = $("#dtEmissao").val();
        let dayArray = dtString.split("/");
        let day = dayArray[0];
        let month = (parseFloat(dayArray[1]) - 1);
        let year = dayArray[2];
        date = new Date(year, month, day).toJSON();
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    }
});

Venda = function () {
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
                            return data.vlVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
                            let result = "";
                            if (data.unidade == "M")
                                result = "METRO";
                            return result;
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

        let qtProdutoAux = $("#Produto_qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);

        let txDesconto = $("#Produto_txDesconto").val().replace(".", "").replace(",", ".");
        let txDescontoAux = 0;

        if (!IsNullOrEmpty(txDesconto)) {
            txDescontoAux += parseFloat(txDesconto);
        }
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
            unidade: $("#Produto_unidade").val(),
            vlVenda: vlVendaProdutoAux,
            qtProduto: qtProdutoAux,
            vlTotal: vlVendaProdutoAux * qtProdutoAux,
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


        else if (IsNullOrEmpty($("#Produto_qtProduto").val()) || $("#Produto_qtProduto").val() == "" || $("#Produto_qtProduto").val() == 0) {
            $("#Produto_qtProduto").blink({ msg: "Informe a quantidade" });
            $("#Produto_qtProduto").focus();
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
        $("#Produto_unidade").val("M");
        $("#Produto_qtProduto").val("");
        $("#Produto_txDesconto").val("");
        $("#Produto_vlTotal").val("");
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
                txDesconto: model.txDesconto,
                vlTotal: model.vlTotal
            }
            console.log(item)
            self.saveProduto(item);
            self.clearProduto();
            self.calcTotalProduto();
        }
    }

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

    self.openEditProduto = function (e, data) {
        let item = dtProdutos.dataSelected.item;
        $("#Produto_id").val(item.codProduto);
        $("#Produto_text").val(item.nomeProduto);
        $("#Produto_vlVenda").val(item.vlVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_unidade").val(item.unidade);
        $("#Produto_qtProduto").val(item.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_txDesconto").val(item.txDesconto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('input[name="Produto.id"]').prop('disabled', true)
        self.calcTotalItem(item.vlVenda);
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
            let totalF = vlTotalVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamento_id").val(), vlTotal: totalF },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelas(data);
                    $("#btnSalvar").attr("disabled", false);
                },
                error: function (request) {
                    alert("Erro ao buscar registro");
                }
            });
        } else {
            $.notify({ message: "Já foram geradas parcelas para esta Venda, verifique!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
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

    self.calcTotalItem = function (vlVenda) {
        
        let qtProduto = $("#Produto_qtProduto").val();
        let txDesconto = $("#Produto_txDesconto").val()

        if (!IsNullOrEmpty(qtProduto)) {
            qtProduto = qtProduto.replace(".", "").replace(",", ".");
            qtProduto = parseFloat(qtProduto)
            let total = 0;

            //qtProdutoAux = parseFloat(qtProdutoAux);
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
