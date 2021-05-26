using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.Select.Paises
{
    public class Select
    {
        public int? id { get; set; }
        public string text { get; set; }

        public string ddi { get; set; }
        public string sigla { get; set; }
        public DateTime? dtCadastro { get; set; }
        public DateTime? dtUltAlteracao { get; set; }

        //public string flCreate { get; set; }
       
    }
}