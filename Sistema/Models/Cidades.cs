using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class Cidades
    {
        [Display(Name = "Código")]
        public int? codCidade { get; set; }

        [Display(Name = "Cidade")]
        public string nomeCidade { get; set; }

        [Display(Name = "DDD")]
        public string ddd { get; set; }

        [Display(Name = "Sigla")]
        public string sigla { get; set; }

        //estado

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data de ult. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

    }
}