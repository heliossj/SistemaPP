using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class Estados : Pai
    {
        [Display(Name = "Estado")]
        public string nomeEstado { get; set; }

        [Display(Name = "UF")]
        public string uf { get; set; }

        public Select.Paises.Select Pais { get; set; }
    }
}