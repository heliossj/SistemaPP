using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.Select.Cidades
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public string ddd { get; set; }
        public string sigla { get; set; }
        public Sistema.Select.Estados.Select EstadoSelect { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtUltAlteracao { get; set; }
    }
}