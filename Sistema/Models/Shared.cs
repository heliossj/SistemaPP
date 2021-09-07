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
        public class ParcelasVM
        {
            public int? idFormaPagamento { get; set; }
            public string nmFormaPagamento { get; set; }
            public string flSituacao { get; set; }
            public DateTime? dtVencimento { get; set; }
            public decimal? vlParcela { get; set; }
            public double? nrParcela { get; set; }
        }
    }
}