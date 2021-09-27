using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class OrdemServico : Pai
    {
        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Data de abertura")]
        public DateTime? dtAbertura { get; set; }

        [Display(Name = "Data de validade")]
        public DateTime? dtValidade { get; set; }

        public string finalizar { get; set; }

        public Select.Funcionarios.Select Funcionario { get; set; }
        public Select.Funcionarios.Select Executante { get; set; }
        public Select.Clientes.Select Cliente { get; set; }
        public Select.CondicaoPagamento.Select CondicaoPagamento { get; set; }

        public Select.Servicos.Select Servico { get; set; }
        public Select.Produtos.Select Produto { get; set; }
        public decimal? vlTotal { get; set; }

        [Display(Name = "Observação")]
        public string observacao { get; set; }

        public class ServicosVM
        {
            public int? codServico { get; set; }
            public string nomeServico { get; set; }
            public string unidade { get; set; }
            public decimal? qtServico { get; set; }
            public decimal? vlServico { get; set; }            
        }
        
        public string jsServicos { get; set; }
        public List<ServicosVM> ServicosOS
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

        public class ProdutosVM
        {
            public int? codProduto { get; set; }
            public string nomeServico { get; set; }
            public string unidade { get; set; }
            public decimal? vlProduto { get; set; }
            public decimal? qtProduto { get; set; }
            public int? codExecutante { get; set; }
            public string nomeExecutante { get; set; }
        }

        public string jsProdutos { get; set; }
        public List<ProdutosVM> ProdutosOS
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
        public List<Models.Shared.ParcelasVM> ParcelasOS
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

        public static SelectListItem[] Situacao
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "A", Text = "ABERTA" },
                    new SelectListItem { Value = "E", Text = "EM ANDAMENTO" },
                    new SelectListItem { Value = "F", Text = "FINALIZADA" },
                    new SelectListItem { Value = "C", Text = "CANCELADA" }
                };
            }
        }

        public static SelectListItem[] Unidade
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "M", Text = "METRO" },
                    new SelectListItem { Value = "U", Text = "UNIDADE" }
                };
            }
        }
    }
}