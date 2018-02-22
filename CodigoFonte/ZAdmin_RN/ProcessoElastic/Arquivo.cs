using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.ProcessoElastic
{
    public class Arquivo
    {
        public int idarquivo { get; set; }
        public string coordenacao { get; set; }
        public string nomearquivo { get; set; }
        public string numdoc { get; set; }
        public string situacao { get; set; }
        public string textoarquivo { get; set; }
        public string link { get; set; }
    }
}
