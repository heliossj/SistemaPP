using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class ContasPagar : Pai
    {
        public Select.Fornecedores.Select Fornecedor { get; set; }
        public Select.FormaPagamento.Select FormaPagamento { get; set; }

        [Display(Name = "Nº parcela")]
        public short nrParcela { get; set; }

        [Display(Name = "Valor da parcela")]
        public decimal vlParcela { get; set; }
        
        [Display(Name = "Data de vencimento")]
        public DateTime dtVencimento { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Data de pagamento")]
        public DateTime? dtPagamento { get; set; }

        public string modelo { get; set; }
        public string serie { get; set; }
        public int numero { get; set; }
    }
}