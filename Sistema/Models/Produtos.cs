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

        [Display(Name = "Largura (cm)")]
        public string largura { get; set; }

        [Display(Name = "NCM")]
        public string ncm { get; set; }

        [Display(Name = "Quant. estoque")]
        public decimal qtEstoque { get; set; }

        [Display(Name = "Valor do custo")]
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

        public static SelectListItem[] Largura
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "5", Text = "5" },
                    new SelectListItem { Value = "7", Text = "7" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "12", Text = "12" },
                    new SelectListItem { Value = "15", Text = "15" },
                };
            }
        }
    }
}