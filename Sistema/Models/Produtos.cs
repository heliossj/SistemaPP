using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Produtos : Pai
    {
        [Display(Name = "Produto")]
        public string nomeProduto { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Unidade")]
        public string unidade { get; set; }

        //grupo
        public Select.Grupos.Select Grupo { get; set; }

        //fornecedor
        public Select.Fornecedores.Select Fornecedor { get; set; }

        [Display(Name = "Largura (cm)")]
        public string largura { get; set; }

        [Display(Name = "NCM")]
        public string ncm { get; set; }

        [Display(Name = "CFOP")]
        public string cfop { get; set; }

        [Display(Name = "Quant. estoque")]
        public decimal? qtEstoque { get; set; }

        [Display(Name = "Valor do custo")]
        public decimal? vlCusto { get; set; }

        [Display(Name = "Valor últ. compra")]
        public decimal? vlUltCompra { get; set; }

        [Display(Name = "Valor de venda")]
        public decimal? vlVenda { get; set; }

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

        public static SelectListItem[] Unidade
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "M", Text = "METRO" },
                    new SelectListItem { Value = "U", Text = "UNIDADE" },
                };
            }
        }

        public static SelectListItem[] Largura
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = " ", Text = " " },
                    new SelectListItem { Value = "7", Text = "7" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "15", Text = "15" },
                    new SelectListItem { Value = "20", Text = "20" },
                };
            }
        }
    }
}