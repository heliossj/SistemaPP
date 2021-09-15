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


    if (!$("#flFinalizar").is(":checked")) {
        $("#divFinaliza").slideUp();
        $("#vlTotal").val("");
    } else {
        $("#divFinaliza").slideDown();
    }

    $("#flFinalizar").click(function () {
        if (!dtServicos.length) {
            $.notify({ message: "Informe ao menos um serviço para finalizar", icon: 'fa fa-exclamation' }, { type: 'danger', z_index: 2000 });
            $("#flFinalizar").prop("checked", false)
        } else {
            if ($(this).is(":checked")) {
                OS.calcTotalProduto;
                OS.calcTotalServico;
                $("#divParcelas").hide();
                $("#divFinaliza").slideDown();
                let total = vlTotalProdutos + vlTotalServicos;
                vlTotalOS = total;
                let totalFormat = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                $("#vlTotal").val(totalFormat)
                dtServicos.atualizarGrid();
                dtProdutos.atualizarGrid();
                $("#divAddServico").slideUp();
                $("#divAddProduto").slideUp();
            } else {
                $("#divFinaliza").slideUp();
                dtServicos.atualizarGrid();
                dtProdutos.atualizarGrid();
                dtParcelas.clear();
                $("#divAddServico").slideDown();
                $("#divAddProduto").slideDown();
                $("#CondicaoPagamento_id").val("")
                $("#CondicaoPagamento_text").val("")
                $("#CondicaoPagamento_btnGerarParcela").attr('disabled', true);
            }
        }
    });

    $("#CondicaoPagamento_btnGerarParcela").click(function () {
        OS.getparcelas();
    });

    $(document).on('tblServicoRowCallback', function (e, data) {
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
                    { data: "codServico" },
                    { data: "nomeServico" },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.vlUnitario.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                    },
                    {
                        data: null,
                        mRender: function (data) {
                            return data.qtServico.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
                            return data.vlUnitario.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
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
        let qtServicoAux = $("#qtServico").val().replace(".", "").replace(",", ".");
        qtServicoAux = parseFloat(qtServicoAux);
        var model = {
            codServico: $("#Servico_id").val(),
            nomeServico: $("#Servico_text").val(),
            vlServico: vlServicoAux,
            qtServico: qtServicoAux,
            vlTotal: vlServicoAux * qtServicoAux
        }
        return model;
    }

    self.validServico = function () {
        let valid = true;

        if (IsNullOrEmpty($("#Servico_id").val()) || $("#Servico_id").val() == "") {
            $("#Servico_id").blink({ msg: "Informe o serviço" });
            $("#Servico_id").focus();
            valid = false;
        }

        else if (IsNullOrEmpty($("#qtServico").val()) || $("#qtServico").val() == "" || $("#qtServico").val() == 0) {
            $("#qtServico").blink({ msg: "Informe a quantidade" });
            $("#qtServico").focus();
            valid = false;
        }

        if (!dtServicos.isEdit) {
            if (dtServicos.exists("codServico", $("#Servico_id").val())) {
                $("#Servico_id").blink({ msg: "Serviço já informado, verifique!" });
                valid = false;
            }
        }
        return valid;
    }

    self.clearServico = function () {
        $("#Servico_id").val("");
        $("#Servico_text").val("");
        $("#Servico_vlServico").val("");
        $("#qtServico").val("");
        $('input[name="Servico.id"]').prop('disabled', false)
    }

    self.addServico = function () {
        if (self.validServico()) {
            let model = self.getModelServico();
            let item = {
                codServico: model.codServico,
                nomeServico: model.nomeServico,
                vlUnitario: model.vlServico,
                qtServico: model.qtServico,
                vlTotal: model.vlTotal
            }
            //dtServicos.addItem(item);
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
        }
        vlTotalServicos = total;
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#fts").text("Total: " + total);
    }

    self.openEditServico = function (e, data) {
        let item = dtServicos.dataSelected.item;
        $("#Servico_id").val(item.codServico);
        $("#Servico_text").val(item.nomeServico);
        $("#Servico_vlServico").val(item.vlUnitario.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#qtServico").val(item.qtServico);
        $('input[name="Servico.id"]').prop('disabled', true)
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
        let qtProdutoAux = $("#qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
            unidade: $("#unidade").val(),
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


        else if (IsNullOrEmpty($("#qtProduto").val()) || $("#qtProduto").val() == "" || $("#qtProduto").val() == 0) {
            $("#qtProduto").blink({ msg: "Informe a quantidade" });
            $("#qtProduto").focus();
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
        $("#unidade").val("M");
        $("#Produto_vlVenda").val("");
        $("#qtProduto").val("");
        $('input[name="Produto.id"]').prop('disabled', false)
    }

    self.addProduto = function () {
        if (self.validProduto()) {
            let model = self.getModelProduto();
            let item = {
                codProduto: model.codProduto,
                nomeProduto: model.nomeProduto,
                unidade: model.unidade,
                vlUnitario: model.vlProduto,
                qtProduto: model.qtProduto,
                vlTotal: model.vlTotal
            }
            //dtProdutos.addItem(item);
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
    }

    self.openEditProduto = function (e, data) {
        let item = dtProdutos.dataSelected.item;
        $("#Produto_id").val(item.codProduto);
        $("#Produto_text").val(item.nomeProduto);
        $("#unidade").val(item.unidade);
        $("#Produto_vlVenda").val(item.vlUnitario.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 }));
        $("#qtProduto").val(item.qtProduto);
        $('input[name="Produto.id"]').prop('disabled', true)
    }

    self.saveProduto = function (data) {
        if (dtProdutos.isEdit) {
            //let vlItem = data.
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

    //self.datatable.atualizarItens();
    //self.datatable.atualizarGrid();

}
