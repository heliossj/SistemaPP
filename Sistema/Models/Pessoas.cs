using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Pessoas : Pai
    {
        [Display(Name = "Tipo")]
        public string tipo { get; set; }

        public string dsTipo { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

        [Display(Name = "Tel. fixo")]
        public string telefoneFixo { get; set; }

        [Display(Name = "Tel. celular")]
        public string telefoneCelular { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        public Select.Cidades.Select Cidade { get; set; }

        //Física

        [Display(Name = "Nome")]
        public string nomePessoa { get; set; }

        [Display(Name = "Apelido")]
        public string apelidoPessoa { get; set; }

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Display(Name = "Data de nascimento")]
        public DateTime? dtNascimento { get; set; }

        [Display(Name = "CPF")]
        public string cpf { get; set; }

        [Display(Name = "RG")]
        public string rg { get; set; }

        //Jurídica

        [Display(Name = "Razão social")]
        public string razaoSocial { get; set; }

        [Display(Name = "Nome fantasia")]
        public string nomeFantasia { get; set; }

        [Display(Name = "Data de fundação")]
        public DateTime? dtFundacao { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "Inscrição Estadual")]
        public string ie { get; set; }

        [Display(Name = "Site")]
        public string site { get; set; }

        // **************

        [Display(Name = "Logradouro")]
        public string dsLogradouro { get; set; }

        [Display(Name = "Nº")]
        public string numero { get; set; }

        [Display(Name = "Complemento")]
        public string complemento { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Display(Name = "CEP")]
        public string cep { get; set; }

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