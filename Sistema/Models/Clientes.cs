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

        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        //logradouro

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

        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Display(Name = "CPF / CNPJ")]
        public string cpfCNPJ { get; set; }

        [Display(Name = "RG / IE")]
        public string rgIE { get; set; }

        [Display(Name = "Data de nascimento")]
        public DateTime? dtNascimento { get; set; }

        //Condicao de pagamento

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data de ult. alteração")]
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