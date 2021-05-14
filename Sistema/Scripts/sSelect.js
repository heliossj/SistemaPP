function sSelect(options) {
    var self = this;
    //variaves privada
    var $select = $("#" + options.prefix + "_select");
    var $btnModal = $('#' + options.prefix + '_Modal');
    var $btnSearch = $('#' + options.prefix + '_btn-localizar');
    var readOnly = (options.readOnly == undefined || readOnly == false) ? false : true;

    //Variaveis pulicas
    this.elements = options.elements;
    this.lastData = null;
    //handllers
    $btnModal.on('shown.bs.modal', function (e) {
        $(document).trigger('Open_' + options.prefix + "_Modal", true);
    });

    $btnSearch.on('click', function (e) {
        if (self.enable == false) {
            return false;
        }
        var result = isValid();
        if (result.invalid) {
            $.notify(result.msg, { type: "error" });
            self.blink()
            return false;
        }
        initModal();
    });

    $(document).on('AfterSelect_' + options.prefix + '_Modal', function (e, data) {
        self.lastData = self.data;
        $('#' + options.prefix + '_Modal').modal('hide');
        $select.empty().select2("trigger", "select", { data: data });
    });

    //Medotos privado
    var unLoad = function () {
        for (var item in self.elements) {
            self.elements[item].val("");
        }
        $(document).trigger('AfterLoad_' + options.prefix, null);
    };

    var getParams = function (params) {

        var result = {
            v: new Date().getTime()
        };
        if (params != undefined) {
            for (var i = 0; i < params.length; i++) {

                var item = params[i];
                var prop = "";
                for (var name in item) {
                    if (name != "type") {
                        prop = name;
                        break;
                    }
                }

                if (prop == "") {
                    throw new Error("Não foi possível determinar o nome do parametro");
                }

                if (item.type == undefined && (item.type != "id" || item.type != "val")) {
                    throw new Error("Não foi possível determinar o type do parametro " + name + ", o type deve ser por [val ou id]. TENTE NÃO CABACEAR");
                }
                if (item.type == "val") {
                    result[prop] = item[prop];
                } else if (item[prop] != "") {
                    result[prop] = $("#" + item[prop]).val();
                }
            }
        }
        return result;
    };

    var load = function (data) {

        var _return = $(document).triggerHandler('BeforeLoad_' + options.prefix, data);
        if (_return != undefined && _return == false) {
            self.data = self.lastData;
        }
        else {
            self.setData(data);
            $(document).trigger('AfterLoad_' + options.prefix, data);
        }
    };

    var isValid = function () {

        var result = {
            invalid: false,
            msg: "<b>Entrada com dependência.</b><br/>"
        };

        itens = options.dependencies;
        if (itens != undefined) {
            for (var i = 0; i < itens.length; i++) {
                var item = itens[i];

                if (item.id != undefined && !IsNullOrEmpty(item.id)) {

                    var $input = $('#' + item.id);
                    if ($input.length > 0 && $input.val() == '') {
                        result.msg += IsNullOrEmpty(item.msg) ? 'Verifique o campo obrigatório<br/>' : item.msg + '<br/>';
                        result.invalid = true;
                        // $input.parent().find(".select2-container").blink().select();
                    }
                }
            }
        }
        return result;
    };

    //  Getters  e Setters
    this.__defineGetter__("data", function () {

        var data = {};
        for (var item in self.elements) {
            if (self.elements[item].length == 0) {
                throw new Error("Não foi possível encontrar a input hidden " + item + " na da consulta de " + options.prefix + ", tente nao cabacear!");
            }
            var aux = self.elements[item].attr('id').split("_");
            var id = aux[aux.length - 1];
            if (!IsNullOrEmpty(self.elements[item].val())) {
                if (id.toLowerCase() == "jsitem") {
                    var js = self.elements[item].val();
                    var obj = JSON.parse(js);
                    for (var i in obj) {
                        if (data[i] == undefined) {
                            data[i] = obj[i];
                        }
                    }
                }
                data[id] = self.elements[item].val();
            }
        }
        if (!readOnly) {
            var select = $select.select2('data')[0];
            if (select != undefined) {
                delete select.selected;
                delete select.disabled;
                delete select._resultId;
                delete select.element;
                for (var s in select) {
                    for (var d in data) {
                        if (data[s] == undefined) {
                            data[s] = select[s];
                        }
                    }
                }
            }
        }
        return Object.keys(data).length === 0 ? null : data;
    });

    this.__defineSetter__("data", function (item) {
        self.lastData = self.data;
        if (item != null && Object.keys(item).length > 0) {
            var $option = $("<option selected></option>").val(item.id).text(item.text);
            self.setData(item);
            $select.empty();
            $select.append($option).trigger('change');
            //$select.select2("addOption", item);
            //$select.select2("trigger", "change");
        }
        else {
            self.clear();
            $select.empty().trigger('change');
        }
        $(document).trigger('AfterSet_' + options.prefix, item);
    });

    this.__defineGetter__("enable", function () {
        return !$select.is(':disabled')
    });

    this.__defineSetter__("enable", function (value) {
        $select.prop("disabled", !value);
        $btnSearch.attr("disabled", !value);
        for (var item in self.elements) {
            self.elements[item].prop("readonly", !value);
        }
    });

    //Metodos publicos

    this.trigger = function (name) {
        $(document).trigger(name + '_' + options.prefix, self.data);
    }

    this.clear = function () {
        $select.val(null).trigger('change');
        for (var item in self.elements) {
            self.elements[item].val("");
        }
    };

    this.focus = function (element) {
        if (element == undefined || element == "text") {
            $select.next().find('.select2-selection').focus();
        } else {
            self.elements.$id.focus().select();
        }
    };

    this.blink = function (options) {
        for (var item in self.elements) {
            if (!self.elements[item].val() && self.elements[item].is(":visible")) {
                self.elements[item].blink();
            }
        }
        $btnSearch.blink();
        $select.parent().blink(options);
    }

    this.open = function () {
        $select.select2('open');
    };

    this.getData = function () {
        var data = {};
        var flag = false;
        for (var item in self.elements) {
            var id = item.substring(1);
            if (self.elements[item].val()) {
                data[id] = self.elements[item].val();
                flag = true;
            }
        }
        return flag ? data : null;
    };

    this.setData = function (data) {
        if (data) {
            for (var item in self.elements) {
                var name = self.elements[item].attr('id');
                var split = name != undefined ? name.split("_") : null;
                var js = split == null ? "" : split[split.length - 1].toLowerCase();
                if (js == "jsitem") {
                    self.elements[item].val(JSON.stringify(data));
                } else {
                    var id = item.substring(1);

                    self.elements[item].val(data[id]);
                }
            }
        } else {
            self.clear();
        }
    };
    this.findById = function (id) {
        if (!IsNullOrEmpty(id)) {
            var result = getParams(options.details.params);
            result.id = id;
            return new Promise(resolve => {
                $.ajax({
                    dataType: 'json',
                    type: 'GET',
                    url: options.details.url,
                    data: result,
                    beforeSend: function () {
                        //RPNotification.pageOverlay(true, "overlay-transparent");
                    },
                    success: function (data) {
                        self.lastData = self.data;
                        $select.val(null).trigger('change');
                        // RPNotification.pageOverlay(false, "overlay-transparent");
                        if (data) {
                            $select.empty().select2("trigger", "select", { data: data });
                        }
                        else {
                            $select.empty().trigger('change').trigger('select2:unselect');
                        }
                        resolve(data);
                    },
                    error: function (request) {
                        Functions.checkRequest(request);
                        // RPNotification.pageOverlay(false, "overlay-transparent");
                    }
                });
            });
        }
    }
    var initModal = function () {
        if (options.query != undefined) {
            let tmplSearch = options.query.tmplSearch == undefined ? "" : tmpl(options.query.tmplSearch, {});

            var $modal = getModal(tmplSearch);
            $("form").append($modal);

            var columns = new Array();
            columns.push({
                sortable: false,
                data: null,
                sClass: 'center',
                mRender: function (data) {
                    return '<a class="btn btn-info btn-sm" href="#" title="Selecionar registro"><i class="fa fa-check"></i> </a>';
                }
            });
            for (var i = 0; i < options.query.columns.length; i++) {
                var item = options.query.columns[i];
                if (item.type == "currency") {
                    item.sClass = "right";
                    item.mRender = function (data) {
                        return Functions.numberFormat(data);
                    };
                }
                columns.push(item);
            }
            let order = options.query.order == undefined ? [[2, "asc"]] : options.query.order;
            var dt = $('#' + options.prefix + '_grid');
            self.modal = $('#' + options.prefix + '_Modal');
            self.table = dt.dataTable({
                ajax: { url: options.query.url },
                sDom: options.query.tmplSearch == undefined ? "<'row'<'visible-lg visible-md hidden-sm hidden-xs  col-md-6'l><'col-md-6'f><'col-md-12'r>>t<'row'<'visible-lg visible-md hidden-sm hidden-xs col-md-6'i><'col-md-6'p>>" : "<'row'<'visible-lg visible-md hidden-sm hidden-xs  col-md-6'l><'col-md-6'p><'col-md-12'r>>t<'row'>",
                columns: columns,
                order: order,
                fnInfoCallback: null,
                fnServerParams: function (aoData) {

                    var result = getParams(options.query.params);
                    for (var name in result) {
                        aoData[name] = result[name];
                    }
                },
                initComplete: function (settings, json) {
                    $('#' + options.prefix + '_grid_filter input').focus();
                },
                fnRowCallback: function (nRow, data) {
                    $('td', nRow).off('click').on('click', function (e) {
                        self.lastData = self.data;
                        $('#' + options.prefix + '_Modal').modal('hide');
                        $select.empty().select2("trigger", "select", { data: data });
                        e.preventDefault();
                    });

                    $('td a', nRow).off('click').on('click', function (e) {
                        self.lastData = self.data;
                        $('#' + options.prefix + '_Modal').modal('hide');
                        e.preventDefault();
                    });
                },
            });

            self.modal.on('hidden.bs.modal', function (e) {
                self.modal.remove();
                self.focus();
            });
            self.modal.on('shown.bs.modal', function (e) {
                Functions.handlers.mask();
                Functions.handlers.datepicker();
                $(document).trigger('Open_' + options.prefix + "_Modal", true);
            });

            if (options.query.btnSearch != undefined) {
                $("." + options.prefix + "_filter").on("change", function (e) {
                    e.preventDefault();
                    self.table.fnDraw();
                });
                $(options.query.btnSearch).on("click", function (e) {
                    e.preventDefault();
                    self.table.fnDraw();
                });
            }
        }
    }
    var getModal = function (form) {
        var th = '<th></th>';
        for (var i = 0; i < options.query.columns.length; i++) {
            th += '<th style="width:' + options.query.columns[i].width + '">' + options.query.columns[i].label + '</th>';
        }
        var table = '<table id="' + options.prefix + '_grid" style="width: 100%" class="table-hover table table-condensed table-bordered table-striped table-select dataTable no-footer">' +
            '<thead>' +
            '<tr>' + th + '</tr > ' +
            '</thead>' +
            '</table>';
        var modal = '<div id="' + options.prefix + '_Modal" class="modal fade" role="dialog">' +
            '<div class="modal-dialog modal-ulg">' +
            '<div class="modal-content">' +
            '<div class="modal-body no-padding">' +
            '<div class="panel panel-primary" style="margin-bottom: -1px;">' +
            '<div class="panel-heading" style="height: 41px; padding-left:2px;font-weight: bold;">' +
            '<h3 class="panel-title pull-right"><button type="button" style="opacity: inherit;color: dark;" class="close" data-dismiss="modal">&times;</button></h3>' +
            '<ul class="nav panel-tabs" style="width:400px">' +
            '<li class="active"><a data-event="true" style="margin-top:1px;color: dark"; data-toggle="tab"><b><i class="fa fa-list-alt" aria-hidden="true"></i> ' + options.placeholder + '</b></a></li>' +
            '</ul>' +
            '</div>' +
            '<div class="panel-body">' +
            '<div class="tab-content pull-left" style="width:100%">' +
            form + table +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>';
        return modal;
    };
    this.init = function () {
        if (!readOnly) {
            //Inicialização do details
            if (options.details != undefined) {
                self.elements.$id.change(function (evt) {
                    var result = isValid();
                    if (result.invalid) {
                        $.notify(result.msg, { type: "error" });
                        self.elements.$id.val("").blink();
                        self.elements.$id.focus();
                        return false;
                    }
                    var value = $(this).val();
                    if (!IsNullOrEmpty(value)) {

                        var result = getParams(options.details.params);
                        result.id = value;

                        $.ajax({
                            dataType: 'json',
                            type: 'GET',
                            url: options.details.url,
                            data: result,
                            beforeSend: function () {
                                RPNotification.pageOverlay(true, "overlay-transparent");
                            },
                            success: function (data) {
                                self.lastData = self.data;
                                $select.val(null).trigger('change');
                                RPNotification.pageOverlay(false, "overlay-transparent");
                                if (data) {
                                    $select.empty().select2("trigger", "select", { data: data });
                                }
                                else {
                                    new PNotify({ title: "Atenção!", text: "Nenhum registro encontrado!", type: "error" });
                                    self.elements.$id.blink();
                                    $select.empty().trigger('change').trigger('select2:unselect');
                                }
                            },
                            error: function (request) {
                                Functions.checkRequest(request);
                                RPNotification.pageOverlay(false, "overlay-transparent");
                            }
                        });
                    }
                    else {
                        $select.val(null).trigger('change').trigger('select2:unselect');
                    }
                }).focus(function () {
                    $(this).select();
                });
                if (options.details.extras != undefined) {
                    for (var i = 0; i < options.details.extras.length; i++) {
                        var item = options.details.extras[i];
                        item.$element.attr("url", item.url);
                        item.$element.change(function (evt) {
                            var result = isValid();
                            if (result.invalid) {
                                $.notify(result.msg, { type: "error" });
                                $(this).val("").blink();
                                $(this).focus();
                                return false;
                            }
                            var value = $(this).val();
                            const el = $(this);
                            if (!IsNullOrEmpty(value)) {

                                var result = getParams(options.details.params);

                                var id = $(this).attr('id');
                                var split = id != undefined ? id.split("_") : null;
                                var name = split == null ? "" : split[split.length - 1];
                                result[name] = value;
                                $.ajax({
                                    dataType: 'json',
                                    type: 'GET',
                                    url: $(this).attr("url"),
                                    data: result,
                                    beforeSend: function () {
                                        RPNotification.pageOverlay(true, "overlay-transparent");
                                    },
                                    success: function (data) {

                                        self.lastData = self.data;
                                        RPNotification.pageOverlay(false, "overlay-transparent");
                                        if (data) {
                                            $select.empty().select2("trigger", "select", { data: data });
                                        }
                                        else {
                                            new PNotify({ title: "Atenção!", text: "Nenhum registro encontrado!", type: "error" });
                                            el.blink();
                                            $select.empty().trigger('change').trigger('select2:unselect');
                                        }
                                    },
                                    error: function (request) {
                                        Functions.checkRequest(request);
                                        RPNotification.pageOverlay(false, "overlay-transparent");
                                    }
                                });
                            }
                            else {
                                $select.val(null).trigger('change').trigger('select2:unselect');
                            }
                        }).focus(function () {
                            $(this).select();
                        });
                    }
                }
            }

            // inicialização do select 2
            if (options.select != undefined) {
                var tags = options.tags == undefined ? false : options.tags;
                $select.select2({
                    placeholder: options.placeholder,
                    allowClear: true,
                    multiple: false,
                    language: "pt-BR",
                    theme: "bootstrap",
                    width: 'width: 100%;',
                    tags: tags,
                    ajax: {
                        delay: 300,
                        dataType: 'json',
                        url: options.select.url,
                        cache: false,
                        //transport: function (params, success, failure) {
                        //    var $request = $.ajax(params);
                        //    //failure = new function () {
                        //    //    return null
                        //    //}
                        //   // $request.then(success);
                        //    $request.catch(failure);

                        //    //params.beforeSend = function (request) {
                        //    //    alert()
                        //    //    var result = isValid();
                        //    //    if (result.invalid) {
                        //    //        alert()
                        //    //        $.notify(result.msg, { type: "error" });
                        //    //        $select.parent().find(".select2-container").blink();
                        //    //        request.abort();
                        //    //        return false;
                        //    //    }
                        //    //};

                        //    return $request;
                        //},
                        data: function (params) {
                            var result = getParams(options.select.params);
                            result.q = params.term == undefined ? "" : params.term;
                            result.page = params.page || 1;
                            return result;
                        },
                        processResults: function (data, params) {

                            params.page = params.page || 1;
                            var more = (params.page * DEFAULT_PAGESIZE) < data.totalCount;
                            return {
                                results: data.results,
                                pagination: {
                                    more: more
                                }
                            };
                        },
                        beforeSend: function (xhr, opts) {
                            var result = isValid();
                            if (result.invalid) {
                                xhr.abort();
                            }
                        },
                    },
                    initSelection: function (element, callback) {
                        var data = {};
                        for (var item in self.elements) {
                            var id = item.substring(1);
                            data[id] = self.elements[item].val();
                        }

                        callback(data);
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    templateResult: options.templateResult,
                    templateSelection: options.templateSelection || function (data) {
                        return data.text;
                    }

                }).on('select2:select', function (evt) {
                    self.focus();
                    load(evt.params.data);
                }).on('select2:opening', function (evt) {
                    self.lastData = self.data;
                    var result = isValid();
                    if (result.invalid) {
                        $.notify(result.msg, { type: "error" });
                        $select.parent().find(".select2-container").blink();
                        evt.preventDefault();
                    }
                }).on('select2:close', function (evt) {
                    if (tags) {
                        load({ id: this.value, text: this.value });
                    }
                    self.focus();
                }).on('select2:open', function (evt) {

                    $(".select2-search__field").css('text-transform', 'uppercase').blur(function () { this.value = this.value.toUpperCase(); });
                }).on("select2:unselect", function (evt) {
                    unLoad();
                });
                var $search = $select.data('select2').dropdown.$search || $select.data('select2').selection.$search;

                $select.next().find('.select2-selection').keydown(function (e) {
                    if (e.keyCode == 40) {
                        $select.select2('open');

                        e.preventDefault();
                    } else if ((e.keyCode >= 48 && e.keyCode <= 90)) {
                        var letra = String.fromCharCode(e.keyCode).trim();
                        $select.select2('open');

                        $search.val(letra);
                        $search.trigger('keyup');

                        e.preventDefault();

                    }
                });

                $search.keydown(function (e) {
                    if (e.which === 9) {
                        var highlighted = $select.data('select2').$dropdown.find('.select2-results__option--highlighted');
                        if (highlighted) {
                            self.lastData = self.data;
                            var data = highlighted.data('data');
                            if (data) {
                                $select.empty().select2("trigger", "select", { data: data });
                            }
                            else {
                                $select.empty().trigger('change').trigger('select2:unselect');
                            }
                        }
                        else {
                            $select.empty().trigger('change').trigger('select2:unselect');
                        }
                        self.focus();
                    }
                });
                if (options.templateHeader != undefined) {
                    var selectResults = $select.data('select2').results.$results.parent();
                    selectResults.prepend(options.templateHeader);
                }
            }

            // Inicialização das dependencias
            if (options.dependencies != undefined) {
                itens = options.dependencies;
                if (itens != undefined) {
                    for (var i = 0; i < itens.length; i++) {
                        var item = itens[i];
                        if (item.clearOn != undefined) {
                            for (var j = 0; j < item.clearOn.length; j++) {
                                $(document).on(item.clearOn[j], function (e, data) {
                                    $select.empty().trigger('change').trigger('select2:unselect');
                                });
                            }
                        }
                    }
                }
            }

        }
    };
}
