using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Select.Servicos
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public decimal? vlServico { get; set; }
        public string unidade { get; set; }

        public static SelectListItem[] Unidade
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "", Text = " " },
                    new SelectListItem { Value = "M", Text = "METRO" },
                    new SelectListItem { Value = "U", Text = "UNIDADE" },
                };
            }
        }

    }
}