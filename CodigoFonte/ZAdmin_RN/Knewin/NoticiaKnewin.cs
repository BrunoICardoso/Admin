using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Knewin
{
    public class NoticiaKnewin
    {

        public int idfonte { get; set; }
        public int idnoticia_knewin { get; set; }
        public string titulo { get; set; }
        public string subtitulo { get; set; }
        public string dominio { get; set; }
        public string autor { get; set; }
        public string conteudo { get; set; }
        public string url { get; set; }
        public DateTime datapublicacao { get; set; }
        public DateTime datacapturaknewin { get; set; }
        public string categoria { get; set; }
        public string localidade { get; set; }
        public string linguagem { get; set; }
        public string nomefonte { get; set; }
        public List<NoticiaImagem> imagens { get; set;}
    }
}
