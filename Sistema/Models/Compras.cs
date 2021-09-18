using Newtonsoft.Json;
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
        public string modelo { get; set; }
        public string modeloAux { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }
        
        [Display(Name = "Série")]
        public string serie { get; set; }
        public string serieAux { get; set; }

        [Display(Name = "Número")]
        public int? nrNota { get; set; }
        public int? nrNotaAux { get; set; }

        [Display(Name = "Data de emissão")]
        public DateTime? dtEmissao { get; set; }
        public string dtEmissaoAux { get; set; }

        [Display(Name = "Data de entrega")]
        public DateTime? dtEntrega { get; set; }

        public Select.Fornecedores.Select Fornecedor { get; set; }
        public int? idFornecedor { get; set; }
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
        public List<Models.Shared.ParcelasVM> ParcelasCompra
        {
            get
            {
                if (string.IsNullOrEmpty(jsParcelas))
                    return new List<Models.Shared.ParcelasVM>();
                return JsonConvert.DeserializeObject<List<Models.Shared.ParcelasVM>>(jsParcelas);
            }
            set
            {
                jsParcelas = JsonConvert.SerializeObject(value);
            }
        }
    }
}