using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.PromocaoRedesSociais
{
    public class PromocaoRedeSocialElastic
    {

    }

    public class fb_post
    {
        public string nomeRede { get; set; }
        public int idpost { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public int compartilhamentos { get; set; }
        public int reacoes { get; set; }
        public string nomeimagem { get; set; }
        public string nomeempresa { get; set; }
        public int qtdcomentarios { get; set; }
        public List<ZAdmin_RN.Promocao.Promocao> promocoes { get; set; }
        public List<comentariosFacebook> comentarios { get; set; }
    }

    public class comentariosFacebook
    {
        public int curtidas { get; set; }
        public string postagem { get; set; }
        public string datahora { get; set; }
        public string urlimagem { get; set; }
        public string nomeusuario { get; set; }
    }

    public class tw_post
    {
        public string nomeRede { get; set; }
        public int idpost { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public int compartilhamentos { get; set; }
        public int comentarios { get; set; }
        public int curtidas { get; set; }
        public int retweets { get; set; }
        public string nomeimagem { get; set; }
        public string nomeempresa { get; set; }
        public List<ZAdmin_RN.Promocao.Promocao> promocoes { get; set; }
    }

    public class yt_video
    {
        public string nomeRede { get; set; }
        public int idvideo { get; set; }
        public string datahora { get; set; }
        public string descricao { get; set; }
        public int compartilhamentos { get; set; }
        public int curtidas { get; set; }
        public int descurtidas { get; set; }
        public int visualizacoes { get; set; }
        public string nomeimagem { get; set; }
        public string nomeempresa { get; set; }
        public List<ZAdmin_RN.Promocao.Promocao> promocoes { get; set; }
        public List<comentariosYoutube> comentarios { get; set; }
    }

    public class comentariosYoutube
    {
        public int curtidas { get; set; }
        public string postagem { get; set; }
        public string datahora { get; set; }
        public string nomeusuario { get; set; }
    }

    public class insta_post
    {
        public string nomeRede { get; set; }
        public int idpost { get; set; }
        public string datahora { get; set; }
        public string postagem { get; set; }
        public int compartilhamentos { get; set; }
        public int curtidas { get; set; }
        public string nomeimagem { get; set; }
        public string nomeempresa { get; set; }
        public int qtdcomentarios { get; set; }
        public List<ZAdmin_RN.Promocao.Promocao> promocoes { get; set; }
        public List<comentariosInstagram> comentarios { get; set; }
    }

    public class comentariosInstagram
    {
        public string postagem { get; set; }
        public string datahora { get; set; }
        public string nomeusuario { get; set; }
    }



}
