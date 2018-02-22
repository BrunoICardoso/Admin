using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    [ElasticsearchType(Name = "promocao")]
    public class PromocaoElastic
    {
        public int idpromocao { get; set; }
        public int? idorgaoregulador { get; set; }
        public int? idmodalidade { get; set; }
        public string nome { get; set; }
        public string certificadoautorizacao { get; set; }
        public string outrosinteressados { get; set; }
        public bool? abrangencia_nacional { get; set; }
        public string dtcadastro { get; set; }
        public string dtvigenciaini { get; set; }
        public string dtvigenciafim { get; set; }
        public string linksitepromocao { get; set; }
        public string linkfacebook { get; set; }
        public string linkinstagram { get; set; }
        public string linktwitter { get; set; }
        public string linkyoutube { get; set; }
        public string mecanicapromo { get; set; }
        public string produtosparticipantes { get; set; }
        public string premiospromo { get; set; }
        public decimal? valorpremio { get; set; }
        public string linkregulamento { get; set; }
        public string textoregulamento { get; set; }
        public bool excluido { get; set; }
        public string nomeorgaoregulador { get; set; }
        public string nomemodalidade { get; set; }
        public string nomepromocao { get; set; }
        public List<AbrangMunicipio> abrangmunicipios { get; set; }
        public List<AbrangEstados> abrangestados { get; set; }
        public List<Noticia> noticias { get; set; }
        public List<FacePost> postsfacebook { get; set; }
        public List<InstaPost> postsinstagram { get; set; }
        public List<TwPost> poststwitter { get; set; }
        public List<VideoYt> videosyoutube { get; set; }
        public List<Empresa> empresas { get; set; }
        public List<Arquivo> arquivosrelacionados { get; set; }
        public List<Arquivo> arquivosregulamento { get; set; }
    }

    public class Imagem
    {

    }

    public class AbrangMunicipio
    {
        public int idestado { get; set; }
        public int idmunicipio { get; set; }
        public string uf { get; set; }
        public string nome { get; set; }
    }

    public class AbrangEstados
    {
        public int idestado { get; set; }
        public string uf { get; set; }
        public string nome { get; set; }
    }

    public class FacePost
    {
        public int compartilhamentos { get; set; }
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public int idpost { get; set; }
        public string nomeimagem { get; set; }
        public string postagem { get; set; }
        public int qtdcomentarios { get; set; }
        //public List<FaceComentario> comentarios { get; set; }
        //public List<Promo> promocoes { get; set; }
    }

    public class FaceComentario
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string nomeusuario { get; set; }
        public string postagem { get; set; }
        public string urlimagem { get; set; }
        public List<FaceComentario> respostas { get; set; }
    }

    public class InstaPost
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public int idpost { get; set; }
        public string nomeimagem { get; set; }
        public string postagem { get; set; }
        public int qtdcomentarios { get; set; }
        //public List<Promo> promocoes { get; set; }
    }

    public class TwPost
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public int idpost { get; set; }
        public string nomeimagem { get; set; }
        public string postagem { get; set; }
        public int retweets { get; set; }
        //public List<Promo> promocoes { get; set; }
    }

    public class VideoYt
    {
        public int curtidas { get; set; }
        public string datahora { get; set; }
        public string descricao { get; set; }
        public int idvideo { get; set; }
        public string nomeimagem { get; set; }
        public int qtdcomentarios { get; set; }
        public int visualizacoes { get; set; }
        public int descurtidas { get; set; }
        //public List<Promo> promocoes { get; set; }
    }


    public class Empresa
    {
        public int idempresa { get; set; }
        public string nome { get; set; }
    }

    public class Arquivo
    {
        public string nomearquivo { get; set; }
        public string url { get; set; }
        public string tipo { get; set; }
    }

    public class Noticia
    {
        public string conteudo { get; set; }
        public string autor { get; set; }
        public string titulo { get; set; }
        public string nomefonte { get; set; }
        public string url { get; set; }
        public int idnoticia { get; set; }
        public string datapublicacao { get; set; }
    }
}
