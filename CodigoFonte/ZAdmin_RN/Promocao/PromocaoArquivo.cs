using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class PromocaoArquivo
    {
        public int? idpromocao { get; set; }
        public int id { get; set; }        
        public string nome { get; set; }
        public string tipo { get; set; }
        public bool excluido { get; set; }
        public string url { get; set; }
        public string conteudo { get; set; }
    }
}
