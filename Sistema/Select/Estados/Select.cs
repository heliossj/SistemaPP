using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.Select.Estados
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public string uf { get; set; }
        public Sistema.Select.Paises.Select PaisSelect { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtUltAlteracao { get; set; }
    }
}