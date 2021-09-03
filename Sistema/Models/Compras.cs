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

        public class ProdutosVM
        {
            public int? codProduto { get; set; }
            public string nomeServico { get; set; }
            public string ncm { get; set; }
            public string cfop { get; set; }
            public decimal? vlUnitario { get; set; }
            public decimal? qtProduto { get; set; }
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
    }
}