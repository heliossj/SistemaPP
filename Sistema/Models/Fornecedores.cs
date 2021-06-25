using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Fornecedores : Pessoas
    {

        public Select.CondicaoPagamento.Select CondicaoPagamento { get; set; }

        [Display(Name = "Observacao")]
        public string observacao { get; set; }
    }
}