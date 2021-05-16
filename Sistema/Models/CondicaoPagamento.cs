using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class CondicaoPagamento
    {
        [Display(Name = "Código")]
        public int? codCondicao { get; set; }

        [Display(Name = "Condição de pagamento")]
        public string nomeCondicao { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data da últ. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

        [Display(Name = "Possuí juros?")]
        public string juros { get; set; }

        [Display(Name = "Taxa de juros (%)")]
        public decimal? txJuros { get; set; }

        [Display(Name = "Possuí parcela?")]
        public string parcela { get; set; }

        [Display(Name = "Quantidade de parcela(s)")]
        public short? qtParcela { get; set; }

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
    }
}