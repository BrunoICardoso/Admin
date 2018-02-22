using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.PromocaoRedesSociais
{
    public class PromocaoRedeSocial
    {
        public int idpost { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public int compartilhamentos { get; set; }
        public int comentarios { get; set; }
        public int reacoes { get; set; }
        
        public List<fb_post> ListaFacebook { get; set; }
        public int TotalListaFacebook { get; set; }

        public List<tw_post> ListaTwitter { get; set; }
        public int TotalListaTwitter { get; set; }

        public List<insta_post> ListaInstagram { get; set; }
        public int TotalListaInstagram { get; set; }

        public List<yt_video> ListaYoutube { get; set; }
        public int TotalListaYoutube { get; set; }
    }
}
