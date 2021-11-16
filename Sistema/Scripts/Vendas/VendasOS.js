$(function () {
    vlTotalServicos = 0;
    vlTotalProdutos = 0;
    vlTotalVenda = 0;
    var venda = new Venda();
    venda.init();

    $("#flTblServicos").val("S");

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
    $('input[name="CondicaoPagamento.id"]').prop('disabled', true);
    $("#CondicaoPagamento_btn-localizar").hide();
    //$("#divAddProduto").hide();


    $("#CondicaoPagamento_id").change(function () {
        dtParcelasServicos.clear();
        $("#flTblServicos").val("");
        dtServicos.atualizarItens();
        dtServicos.atualizarGrid();
        $("#divAddServico").show();
        $("#btnSalvar").attr("disabled", true);
        $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true);
    })

    $("#CondicaoPagamentoDois_id").change(function () {
        dtParcelasProdutos.clear();
        $("#flTblProdutos").val("");
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
        $("#divAddProduto").show();
        $("#CondicaoPagamentoDois_btnGerarParcela").attr('disabled', true);
    })

    $(document).on('AfterLoad_CondicaoPagamento', function (e, data) {
        if (!IsNullOrEmpty(data)) {
            $("#flTblServicos").val("S");
            $("#divAddServico").hide();
            dtParcelasServicos.clear();
            dtServicos.atualizarItens();
            dtServicos.atualizarGrid();
        } else {
            alert("ppp");
        }
    });

    $(document).on('AfterLoad_CondicaoPagamentoDois', function (e, data) {
        if (!IsNullOrEmpty(data)) {
            $("#flTblProdutos").val("S");
            alert();
            $("#divAddProduto").hide();
            dtParcelasProdutos.clear();
            dtProdutos.atualizarItens();
            dtProdutos.atualizarGrid();
        } else {
            alert("ppp");
        }
    });

    $("#Produto_qtProduto").change(function () {
        let vlVenda = $("#Produto_vlVenda").val()
        if (!IsNullOrEmpty(vlVenda)) {
            vlVenda = vlVenda.replace(".", "").replace(",", ".");
            vlVenda = parseFloat(vlVenda);
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
            venda.calcTotalItem(vlVenda);
        } else {
            $("#Produto_vlTotal").val("")
        }
    })

    $(document).on('AfterLoad_Produto', function (e, data) {
        venda.calcTotalItem(data.vlVenda);
    })

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        venda.getparcelasServicos();
    });

    $("#CondicaoPagamentoDois_btnGerarParcela").click(function () {
        venda.getparcelasProdutos();
    });

    $(document).on('tblServicoRowCallback', function (e, data) {
        let flTblServicos = $("#flTblServicos").val()
        if (flTblServicos == "S") {
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

    if (dtProdutos.length > 0) {
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
    }

    // SERVIÇO

    $("#addServico").click(function () {
        venda.addServico();
    });

    $(document).on("tblServicoAfterDelete", function () {
        venda.calcTotalServico();
        venda.clearProduto();
    });

    $(document).on("tblServicoOpenEdit", venda.openEditServico);
    $(document).on("tblServicoCancelEdit", venda.clearServico);

    $("#Servico_qtServico").change(function () {
        let vlServico = $("#Servico_vlServico").val()
        if (!IsNullOrEmpty(vlServico)) {
            vlServico = vlServico.replace(".", "").replace(",", ".");
            vlServico = parseFloat(vlServico);
            venda.calcTotalItemServico(vlServico);
        }
    })

    $(document).on('AfterLoad_Servico', function (e, data) {
        venda.calcTotalItemServico(data.vlServico);
    })

    if (dtServicos.length > 0) {
        $('#btnSalvar').prop('disabled', false)
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
    }


    // LOAD 

    $("#divAddServico").hide();
    $("#CondicaoPagamento_btnGerarParcela").attr('disabled', false);
    $("#btnSalvar").attr("disabled", true);

    if (dtProdutos.length > 0) {
        $("#CondicaoPagamentoDois_btnGerarParcela").attr('disabled', false);
        $("#divAddProduto").hide();
        $("#flTblProdutos").val("S");
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    } else {
        $("#flTblProdutos").val("");
        $("#CondicaoPagamentoDois_id").prop('disabled', true);
        $("#CondicaoPagamentoDois_btnGerarParcela").attr('disabled', true);
        $("#CondicaoPagamentoDois_btn-localizar").hide();
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    }

    //if (!IsNullOrEmpty(idCond)) {
    //    $("#flTblProdutos").val("S");
    //    $("#divAddProduto").hide();
    //    $("#CondicaoPagamento_btnGerarParcela").attr('disabled', false)
    //    dtProdutos.atualizarItens();
    //    dtProdutos.atualizarGrid();
    //}

    //if (dtProdutos.length > 0) {
    //    $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
    //    $("#CondicaoPagamento_btn-localizar").show();
    //}

    
});

Venda = function () {
    self = this;
    dtProdutos = null;
    dtServicos = null;
    dtParcelasServicos = null;
    dtParcelasProdutos = null;

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

        dtServicos = new tDataTable({
            table: {
                jsItem: "jsServicos",
                name: "tblServico",
                remove: true,
                edit: true,
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

        // PARCELAS
        dtParcelasServicos = new tDataTable({
            table: {
                jsItem: "jsParcelasServicos",
                name: "tblParcelasServicos",
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

        dtParcelasProdutos = new tDataTable({
            table: {
                jsItem: "jsParcelasProdutos",
                name: "tblParcelasProdutos",
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
            $('input[name="CondicaoPagamentoDois.id"]').prop('disabled', false)
            $("#CondicaoPagamentoDois_btn-localizar").show();
            //$("#flTblProdutos").val("S");
        } else {
            $('input[name="CondicaoPagamentoDois.id"]').prop('disabled', true)
            $("#CondicaoPagamentoDois_btn-localizar").hide();
            $("#CondicaoPagamentoDois_btnGerarParcela").attr('disabled', true)
            //$("#flTblProdutos").val("");
        }
        //dtProdutos.atualizarItens();
        //dtProdutos.atualizarGrid();
        vlTotalProdutos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        $("#vlTotalProdutos").val(total);
        self.calcTotal();

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

    // PARCELAS SERVIÇOS

    self.getparcelasServicos = function () {
        if (!dtParcelasServicos.length) {
            let totalF = vlTotalServicos.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamento_id").val(), vlTotal: totalF },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelasServicos(data);
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

    self.setParcelasServicos = function (data) {
        let itens = data.parcelas;
        for (var i = 0; i < itens.length; i++) {
            //let dtParcela = JSONDate(itens[i].dtVencimento,)
            let item = {
                idFormaPagamento: itens[i].idFormaPagamento,
                nmFormaPagamento: itens[i].nmFormaPagamento,
                flSituacao: itens[i].flSituacao,
                dtVencimento: itens[i].dtVencimento,
                vlParcela: itens[i].vlParcela,
                nrParcela: itens[i].nrParcela
            }
            dtParcelasServicos.addItem(item);
        }
    }

    // PARCELAS PRODUTOS

    self.getparcelasProdutos = function () {
        if (!dtParcelasProdutos.length) {
            let totalF = vlTotalProdutos.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamentoDois_id").val(), vlTotal: totalF },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelasProdutos(data);
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

    self.setParcelasProdutos = function (data) {
        let itens = data.parcelas;
        for (var i = 0; i < itens.length; i++) {
            //let dtParcela = JSONDate(itens[i].dtVencimento,)
            let item = {
                idFormaPagamento: itens[i].idFormaPagamento,
                nmFormaPagamento: itens[i].nmFormaPagamento,
                flSituacao: itens[i].flSituacao,
                dtVencimento: itens[i].dtVencimento,
                vlParcela: itens[i].vlParcela,
                nrParcela: itens[i].nrParcela
            }
            dtParcelasProdutos.addItem(item);
        }
    }

    // ************************************************************************************************

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

    // SERVIÇOS

    self.getModelServico = function () {
        let vlServico = $("#Servico_vlServico").val().replace(".", "").replace(",", ".");
        let vlServicoAux = parseFloat(vlServico);
        let qtServicoAux = $("#Servico_qtServico").val().replace(".", "").replace(",", ".");
        qtServicoAux = parseFloat(qtServicoAux);
        var model = {
            codExecutante: $("#Executante_id").val(),
            nomeExecutante: $("#Executante_text").val(),
            codServico: $("#Servico_id").val(),
            nomeServico: $("#Servico_text").val(),
            unidade: $("#Servico_unidade").val(),
            vlServico: vlServicoAux,
            qtServico: qtServicoAux,
            vlTotal: vlServicoAux * qtServicoAux
        }
        return model;
    }

    self.validServico = function () {
        let valid = true;

        if (IsNullOrEmpty($("#Executante_id").val()) || $("#Executante_id").val() == "") {
            $("#Executante_id").blink({ msg: "Informe o executante" });
            $("#Executante_id").focus();
            valid = false;
        }

        else if (IsNullOrEmpty($("#Servico_id").val()) || $("#Servico_id").val() == "") {
            $("#Servico_id").blink({ msg: "Informe o serviço" });
            $("#Servico_id").focus();
            valid = false;
        }

        else if (IsNullOrEmpty($("#Servico_qtServico").val()) || $("#Servico_qtServico").val() == "" || $("#Servico_qtServico").val() == 0) {
            $("#Servico_qtServico").blink({ msg: "Informe a quantidade" });
            $("#Servico_qtServico").focus();
            valid = false;
        }

        if (!dtServicos.isEdit) {
            if (dtServicos.exists("codServico", $("#Servico_id").val()) && dtServicos.exists("codExecutante", $("#Executante_id").val())) {
                $("#Servico_id").blink({ msg: "Já existe um servico com este executante informado, verifique!" });
                valid = false;
            }
        }
        return valid;
    }

    self.clearServico = function () {
        $("#Servico_id").val("");
        $("#Servico_text").val("");
        $("#Servico_vlServico").val("");
        $("#Servico_unidade").val("");
        $("#Executante_id").val("");
        $("#Executante_text").val("");
        $("#Servico_qtServico").val("");
        $("#Servico_vlTotal").val("");
        $('input[name="Servico.id"]').prop('disabled', false)
        $('input[name="Executante.id"]').prop('disabled', false)
    }

    self.addServico = function () {
        if (self.validServico()) {
            let model = self.getModelServico();
            let item = {
                codExecutante: model.codExecutante,
                nomeExecutante: model.nomeExecutante,
                codServico: model.codServico,
                nomeServico: model.nomeServico,
                vlServico: model.vlServico,
                qtServico: model.qtServico,
                unidade: model.unidade,
                vlTotal: model.vlTotal
            }
            self.saveServico(item);
            self.clearServico();
            self.calcTotalServico();
        }
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
            $("#btnSalvar").attr("disabled", true);
        }
        vlTotalServicos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#fts").text("Total: " + total);
        $("#vlTotalServicos").val(total)
        self.calcTotal();
    }

    self.openEditServico = function (e, data) {
        let item = dtServicos.dataSelected.item;
        $("#Servico_id").val(item.codServico);
        $("#Servico_text").val(item.nomeServico);
        $("#Servico_vlServico").val(item.vlServico.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Servico_unidade").val(item.unidade);
        $("#Servico_qtServico").val(item.qtServico.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Executante_id").val(item.codExecutante);
        $("#Executante_text").val(item.nomeExecutante);
        $('input[name="Servico.id"]').prop('disabled', true)
        $('input[name="Executante.id"]').prop('disabled', true)
        self.calcTotalItemServico(item.vlServico)
    }

    self.saveServico = function (data) {
        if (dtServicos.isEdit) {
            dtServicos.editItem(data);
        } else {
            dtServicos.addItem(data)
        }
    }

    self.calcTotalItemServico = function (vlServico) {
        let qtServico = $("#Servico_qtServico").val()
        if (!IsNullOrEmpty(qtServico)) {
            let qt = qtServico.replace(".", "").replace(",", ".");
            let vl = vlServico;
            qt = parseFloat(qt);
            let total = qt * vl;
            total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $("#Servico_vlTotal").val(total)
        } else {
            $("#Servico_vlTotal").val("")
        }
    }

    self.calcTotal = function () {
        let total = vlTotalServicos + vlTotalProdutos;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#vlTotal").val(total)
        //vlTotalOS = vlTotalServicos + vlTotalProdutos;
        //vlTotalOS = vlTotalOS.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        //$("#vlTotal").val(vlTotalOS)
    }


    //self.datatable.atualizarItens();
    //self.datatable.atualizarGrid();

}
