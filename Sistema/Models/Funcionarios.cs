using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Funcionarios : Pessoas
    {
        [Display(Name = "CNH")]
        public string cnh { get; set; }

        [Display(Name = "Data de admissão")]
        public DateTime? dtAdmissao { get; set; }
        public DateTime? dtAdmissaoAux { get; set; }

        [Display(Name = "Data de demissão")]
        public DateTime? dtDemissao { get; set; }

        [Display(Name = "Salário")]
        public decimal? vlSalario { get; set; }



    }
}