using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class Logradouros
    {
        [Display(Name = "Código")]
        public int? codLogradouro { get; set; }

        [Display(Name = "Logradouro")]
        public string nomeLogradouro { get; set; }

    }
}