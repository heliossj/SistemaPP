using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class ContasContabeis : Pai
    {
        [Display(Name = "Conta")]
        public string nomeConta { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Saldo")]
        public decimal? vlSaldo { get; set; }

        [Display(Name = "Classificação")]
        public string classificacao { get; set; }

        public static SelectListItem[] Situacao
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "A", Text = "ATIVA" },
                    new SelectListItem { Value = "I", Text = "INATIVA" }
                };
            }
        }

        public class LancamentoVM
        {
            public int? codLancamento { get; set; }
            public string descricao { get; set; }
            public DateTime? dtMovimento { get; set; }
            public decimal? vlLancamento { get; set; }
            public string tipo { get; set; }
        }

        public string jsLancamentos { get; set; }
        public List<LancamentoVM> Lancamentos
        {
            get
            {
                if (string.IsNullOrEmpty(jsLancamentos))
                    return new List<LancamentoVM>();
                return JsonConvert.DeserializeObject<List<LancamentoVM>>(jsLancamentos);
            }
            set
            {
                jsLancamentos = JsonConvert.SerializeObject(value);
            }
        }
    }
}