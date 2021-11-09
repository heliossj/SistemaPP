using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.Select.CondicaoPagamento
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public decimal? txJuros { get; set; }
        public decimal? multa { get; set; }
        public decimal? desconto { get; set; }
       
    }
}