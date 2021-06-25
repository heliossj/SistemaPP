using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class Paises : Pai
    {
        [Display(Name = "País")]
        public string nomePais { get; set; }

        [Display(Name = "DDI")]
        public string DDI { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }
    }
}