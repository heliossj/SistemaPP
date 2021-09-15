using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Vendas : Pai
    {
        [Display(Name = "Data")]
        public DateTime? dtVenda { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        public string finalizar { get; set; }
        public decimal? vlTotal { get; set; }

        public Select.Funcionarios.Select Funcionario { get; set; }
        public Select.Funcionarios.Select Cliente { get; set; }
        public Select.Produtos.Select Produto { get; set; }
        public Select.CondicaoPagamento.Select CondicaoPagamento { get; set; }

        public class ProdutosVM
        {
            public int? codProduto { get; set; }
            public string nomeProduto { get; set; }
            public decimal? vlVenda { get; set; }
            public decimal? qtProduto { get; set; }
            public string unidade { get; set; }
            public decimal? txDesconto { get; set; }
            public decimal vlTotal { get; set; }
        }

        public class ParcelasVM
        {
            public int? idFormaPagamento { get; set; }
            public string nmFormaPagamento { get; set; }
            public string flSituacao { get; set; }
            public DateTime? dtVencimento { get; set; }
        }

        public string jsProdutos { get; set; }
        public string jsParcelas { get; set; }

        public List<ProdutosVM> ProdutosCompra
        {
            get
            {
                if (string.IsNullOrEmpty(jsProdutos))
                    return new List<ProdutosVM>();
                return JsonConvert.DeserializeObject<List<ProdutosVM>>(jsProdutos);
            }
            set
            {
                jsProdutos = JsonConvert.SerializeObject(value);
            }
        }

        public List<ParcelasVM> ParcelasCompra
        {
            get
            {
                if (string.IsNullOrEmpty(jsParcelas))
                    return new List<ParcelasVM>();
                return JsonConvert.DeserializeObject<List<ParcelasVM>>(jsParcelas);
            }
            set
            {
                jsParcelas = JsonConvert.SerializeObject(value);
            }
        }

        public static SelectListItem[] Situacao
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "N", Text = "NORMAL"},
                    new SelectListItem { Value = "C", Text = "CANCELADA"}
                };
            }
        }
    }
}