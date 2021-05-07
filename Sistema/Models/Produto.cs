using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Produto
    {
        [Display(Name = "Código")]
        public int? codProduto { get; set; }

        [Display(Name = "Descricao")]
        public string descricaoProduto { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Unidade")]
        public string unidade { get; set; }

        //grupo

        //fornecedor

        [Display(Name = "NCM")]
        public string ncm { get; set; }

        [Display(Name = "Quant. estoque")]
        public decimal qtEstoque { get; set; }

        [Display(Name = "Valor do cursto")]
        public decimal vlCusto { get; set; }

        [Display(Name = "Valor últ. compra")]
        public decimal vlUltCompra { get; set; }

        [Display(Name = "Valor de venda")]
        public decimal vlVenda { get; set; }

        [Display(Name = "Observacao")]
        public string observacao { get; set; }

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