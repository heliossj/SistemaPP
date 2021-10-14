$(function () {
    vlTotalServicos = 0;
    vlTotalProdutos = 0;
    vlTotalOS = 0;

    var OS = new OrdemServico();
    OS.init();

    $("#addProduto").click(function () {
        OS.addProduto();
    });

    $("#addServico").click(function () {
        OS.addServico();
    });

    $(document).on("tblServicoAfterDelete", function () {
        OS.calcTotalServico();
        OS.clearProduto();
    });
    $(document).on("tblProdutoAfterDelete", function () {
        OS.calcTotalProduto();
        OS.clearProduto();
    });

    $(document).on("tblProdutoOpenEdit", OS.openEditProduto);
    $(document).on("tblProdutoCancelEdit", OS.clearProduto);

    $(document).on("tblServicoOpenEdit", OS.openEditServico);
    $(document).on("tblServicoCancelEdit", OS.clearServico);

    //document.getElementById('Funcionario_nmFuncionario').readOnly = true;
    //if (!$("#flFinalizar").is(":checked")) {
    //    $("#divFinaliza").slideUp();
    //    $("#vlTotal").val("");
    //} else {
    //    $("#divFinaliza").slideDown();
    //}

    //$("#flFinalizar").click(function () {
    //    if (!dtServicos.length) {
    //        $.notify({ message: "Informe ao menos um serviço para finalizar", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
    //        $("#flFinalizar").prop("checked", false)
    //    } else {
    //        if ($(this).is(":checked")) {
    //            OS.calcTotalProduto;
    //            OS.calcTotalServico;
    //            $("#divParcelas").hide();
    //            $("#divFinaliza").slideDown();
    //            let total = vlTotalProdutos + vlTotalServicos;
    //            vlTotalOS = total;
    //            let totalFormat = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
    //            $("#vlTotal").val(totalFormat)
    //            dtServicos.atualizarGrid();
    //            dtProdutos.atualizarGrid();
    //            $("#divAddServico").slideUp();
    //            $("#divAddProduto").slideUp();
    //        } else {
    //            $("#divFinaliza").slideUp();
    //            dtServicos.atualizarGrid();
    //            dtProdutos.atualizarGrid();
    //            dtParcelas.clear();
    //            $("#divAddServico").slideDown();
    //            $("#divAddProduto").slideDown();
    //            $("#CondicaoPagamento_id").val("")
    //            $("#CondicaoPagamento_text").val("")
    //            $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true);
    //        }
    //    }
    //});

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        OS.getparcelas();
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

    $("#Servico_qtServico").change(function () {
        let vlServico = $("#Servico_vlServico").val()
        if (!IsNullOrEmpty(vlServico)) {
            vlServico = vlServico.replace(".", "").replace(",", ".");
            vlServico = parseFloat(vlServico);
            OS.calcTotalItemServico(vlServico);
        }
    })

    $(document).on('AfterLoad_Servico', function (e, data) {
        OS.calcTotalItemServico(data.vlServico);
    })

    $("#Produto_qtProduto").change(function () {
        let vlProduto = $("#Produto_vlVenda").val()
        if (!IsNullOrEmpty(vlProduto)) {
            vlProduto = vlProduto.replace(".", "").replace(",", ".");
            vlProduto = parseFloat(vlProduto);
            OS.calcTotalItemProduto(vlProduto)
        }
    })

    $(document).on('AfterLoad_Produto', function (e, data) {
        OS.calcTotalItemProduto(data.vlVenda)
    })

    $('#btnSalvar').prop('disabled', true)
    $('input[name="CondicaoPagamento.id"]').prop('disabled', true)
    $("#CondicaoPagamento_btn-localizar").hide();

    $(document).on('AfterLoad_CondicaoPagamento', function (e, data) {
        $("#divAddServico").hide();
        $("#divAddProduto").hide();
        $("#flTblServicos").val("S");
        dtServicos.atualizarItens();
        dtServicos.atualizarGrid();
        dtProdutos.atualizarItens();
        dtProdutos.atualizarGrid();
    })

    $("#CondicaoPagamento_id").change(function () {
        if (IsNullOrEmpty($(this).val())) {
            $("#flTblServicos").val("");
            $("#divAddServico").show();
            $("#divAddProduto").show();
            dtServicos.atualizarItens();
            dtServicos.atualizarGrid();
            dtProdutos.atualizarItens();
            dtProdutos.atualizarGrid();
        }
    })

    //load
    if (dtServicos.length > 0) {
        $('#btnSalvar').prop('disabled', false)
        $('input[name="CondicaoPagamento.id"]').prop('disabled', false)
        $("#CondicaoPagamento_btn-localizar").show();
    }



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
                            let result = data.vlTotal;
                            result = result.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                            return result;
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
                remove: true,
                edit: true,
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
                            return data.vlTotal.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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

    //Serviço
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
            console.log(item)
            self.saveServico(item);
            self.clearServico();
            self.calcTotalServico();
        }
    }

    self.calcTotalServico = function () {
        let total = 0;
        if (dtServicos.length && dtServicos.length > 0) {
            for (var i = 0; i < dtServicos.length; i++) {
                let totalServico = dtServicos.data[i].vlTotal;
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

    //Produto
    self.getModelProduto = function () {
        let vlProduto = $("#Produto_vlVenda").val().replace(".", "").replace(",", ".");
        let vlProdutoAux = parseFloat(vlProduto);
        let qtProdutoAux = $("#Produto_qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
            unidade: $("#Produto_unidade").val(),
            vlProduto: vlProdutoAux,
            qtProduto: qtProdutoAux,
            vlTotal: vlProdutoAux * qtProdutoAux
        }
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
        $("#Produto_unidade").val("");
        $("#Produto_vlVenda").val("");
        $("#Produto_qtProduto").val("");
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
                vlProduto: model.vlProduto,
                qtProduto: model.qtProduto,
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
                let totalProduto = dtProdutos.data[i].vlTotal;
                total += totalProduto;
            }
        }
        vlTotalProdutos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
        self.calcTotal();
    }

    self.openEditProduto = function (e, data) {
        let item = dtProdutos.dataSelected.item;
        $("#Produto_id").val(item.codProduto);
        $("#Produto_text").val(item.nomeProduto);
        $("#Produto_unidade").val(item.unidade);
        $("#Produto_vlVenda").val(item.vlProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#Produto_qtProduto").val(item.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $('input[name="Produto.id"]').prop('disabled', true)
        self.calcTotalItemProduto(item.vlProduto)
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
            let totalF = vlTotalOS.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: Action.getParcelas,
                data: { idCondicaoPagamento: $("#CondicaoPagamento_id").val(), vlTotal: totalF },
                success: function (data) {
                    $.notify({ message: data.message, icon: 'fa fa-exclamation' }, { type: 'success', z_index: 2000 });
                    self.setParcelas(data);
                    $('#btnSalvar').prop('disabled', false)
                },
                error: function (request) {
                    alert("Erro ao buscar registro");
                }
            });
        } else {
            $.notify({ message: "Já foram geradas parcelas para esta Ordem de Serviço, verifique!", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
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

    self.calcTotalItemProduto = function (vlProduto) {
        let qtProduto = $("#Produto_qtProduto").val()
        if (!IsNullOrEmpty(qtProduto)) {
            let qt = qtProduto.replace(".", "").replace(",", ".");
            let vl = vlProduto;
            qt = parseFloat(qt);
            let total = qt * vl;
            total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
            $("#Produto_vlTotal").val(total)
        } else {
            $("#Produto_vlTotal").val("")
        }
    }

    self.calcTotal = function () {
        vlTotalOS = vlTotalServicos + vlTotalProdutos;
        vlTotalOS = vlTotalOS.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#vlTotal").val(vlTotalOS)
    }
}
