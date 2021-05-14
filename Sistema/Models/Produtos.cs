using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Produtos
    {
        [Display(Name = "Código")]
        public int? codProduto { get; set; }

        [Display(Name = "Produto")]
        public string nomeProduto { get; set; }

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

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data da últ. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

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

        public static SelectListItem[] Unidade
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "M", Text = "METRO" },
                };
            }
        }
    }
}