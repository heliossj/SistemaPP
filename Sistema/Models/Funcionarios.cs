using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Models
{
    public class Funcionarios
    {
        [Display(Name = "Código")]
        public int? codFuncionario { get; set; }

        [Display(Name = "Funcionário")]
        public string nomeFuncionario { get; set; }

        [Display(Name = "Situação")]
        public string situacao { get; set; }

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

        [Display(Name = "CEP")]
        public string cep { get; set; }

        [Display(Name = "CPF")]
        public string cpf { get; set; }

        [Display(Name = "RG")]
        public string rg { get; set; }

        [Display(Name = "Data de nascimento")]
        public DateTime? dtNascimento { get; set; }

        [Display(Name = "CNH")]
        public string cnh { get; set; }

        [Display(Name = "Data de admissão")]
        public DateTime? dtAdmissao { get; set; }

        [Display(Name = "Data de demissão")]
        public DateTime? dtDemissao { get; set; }

        [Display(Name = "Salário")]
        public decimal? vlSalario { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime? dtCadastro { get; set; }

        [Display(Name = "Data da últ. alteração")]
        public DateTime? dtUltAlteracao { get; set; }

        //Condicao de pagamento
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