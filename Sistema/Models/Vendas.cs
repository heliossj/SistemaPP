﻿using Newtonsoft.Json;
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
        public int? codOrdemServico { get; set; }

        [Display(Name = "Data da venda")]
        public DateTime? dtVenda { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        public string finalizar { get; set; }
        public decimal? vlTotal { get; set; }
        public string modelo { get; set; }

        public Select.Funcionarios.Select Funcionario { get; set; }
        public Select.Clientes.Select Cliente { get; set; }
        public Select.Produtos.Select Produto { get; set; }
        public Select.CondicaoPagamento.Select CondicaoPagamento { get; set; }

        public class ProdutosVM
        {
            public int? codProduto { get; set; }
            public string nomeProduto { get; set; }
            public string unidade { get; set; }
            public decimal qtProduto { get; set; }
            public decimal vlVenda { get; set; }
            public decimal? txDesconto { get; set; }
            public decimal vlTotal { get; set; }
        }

        public class ServicosVM
        {
            public int? codServico { get; set; }
            public string nomeServico { get; set; }
            public string unidade { get; set; }
            public decimal? qtServico { get; set; }
            public decimal? vlServico { get; set; }
            public int? codExecutante { get; set; }
            public string nomeExecutante { get; set; }
            public decimal? total { get; set; }
        }

        public string jsServicos { get; set; }
        public List<ServicosVM> ServicosVenda
        {
            get
            {
                if (string.IsNullOrEmpty(jsServicos))
                    return new List<ServicosVM>();
                return JsonConvert.DeserializeObject<List<ServicosVM>>(jsServicos);
            }
            set
            {
                jsServicos = JsonConvert.SerializeObject(value);
            }
        }

        public string jsProdutos { get; set; }
        public string jsParcelas { get; set; }

        public List<ProdutosVM> ProdutosVenda
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

        public List<Shared.ParcelasVM> ParcelasVenda
        {
            get
            {
                if (string.IsNullOrEmpty(jsParcelas))
                    return new List<Shared.ParcelasVM>();
                return JsonConvert.DeserializeObject<List<Shared.ParcelasVM>>(jsParcelas);
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