using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class Noticias
    {

        public int idnoticia { get; set; }
        public string titulo { get; set; }
        public string nomefonte { get; set; }
        public string datapublicacao { get; set; }
        public string conteudo { get; set; }
        public string autor { get; set; }
        public string url { get; set; }

        public List<TagPromocao> promocoes { get; set; }

    }
}
