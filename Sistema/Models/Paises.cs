using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class Paises
    {
        [Display(Name = "Código")]
        public int? codPais { get; set; }

        [Display(Name = "País")]
        public string nomePais { get; set; }

        [Display(Name = "DDI")]
        public string DDI { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data da últ. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

        public Select.Paises.Select Pais { get; set; }
    }
}