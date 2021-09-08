﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Compras : Pai
    {
        [Display(Name = "Modelo")]
        public short modelo { get; set; }
        
        [Display(Name = "Série")]
        public short serie { get; set; }

        [Display(Name = "Número")]
        public int nrNota { get; set; }

        [Display(Name = "Data de emissão")]
        public DateTime? dtEmissao { get; set; }

        [Display(Name = "Data de entrega")]
        public DateTime? dtEntrega { get; set; }

        public Select.Fornecedores.Select Fornecedor { get; set; }
        public Select.CondicaoPagamento.Select CondicaoPagamento { get; set; }
        public Select.Produtos.Select Produto { get; set; }

        [Display(Name = "Observação")]
        public string observacao { get; set; }

        public string finalizar { get; set; }
        public decimal? vlTotal { get; set; }

        public class ProdutosVM
        {
            public int? codProduto { get; set; }
            public string nomeProduto { get; set; }
            public string unidade { get; set; }
            public decimal? qtProduto { get; set; }
            public decimal? vlVenda { get; set; }
            public decimal? vlCompra { get; set; }
            public decimal? txDesconto { get; set; }
            public decimal? vlTotal { get; set; }
        }
        public string jsProdutos { get; set; }
        
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
        public string jsParcelas { get; set; }
        public List<Shared.ParcelasVM> ParcelasCompra
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
    }
}