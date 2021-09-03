$(function () {
    var compra = new Compra();
    compra.init();

    //$("#addProduto").click(function () {
    //    compra.addProduto();
    //});

    //$(document).on("tblServicoAfterDelete", OS.calcTotalServico);

});

Compra = function () {
    self = this;
    dtServicos = null;
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
                    { data: "nomeProduto" },
                    { data: "nomeProduto" },
                    { data: "nomeProduto" },
                    { data: "nomeProduto" },
                ]
            },
        });
        //self.calcTotalProduto();

        dtParcelas = new tDataTable({
            table: {
                jsItem: "jsParcelas",
                name: "tblParcelas",
                order: [[1, "asc"]],
                columns: [
                    { data: null },
                    { data: null },
                    { data: null },
                    { data: null },
                    //{
                    //    data: null,
                    //    mRender: function (data) {
                    //        return data.vlUnitario.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    //    }
                    //},
                    //{
                    //    data: null,
                    //    mRender: function (data) {
                    //        return data.qtProduto.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    //    }
                    //},
                    //{
                    //    data: null,
                    //    mRender: function (data) {
                    //        return data.vlTotal.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    //    }
                    //},
                ]
            },
        });
    }

    //Serviço
    self.getModelServico = function () {
        let vlServicoAux = parseFloat($("#Servico_vlServico").val());
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

        else if (dtServicos.exists("codServico", $("#Servico_id").val())) {
            $("#Servico_id").blink({ msg: "Serviço já informado, verifique!" });
            valid = false;
        }

        return valid;
    }

    self.clearServico = function () {
        $("#Servico_id").val("");
        $("#Servico_text").val("");
        $("#Servico_vlServico").val("");
        $("#qtServico").val("")
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
            dtServicos.addItem(item);
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
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#fts").text("Total: " + total);
    }

    //Produto
    self.getModelProduto = function () {
        let vlProdutoAux = parseFloat($("#Produto_vlVenda").val());
        let qtProdutoAux = $("#qtProduto").val().replace(".", "").replace(",", ".");
        qtProdutoAux = parseFloat(qtProdutoAux);
        var model = {
            codProduto: $("#Produto_id").val(),
            nomeProduto: $("#Produto_text").val(),
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

        else if (dtProdutos.exists("codProduto", $("#Produto_id").val())) {
            $("#Produto_id").blink({ msg: "Produto já informado, verifique!" });
            valid = false;
        }

        return valid;
    }

    self.clearProduto = function () {
        $("#Produto_id").val("");
        $("#Produto_text").val("");
        $("#Produto_vlVenda").val("");
        $("#qtProduto").val("")
    }

    self.addProduto = function () {
        if (self.validProduto()) {
            let model = self.getModelProduto();
            let item = {
                codProduto: model.codProduto,
                nomeProduto: model.nomeProduto,
                vlUnitario: model.vlProduto,
                qtProduto: model.qtProduto,
                vlTotal: model.vlTotal
            }
            dtProdutos.addItem(item);
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
        total = total.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
        $("#ftp").text("Total: " + total);
    }

    //self.datatable.atualizarItens();
    //self.datatable.atualizarGrid();

}
