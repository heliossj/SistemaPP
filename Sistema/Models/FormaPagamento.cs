using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class FormaPagamento
    {
        [Display(Name = "Código")]
        public int? codForma { get; set; }

        [Display(Name = "Nome")]
        public string nomeForma { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

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
    }
}