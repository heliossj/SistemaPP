using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema.Models
{
    public class ContasReceber : Pai
    {
        public int codVenda { get; set; }
        public Select.Clientes.Select Cliente { get; set; }
        public Select.FormaPagamento.Select FormaPagamento { get; set; }
        public Select.ContasContabeis.Select ContaContabil { get; set; }

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

        public decimal txJuros { get; set; }
        public decimal multa { get; set; }
        public decimal desconto { get; set; }
    }
}