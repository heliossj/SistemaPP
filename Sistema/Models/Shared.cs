using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    //Classes compartilhadas com várias telas
    public class Shared
    {
        //Parcelas da compra
        public class ParcelasVM
        {
            public int? idFormaPagamento { get; set; }
            public string nmFormaPagamento { get; set; }
            public DateTime? dtVencimento { get; set; }
            public decimal vlParcela { get; set; }
            public double? nrParcela { get; set; }
            public string situacao { get; set; }
            public DateTime? dtPagamento { get; set; }
        }

        //Produtos Compra
        public class ProdutosVM
        {
            public int? idProduto { get; set; }
            public string nmProduto { get; set; }
            public string unidade { get; set; }
            public decimal? vlUnitario { get; set; }
            public decimal? qtProduto { get; set; }
        }
    }
}