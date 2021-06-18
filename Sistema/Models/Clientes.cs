using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Clientes
    {
        [Display(Name = "Código")]
        public int? codCliente { get; set; }

        [Display(Name = "Tipo")]
        public string tipo { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Nome")]
        public string nomeCliente { get; set; }

        [Display(Name = "Razão Social")]
        public string razaoSocial { get; set; }

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Display(Name = "Logradouro")]
        public string dsLogradouro { get; set; }

        [Display(Name = "Nº")]
        public string numero { get; set; }

        [Display(Name = "Complemento")]
        public string complemento { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Display(Name = "Tel. fixo")]
        public string telefoneFixo { get; set; }

        [Display(Name = "Tel. celular")]
        public string telefoneCelular { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        //cidade
        public Select.Cidades.Select Cidade { get; set; }

        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Display(Name = "CPF")]
        public string cpf { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "RG")]
        public string rg { get; set; }

        [Display(Name = "IE")]
        public string ie { get; set; }

        [Display(Name = "Data de nascimento")]
        public DateTime? dtNascimento { get; set; }

        [Display(Name = "Data de fundação")]
        public DateTime? dtFundacao { get; set; }

        //Condicao de pagamento

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

        public static SelectListItem[] Sexo
        {
            get
            {
                return new[]
                {
                    new SelectListItem { Value = "M", Text = "MASCULINO" },
                    new SelectListItem { Value = "F", Text = "FEMININO" }
                };
            }
        }

    }
}