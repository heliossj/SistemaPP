using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class OrdemServico
    {
        [Display(Name = "Nº")]
        public int? codOS { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Data de abertura")]
        public DateTime? dtAbertura { get; set; }

        [Display(Name = "Data de validade")]
        public DateTime? dtValidade { get; set; }

        //Cliente

        //Condição de pagamento

        [Display(Name = "Observação")]
        public string observacao { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data de ult. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

        public class Servicos
        {
            public int? codServico { get; set; }
            public string nomeServico { get; set; }
            public decimal? vlUnitario { get; set; }
            public decimal? qtServico { get; set; }
            public decimal? vlTotal { get; set; }
        }
        
        public List<Servicos> ServicosOS { get; set; }

        public class Produtos
        {
            public int? codProduto { get; set; }
            public string nomeServico { get; set; }
            public decimal? vlUnitario { get; set; }
            public decimal? qtServico { get; set; }
            public decimal vlTotal { get; set; }
        }

        public List<Produtos> ProdutosOS { get; set; }

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