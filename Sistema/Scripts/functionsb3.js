/* eslint-disable */
const baseUrl = ((document.getElementsByTagName('base')[0] || {}).href || "").replace(/\/$/, '');
const semver = {
  get GetVersion() {
    let meta = document.querySelector('head meta[name=version]');
    return !meta ? '/222223'  : meta.getAttribute('value');
  }
};


$(function () {
  $.ajaxSetup({
    beforeSend: function (xhr, options) {
      let meta = document.querySelector('head meta[name=version]');
      options.url = `${options.url}${options.url.indexOf('?') >= 0 ? '&' : '?'}_v=${semver.GetVersion}`;
    }
  });

  $(document).ajaxError(function (e, xhr, settings, exception) {
    if (xhr.status === 401) {
      window.location.href = $('body').data('url-logout');
    }
    else {
      if (exception !== "abort") {
        let messageInfo = {
          message: "Ajax error response",
          stacktrace: JSON.stringify({
            errorThrown: exception,
            url: settings.url,
            type: settings.type,
            requestData: {}
          })
        };
        //RPClientLogger.sendToServer(messageInfo);
      }
    }
  });
});

$(document).ready(function () {
  $(document).mousedown(function (e) {
    if (e.which === 2) {

      var tgt = $(e.target);

      if (tgt.data('tab-id')) {
        var tabId = tgt.parent().data('tab-id');
        if (tabId) {
          Tabs.close(tabId);
        }
      }

      return false;
    }
    return true;
  });
});

class Tabs {
  constructor() { }

  static handler(e, blank) {
    let $element = $(e);
    let url = $element.prop('href');

    if (!blank) {
      let id = $element.data('tab-id') ? $element.data('tab-id') : new Date().getTime();
      Tabs.open(id, ($element.text() || "").trim(), url);
      return false;
    }
  }

  static open(id, title, url) {
    let $headerElement = window.parent.$('#header-tab').length ? window.parent.$('#header-tab') : $('#header-tab');
    let $contentElement = window.parent.$('#content-tab').length ? window.parent.$('#content-tab') : $('#content-tab');

    if ($headerElement.length === 0 || $contentElement.length === 0) {
      bootbox.confirm({
        message: "Deseja abrir esta página em nova janela do navegador?",
        buttons: {
          cancel: { label: 'Não', className: 'btn-default' },
          confirm: { label: 'Sim', className: 'btn-primary' }
        },
        callback: function (result) {
          window.open(url, result ? '_blank' : '_self').focus();
        }
      });
      return;
    }

    let $headerTab = $headerElement;
    let $contentTab = $contentElement;
    let $header = $(`
        <li role="presentation" class="active sistema-tab-item sistema-tab-item-${id}" data-tab-id="${id}">
            <a title="${title}" href="#${id}" data-tab-id="${id}" role="tab" data-toggle="tab" class="clearfix sistema-tab-item-a">
                <i class="fa fa-circle-o tab-icon" data-tab-id="${id}"></i>
                <i class="fa fa-dot-circle-o tab-icon active" data-tab-id="${id}"></i>
                <i class="fa fa-link tab-icon origem" data-tab-id="${id}"></i>
                <span class="tab-text sistema-tab-item-text" data-tab-id="${id}">${title}</span>
                <span onclick="return Tabs.close('${id}')" title="Fechar aba" class="close" data-tab-id="${id}">×</span>
            </a>
        </li>`);

    let $content = $(`
        <div class="tab-pane active overlay" role="tabpanel" id="${id}" style="width:100%; position:absolute; top:0px; bottom:0px">
            <iframe frameborder="0" class="iframe-content" src="${url}"></iframe>
        </div>`);

    $headerTab.append($header);
    $contentTab.append($content);
    $headerTab.find('li.active').removeClass('active');
    $header.children('a').tab('show');
    $headerTab.scrollingTabs('refresh').scrollingTabs('scrollToActiveTab');

    $($content.children('iframe')).on('load', function () {
      $content.removeClass('overlay');
    });

    Tabs.openList.push(`#${id}`);
    $(window.parent).trigger('open.tab');
  }

  static close(id) {
    $('#header-tab').find(`[data-tab-id=${id}]`).parents('li').remove();
    $(`#${id}`, $('#content-tab')).remove();
    $('#header-tab a:last').tab('show');
    $('#header-tab').scrollingTabs('refresh');

    let index = Tabs.openList.indexOf(`#${id}`);
    if (index > -1) {
      Tabs.openList.splice(index, 1);
    }
  }
};

Tabs.openList = [];

var Functions = new function () {

  this.detailsDataTables = function (format, td) {
    var dfd = jQuery.Deferred();

    $(td).off('click').on('click', function () {
      var tr = $(this).closest('tr');
      var row = $(this).closest('table').DataTable().row(tr);
      if (row.child.isShown()) {
        $('div.slider', row.child()).slideUp(function () {
          row.child.hide();
          tr.removeClass('shown');
          dfd.notify("hidden", row.data());
        });
      }
      else {
        row.child(format(row.data())).show();
        tr.addClass('shown');
        $('div.slider', row.child()).slideDown();
        dfd.notify("shown", row.data());
        $(document).trigger('ShowDetails', row.data());
      }
    });

    return dfd.promise();
  };

  this.linkAction = function (table, td) {
    $(table).find(td)
      .css('cursor', 'pointer')
      .click(function (e) {
        var href = $(this).parent().find('a').attr("href");
        if (href != undefined)
          window.location.href = $(this).parent().find('a').attr("href");
      });

  };

  this.diaPorExtenso = function (dia) {
    if (dia == 1)
      return "Domingo";
    if (dia == 2)
      return "Segunda";
    if (dia == 3)
      return "Terça";
    if (dia == 4)
      return "Quarta";
    if (dia == 5)
      return "Quinta";
    if (dia == 6)
      return "sexta";
    if (dia == 7)
      return "Sábado";
    return dia;
  };

  this.numeroDia = function (dia) {
    if (dia.toLowerCase() == "domingo")
      return 1;
    if (dia.toLowerCase() == "segunda")
      return 2;
    if (dia.toLowerCase() == "terça")
      return 3;
    if (dia.toLowerCase() == "quarta")
      return 4;
    if (dia.toLowerCase() == "quinta")
      return 5;
    if (dia.toLowerCase() == "sexta")
      return 6;
    if (dia.toLowerCase() == "sabado")
      return 7;
    return dia;
  };
  this.getTurno = function (turno) {
    if (turno == "M")
      return "Manhã";
    if (turno == "T")
      return "Tarde";
    if (turno == "N")
      return "Noite";
    return turno;
  };

  this.maskDocumento = function (doc) {
    if (IsNullOrEmpty(doc))
      return doc;

    doc = doc.trim();
    if (doc.length == 11) {
      doc = doc.substr(0, 3) + '.' + doc.substr(3, 3) + '.' + doc.substr(6, 3) + '-' + doc.substr(9, 2);
    }
    else if (doc.length == 14) {
      doc = doc.substr(0, 2) + '.' + doc.substr(2, 3) + '.' + doc.substr(5, 3) + '/' + doc.substr(8, 4) + '-' + doc.substr(12, 2);
    }
    return doc
  };

  this.dataPorExtencoAno = function (data) {
    var currentTime = data;
    if ((data instanceof Date) == false) {

      var parts = data.split("/");
      var yer = parts[2].length > 4 ? parts[2].substring(0, 4) : parts[2];
      currentTime = new Date(yer, parts[1] - 1, parts[0]);
    }
    var ano = currentTime.getFullYear();
    return ano;

  };

  this.dataPorExtencoMesAno = function (data) {
    var currentTime = data;
    if ((data instanceof Date) == false) {

      var parts = data.split("/");
      var yer = parts[2].length > 4 ? parts[2].substring(0, 4) : parts[2];
      currentTime = new Date(yer, parts[1] - 1, parts[0]);
    }
    var mes = currentTime.getMonth();
    var Mes = currentTime.getUTCMonth();

    if (mes < 10)
      mes = "0" + mes;

    var arrayMes = new Array();
    arrayMes[0] = "Janeiro";
    arrayMes[1] = "Fevereiro";
    arrayMes[2] = "Março";
    arrayMes[3] = "Abril";
    arrayMes[4] = "Maio";
    arrayMes[5] = "Junho";
    arrayMes[6] = "Julho";
    arrayMes[7] = "Agosto";
    arrayMes[8] = "Setembro";
    arrayMes[9] = "Outubro";
    arrayMes[10] = "Novembro";
    arrayMes[11] = "Dezembro";

    return arrayMes[Mes] + " de " + Functions.dataPorExtencoAno(data);

  };

  this.dataPorExtencoDiaMesAno = function (data) {
    var currentTime = data;
    if ((data instanceof Date) == false) {

      var parts = data.split("/");
      currentTime = new Date(parts[2], parts[1] - 1, parts[0]);
    }
    var dia = currentTime.getDate();
    var Dia = currentTime.getDay();
    if (dia < 10)
      dia = "0" + dia;

    arrayDia = new Array();
    arrayDia[0] = "Domingo";
    arrayDia[1] = "Segunda-Feira";
    arrayDia[2] = "Terça-Feira";
    arrayDia[3] = "Quarta-Feira";
    arrayDia[4] = "Quinta-Feira";
    arrayDia[5] = "Sexta-Feira";
    arrayDia[6] = "Sábado";
    return arrayDia[Dia] + ", " + dia + " de " + Functions.dataPorExtencoMesAno(data);

  };

  this.guid = function () {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      var r = (d + Math.random() * 16) % 16 | 0;
      d = Math.floor(d / 16);
      return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
  };

  this.parseFloatInput = function (input) {
    return Functions.parseFloat(input.val());
    //if (input.val() == "")
    //    return 0;
    //return parseFloat(input.autoNumeric("get"));
  };

  this.includeCSS = function (file_path) {
    var link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = file_path;
    document.getElementsByTagName('body')[0].appendChild(link);
  };

  this.includeJS = function (file_path) {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = file_path;
    document.getElementsByTagName('body')[0].appendChild(script);
  };

  this.isCNPJ = function (cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
      return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
      cnpj == "11111111111111" ||
      cnpj == "22222222222222" ||
      cnpj == "33333333333333" ||
      cnpj == "44444444444444" ||
      cnpj == "55555555555555" ||
      cnpj == "66666666666666" ||
      cnpj == "77777777777777" ||
      cnpj == "88888888888888" ||
      cnpj == "99999999999999")
      return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
      soma += numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
        pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
      return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
      soma += numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
        pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
      return false;

    return true;
  };

  this.autoCompleteOff = function () {
    $('input').attr('autocomplete', 'off');
  };

  // retorna o valor atributo informado como objeto json
  this.getValueAttribute = function (el, attributeName) {
    var node = el.attr(attributeName);

    if (typeof node == 'string') {
      return eval('(' + node + ')');
    }
    else if (typeof node == 'object') {
      if (node.getNamedItem(attributeName)) {
        return eval('(' + node.getNamedItem(attributeName)['value'] + ')');
      }
    }
  };

  // adiciona ou sobrescreve metodos do jquery.validate
  this.customValidators = function () {
    if ($.validator) {

      $.validator.setDefaults({
        highlight: function (element) {
          $(element).closest(".form-container").addClass("has-error");
          $("button").button('reset');
        },
        unhighlight: function (element) {
          $(element).closest(".form-container").removeClass("has-error");
        },
        errorPlacement: function (error, element) { },
        invalidHandler: function (form, validator) {

        }
      });
      $.validator.addMethod('date', function (value, element, params) {
        if (this.optional(element) || value == '__/__/____') {
          return true;
        }

        var ok = true;
        try {
          $.datepicker.parseDate('dd/mm/yy', value);
        }
        catch (err) {
          ok = false;
        }
        return ok;
      });
    }
  };

  // contem os handlers de elementos de entrada (a, input, textarea, select...)
  this.handlers = new function () {

    var ModalReport = function (el) {
      $this = el;

      var inputs = $("#" + $this.attr('data-form') + " :input");

      var $template = "",
        $originForm = ($this[0] && $this[0].tagName.toLowerCase() == 'form') ? $this : null,
        $form = null,
        params = "",
        modalId = "",
        href = ($this[0] && $this[0].tagName.toLowerCase() == 'form') ? $this.attr('action') : $this.attr('href'),
        formData = {};

      // define o valor 'modal-id' se ainda estiver definido
      if (!$this.data('modal-id')) {
        $this.data('modal-id', new Date().getTime());
      }

      // obtem o valor 'modal-id'
      modalId = $this.data('modal-id');

      //target = "_blank"
      //'<a href="{1}" target="_blank" class="btn btn-primary" rel="tooltip" data-placement="bottom" data-container="#{0}" title="Abre o relatório para visualização"><i class="icon-eye-open icon-white"></i> Abrir</a> ' +
      //(href.indexOf('?') > 0 ? (href + location.search.replace('?', '&')) : href + location.search)

      // se ainda nao existir uma estrutura no DOM para a janela
      if (!$this.data('has-modal')) {
        // define o template
        $template =
          '<div id="{0}" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-keyboard="false" >' +
          '<div class="modal-dialog" style="width:660px;">' +
          '<div class="modal-content">' +
          '<div class="modal-header">' +
          '<button type="button" title="Fechar" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
          '<h3 id="myModalLabel">Opções do relatório</h3>' +
          '</div>' +
          '<div class="modal-body" style="overflow:inherit">' +
          '<form action="{1}" class="nomargin" target="_blank">{2}' +
          '<div class="alert hide"></div>' +
          '<div class="form-group" style="margin-bottom:9px">' +
          '<textarea class="input-block-level form-control" style="height:90px" name="descricao" placeholder="Insira uma descrição para o relatório... (apenas para salvar ou agendar)"></textarea>' +
          '</div>' +
          '<div style="margin-bottom:5px; height:12px;" class="progress progress-striped active hide">' +
          '<div class="progress-bar" style="width: 100%;"></div>' +
          '</div>' +

          '<div class="btn-group"style="margin-right:8px">' +
          '<button class="btn btn-primary" type="submit" name="acao" value="1" rel="tooltip" data-placement="bottom" data-container="#{0}" title="Abre o relatório para visualização"><i class="fa fa-folder-open"></i> Abrir</button> ' +
          '<button class="btn btn-primary dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>' +
          '<ul class="dropdown-menu ddm-rel">' +
          '<li><a href="#" data-ext="pdf"><!--<i class="fa fa-file-o"></i> -->PDF</a></li>' +
          '<li class="divider"></li>' +
          '<li><a href="#" data-ext="xls"><!--<i class="fa fa-table"></i> -->XLS</a></li>' +
          '<li><a href="#" data-ext="txt"><!--<i class="fa fa-file-text"></i> -->TXT</a></li>' +
          '<li><a href="#" data-ext="doc"><!--<i class="fa fa-file-text-o"></i> -->DOC</a></li>' +
          '<li><a href="#" data-ext="rtf"><!--<i class="fa fa-file-text"></i> -->RTF</a></li>' +
          '<li><a href="#" data-ext="html"><!--<i class="fa fa-code"></i> -->HTML</a></li>' +
          '</ul>' +
          '</div>' +

          '<div class="btn-group"style="margin-right:8px">' +
          '<button class="btn btn-success" type="submit" name="acao" value="2" rel="tooltip" data-placement="bottom" data-container="#{0}" title="Salva o relatório no repositório"><i class="fa fa-save"></i> Salvar</button> ' +
          '<button class="btn btn-success dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>' +
          '<ul class="dropdown-menu ddm-rel">' +
          '<li><a href="#" data-ext="pdf">PDF</a></li>' +
          '<li class="divider"></li>' +
          '<li><a href="#" data-ext="xls">XLS</a></li>' +
          '<li><a href="#" data-ext="txt">TXT</a></li>' +
          '<li><a href="#" data-ext="doc">DOC</a></li>' +
          '<li><a href="#" data-ext="rtf">RTF</a></li>' +
          '</ul>' +
          '</div>' +

          '<div class="btn-group"style="margin-right:8px">' +
          '<button class="btn btn-default" type="submit" name="acao" value="9" rel="tooltip" data-placement="bottom" data-container="#{0}" title="Abre e salva relatório no repositório""><i class="fa fa-download"></i> Abrir e salvar</button> ' +
          '<button class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>' +
          '<ul class="dropdown-menu ddm-rel">' +
          '<li><a href="#" data-ext="pdf">PDF</a></li>' +
          '<li class="divider"></li>' +
          '<li><a href="#" data-ext="xls">XLS</a></li>' +
          '<li><a href="#" data-ext="txt">TXT</a></li>' +
          '<li><a href="#" data-ext="doc">DOC</a></li>' +
          '<li><a href="#" data-ext="rtf">RTF</a></li>' +
          '</ul>' +
          '</div>' +

          '<div class="form-group input-group pull-right nomargin">' +
          '<div class="btn-group">' +
          '<input type="text" name="data" class="form-control input-date" placeholder="dd/mm/aaaa hh:mm" mask="99/99/9999 99:99" title="Data e hora para o agendamento" style="width:153px" />' +
          '<button type="submit" name="acao" value="3" class="btn btn-default" rel="tooltip" data-placement="bottom" data-container="#{0}" title="Agenda a geração do relatório"><i class="fa fa-calendar"></i> Agendar</button>' +
          '<button class="btn btn-default dropdown-toggle" data-toggle="dropdown"><span class="caret"></span></button>' +
          '<ul class="dropdown-menu pull-right ddm-rel">' +
          '<li><a href="#" data-ext="pdf">PDF</a></li>' +
          '<li class="divider"></li>' +
          '<li><a href="#" data-ext="xls">XLS</a></li>' +
          '<li><a href="#" data-ext="txt">TXT</a></li>' +
          '<li><a href="#" data-ext="doc">DOC</a></li>' +
          '<li><a href="#" data-ext="rtf">RTF</a></li>' +
          '</ul>' +
          '</div>' +
          '</div>' +
          '</form>' +
          '</div>' +
          '</div>' +
          '</div>' +
          '</div>';

        if ($this[0] && $this[0].tagName.toLowerCase() == 'a') {
          var p = $.deparam($this.attr('href').split('?')[1]);

          for (var i in p) {
            params += `<input type="hidden" name="${i}" value="${p[i]}"/>`;
          };
        }



        // percorre todos elementos da querystring e adiciona no formulario como campo hidden
        for (var i in $.getQueryString()) {
          params += `<input type="hidden" name="${i}" value="${$.getQueryString(i)}"/>`;
        };


        if (inputs.length > 0) {
          // percorre todos elementos do form pai e adiciona no formulario como campo hidden
          inputs.each(function (key) {
            var name = $(this).attr("name");
            for (var i in $.getQueryString()) {
              if (name == i) {
                name = "";
                break;
              }
            };
            if (name) {
              params += `<input type="hidden" name="${name}" value="${$(this).val()}"/>`;
            }
          });
        }

        // cria objeto jquery com template
        $template = $($.format($template, modalId, href, params));

        // obtem o formulario
        $form = $template.find('form');

        // busca os botoes submit dentro do form e ouve o evento click
        $form.find('[type=submit]').click(function (e) {
          // seta o valor da acao no formulario
          $form.data('acao', e.target.value);
          var hidden = $('<input type="hidden" name="extensao"/>').val($(this).attr('data-ext'));
          $form.find('[type=hidden][name=extensao]').length == 0 ? $form.prepend(hidden) : $form.find('[type=hidden][name=extensao]').val($(this).attr('data-ext'));
          if ($originForm) {
            $originForm.find('[type=hidden][name=extensao]').length == 0 ? $originForm.prepend(hidden) : $originForm.find('[type=hidden][name=extensao]').val($(this).attr('data-ext'));
          }
        });

        $form.find('.dropdown-menu a').click(function (e) {
          e.preventDefault();
          $(this).parents('.btn-group').children('[type=submit]').attr('data-ext', $(this).attr('data-ext')).click();
        });

        // ouve o evento submit do formulario
        $form.submit(function (e) {
          var acao = $(this).data('acao');
          var sendForm = true;
          var $alert = $form.find('.alert');

          $alert.addClass('hide');
          $form.find('.form-group').removeClass('danger');

          // se a acao for "Abrir"
          if (acao == 1) {

            // se houver o form de origem
            if ($originForm) {
              // previne o envio do form da modal
              e.preventDefault();

              // submete o form de origem
              $originForm.data('forcesubmit', true).submit().data('forcesubmit', false);
            }
          }
          // se a acao for "Salvar" ou "Abrir e Salvar"
          else if ((acao == 2 || acao == 9) && Functions.isNullOrEmpty($form.find('textarea').val())) {
            // previne o envio do form da modal
            e.preventDefault();

            sendForm = false;
            $form.find('textarea').focus().addClass('input-validation-error');
            $alert.html('<strong>Atenção...</strong> insira uma descrição para o relatório!').addClass('alert-danger').removeClass('hide');
          }
          // se a acao for "Agendar"
          else if (acao == 3) {
            if (Functions.isNullOrEmpty($form.find('textarea').val())) {
              // previne o envio do form da modal
              e.preventDefault();

              sendForm = false;
              $form.find('textarea').focus().addClass('input-validation-error');
              $alert.html('<strong>Atenção...</strong> insira uma descrição para o agendamento!').addClass('alert-danger').removeClass('hide');
            }
            else if (Functions.isNullOrEmpty($form.find('.input-date').val())) {
              // previne o envio do form da modal
              e.preventDefault();

              sendForm = false;
              $form.find('.input-date').focus().addClass('input-validation-error');
              $alert.html('<strong>Atenção...</strong> informe uma data para o agendamento!').addClass('alert-danger').removeClass('hide');
            }
          }

          // se o formulario da modal for valido e a Acao for "Salvar", "Agendar" ou "Abrir e Salvar"
          if (sendForm && (acao == 2 || acao == 3 || acao == 9)) {

            // se a Acao for "Salvar" ou "Agendar"
            if (acao == 2 || acao == 3) {
              // previne o envio do form da modal
              e.preventDefault();
            }

            // serializa formulario do modal
            formData = $form.serializeArray();

            // adiciona a acao selecionada nos dados serializados
            formData.push({ name: 'acao', value: (acao == 9 ? 2 : acao) });

            // busca os elementos no DOM
            var $buttons = $form.find('[type=submit]');
            var $progress = $form.find('.progress');

            // desabilita botoes e remove mensagem
            $buttons.addClass('disabled').attr('disabled');
            $progress.removeClass("hide");

            // faz requisicao ajax
            $.ajax({
              type: $originForm ? $originForm.attr('method').toUpperCase() : 'GET',
              dataType: 'json',
              url: href,
              data: $originForm ? formData.concat($originForm.serializeArray()) : formData,
              success: function (data) {
                $buttons.removeClass('disabled').removeAttr('disabled');
                $progress.addClass("hide");
                $alert.text(data.Message).removeClass('alert-success alert-danger').addClass(data.Success ? 'alert-success' : 'alert-danger').removeClass('hide');
              }
            });

            // se houver o form de origem e a Acao for "Abrir e Salvar"
            if ($originForm && acao == 9) {
              // previne o envio do form da modal
              e.preventDefault();

              // submete o formulario original
              $originForm.data('forcesubmit', true).submit().data('forcesubmit', false);
            }
          }
        });

        // adiciona template ao body
        $('body').append($template);

        // renderiza tooltip
        Functions.handlers.tooltip();

        // renderiza mascara
        Functions.handlers.mask();

        // seta que ja existe estrutura da janela
        $this.data('has-modal', true);
      }

      // exibe modal
      $('#' + modalId).modal('show');
      $('#' + modalId).on('hide.bs.modal', function () {
        $this.data('has-modal', false);
        $('#' + modalId).remove();
      });
    };

    //function to initiate Touch Spin
    this.runTouchSpin = function () {
      $('.touchspin').each(function () {
        var min = $(this).attr('min');
        var max = $(this).attr('max');
        var step = $(this).attr('step');
        var decimals = $(this).attr('decimals');
        var postfix = $(this).attr('postfix');
        var verticalbuttons = $(this).attr('verticalbuttons');
        $(this).TouchSpin({
          min: min == null ? -1000000000 : min,
          max: max == null ? 1000000000 : max,
          step: step,
          decimals: decimals,
          verticalbuttons: verticalbuttons,
          verticalupclass: 'glyphicon glyphicon-plus',
          verticaldownclass: 'glyphicon glyphicon-minus'
        });
      });
    };
    this.relogio = function () {

      $('.relogio').each(function () {
        Functions.handlers.moveRelogio($(this));
      });
    };

    this.timer = function (elemento, type, message, date) {
      var agora = new Date();
      agora = date == undefined ? new Date(agora.getFullYear(), agora.getMonth(), agora.getDate()) : date;
      var hora = agora.getHours()
      var minuto = agora.getMinutes()
      var segundo = agora.getSeconds();
      var msg = message == undefined ? "" : message;

      str_segundo = new String(segundo)
      if (str_segundo.length == 1)
        segundo = "0" + segundo

      str_minuto = new String(minuto)
      if (str_minuto.length == 1)
        minuto = "0" + minuto

      str_hora = new String(hora)
      if (str_hora.length == 1)
        hora = "0" + hora

      result = msg + " (" + hora + ":" + minuto + ":" + segundo + ")";
      if (type == "text") {
        $(elemento).text(result);
      } else {
        $(elemento).val(result);
      }

      setTimeout(function () {
        agora = new Date(agora.getTime() + 1000 * 1);
        Functions.handlers.timer(elemento, type, message, agora);
      }, 1000);
    };

    this.moveRelogio = function (elemento, type, message) {
      agora = new Date()
      hora = agora.getHours()
      minuto = agora.getMinutes()
      segundo = agora.getSeconds();
      var msg = message == undefined ? "" : (message + ": ")

      str_segundo = new String(segundo)
      if (str_segundo.length == 1)
        segundo = "0" + segundo

      str_minuto = new String(minuto)
      if (str_minuto.length == 1)
        minuto = "0" + minuto

      str_hora = new String(hora)
      if (str_hora.length == 1)
        hora = "0" + hora

      result = msg + agora.toLocaleDateString() + " " + hora + ":" + minuto + ":" + segundo;
      if (type == "text") {
        $(elemento).text(result);
      } else {
        $(elemento).val(result);
      }

      setTimeout(function () {
        Functions.handlers.moveRelogio(elemento, type, message);
      }, 1000);
    };

    //function to initiate Mask Money
    this.runMaskMoney = function () {

      $('.currency').each(function () {

        var aSep = $(this).attr('asep');
        var aDec = $(this).attr('adec');
        var vMin = $(this).attr('vmin');
        var vMax = $(this).attr('vmax');
        var mDec = $(this).attr('mdec');
        var moeda = $(this).attr('moeda');
        var metod = $(this).attr('metod');
        var format = ($(this).attr('format') == "True" || $(this).attr('format') == "true") ? true : false;

        vMin = IsNullOrEmpty(vMin) ? '0' : vMin;
        vMax = IsNullOrEmpty(vMax) ? '9999999999.99999' : vMax;
        aSep = aSep == null ? '.' : aSep;
        aDec = IsNullOrEmpty(aDec) ? ',' : aDec;
        mDec = IsNullOrEmpty(mDec) ? '2' : mDec;
        metod = IsNullOrEmpty(metod) ? "init" : metod;

        $(this).autoNumeric(metod, {
          aSep: aSep,
          aDec: aDec,
          vMin: vMin,
          vMax: vMax,
          mDec: mDec,
        })
          .css('text-align', 'right')
          .on("change", function (e) {
            if (!IsNullOrEmpty(this.value))
              this.value = Functions.numberFormat(Functions.parseFloat(this.value), mDec, format);
          });
      });

      $(".taxa").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: -999.99,
        vMax: 999.99
      });

      $(".peso").autoNumeric('init', {
        aSep: '.',
        aDec: ',',
        nBracket: '(,)',
        vMin: 0,
        mDec: 3,
        vMax: 199.99
      });

      $(".integer").autoNumeric('init', {
        vMin: 0,
        vMax: 9999999,
        mDec: 0,
        aSep: ''
      });
    };

    var PhoneMask = function (input) {
      input.unmask();
      var fone = input.val().replace(/\D/g, '');
      if (fone.length <= 10) {
        input.mask("(99) 9999-9999?9");
      } else {
        input.mask("(99) 99999-999?9");
      }
    };

    // todos elementos com o atributo [data-modal-report=true] ao ser clicado abrirao uma janela com opcoes para relatorio
    this.modalReport = function () {
      $('body').on({
        click: function (e) {
          e.preventDefault();
          ModalReport($(this));
        }
      }, 'a[data-modal-report=true]');

      $('body').on({
        submit: function (e) {
          var $this = $(this);

          if (!$this.data('forcesubmit')) {
            e.preventDefault();
          }
          if ($.validator) {
            if ($this.valid()) {
              ModalReport($this);
            }
          }
        }
      }, 'form[data-modal-report=true]');
    };

    // exibe tooltip para todos elementos com o atributo [rel=tooltip]
    this.tooltip = function () {
      //$('[rel=tooltip]').tooltip();
    };

    // fix para placeholder em navegadores sem suporte
    this.placeholder = function () {
      if (!(typeof Modernizr == 'undefined') && !Modernizr.input.placeholder) {
        Placeholders.init();
      }
    };

    // fix para autofocus para navegadores sem suporte
    this.autofocus = function () {
      if (!(typeof Modernizr == 'undefined') && !Modernizr.input.autofocus) {
        // seta o foco no primeiro elemento com o atributo [autofocus]
        $('[autofocus]')[0].setFocus();
      }
    };

    this.autoselect = function () {
      //$('[data-select-onfocus]').setFocus({
      //    callback: function () {
      //        $(this).select();
      //    }
      //});
    };

    // todos elementos com a classe "datepicker" exibirao um calendario
    this.datepicker = function () {
      //$('.datepicker').datepicker({
      //    format: 'mm/dd/yyyy',
      //    startDate: '-3d'
      //})
      $('.datepicker').each(function () {
        //    var _attrs = {
        //        changeMonth: true,
        //        changeYear: true,
        //        showOtherMonths: true,
        //        selectOtherMonths: true,
        //        showButtonPanel: true,
        //        yearRange: 'c-15:c+15'
        //    };

        //    var attrs = Functions.getValueAttribute($(this), 'datepicker');
        //    attrs = $.extend(_attrs, attrs);

        $(this).datepicker({
          locale: 'pt-BR',
          format: 'DD/MM/YYYY'
        });
      });
      $('.monthyearpicker').each(function () {
        $(this).datetimepicker({
          locale: 'pt-BR',
          format: 'MM/YYYY'
        });
      });
      $('.timepicker').each(function () {

        $(this).datetimepicker({
          locale: 'pt-BR',
          format: 'LT',
          stepping: $(this).data('stepping')
        });
      });
      $('.datetimepicker').each(function () {

        $(this).datetimepicker({
          locale: 'pt-BR',
        });
      });
    };

    // todos elementos com a classe "spinner" ou com o atributo [spinner] exibirao a funcionalidade
    this.spinner = function () {
      $('.spinner, [spinner]').each(function () {
        var _attrs = {
          culture: 'pt-BR'
        };

        var attrs = Functions.getValueAttribute($(this), 'spinner');
        attrs = $.extend(_attrs, attrs);

        $(this).spinner(attrs)
          .addClass('ui-spinner-box')
          .parent()
          .find(".ui-spinner-button")
          .replaceWith(function () {
            return $("<button>", {
              type: 'button',
              'class': this.className,
              tabindex: -1
            });
          });

        if (attrs) {
          if (attrs.start) {
            $(this).spinner("value", attrs.start);
          }
        }
      });
    };

    // exibe funcionalidade de contador de caracteres a todos elementos textarea com o atributo [maxlength]
    this.counterTextarea = function () {
      $('textarea[maxlength]:not([data-counter=true]):not(:disabled):not([readonly])').each(function () {
        if ($(this).data('native-counter')) {
          return;
        }
        var defaults = {};
        defaults.goal = $(this).attr('maxlength');
        defaults.text = ($(this).attr('data-textarea-label') || true).toString().toLowerCase() === 'true';
        defaults.type = $(this).attr('data-textarea-type');
        defaults = $.extend({ type: 'char' }, defaults);
        $(this).attr('data-counter', true).counter(defaults);
      });
      $("textarea[maxlength]").bind('input propertychange', function () {
        let enteredText = this.value.split("\n");
        let enter = (enteredText.length - 1);

        let maxLength = $(this).attr('maxlength');
        length = enter + $(this).val().length;


        if (length > maxLength) {
          $(this).val($(this).val().substring(0, maxLength - enter));
        }
      })
    };

    this.textaAreaAutoSize = function () {
      autosize(document.querySelectorAll('textarea'));
    };

    // adiciona a funcionalidade de mascara a todos inputs com atributo [mask]
    this.mask = function () {
      $('input[mask]').each(function () {
        $(this).mask($(this).attr('mask'));
      });

      $('input.mask.fone').each(function () {
        PhoneMask($(this));
      })
        .focusout(function () {
          PhoneMask($(this));
        });
    };

    // adiciona a funcionalidade de foco a todos inputs com atributo [autofocus]
    this.autofocus = function () {
      $('input[autofocus]').focus();
    };

    // remove a funcionalidade mascara para o elemento informado
    this.unmask = function ($input) {
      if ($input instanceof jQuery) {
        $input.val($input.val().replace(/[\s\-\.\/\(\)]/g, ''));
      }
      else {
        return $input.replace(/[\s\-\.\/\(\)]/g, '');
      }
    };

    // adiciona a funcionalidade filter para todos elementos com o atributo [filter]
    // opcoes de filter: 'alphanumeric', 'numeric', 'alpha', 'floatnumber', 'decimal'
    this.filter = function () {
      $('input[filter]').each(function () {
        var filters = $(this).attr('filter').toString().split('|');
        var re = new RegExp(/\((.+?|s?)\)/g);

        for (var i = 0; i < filters.length; i++) {
          var filter = $.trim(filters[i]).replace(re, '');
          var param = $.trim(filters[i]).replace(filter, '').replace(/\(\)/, '');
          if (param) {
            param = eval(param);
          }

          switch (filter) {
            case 'alphanumeric':
              $(this).alphanumeric(param);
              break;

            case 'numeric':
              $(this).numeric(param);
              break;

            case 'alpha':
              $(this).alpha(param);
              break;

            case 'floatnumber':
            case 'decimal':

              var aSep = $(this).attr('asep');
              var aDec = $(this).attr('adec');
              var vMin = $(this).attr('vmin');
              var vMax = $(this).attr('vmax');
              var mDec = $(this).attr('mdec');
              var moeda = $(this).attr('moeda');
              var metod = $(this).attr('metod');
              var format = ($(this).attr('format') == "True" || $(this).attr('format') == "true") ? true : false;

              vMin = IsNullOrEmpty(vMin) ? '0' : vMin;
              vMax = IsNullOrEmpty(vMax) ? '9999999999.99999' : vMax;
              metod = IsNullOrEmpty(metod) ? "init" : metod;
              aSep = aSep == null ? '.' : aSep;
              aDec = IsNullOrEmpty(aDec) ? ',' : aDec;
              mDec = IsNullOrEmpty(mDec) ? '2' : mDec;

              $(this).autoNumeric(metod, {
                aSep: aSep,
                aDec: aDec,
                vMin: vMin,
                vMax: vMax,
                mDec: mDec,
              }).on("change", function (e) {
                if (!IsNullOrEmpty(this.value))
                  this.value = Functions.numberFormat(Functions.parseFloat(this.value), mDec, format);
              });
              break;
          }
        }
      });
    };

    // captura todos os dados da tela query e envia para a janela pai
    this.selectModalItem = function (data) {
      $('[prefix="' + $('#context-prefix').val() + '"][from]', parent.window.document).each(function () {
        $(this).val(data[$(this).attr('from')]);
      });
      window.parent.$(window.parent.document).trigger($('#context-prefix').val() + '_modal-close');
    };

    // ao clicar no elemento com o atributo [data-form-type=submit], submete o formulario pai
    this.actionSubmitForm = function () {
      $('[data-form-type=submit]').click(function (e) {
        e.preventDefault();

        if ($(this).attr('data-form-element')) {
          $($(this).attr('data-form-element')).submit();
        }
        else {
          $(this).parents('form').submit();
        }
      });
    };

    // ao clicar no elemento com o atributo [data-form-type=submit], reseta o formulario pai
    this.actionResetForm = function () {
      $('[data-form-type=reset]').click(function (e) {
        e.preventDefault();
        //$(this).parents('form')[0].reset();
        window.location.href = window.location.href;
      });
    };

    // previne que o formulario seja submetido mais de uma vez
    this.preventManySubmitForm = function () {
      $('form').submit(function (e) {
        //$('[mask]').each(function () {
        //    window.Functions.handlers.unmask($(this));
        //});
        $('select').prop('disabled', false);
        $(this).submit(function () {
          e.preventDefault();
        });
      });
    };

    // previne que o formulario seja submetido ao pressionar a tecla ENTER
    this.preventSubmitFormOnEnter = function () {
      // busca todos elementos que nao tenham a classe "enter" ou o id filter
      $('input:not(.enter, #filter), select:not(.enter)').keypress(function (e) {
        if (e.which == 13) {
          e.preventDefault();
        }
      });
    };

    // captura todos elementos que contenham o seletor informado e faz redirecionamento
    this.redirectPageSize = function (selector) {
      $(selector ? selector : '.datatable-pagesize').bind('change', function () {
        Functions.redirectPageSize($(this).val());
      });
    };

    this.loadingState = function (reset) {
      if (reset === true) {
        $('[data-loading-text]').button('reset');
        $('.on-loading-state').removeClass('on-loading-state').prop('disabled', false);
        return;
      }

      $('[data-loading-text]').click(function () {
        $('[data-loading-text]').button('loading');
        $('[type="submit"]:not([data-loading-text])').addClass('on-loading-state').prop('disabled', true);
      });
    };

    this.select2 = function () {


      $('[datadrop="dataselect2"]').each(function (index) {
        var self = $(this);

        var data = JSON.parse(self.attr("data").replace(/&quot;/ig, '"'));

        self.select2({
          data: data,
          language: "pt-BR",
          theme: "bootstrap",
          width: 'width: 100%;',
          formatResult: function (data) {
            return '<div class="row">' +
              '<div class="col-lg-2  col-md-2  col-sm-3 col-xs-3">' + data.id + '</div>' +
              '<div class="col-lg-10 col-md-10 col-sm-9 col-xs-9">' + data.text + '</div>' +
              '</div>';
          }
        });
      });
      $('[datadrop="select2"]').each(function (index) {
        var $self = $(this);

        $self.select2({
          language: "pt-BR",
          placeholder: $self.attr("placeholder"),
          theme: "bootstrap",
          selectOnClose: true,
          width: 'width: 100%;',
        }).on('select2:select', function (evt) {
          $self.next().find('.select2-selection').focus();
        }).on('select2:close', function (evt) {
          $self.next().find('.select2-selection').focus();
        }).on('select2:open', function (evt) {
          $(".select2-search__field").css('text-transform', 'uppercase').blur(function () { this.value = this.value.toUpperCase(); });
        });

        $self.next().find('.select2-selection').keydown(function (e) {
          if (e.keyCode == 40) {
            $self.select2('open');

            e.preventDefault();
          } else if ((e.keyCode >= 48 && e.keyCode <= 90)) {
            var letra = String.fromCharCode(e.keyCode).trim();
            $self.select2('open');

            var $search = $select.data('select2').dropdown.$search || $select.data('select2').selection.$search;

            $search.val(letra);
            $search.trigger('keyup');

            e.preventDefault();

          }
        });
      });

      $.fn.setData = function (value) {
        $(this).val(value).trigger("change");
        return this;
      };
    }

    this.buttonPrint = function () {

      $('[data-print-index]').off('click').on('click', function (e) {
        e.preventDefault();
        let $form = document.querySelector('form');
        let $ext = $form.querySelector('[name="report.Extensao"]');
        if ($ext) {
          $ext.value = $(this).data('format');
        }
        else {
          $form.insertAdjacentHTML('beforeend', `<input type="hidden" name="report.Extensao" value="${$(this).data('format')}" />`);
        }

        let params = $("form").serialize();
        let paramsForm = params.toLowerCase().split("&");
        let paramsQuerystring = window.location.search.replace("?", "").split("&");
        let allParams = paramsForm.concat(paramsQuerystring);
        let querySearch = "";

        allParams.forEach(function (fragment) {
          if (!window.IsNullOrEmpty(fragment)) {
            querySearch += `&${fragment}`;
          }
        });

        this.href = `${this.href.split("?")[0]}?${querySearch}`;
        if ($(this).data('format') === "pdf") {
          (new Functions.modalPDF()).open(this.href, true, true);
        }
        else {
          window.open(this.href)
        }
      });

      $('[data-print]').off('click').click(function (e) {
        e.preventDefault();
        const format = $(this).data("format");

        if (format === 'pdf') {
          document.querySelectorAll('[data-parent]').forEach(el => {
            el.setAttribute('disabled', true);
            if (el.getAttribute('data-format') === 'pdf') {
              el.innerText = 'Carregando...';
            }
          });
        }

        let $input = $(`<input type="hidden" name="report.Extensao" value="${format}" />`);
        $("form").append($input).submit();
        $input.remove();
      });

      $('[data-save]').off('click').click(function (e) {
        e.preventDefault();
        let $input = $(`<input type="hidden" name="report.Extensao" value="${$(this).data("format")}" />`);
        let $input2 = $(`<input type="hidden" name="report.Acao" value="2" />`);
        $("form").append($input).append($input2).submit();
        $input.remove();
        $input2.remove();
      });

      $('[data-agenda]').off('click').click(function (e) {
        e.preventDefault();
        var data = $('[data-input-agenda]').val();

        if (window.IsNullOrEmpty(data)) {
          alert("Para agendar o relatório informe uma data e hora válida!");
          $('[data-input-agenda]').focus();
          return false;
        }

        let $input = $(`<input type="hidden" name="report.Extensao" value="${$(this).data("format")}" />`);
        let $input2 = $(`<input type="hidden" name="report.Acao" value="3" />`);
        let $input3 = $(`<input type="hidden" name="report.Data" value="${data}" />`);
        $("form").append($input).append($input2).append($input3).submit();
        $input.remove();
        $input2.remove();
        $input3.remove();
      });

      $('[data-agenda-lateral]').off('click').click(function (e) {
        var data = $('[data-input-agenda-lateral]').val();

        if (window.IsNullOrEmpty(data)) {
          alert("Para agendar o relatório informe uma data e hora válida!");
          $('[data-input-agenda-lateral]').focus();
          return false;
        }

        let $input = $(`<input type="hidden" name="report.Extensao" value="${$(this).data("format")}" />`);
        let $input2 = $(`<input type="hidden" name="report.Acao" value="3" />`);
        let $input3 = $(`<input type="hidden" name="report.Data" value="${data}" />`);
        $("form").append($input).append($input2).append($input3).submit();
        $input.remove();
        $input2.remove();
        $input3.remove();
      });
    }

    this.message = function () {

      jQuery.extend(jQuery.validator.messages, {
        required: "Este campo é obrigatórios.",
        remote: "Por favor corrigir esse campo.",
        email: "Por favor insira um endereço de e-mail válido.",
        url: "Por favor, digite uma URL válida.",
        date: "Digite uma data válida.",
        dateISO: "Digite uma data válida (ISO).",
        number: "Por favor insira um número válido.",
        digits: "Introduza apenas dígitos.",
        creditcard: "Por favor insira um número de cartão de crédito válido.",
        equalTo: "Por favor, insira o mesmo valor novamente.",
        accept: "Por favor insira um valor com uma extensão válida.",
        maxlength: $.validator.format("Por favor, insira mais do que {0} caracteres."),
        minlength: $.validator.format("Por favor, insira pelo menos {0} caracteres."),
        rangelength: $.validator.format("Por favor insira um valor entre {0} e {1} caracteres."),
        range: $.validator.format("Por favor insira um valor entre {0} e {1}."),
        max: $.validator.format("Por favor insira um valor menor ou igual a {0}."),
        min: $.validator.format("Por favor insira um valor maior ou igual a {0}.")
      });
    };

    this.confirmBeforeUnload = function () {
      if (jQuery().areYouSure) {
        $('form:not(#searchForm)').areYouSure();
      }
    };
  };

  // detecta a versao do internet explorer e adiciona a versao no elemento html
  this.detectIeVersion = function () {
    if (bowser.msie) {
      $("html").addClass("ie").addClass("ie" + parseInt(bowser.version));
    }
  };

  // Privado - Corrige o tamanho dos elementos com a classe .container-nav-list
  var _fixNavList = function () {
    var container = $('.container-nav-list.affix');
    container.width(container.parent('.span2').width()
      - parseInt(container.css('border-left-width')) - parseInt(container.css('padding-left'))
      - parseInt(container.css('border-right-width')) - parseInt(container.css('padding-right'))
    );
  };

  // Corrige o tamanho dos elementos com a classe .container-nav-list
  this.fixNavList = function () {
    _fixNavList();

    $(window).bind('resize', function () {
      _fixNavList();
    });
  };

  // remove tabindex dos campos readonly
  this.tabIndexNone = function () {
    $('input[readonly]').attr('tabindex', '-1');
  };

  this.numberFormat = function (value, mDec, flDecimal) {

    aSep = '.';
    aDec = ',';
    mDec = IsNullOrEmpty(mDec) ? 2 : mDec;

    var n = value;
    var prec = mDec;
    n = !isFinite(+n) ? 0 : +n;
    prec = !isFinite(+prec) ? 0 : Math.abs(prec);
    var sep = aSep;
    var dec = aDec;

    var s = (prec > 0) ? n.toFixed(prec) : Math.round(n).toFixed(prec);

    var abs = Math.abs(n).toFixed(prec);
    var _, i;

    if (abs >= 1000) {
      _ = abs.split(/\D/);
      i = _[0].length % 3 || 3;

      _[0] = s.slice(0, i + (n < 0)) +
        _[0].slice(i).replace(/(\d{3})/g, sep + '$1');

      s = _.join(dec);
    } else {
      s = s.replace('.', dec);
    }
    if (flDecimal != undefined && flDecimal == true) {
      s = Functions.parseFloat(s).toString().replace('.', dec);
    }
    return s;
  };

  // redireciona para com requisicao de paginacao
  this.redirectPageSize = function (pagesize) {
    var url = '';
    if (window.location.href.match(/pagesize=(\d*)/g)) {
      url = window.location.href
        .replace(/pagesize=(\d*)/g, 'pagesize=' + pagesize)
        .replace(/page=(\d*)/g, 'page=1');
    }
    else {
      url = window.location.href + '&pagesize=' + pagesize;
    }
    window.location.href = url;
  };

  // retorna true se o valor for um numero
  this.isNumber = function (value) {
    return !isNaN(parseFloat(value)) && isFinite(value);
  };

  // retorna true se o valor for nulo ou vazio
  this.isNullOrEmpty = function (value) {
    if (value == null) {
      return true;
    }
    return (!value || $.trim(value) == '' || value == null);
  };

  // executa acao de acordo com a requisicao informada
  this.checkRequest = function (request) {
    if (request.status == 401) {
      window.location.href = $('body').attr('data-url-auth');
    }
    else {
      if (request.status) {
        this.alert(request.responseText);

      }
    }
  };

  this.alert = function (body, title) {
    if (!title) {
      title = "Aviso";
    }
    var modal = '<div id="modal" class="modal fade">' +
      '<div class="modal-dialog" style="padding-top: 124px;">' +
      '<div class="modal-content">' +
      '<div class="modal-header" style="padding: 5px">' +
      '<h4 class="modal-title" style="">' + title + '</h4>' +
      '</div>' +
      '<div class="modal-body"> ' + body + '</div>' +
      '<div class="modal-footer" style="padding: 0;">' +
      '<button type="button" class="btn btn-sm btn-default" style="margin: 3px !important;" data-dismiss="modal">Fechar</button>' +
      '</div>' +
      '</div>' +
      '</div>' +
      '</div>';

    $('body').append(modal);
    $("#modal").modal('show');
    $('#modal').on('hide.bs.modal', function () {
      $("#modal").remove();
    });
  };

  this.parseInt = function (str) {

    if (window.IsNullOrEmpty(str)) {
      return 0;
    }
    if (!isNaN(str)) {
      var value = parseInt(str);
      return value;
    }
    value = isNaN(value) ? 0 : value;
    return value;
  };

  this.parseFloat = function (str) {
    str = str + "";
    if (window.IsNullOrEmpty(str)) {
      return 0;
    }

    str = str.split(".").join("");
    str = str.split(",").join(".");

    var value = parseFloat(str);
    value = isNaN(value) ? 0 : value;
    return value;
  };

  // retorna o valor JSON do elemento informado
  this.getJsonData = function (element, remove) {
    var result = [];
    remove = remove == undefined ? true : remove;

    if ($(element).get(0)) {
      result = eval($(element).val());
      if (remove) {
        $(element).remove();
      }
    }

    return result;
  };

  // captura texto selecionado no DOM
  this.getSelectedText = function () {
    //TODO: verificar a possibilidade de buscar o texto selecionado de um elemento especifico (DIV)
    var text = "";
    if (window.getSelection) {
      text = window.getSelection().toString();
    } else if (document.selection && document.selection.type != "Control") {
      text = document.selection.createRange().text;
    }
    return text;
  };

  // encode html
  this.htmlEncode = function (value) {
    return $('<div/>').text(value).html();
  };

  // decode html
  this.htmlDecode = function (value) {
    return $('<div/>').html(value).text();
  };

  // flashmessage
  this.flashMessage = new function () {
    // adiciona mensagem
    this.add = function (element, message) {
      var _alert;
      if ($.data(element[0], 'modal_message')) {
        _alert = $.data(element[0], 'modal_message');
      }
      else {
        _alert = $('<div class="alert" style="margin:10px"/>');
      }
      _alert.text(message);
      $.data(element[0], 'modal_message', _alert);
      _alert.insertAfter(element);
    };

    // remove mensagem
    this.remove = function (element) {
      var _alert = $.data(element[0], 'modal_message');
      if (_alert) {
        _alert.remove();
      }
    };
  };

  // adiciona input hidden em formulario informado
  this.addHiddenInput = function (form, id, attributes) {
    var input, _exists = false;
    var _input = $('#' + id, form);

    if (_input.length) {
      input = _input;
      _exists = true;
    }
    else {
      input = $('<input type="hidden" id="' + id + '"/>');
    }

    $.each(attributes, function (att) {
      if (att != 'type' && att != 'id') {
        input.attr(att, attributes[att]);
      }
    });

    if (!_exists) {
      $(form).prepend(input);
    }

    return input;
  };

  // progressbar
  this.progressbar = new function () {
    // atuliza progressbar
    this.update = function (progress, index, total) {
      var perc = 5 * total / 100;
      if (Math.floor((index + 1) % perc) === 0 || Math.floor((index + 1) % perc) === .5) {
        progress.update(((index + 1) / total) * 100);
      }
    };
  };

  // pdf modal
  this.modalPDF = function () {
    var _dialog = null,
      _onRequestAppend = false;

    var _open = function (force, url, printable, downloadable) {
      const urlParam = new URLSearchParams(`version=${semver.GetVersion}&url=${encodeURIComponent(url)}&force=${force}&printable=${printable}&downloadable=${downloadable}`);
      _dialog = bootbox.dialog({
        size: 'large',
        className: 'report-modal no-padding',
        onEscape: true,
        backdrop: true,
        closeButton: true,
        animate: false,
        message: `<iframe src="/rp/sistema/report/viewer?${urlParam}"></iframe>`
      });

      _dialog.init(function () {
        _dialog.find('.modal-dialog').css('transform', 'none');
        _dialog.find('.modal-content').addClass('overlay');
      });

      _dialog.on("hidden.bs.modal", function () {
        localforage.setDriver(localforage.INDEXEDDB).then(function () {
          localforage.removeItem(window.btoa(unescape(encodeURIComponent(location.pathname))));
        });
      });
    };

    return {
      open: function (url, printable, downloadable) {
        _open(false, url, printable, downloadable);
      },

      openBase64: function (base64, printable, downloadable) {
        const parsed = JSON.parse(base64.replace(/&quot;/g, '"'))
        localforage.setDriver(localforage.INDEXEDDB).then(function () {
          const key = window.btoa(unescape(encodeURIComponent(location.pathname)));
          localforage.setItem(key, parsed.string);
          _open((parsed.length > (0.95 * 1024 * 1024)), `blob://${key}`, printable, downloadable);
        });
      }
    };
  };

  this.openModalPDF = function (url, printable, downloadable) {
    (new Functions.modalPDF()).open(url, printable, downloadable);
    return false;
  };

  this.dataToPrint = function (url, type) {
    $.loadingMessage("show", "Enviando para a impressora");
    $.ajax({
      type: 'GET',
      dataType: 'json',
      url: url,
      data: { type: type },
      success: function (result) {
        //new PNotify({ title: "Aviso!", text: result, type: "success", icon: "fa fa-print" });
        //$.loadingMessage("hide");
        $.ajax({
          contentType: 'application/json',
          dataType: 'json',
          type: 'POST',
          url: "http://localhost:3003/print-printetiqueta",
          data: JSON.stringify(result),
          success: function (data) {
            $.loadingMessage("hide");
            //new PNotify({ title: "Aviso!", text: data, type: "success", icon: "fa fa-print" });

          },
          error: function (request) {
            //Functions.checkRequest(request)
            $.loadingMessage("hide");

            if (request.status == 0 || request.status == 404 || request.status == 500 || !request.responseText) {
              //new PNotify({ title: "Aviso!", text: "Não foi possível enviar, entre em contato com o administrador", type: "error", icon: "fa fa-exclamation-triangle" });
            } else {
              //new PNotify({ title: "Aviso!", text: request.responseText, type: request.status === 200 ? "success" : "error", icon: request.status === 200 ? "fa fa-print" : "fa fa-exclamation-triangle" });
            }
          },
        })
      },
      error: function (request) {
        //Functions.checkRequest(request)

        if (request.status == 0 || request.status == 404 || request.status == 500 || !request.responseText) {
          //new PNotify({ title: "Aviso!", text: "Não foi possível enviar, entre em contato com o administrador", type: "error", icon: "fa fa-exclamation-triangle" });
        } else {
          //new PNotify({ title: "Aviso!", text: request.responseText, type: "error", icon: "fa fa-exclamation-triangle" });
        }
      }
    });
    return false;
  };
}

var IsNullOrEmpty = function (value) {
  return Functions.isNullOrEmpty(value);
};

var isMobile = {
  Android: function () {
    return /Android/i.test(navigator.userAgent);
  },
  BlackBerry: function () {
    return /BlackBerry/i.test(navigator.userAgent);
  },
  iOS: function () {
    return /iPhone|iPad|iPod/i.test(navigator.userAgent);
  },
  Windows: function () {
    return /IEMobile/i.test(navigator.userAgent);
  },
  any: function () {
    return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Windows());
  }
};

if (typeof String.prototype.endsWith !== 'function') {
  String.prototype.endsWith = function (suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
  };
}

if (typeof String.prototype.replaceLast !== 'function') {
  String.prototype.replaceLast = function (find, replace) {
    var index = this.lastIndexOf(find);

    if (index >= 0) {
      return this.substring(0, index) + replace + this.substring(index + find.length);
    }

    return this.toString();
  };
}

/**************************************************************************
* http://blog.shiguenori.com/2009/03/21/number_format-em-javascript/
***************************************************************************/
var NumberFormat = function (number, decimals, dec_point, thousands_sep) {
  // %   nota 1: Para 1000.55 retorna com precisão 1 no FF/Opera é 1,000.5, mas no IE é 1,000.6
  // *     exemplo 1: number_format(1234.56);
  // *     retorno 1: '1,235'
  // *     exemplo 2: number_format(1234.56, 2, ',', ' ');
  // *     retorno 2: '1 234,56'
  // *     exemplo 3: number_format(1234.5678, 2, '.', '');
  // *     retorno 3: '1234.57'
  // *     exemplo 4: number_format(67, 2, ',', '.');
  // *     retorno 4: '67,00'
  // *     exemplo 5: number_format(1000);
  // *     retorno 5: '1,000'
  // *     exemplo 6: number_format(67.311, 2);
  // *     retorno 6: '67.31'

  var n = number, prec = (decimals) ? decimals : 2;
  n = !isFinite(+n) ? 0 : +n;
  prec = !isFinite(+prec) ? 0 : Math.abs(prec);
  var sep = (typeof thousands_sep == 'undefined') ? '.' : thousands_sep;
  var dec = (typeof dec_point == 'undefined') ? ',' : dec_point;

  var s = (prec > 0) ? n.toFixed(prec) : Math.round(n).toFixed(prec); //fix for IE parseFloat(0.55).toFixed(0) = 0;

  var abs = Math.abs(n).toFixed(prec);
  var _, i;

  if (abs >= 1000) {
    _ = abs.split(/\D/);
    i = _[0].length % 3 || 3;

    _[0] = s.slice(0, i + (n < 0)) +
      _[0].slice(i).replace(/(\d{3})/g, sep + '$1');

    s = _.join(dec);
  } else {
    s = s.replace('.', dec);
  }

  return s;
}


/*******************************************************************************
* contains
* Verifica se a palavra contem o termo informado
*******************************************************************************/

String.prototype.contains = function (str, ignoreCase) {
  if (ignoreCase === undefined || ignoreCase === null) {
    ignoreCase = true;
  }
  return (ignoreCase ? this.toUpperCase() : this).indexOf(ignoreCase ? str.toString().toUpperCase() : str.toString()) >= 0;
};

/**************************************************************************
* Jquery regex
* http://james.padolsey.com/javascript/regex-selector-for-jquery/
**************************************************************************/
jQuery.expr[':'].regex = function (elem, index, match) {
  var matchParams = match[3].split(','),
    validLabels = /^(data|css):/,
    attr = {
      method: matchParams[0].match(validLabels) ?
        matchParams[0].split(':')[0] : 'attr',
      property: matchParams.shift().replace(validLabels, '')
    },
    regexFlags = 'ig',
    regex = new RegExp(matchParams.join('').replace(/^\s+|\s+$/g, ''), regexFlags);
  return regex.test(jQuery(elem)[attr.method](attr.property));
}

/*******************************************************************************
* Converte uma data de um JsonResult
* Permite formatar caso seja passado um formato para a data. ex: dd/MM/yyyy
* Requer a bibliteca Date.js
*******************************************************************************/
String.prototype.toDateFromJson = function (format) {
  if (this) {
    if (/\/Date\(-?[\d]+\)\//.test(this)) {
      var extract = parseInt(/-?[\d]+/.exec(this)[0]);
      return Date.parse(new Date(extract)).toString(format ? format : null, false);
    }
    else if (/\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}/.test(this)) {
      return Date.parse(this).toString(format ? format : null);
    }
    else if (/^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.test(this)) {
      return Date.parse(this).toString(format ? format : null);
    }
    return this;
  }
  return null;
};

/*******************************************************************************
* lpad
* Preenche à esqueda uma string para um certo tamanho com outra string
*******************************************************************************/
String.prototype.lpad = function (length, string) {
  var fill = string;
  for (var i = 0; i < length; i++) { fill += string; }

  return (fill + this).slice(length * -1);
};

/*******************************************************************************
* startsWith
* Verifica se a palavra começa com o termo informado
*******************************************************************************/
if (typeof String.prototype.startsWith != 'function') {
  // see below for better implementation!
  String.prototype.startsWith = function (str) {
    return this.indexOf(str) == 0;
  };
}

/*******************************************************************************
* pushUnique
* Adiciona um valor em um array se o mesmo ainda não foi adicionado
*******************************************************************************/
Array.prototype.pushUnique = function (item) {
  //if (this.indexOf(item) == -1) {
  if (jQuery.inArray(item, this) == -1) {
    this.push(item);
    return true;
  }
  return false;
}

/*******************************************************************************
* filter
* busca por valores de acordo com a condicao
*******************************************************************************/
if (typeof Array.prototype.filter != 'function') {
  Array.prototype.find = function (fun) {
    var len = this.length;
    if (typeof fun != "function")
      throw new TypeError();

    var res = new Array();
    var thisp = arguments[1];
    for (var i = 0; i < len; i++) {
      if (i in this) {
        var val = this[i]; // in case fun mutates this
        if (fun.call(thisp, val, i, this))
          res.push(val);
      }
    }
    return res;
  };
}


/*******************************************************************************
* clone
* copia de objetos
*******************************************************************************/
if (typeof Array.prototype.clone != 'function') {
  Array.prototype.clone = function () {
    return this.slice(0);
  };
}

/*******************************************************************************
* trim
* Remove espaços em branco do início e do fim de uma string
*******************************************************************************/
String.prototype.trim = function () {
  return this.replace(/^\s+|\s+$/g, '');
};

/*******************************************************************************
* ltrim
* Remove espaços em branco do início de uma string
*******************************************************************************/
String.prototype.ltrim = function () {
  return this.replace(/^\s+/, '');
};

/*******************************************************************************
* ltrim
* Remove espaços em branco do fim de uma string
*******************************************************************************/
String.prototype.rtrim = function () {
  return this.replace(/\s+$/, '');
};

/*******************************************************************************
* ltrim
* Remove todos espaços em branco de uma string
*******************************************************************************/
String.prototype.fulltrim = function () {
  return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' ');
};

/*******************************************************************************
* Object size
* retorna a quantidade de elementos de um objeto
*******************************************************************************/
Object.size = function (obj) {
  var size = 0, key;
  for (key in obj) {
    if (obj.hasOwnProperty(key)) size++;
  }
  return size;
};

String.prototype.truncateAtWord = function (length) {
  var input = this;

  if (Functions.isNullOrEmpty(input) || input.length < length)
    return input;

  var iNextSpace = input.lastIndexOf(" ", length);

  return `${input.substring(0, (iNextSpace > 0) ? iNextSpace : length).fulltrim()}...`;
};

/*******************************************************************************
* LinqJs - where
* Filters a sequence of values based on a predicate.
*******************************************************************************/
Array.prototype.where = Array.prototype.filter || function (predicate, context) {
  context = context || window;
  var arr = [];
  var l = this.length;
  for (var i = 0; i < l; i++)
    if (predicate.call(context, this[i], i, this) === true) arr.push(this[i]);
  return arr;
};

/*******************************************************************************
* LinqJs - first
* Returns the first element of a sequence.
*******************************************************************************/
Array.prototype.first = function (predicate, def) {
  var l = this.length;
  if (!predicate) return l ? this[0] : def == null ? null : def;
  for (var i = 0; i < l; i++)
    if (predicate(this[i], i, this))
      return this[i];
  return def == null ? null : def;
};

/*******************************************************************************
* LinqJs - select
* Projects each element of a sequence into a new form.
*******************************************************************************/
Array.prototype.select = Array.prototype.map || function (selector, context) {
  context = context || window;
  var arr = [];
  var l = this.length;
  for (var i = 0; i < l; i++)
    arr.push(selector.call(context, this[i], i, this));
  return arr;
};
Date.prototype.addDay = function (day) {
  var dat = new Date(this.valueOf());
  dat.setDate(dat.getDate() + day);
  return dat;
}
Date.prototype.addMinutes = function (minute) {
  var dat = new Date(this.valueOf());
  dat.setMinutes(dat.getMinutes() + minute);
  return dat;
}
Date.prototype.addHours = function (h) {
  var copiedDate = new Date();
  copiedDate.setTime(this.getTime() + (h * 60 * 60 * 1000));
  return copiedDate;
}

Date.prototype.addMonths = function (value) {
  var n = this.getDate();
  this.setDate(1);
  this.setMonth(this.getMonth() + value);
  this.setDate(Math.min(n, this.getDaysInMonth()));
  return this;
};
String.prototype.toDate = function () {
  var date = this.valueOf();
  if (date instanceof Date) {
    return date;
  }
  var parts = date.split("/");
  var yer = parts[2].length > 4 ? parts[2].substring(0, 4) : parts[2];
  var dat = new Date(yer, parts[1] - 1, parts[0]);
  return dat;
}

/*******************************************************************************
* JSON - unflatten
* Unflat object with dot notation
*******************************************************************************/
JSON.unflatten = function (data) {
  "use strict";
  if (Object(data) !== data || Array.isArray(data))
    return data;
  var regex = /\.?([^.\[\]]+)|\[(\d+)\]/g,
    resultholder = {};
  for (var p in data) {
    var cur = resultholder,
      prop = "",
      m;
    while (m = regex.exec(p)) {
      cur = cur[prop] || (cur[prop] = (m[2] ? [] : {}));
      prop = m[2] || m[1];
    }
    cur[prop] = data[p];
  }
  return resultholder[""] || resultholder;
};

/*******************************************************************************
* JSON - flatten
* Flat object to dot notation
*******************************************************************************/
JSON.flatten = function (data) {
  var result = {};
  function recurse(cur, prop) {
    if (Object(cur) !== cur) {
      result[prop] = cur;
    } else if (Array.isArray(cur)) {
      for (var i = 0, l = cur.length; i < l; i++)
        recurse(cur[i], prop + "[" + i + "]");
      if (l == 0)
        result[prop] = [];
    } else {
      var isEmpty = true;
      for (var p in cur) {
        isEmpty = false;
        recurse(cur[p], prop ? prop + "." + p : p);
      }
      if (isEmpty && prop)
        result[prop] = {};
    }
  }
  recurse(data, "");
  return result;
}

/*******************************************************************************
* Simulate thread
*******************************************************************************/
//loops through an array in segments
var ThreadLoop = function (array) {
  var self = this;

  //holds the threaded work
  var thread = {
    work: null,
    wait: null,
    index: 0,
    total: array.length,
    finished: false
  };

  //set the properties for the class
  this.collection = array;
  this.finish = function () { };
  this.action = function () { throw "You must provide the action to do for each element"; };
  this.interval = 1;

  //set this to public so it can be changed
  var chunk = parseInt(thread.total * .005);
  this.chunk = (chunk == NaN || chunk == 0) ? thread.total : chunk;

  //end the thread interval
  thread.clear = function () {
    window.clearInterval(thread.work);
    window.clearTimeout(thread.wait);
    thread.work = null;
    thread.wait = null;
  };

  //checks to run the finish method
  thread.end = function () {
    if (thread.finished) { return; }
    self.finish(thread);
    thread.finished = true;
  };

  //set the function that handles the work
  thread.process = function () {
    //if (thread.index >= thread.total) { return false; }

    //thread, do a chunk of the work
    if (thread.work) {
      var part = Math.min((thread.index + self.chunk), thread.total);
      while (thread.index < part) {
        self.action(self.collection[thread.index], thread.index, thread.total);
        thread.index++;
      }
    }
    else {

      //no thread, just finish the work
      while (thread.index++ < thread.total) {
        self.action(self.collection[thread.index], thread.index, thread.total);
      }
    }

    //check for the end of the thread
    if (thread.index >= thread.total) {
      thread.clear();
      thread.end();
    }

    //return the process took place
    return true;

  };

  //set the working process
  self.start = function () {
    thread.finished = false;
    thread.index = 0;
    thread.work = window.setInterval(thread.process, self.interval);
  };

  //stop threading and finish the work
  self.wait = function (timeout) {

    //create the waiting function
    var complete = function () {
      thread.clear();
      thread.process();
      thread.end();
    };

    //if there is no time, just run it now
    if (!timeout) {
      complete();
    }
    else {
      thread.wait = window.setTimeout(complete, timeout);
    }
  };

};

// Plugins
//////////////////////////////////////////////////////////////////////
; (function ($) {

  /*****************************************************************
  * plugin que retorna a query string da url
  *****************************************************************/
  $.extend({
    getUrlParams: function () {
      var vars = [], hash;
      var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
      for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[decodeURIComponent(hash[0])] = decodeURIComponent(hash[1]);
      }
      return vars;
    },
    getUrlParam: function (name) {
      return $.getUrlParams()[name];
    },
    getQueryString: function (name) {
      function parseParams() {
        var params = {},
          e,
          a = /\+/g,  // Regex for replacing addition symbol with a space
          r = /([^&=]+)=?([^&]*)/g,
          d = function (s) { return decodeURIComponent(s.replace(a, " ")); },
          q = window.location.search.substring(1);

        while (e = r.exec(q))
          params[d(e[1])] = d(e[2]);

        return params;
      }

      if (!this.queryStringParams) {
        this.queryStringParams = parseParams();
      }

      if (name) {
        return this.queryStringParams[name];
      }
      else {
        return this.queryStringParams;
      }
    },

    deparam: function (urlParams) {
      var search = urlParams;
      return search ? JSON.parse('{"' + search.replace(/&/g, '","').replace(/=/g, '":"') + '"}',
        function (key, value) { return key === "" ? value : decodeURIComponent(value) }) : {};
    },

    loadingBox: function (show, message) {
      if (show === 'show') {
        //RPNotification.messageBox()
      }
      else {
        //$box.remove();
      }
    },

    notify: function (message, options) {
      //new PNotify($.extend({ title: false, icon: false, text: message, type: "info", delay: 4000, buttons: { sticker: false } }, options));
    },

    loadingMessage: function (show, message, time) {
      if (show === 'show') {
        //RPNotification.pageOverlay(true);
      } else {
        //RPNotification.pageOverlay(false);
      }
    },

    removeAccents: function (value) {
      var rs = value;
      rs = rs.replace(new RegExp("\\s", 'g'), "");
      rs = rs.replace(new RegExp("[àáâãäå]", 'g'), "a");
      rs = rs.replace(new RegExp("æ", 'g'), "ae");
      rs = rs.replace(new RegExp("ç", 'g'), "c");
      rs = rs.replace(new RegExp("[èéêë]", 'g'), "e");
      rs = rs.replace(new RegExp("[ìíîï]", 'g'), "i");
      rs = rs.replace(new RegExp("ñ", 'g'), "n");
      rs = rs.replace(new RegExp("[òóôõö]", 'g'), "o");
      rs = rs.replace(new RegExp("œ", 'g'), "oe");
      rs = rs.replace(new RegExp("[ùúûü]", 'g'), "u");
      rs = rs.replace(new RegExp("[ýÿ]", 'g'), "y");
      rs = rs.replace(new RegExp("\\W", 'g'), "");
      return rs;
    },

    format: function (source, params) {
      if (arguments.length == 1)
        return function () {
          var args = $.makeArray(arguments);
          args.unshift(source);
          return $.format.apply(this, args);
        };
      if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
      }
      if (params != null && params.constructor != Array) {
        params = [params];
      }
      $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
      });
      return source;
    },

    readyFormIframe: function (iframe, callback) {
      if (iframe.contents().find('form').length == 0) {
        setTimeout(function () {
          $.readyFormIframe(iframe, callback);
        }, 1);
      }
      else {
        if (typeof callback == 'function') {
          callback();
        }
      }
    }
  });

  $.fn.keyenter = function (fn) {

    return this.each(function () {
      $(this).bind('enterPress', fn);
      $(this).keypress(function (e) {
        if (e.keyCode == 13) {
          $(this).trigger("enterPress");
          e.preventDefault();
        }
      })
    });
  };

  $.fn.passwordStrength = function (options) {
    var defaults = {
      bar: '',
      width: 200,
      showLabel: true
    };
    options = $.extend(defaults, options);
    var wrapper, label, progress, bar, self;

    var strength = function (text) {
      var label = new Array();
      label[0] = "Muito curta";
      label[1] = "Muito fraca";
      label[2] = "Fraca";
      label[3] = "Boa";
      label[4] = "Forte";
      label[5] = "Muito forte";

      var score = 0;

      if (text.length > 0) {
        //if password bigger than 6 give 1 point
        if (text.length >= 6) score++;

        //if password has uppercase characters give 1 point	
        if (text.match(/[A-Z]/g)) score++;

        //if password has lowercase characters give 1 point	
        if (text.match(/[a-z]/g)) score++;

        //if password has at least one number give 1 point
        if (text.match(/\d+/)) score++;

        //if password has at least one special caracther give 1 point
        if (text.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;

        //TODO: verificar se existe caractere duplicado. Se houver diminui um score
      }

      return {
        score: score == 0 ? 0 : score * 20,
        label: label[score],
        role: "progressbar",
        color: score <= 1 ? 'progress-bar-danger' : (score <= 3 ? 'progress-bar-warning' : 'progress-bar-success')
      };
    }

    var _text;

    return this.each(function () {
      self = $(this);

      if ($('html').hasClass('ie')) {
        _text = self.val() == self.attr('placeholder') ? '' : self.val();
      }
      else {
        _text = self.val();
      }

      wrapper = $(options.bar).css('width', options.width);
      if (options.showLabel) {
        label = $('<span/>').css('font-size', '11px');
      }
      progress = $('<div class="progress"/>').css('height', 14);
      bar = $('<div class="progress-bar"/>');
      progress.append(bar);
      if (options.showLabel) {
        wrapper.append(label);
      }
      wrapper.append(progress);

      var o = strength(_text);
      progress.removeClass('progress-success').removeClass('progress-warning').removeClass('progress-danger');
      progress.addClass(o.color);
      if (options.showLabel) {
        label.text(o.label);
      }
      bar.width(o.score + '%');

      self.bind('keyup', function () {
        if ($('html').hasClass('ie')) {
          _text = self.val() == self.attr('placeholder') ? '' : self.val();
        }
        else {
          _text = self.val();
        }

        o = strength(_text);
        progress.removeClass('progress-success').removeClass('progress-warning').removeClass('progress-danger');
        progress.addClass(o.color);
        if (options.showLabel) {
          label.text(o.label);
        }
        bar.width(o.score + '%');
      });
    });

  };

  $.fn.dialog = function (options) {
    var defaults = {
      width: 600,
      height: null
    };
    options = $.extend(defaults, options);
    var _this = this;
    var _body = _this.children().children().children('.modal-body');
    var _dialog = _this.children('.modal-dialog');

    _this.addClass('modal hide fade')
      .attr('tabindex', '-1')
      .attr('role', 'dialog')
      .attr('aria-hidden', 'true')
      .attr('aria-labelledby', 'lbl-' + _this.attr('id'))
      .attr('data-keyboard', 'false');

    _dialog.css('width', options.width).css('height', options.height);

    // se existir a tag para adicionar iframe no conteudo
    if (_body.is('[data-iframe-url]')) {
      _body.empty();
      _body.css({ padding: 0, overflow: 'hidden' });
      var iframe = $('<iframe frameborder="0" class="hide"/>').attr('data-src', _body.attr('data-iframe-url')).css({ width: '100%', height: '100%' }).appendTo(_body);
      $(iframe).on('load', function () {
        if (_body.is('[data-iframe-prefix]')) {
          // verifica se nao existe um campo #context_prefix no dom do iframe
          if ($(this).contents().find('#context-prefix').length == 0) {
            // cria/adiciona o campo #context_prefix com o valor do atributo
            $(this).contents().find('body').append($('<input type="hidden" id="context-prefix" />').val(_body.attr('data-iframe-prefix')));
          }
        }
      });

      _body.removeAttr('data-iframe-url');
    }

    // se a altura for definida
    if (options.height != null) {
      _this.on('show.bs.modal', function () {
        _body.height(
          options.height
          - parseInt(_this.find('.modal-header').outerHeight())
          - parseInt(_this.find('.modal-footer').outerHeight())
          - parseInt(_body.css('padding-top'))
          - parseInt(_body.css('padding-bottom'))
        );
      });

      //_this.css('height', options.height);
    }

    _this.find('.modal-header h3').attr('id', 'lbl-' + _this.attr('id'));
    _this.on('show.bs.modal', function () {
      var iframe = _body.children('iframe');
      _this.removeClass('hide');
      if (iframe) {
        if (iframe.attr('data-src')) {
          _body.addClass('loading');
          iframe.on('load', function () {
            _body.removeClass('loading');
            iframe.removeClass('hide');
          });

          iframe.attr('src', iframe.attr('data-src'));
          iframe.removeAttr('data-src');
        }
      }
    });

    return _this;
  };
  /*****************************************************************
  * plugin que faz o input piscar com uma cor diferente
  *****************************************************************/
  $.fn.blink = function (options) {
    var defaults = {
      cssClass: 'input-validation-error',
      microseconds: 5000,
      value: null,
      msg: null
    };
    options = $.extend(defaults, options);
    var self;
    var time = null;
    if (options.msg != null)
      var $err = $('<span class="field-validation-error">' + options.msg + '</span>');
    return this.each(function () {
      self = $(this);
      self.addClass(options.cssClass);

      if (options.msg != null) {
        if (self.parent("div").find(".field-validation-error").length > 0) {
          self.parent("div").find(".field-validation-error").remove();
        }
        self.parent("div").append($err);
      }
      self.parent().find(".select2-container").addClass(options.cssClass);

      if ($.data(self[0], 'blink_time') != null) {
        clearTimeout($.data(self[0], 'blink_time'));
      }

      $.data(self[0], 'blink_time', setTimeout(function () {
        self.removeClass(options.cssClass);
        self.parent().find(".select2-container").removeClass(options.cssClass);
        if (options.msg != null) {
          $err.remove();
        }
      }, options.microseconds));

      if (options.value !== null) {
        self.val(options.value);
      }
    });
  };

  //TODO: remover
  $.fn.Report = function (options) {
    var href;
    return this.each(function () {
      href = $(this).attr('href');
      $(this)
        .attr('target', '_blank')
        .attr('href', href.indexOf('?') > 0 ? (href + location.search.replace('?', '&')) : href + location.search);
    });
  };

  $.fn.setFocus = function (options) {
    var self = $(this);
    var timeout, callback = {};

    if (Functions.isNumber(options)) {
      timeout = options;
    }
    else {
      var defaults = {
        timeout: 100
      };
      options = $.extend(defaults, options);

      callback = options.callback;
      timeout = options.timeout;
    }

    setTimeout(function () {
      self.focus(callback);
    }, timeout);
    return this;
  };

  $.fn.renderIcons = function (options) {
    var $these = this;

    if (typeof options == "string") {
      if (options == "reload") {
        return $these.each(function () {
          $(this).next().removeClass($(this).prop('checked') ? 'glyphicon glyphicon-unchecked' : 'glyphicon glyphicon-check').addClass($(this).prop('checked') ? 'glyphicon glyphicon-check' : 'glyphicon glyphicon-unchecked');
        });
      }
      return;
    }

    var defaults = {
      change: null
    };

    options = $.extend(defaults, options);

    var $this;
    return $these.each(function () {
      $this = $(this);
      $this.hide().after('<i class="' + ($this.prop('checked') ? 'glyphicon glyphicon-check' : 'glyphicon glyphicon-unchecked') + '"/> ');
      $this
        .change(function () {
          var values = [];

          if ($this.prop('type') == 'checkbox') {
            $(this).next().removeClass($(this).prop('checked') ? 'glyphicon glyphicon-unchecked' : 'glyphicon glyphicon-check').addClass($(this).prop('checked') ? 'glyphicon glyphicon-check' : 'glyphicon glyphicon-unchecked');

            $these.each(function (i, e) {
              if ($(e).is(':checked')) {
                values.push($(this).val());
              }
            });
          }
          else if ($this.prop('type') == 'radio') {
            $these.each(function (i, e) {
              $(e).next().removeClass('glyphicon glyphicon-check').addClass('glyphicon glyphicon-unchecked');
            });
            $(this).next().removeClass('glyphicon glyphicon-unchecked').addClass('glyphicon glyphicon-check');

            values.push($(this).val());
          }

          if (typeof options.change == "function") {
            options.change(values);
          }
        })
        .parents('li').click(function (e) { e.stopPropagation(); });
    });
  };

  $.fn.CheckCapslock = function (options) {
    var defaults = {
      onCreate: null,
      message: ' <strong>Atenção</strong> a tecla "capslock" está acionada',
      cssClass: 'alert note hide',
      icon: 'glyphicon glyphicon-exclamation-sign',
    };
    options = $.extend(defaults, options);

    // cria elemento da mensagem
    $message = $('<p/>');
    $message.addClass(options.cssClass);
    $message.html(options.message);
    $message.prepend($('<i/>').addClass(options.icon));

    // insere mensagem depois do input
    this.after($message);

    // dispara funcao
    if (typeof options.onCreate == "function") {
      options.onCreate($message, this);
    }

    // captura evento do input
    this.on('keypress', function (e) {
      var s = String.fromCharCode(e.which);
      if (s.toUpperCase() === s && s.toLowerCase() !== s && !e.shiftKey) {
        $message.removeClass('hide');
      }
      else {
        $message.addClass('hide');
      }
    });
  };

  //
  var re = /([^&=]+)=?([^&]*)/g;
  var decode = function (str) {
    return decodeURIComponent(str.replace(/\+/g, ' '));
  };
  $.parseParams = function (query) {

    // recursive function to construct the result object
    function createElement(params, key, value) {
      key = key + '';

      // if the key is a property
      if (key.indexOf('.') !== -1) {
        // extract the first part with the name of the object
        var list = key.split('.');

        // the rest of the key
        var new_key = key.split(/\.(.+)?/)[1];

        // create the object if it doesnt exist
        if (!params[list[0]]) params[list[0]] = {};

        // if the key is not empty, create it in the object
        if (new_key !== '') {
          createElement(params[list[0]], new_key, value);
        } else console.warn('parseParams :: empty property in key "' + key + '"');
      } else
        // if the key is an array    
        if (key.indexOf('[') !== -1) {
          // extract the array name
          var list = key.split('[');
          key = list[0];

          // extract the index of the array
          var list = list[1].split(']');
          var index = list[0]

          // if index is empty, just push the value at the end of the array
          if (index == '') {
            if (!params) params = {};
            if (!params[key] || !$.isArray(params[key])) params[key] = [];
            params[key].push(value);
          } else
          // add the value at the index (must be an integer)
          {
            if (!params) params = {};
            if (!params[key] || !$.isArray(params[key])) params[key] = [];
            params[key][parseInt(index)] = value;
          }
        } else
        // just normal key
        {
          if (!params) params = {};
          params[key] = value;
        }
    }

    // be sure the query is a string
    query = query + '';

    if (query === '') query = window.location + '';

    var params = {}, e;
    if (query) {
      // remove # from end of query
      if (query.indexOf('#') !== -1) {
        query = query.substr(0, query.indexOf('#'));
      }

      // remove ? at the begining of the query
      if (query.indexOf('?') !== -1) {
        query = query.substr(query.indexOf('?') + 1, query.length);
      } else return {};

      // empty parameters
      if (query == '') return {};

      // execute a createElement on every key and value
      while (e = re.exec(query)) {
        var key = decode(e[1]);
        var value = decode(e[2]);
        createElement(params, key, value);
      }
    }
    return params;
  };
})(jQuery);


function Controller(form, datatable) {

  var self = this;
  var fields = form.fields;
  var first = null;

  self = {
    dataTable: datatable,
    validarForm: function () {

      var valid = true;

      var trigger = $(document).triggerHandler(form.name + 'BeforeValidate', true);

      if (trigger == false)
        return false;

      var msg = "<h4>Verifique os campos abaixo!</h4>";
      for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        if (field.required == true && IsNullOrEmpty(field.input.val())) {
          field.input.blink();
          valid = false;
          if (field.msg != undefined) {
            msg += field.msg + "<br/>"
          }
        }
        if (self.dataTable.isEdit == false) {

          if (field.unique == true && self.dataTable.exists(field.name, field.input.val())) {

            field.input.blink();
            valid = false;
            if (field.label != undefined) {
              msg += "O " + field.label + " já foi informado, verifique!<br/>"
            } else {
              msg += "Este item já foi informado, verifique!<br/>"

            }
          }
        }
      }
      if (!valid) {
        $.notify(msg, { type: "error" });
      }
      return valid;
    },
    getModel: function (model) {
      for (var i = 0; i < fields.length; i++) {
        var val = null;
        var field = fields[i];
        if (field.input.length > 0) {

          if (field.type == "currency")
            val = Functions.parseFloatInput(field.input);
          else if (field.type == "numeric") {

            var numeric = parseInt(field.input.val());

            if (!isNaN(numeric))
              val = numeric;

          } else {
            val = field.input.val();
          }
        }

        model[field.name] = val;

      }
      var trigger = $(document).triggerHandler(form.name + 'GetModel', model);
      if (trigger != undefined) {
        return trigger;
      }
      return model;
    },
    limparForm: function () {
      for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        if (field.clear == true || field.clear == undefined) {
          field.input.val("");
          if (field.content == "select2") {
            if (shared_function[field.prefix] == undefined) {
              field.input.val(null).trigger('change').trigger('select2:unselect');
            } else {
              shared_function[field.prefix].clear();
            }
          }
        }

      }
    },
    adicionar: function () {
      if (self.validarForm()) {
        var model = self.getModel({});

        self.dataTable.addItem(model);
        self.limparForm();
        if (first != null) {
          if (first.content == "select2")
            first.input.next().find('.select2-selection').focus();
          else
            first.input.focus();
        }
      }

    }
  }

  form.btn.click(function (e) {
    if (self.dataTable.isEdit) {
      self.dataTable.editItem(self.dataTable.dataSelected.item)
    } else {
      self.adicionar();
    }
    return false;
  });

  var init = function () {
    if (!form) {
      throw "o form nao foi informado, favor nao cabacear!";
    }
    if (!form.btn) {
      throw "o botao do form nao foi informado, favor nao cabacear!";
    }
    for (var i = 0; i < fields.length; i++) {
      var field = fields[i];
      if (field.first == true) {
        first = field;
      }
    }

    $(document).on(self.dataTable.settings.table.name + 'OpenEdit', function (e, data) {
      for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        //if (field.content == "select2") {
        //    var item = { id: data.item[field.name], text: data.item[field.name] };

        //    field.input.select2("trigger", "select", { data: data });
        //}
        if (field.type == "currency") {
          field.input.val(Functions.numberFormat(data.item[field.name]));
        }
        else if (field.content == "select2") {
          if (shared_function[field.prefix] == undefined) {
            field.input.val(data.item[field.name]).trigger('change').trigger('select2:unselect');
          }
        } else {
          field.input.val(data.item[field.name]);
        }

        $(document).trigger(form.name + 'OpenEdit', data);
      }
    });

    $(document).on(self.dataTable.settings.table.name + 'CancelEdit', function (e, data) {
      self.limparForm();
    });

    $(document).on(self.dataTable.settings.table.name + 'ValidateEdit', function (e, data) {
      return self.validarForm();
    });

    $(document).on(self.dataTable.settings.table.name + 'BeforEdit', function (e, data) {
      self.dataTable.dataSelected.item = self.getModel(self.dataTable.dataSelected.item);
    });

    $(document).on(self.dataTable.settings.table.name + 'AfterEdit', function (e, data) {
      self.limparForm();
    });
  };
  init();
}

window.openModalPDF = Functions.openModalPDF;
window.dataToPrint = Functions.dataToPrint;
window.dataToPrint = Functions.dataToPrint;
