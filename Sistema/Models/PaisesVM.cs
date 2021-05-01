using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class PaisesVM
    {
        [Display(Name = "Código")]
        public int? idPais { get; set; }

        [Display(Name = "País")]
        public string nmPais { get; set; }

        [Display(Name = "DDI")]
        public string DDI { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }
    }
}