using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Select.ContasContabeis
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public string classificacao { get; set; }
        public decimal? vlSaldo { get; set; }
        public string situacao { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtUltAlteracao { get; set; }

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