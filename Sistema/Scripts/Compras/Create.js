$(function () {
    vlTotalCompra = 0;
    date = null;
    dateEntrega = null;
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

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        compra.getparcelas();
        return false;
    });

    $(document).on('tblProdutoRowCallback', function (e, data) {
        let flTblProdutos = $("#flTblProdutos").val()
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

    let dtAux = $("#dtEmissao")
    dtAux.change(function () {
        let dtString = dtAux.val();
        let dayArray = dtString.split("/");
        let day = dayArray[0];
        let month = (parseFloat(dayArray[1]) - 1);
        let year = dayArray[2];
        date = new Date(year, month, day).toJSON();
        $("#dtEmissaoAux").val(dtAux.val())
    });

    let dtEnt = $("#dtEntrega")
    dtEnt.change(function () {
        let dtString = dtEnt.val();
        let dayArray = dtString.split("/");
        let day = dayArray[0];
        let month = (parseFloat(dayArray[1]) - 1);
        let year = dayArray[2];
        dateEntrega = new Date(year, month, day).toJSON();
        $("#dtEntregaAux").val(dtEnt.val())
    });

    $("#btnSalvar").attr("disabled", true);
    $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
    $("#CondicaoPagamento_btn-localizar").hide();
    $("#divAddProduto").hide();

    let modelo = $("#modelo")
    modelo.change(function () {
        let id = $("#Fornecedor_id").val();
        compra.verificaNF(id);
        $("#modeloAux").val(modelo.val())
    })

    let serie = $("#serie")
    serie.change(function () {
        let id = $("#Fornecedor_id").val();
        compra.verificaNF(id);
        $("#serieAux").val(serie.val())
    })

    let numero = $("#nrNota")
    numero.change(function () {
        let id = $("#Fornecedor_id").val();
        compra.verificaNF(id);
        $("#nrNotaAux").val(numero.val())
    })

    $(document).on('AfterLoad_Fornecedor', function (e, data) {
        compra.verificaNF(data.id);
    });

    $("#Fornecedor_id").change(function () {
        if (IsNullOrEmpty($(this).val())) {
            $("#divAddProduto").hide();
        }
    })

    $("#CondicaoPagamento_id").change(function () {
        dtParcelas.clear();
        let idCondicao = $("#CondicaoPagamento_id").val()
        if (IsNullOrEmpty(idCondicao)) {
            $("#divAddProduto").show();
            $('input[name="dtEmissao"]').prop('disabled', false);
            $('input[name="dtEntrega"]').prop('disabled', false)
            $("#flTblProdutos").val("")
        } else {
            $("#divAddProduto").hide();
        }
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    })

    $(document).on('AfterLoad_CondicaoPagamento', function (e, data) {
        $("#flTblProdutos").val("S")
        $("#divAddProduto").hide();
        dtParcelas.clear();
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    });
   
    $("#Produto_qtProduto").change(function () {
        compra.calcTotalItem();
    })

    $("#Produto_vlCompra").change(function () {
        compra.calcTotalItem();
    })

    $("#Produto_txDesconto").change(function () {
        compra.calcTotalItem();
    })

    //load
    let idCond = $("#CondicaoPagamento_id").val()
    if (!IsNullOrEmpty(idCond)) {
        $("#flTblProdutos").val("S");
        $('input[name="dtEmissao"]').prop('disabled', true)
        $('input[name="dtEntrega"]').prop('disabled', true)
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

    let idFor = $("#Fornecedor_id").val()
    if (!IsNullOrEmpty(idFor)) {
        $("#Fornecedor_btn-localizar").hide();
    }

    if (dtProdutos.length > 0) {
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
    }

    if (!IsNullOrEmpty(idCond) && dtParcelas.length > 0) {
        $("#btnSalvar").attr("disabled", false);
    }
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
                            if (data.unidade == "U")
                                result = "UNIDADE";

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
                            let vlCompra = data.vlCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                            return vlCompra;
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
                ]
            },
        });

        if (dtParcelas.length > 0) {
            $("#flFinalizar").prop("checked", true)
            let total = vlTotalCompra;
            let totalFormat = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $("#vlTotal").val(totalFormat);
        }
    }

    self.getModelProduto = function () {
        let vlVendaProduto = $("#Produto_vlVenda").val().replace(".", "").replace(",", ".");
        let vlVendaProdutoAux = parseFloat(vlVendaProduto);

        let vlCompraProduto = $("#Produto_vlCompra").val().replace(".", "").replace(",", ".");
        let vlCompraProdutoAux = parseFloat(vlCompraProduto);

        let qtProdutoAux = $("#Produto_qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);

        let txDesconto = $("#Produto_txDesconto").val().replace(".", "").replace(",", ".");
        let txDescontoAux = 0;

        let vlTotalAux = 0;

        if (!IsNullOrEmpty(txDesconto)) {
            txDescontoAux += parseFloat(txDesconto);
            let calcDesc = (txDescontoAux * vlCompraProdutoAux) / 100;
            vlTotalAux = vlCompraProdutoAux - calcDesc;
        } else {
            vlTotalAux = vlCompraProdutoAux;
        }
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
            unidade: $("#Produto_unidade").val(),
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

        else if (IsNullOrEmpty($("#Produto_qtProduto").val()) || $("#Produto_qtProduto").val() == "" || $("#Produto_qtProduto").val() == 0) {
            $("#Produto_qtProduto").blink({ msg: "Informe a quantidade" });
            $("#Produto_qtProduto").focus();
            valid = false;
        }

        else if (IsNullOrEmpty($("#Produto_vlCompra").val()) || $("#Produto_vlCompra").val() == 0) {
            $("#Produto_vlCompra").blink({ msg: "Informe o valor de compra" });
            $("#Produto_vlCompra").focus();
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
        $("#Produto_unidade").val("");
        $("#Produto_qtProduto").val("");
        $("#Produto_vlCompra").val("");
        $("#Produto_txDesconto").val("");
        $("#Produto_vlVenda").val("");
        $("#Produto_vlTotal").val("")
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
            $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
            $("#CondicaoPagamento_btn-localizar").show();
            //desabilita campos nota fiscal
            $('input[name="modelo"]').prop('disabled', true)
            $('input[name="serie"]').prop('disabled', true)
            $('input[name="nrNota"]').prop('disabled', true)
            $('input[name="Fornecedor.id"]').prop('disabled', true)
            $("#Fornecedor_btn").removeAttr('disabled', true)
            $("#Fornecedor_btn-localizar").hide();
        } else {
            $("#divAddProduto").show();
            //reaabilita campos nota fiscal
            $('input[name="modelo"]').prop('disabled', false)
            $('input[name="serie"]').prop('disabled', false)
            $('input[name="nrNota"]').prop('disabled', false)
            $('input[name="Fornecedor.id"]').prop('disabled', false)
            $("#Fornecedor_btn").removeAttr('disabled', false)
            
            $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
            $("#CondicaoPagamento_btn-localizar").hide();

            $("#CondicaoPagamento_id").val("")
            $("#CondicaoPagamento_text").val("")
            $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true)

            $("#Fornecedor_btn-localizar").show();
        }

        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        vlTotalCompra = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#vlTotal").val(vlTotalCompra)
    }

    self.openEditProduto = function (e, data) {
        let item = dtProdutos.dataSelected.item;
        $("#Produto_id").val(item.codProduto);
        $("#Produto_text").val(item.nomeProduto);
        $("#Produto_vlVenda").val(item.vlVenda.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_unidade").val(item.unidade);
        $("#Produto_qtProduto").val(item.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_vlCompra").val(item.vlCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_txDesconto").val(item.txDesconto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('input[name="Produto.id"]').prop('disabled', true)
        self.calcTotalItem();
    }

    self.saveProduto = function (data) {
        if (dtProdutos.isEdit) {
            dtProdutos.editItem(data);
        } else {
            dtProdutos.addItem(data)
        }
    }

    self.getparcelas = function () {
        let valid = true;
        if (IsNullOrEmpty(date)) {
            //$("#dtEmissao").blink({msg: "Informe a data de emissão"})
            $.notify({ message: "Informe a data de emissão!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
            valid = false;
        } else if (IsNullOrEmpty(dateEntrega)) {
            $.notify({ message: "Informe a data de entrega!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
            valid = false;
        } else if (dateEntrega < date) {
            $.notify({ message: "A data de entrega não pode ser menor que a data de Emissão!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
            valid = false;
        }
        if (!dtParcelas.length && valid ) {
            let totalF = vlTotalCompra.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamento_id").val(), vlTotal: totalF, dtIiniParcela: date },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelas(data);
                    $("#btnSalvar").attr("disabled", false);
                    $('input[name="dtEmissao"]').prop('disabled', true)
                    $('input[name="dtEntrega"]').prop('disabled', true)
                },
                error: function (request) {
                    alert("Erro ao buscar registro");
                }
            });
        }
        else if (dtParcelas.length) {
            $.notify({ message: "Já foram geradas parcelas para esta Compra, verifique!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
        }
    }

    self.setParcelas = function (data) {
        let itens = data.parcelas;
        for (var i = 0; i < itens.length; i++) {
            let item = {
                idFormaPagamento: itens[i].idFormaPagamento,
                nmFormaPagamento: itens[i].nmFormaPagamento,
                flSituacao: itens[i].flSituacao,
                dtVencimento: itens[i].dtVencimento,
                vlParcela: itens[i].vlParcela,
                nrParcela: itens[i].nrParcela
            }
            dtParcelas.addItem(item);
        }
        $("#divParcelas").slideDown();
    }

    self.verificaNF = function (id) {
        let modelo = $("#modelo");
        let serie = $("#serie");
        let numero = $("#nrNota");
        if (!IsNullOrEmpty(modelo.val()) && !IsNullOrEmpty(serie.val()) && !IsNullOrEmpty(numero.val()) && !IsNullOrEmpty(id)) {
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.verificaNF,
                data: { modelo: modelo.val(), serie: serie.val(), numero: numero.val(), codFornecedor: id },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: data.type, z_index: 2000 });
                    if (data.type == "success") {
                        //seta valor válido
                        let modelo = $("#modelo").val()
                        $("#modeloAux").val(modelo)
                        let serie = $("#serie").val()
                        $("#serieAux").val(serie)
                        let numero = $("#nrNota").val()
                        $("#nrNotaAux").val(numero)
                        let idFornecedor = $("#Fornecedor_id").val()
                        $("#idFornecedor").val(idFornecedor)
                        //habilita adicionar produto
                        $("#divAddProduto").slideDown();

                    } else {
                        $("#divAddProduto").hide();
                    }
                },
                error: function (request) {
                    alert("Erro ao buscar registro");
                }
            });
        } else {
            $("#divAddProduto").hide();
        }
    }

    self.calcTotalItem = function () {

        let vlCompraProduto = $("#Produto_vlCompra").val().replace(".", "").replace(",", ".");
        let vlCompraProdutoAux = parseFloat(vlCompraProduto);

        let qtProdutoAux = $("#Produto_qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);

        let txDesconto = $("#Produto_txDesconto").val().replace(".", "").replace(",", ".");
        let txDescontoAux = 0;

        if (!IsNullOrEmpty(vlCompraProduto) && !IsNullOrEmpty(qtProdutoAux)) {
            let total = 0;

            if (!IsNullOrEmpty(txDesconto)) {
                let desc = parseFloat(txDesconto);
                let txDesc = (desc * vlCompraProdutoAux) / 100
                vlCompraProdutoAux = vlCompraProdutoAux - txDesc;
            }
            total = vlCompraProdutoAux * qtProdutoAux;
            $("#Produto_vlTotal").val(total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }))
        } else {
            $("#Produto_vlTotal").val("")
        }
    }
}