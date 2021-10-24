$(function () {

    dtLancamentos = new tDataTable({
        table: {
            jsItem: "jsLancamentos",
            name: "tblLancamentos",
            order: [[0, "asc"]],
            columns: [
                {
                    data: "codLancamento",
                    sClass: "right"
                },
                {
                    data: null,
                    mRender: function (data) {
                        var dtCompleta = "";
                        if (!IsNullOrEmpty(data.dtMovimento)) {
                            var dt = new Date(data.dtMovimento);
                            var day = dt.getDate();
                            var month = dt.getMonth() + 1;
                            var year = dt.getFullYear();
                            var dtCompleta = day + "/" + month + "/" + year;
                        }
                        return dtCompleta;
                    }
                },
                {
                    data: null,
                    sClass: "right",
                    mRender: function (data) {
                        return data.vlLancamento.toLocaleString('pt-br', { currency: 'BRL', minimumFractionDigits: 2, maximumFractionDigits: 2 });
                    }
                },
                { data: "descricao" }
            ]
        },
    });














});
