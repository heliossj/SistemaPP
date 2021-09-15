using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class CondicaoPagamento : Pai
    {
        [Display(Name = "Condição de pagamento")]
        public string nomeCondicao { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Taxa de juros (%)")]
        public decimal? txJuros { get; set; }

        [Display(Name = "Porcentagem (%)")]
        public decimal? txPercentual { get; set; }

        [Display(Name = "Dias")]
        public short? qtDias { get; set; }

        [Display(Name = "Multa (%)")]
        public decimal? multa { get; set; }

        [Display(Name = "Desconto (%)")]
        public decimal? desconto { get; set; }

        [Display(Name = "Total (%)")]
        public decimal? txPercentualTotal { get; set; }
        public decimal? txPercentualTotalAux { get; set; }

        public Select.FormaPagamento.Select FormaPagamento { get; set; }

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

        public static SelectListItem[] Juros
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "N", Text = "NÃO" },
                    new SelectListItem { Value = "S", Text = "SIM" },

                };
            }
        }

        public static SelectListItem[] Parcela
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "N", Text = "NÃO" },
                    new SelectListItem { Value = "S", Text = "SIM" },
                };
            }
        }

        public class CondicaoPagamentoVM
        {
            public int? codCondicaoPagamento { get; set; }
            public short? nrParcela { get; set; }
            public short? qtDias { get; set; }
            public decimal txPercentual { get; set; }
            public int? codFormaPagamento { get; set; }
            public string nomeFormaPagamento { get; set; }
        }

        public string jsItens { get; set; }
        public List<CondicaoPagamentoVM> ListCondicao
        {
            get
            {
                if (string.IsNullOrEmpty(jsItens))
                    return new List<CondicaoPagamentoVM>();
                return JsonConvert.DeserializeObject<List<CondicaoPagamentoVM>>(jsItens);
            }
            set
            {
                jsItens = JsonConvert.SerializeObject(value);
            }
        }
    }
}