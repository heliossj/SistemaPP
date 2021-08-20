using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.Select.Grupos
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }
        public string situacao { get; set; }
        public string observacao { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtUltAlteracao { get; set; }
    }
}