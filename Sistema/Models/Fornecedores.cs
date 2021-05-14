using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Fornecedores
    {
        [Display(Name = "Código")]
        public int? codFornecedor { get; set; }

        [Display(Name = "Tipo")]
        public string tipo { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Razão social")]
        public string razaoSocial { get; set; }

        [Display(Name = "Nome fantasia")]
        public string nomeFantasia { get; set; }

        [Display(Name = "Logradouro")]
        public string dsLogradouro { get; set; }

        [Display(Name = "Nº")]
        public string numero { get; set; }

        [Display(Name = "Complemento")]
        public string complemento { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        // Cidade 

        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Display(Name = "Site")]
        public string site { get; set; }

        [Display(Name = "Tel. fixo")]
        public string telefoneFixo { get; set; }

        [Display(Name = "Tel. celular")]
        public string telefoneCelular { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "IE")]
        public string IE { get; set; }

        // Condição de pagamento

        [Display(Name = "Observacao")]
        public string observacao { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data da últ. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

        public static SelectListItem[] Situacao
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "A", Text = "ATIVA" },
                    new SelectListItem { Value = "I", Text = "INATIVA" }
                };
            }
        }

        public static SelectListItem[] Tipo
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "F", Text = "FÍSICA" },
                    new SelectListItem { Value = "J", Text = "JURÍDICA" }
                };
            }
        }
    }
}