using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class PromocaoNoticia
    {
        string _server = "";
        string _indexElastic = "";

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public PromocaoNoticia(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        private int _totalNoticias;

        public int totalNoticias
        {
            get { return _totalNoticias; }
            set { totalNoticias = value; }
        }

        public List<Noticias> RetornaNoticias(filtroPromocaoNoticia filtro)
        {

            List<Noticias> listaNoticias;

            /*
            FONTES:
            1 -> knewin
            2 -> ElasticSearch
            */

            switch (filtro.fontePesquisa)
            {
                case 1:

                    listaNoticias = RetornaNoticiasKnewin(filtro);

                    break;

                case 2:

                    listaNoticias = RetornaNoticiasElasticSearch(filtro);

                    break;


                default:

                    listaNoticias = RetornaNoticiasElasticSearch(filtro);

                    break;
            }

            return listaNoticias.Any() ? listaNoticias : null;
        }

        private List<Noticias> RetornaNoticiasElasticSearch(filtroPromocaoNoticia filtro)
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            DateTime dtInicial = filtro.dataInicial == null ? DateTime.Now.AddYears(-1) : new DateTime(filtro.dataInicial.Value.Year, filtro.dataInicial.Value.Month, filtro.dataInicial.Value.Day, 00, 00, 00, 123);

            DateTime dtFinal = filtro.dataFinal == null ? DateTime.Now : new DateTime(filtro.dataFinal.Value.Year, filtro.dataFinal.Value.Month, filtro.dataFinal.Value.Day, 23, 59, 59, 123);

            string dataInicialFormatada = string.Format("{0:dd/MM/yy HH:mm:ss}", dtInicial);
            string dataFinalFormatada = string.Format("{0:dd/MM/yy HH:mm:ss}", dtFinal);

            //filtro.empresa = filtro.empresa == 0 || filtro.empresa == null ? filtro.empresa == 0 : filtro.empresa;

            int posicao = filtro.pagina > 1 ? (((filtro.pagina * filtro.quantidade) - 10) + 1) : filtro.pagina - 1;

            ISearchResponse<Noticias> response;

            if (filtro.empresa == 0)
            {
                response = client.Search<Noticias>(s => s
                                .From(posicao)
                                .Query(q =>

                                        //(q.Term(p => p.titulo, filtro.pesquisa.ToLower()) ||
                                        //    q.Term(p => p.conteudo, filtro.pesquisa.ToLower())) &&
                                        q.DateRange(d => d
                                            .Field(f => f.datapublicacao)
                                            .GreaterThanOrEquals(dataInicialFormatada)
                                            .LessThanOrEquals(dataFinalFormatada)
                                        )
                                        &&
                                    //(
                                    //q.MatchPhrase(m => m
                                    //   .Analyzer("standard")
                                    //   .Field(p => p.titulo)
                                    //   .Query(filtro.pesquisa)
                                    //   .Operator(Operator.And)
                                    //)
                                    //    ||
                                    //    q.MatchPhrase(m => m
                                    //       .Analyzer("standard")
                                    //       .Field(p => p.conteudo)
                                    //       .Query(filtro.pesquisa)
                                    //       .Operator(Operator.Or)
                                    //    )
                                    //)

                                    q.MultiMatch(mm => mm
                                            .Analyzer("standard")
                                            .Query(filtro.pesquisa)
                                            .Operator(Operator.And)
                                            .Fields(fi => fi
                                                .Field("titulo")
                                                .Field("conteudo")
                                            )
                                        )
                                    )
                                    .Sort(x => x.Descending(p => p.datapublicacao))
                               );
            }
            else
            {

                response = client.Search<Noticias>(s => s
                           .From(posicao)
                           .Query(q =>
                               q.Bool(b => b.Must(m => m.Term("empresas.idempresa", filtro.empresa))) &&
                              //(q.Term(p => p.titulo, filtro.pesquisa.ToLower()) ||
                              // q.Term(p => p.conteudo, filtro.pesquisa.ToLower())) &&

                               q.MultiMatch(mm => mm
                                            .Analyzer("standard")
                                            .Query(filtro.pesquisa)
                                            .Operator(Operator.And)
                                            .Fields(fi => fi
                                                .Field("titulo")
                                                .Field("conteudo")
                                            )
                                        ) &&

                               q.DateRange(d => d
                                   .Field(f => f.datapublicacao)
                                   .GreaterThanOrEquals(dataInicialFormatada)
                                   .LessThanOrEquals(dataFinalFormatada)
                               ))
                               .Sort(x => x.Descending(p => p.datapublicacao))
                               );

            }


            _totalNoticias = Convert.ToInt32(response.Total);

            var listaNoticias = response.Documents.ToList<Noticias>().OrderByDescending(x => DateTime.Parse(x.datapublicacao)).ToList();
            //.Skip((filtro.pagina - 1) * filtro.quantidade).Take(filtro.quantidade).ToList();

            return listaNoticias;

        }

        private List<Noticias> RetornaNoticiasKnewin(filtroPromocaoNoticia filtro)
        {

            string Key = "2e9ee79e-7a4f-4f2f-bca3-25402e113997";

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://data.knewin.com/news");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "";

                    //ajusta paginação
                    /*                     
                    quantidade padrão de retorno noticias Knewin : 10; 
                    */

                    filtro.dataInicial = filtro.dataInicial == null ? DateTime.Now.AddYears(-1) : filtro.dataInicial;
                    filtro.dataFinal = filtro.dataFinal == null ? DateTime.Now : filtro.dataFinal;


                    int posicao = filtro.pagina > 1 ? (((filtro.pagina * filtro.quantidade) - 10) + 1) : filtro.pagina - 1;

                    if (filtro.dataInicial != null && filtro.dataFinal != null)
                    {

                        var dtInicial = filtro.dataInicial.Value.ToString("yyyy-MM-ddT00:00:00");
                        var dtFinal = filtro.dataFinal.Value.ToString("yyyy-MM-ddT00:00:00");

                        var listaFontesNoticias = RetornaListaFontesKnewin();

                        var listaFormatada = string.Join(",", listaFontesNoticias);

                        json = "{" +
                                        "\"key\":\"" + Key + "\"," +
                                        "\"query\":\"" + filtro.pesquisa.Replace("\"", "\\\"") + "\"," +
                                        "\"offset\":\"" + posicao + "\"," +
                                        "\"defaultOperator\":\"AND\"," +
                                        "\"filter\":" +
                                        "{" +
                                         "\"untilPublished\":\"" + dtFinal + "\"," +
                                          "\"sincePublished\":\"" + dtInicial + "\"," +
                                          "\"sourceId\":[" + listaFormatada + "]" +
                                    "}}";

                    }
                    else if (filtro.dataInicial != null && filtro.dataFinal != null)
                    {

                        var dtInicial = filtro.dataInicial.Value.ToString("yyyy-MM-ddT00:00:00");
                        var dtFinal = filtro.dataFinal.Value.ToString("yyyy-MM-ddT00:00:00");

                        var listaFontesNoticias = RetornaListaFontesKnewin();

                        var listaFormatada = string.Join(",", listaFontesNoticias);

                        json = "{" +
                                        "\"key\":\"" + Key + "\"," +
                                        "\"query\":\"" + filtro.pesquisa.Replace("\"", "\\\"") + "\"," +
                                        "\"offset\":\"" + posicao + "\"," +
                                        "\"defaultOperator\":\"AND\"," +
                                        "\"filter\":" +
                                        "{" +
                                         "\"untilPublished\":\"" + dtFinal + "\"," +
                                          "\"sincePublished\":\"" + dtInicial + "\"," +
                                          "\"sourceId\":[" + listaFormatada + "]" +
                                    "}}";


                    }
                    else if (filtro.dataInicial == null && filtro.dataFinal == null)
                    {


                        var dtInicial = filtro.dataInicial.Value.ToString("yyyy-MM-ddT00:00:00");
                        var dtFinal = filtro.dataFinal.Value.ToString("yyyy-MM-ddT00:00:00");

                        var listaFontesNoticias = RetornaListaFontesKnewin();

                        var listaFormatada = string.Join(",", listaFontesNoticias);

                        json = "{" +
                                        "\"key\":\"" + Key + "\"," +
                                        "\"query\":\"" + filtro.pesquisa.Replace("\"", "\\\"") + "\"," +
                                        "\"offset\":\"" + posicao + "\"," +
                                        "\"defaultOperator\":\"AND\"," +
                                        "\"filter\":" +
                                        "{" +
                                          "\"sourceId\":[" + listaFormatada + "]" +
                                    "}}";

                    }
                    else if (filtro.dataInicial == null && filtro.dataFinal == null)
                    {

                        var dtInicial = filtro.dataInicial.Value.ToString("yyyy-MM-ddT00:00:00");
                        var dtFinal = filtro.dataFinal.Value.ToString("yyyy-MM-ddT00:00:00");

                        var listaFontesNoticias = RetornaListaFontesKnewin();

                        var listaFormatada = string.Join(",", listaFontesNoticias);

                        json = "{" +
                                        "\"key\":\"" + Key + "\"," +
                                        "\"query\":\"" + filtro.pesquisa.Replace("\"", "\\\"") + "\"," +
                                        "\"offset\":\"" + posicao + "\"," +
                                        "\"defaultOperator\":\"AND\"," +
                                        "\"filter\":" +
                                        "{" +
                                          "\"sourceId\":[" + listaFormatada + "]" +
                                    "}}";

                    }
                    else
                    {

                        json = "{" +
                                             "\"key\":\"" + Key + "\"," +
                                             "\"query\":\"" + filtro.pesquisa.Replace("\"", "\\\"") + "\"," +
                                             "\"offset\":\"" + posicao + "\"," +
                                             "\"defaultOperator\":\"AND\"" +
                                         "}";

                    }

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var response = (HttpWebResponse)httpWebRequest.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                var resposta = JsonConvert.DeserializeObject<RootObject>(responseString);


                this._totalNoticias = resposta.num_docs;

                List<Noticias> lista = new List<Noticias>();

                foreach (var x in resposta.hits)
                {

                    Noticias noticia = new Noticias()
                    {
                        autor = x.author,
                        conteudo = x.content,
                        datapublicacao = x.published_date,
                        idnoticia = x.id,
                        nomefonte = x.source,
                        titulo = x.title,
                        url = x.url,
                        promocoes = RetornaPromocoesNoticiasKnewin(x.id)
                    };

                    lista.Add(noticia);

                }

                return lista.OrderByDescending(d => DateTime.Parse(d.datapublicacao)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<TagPromocao> RetornaPromocoesNoticiasKnewin(int idNoticiaKnewin)
        {

            ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

            var promosNoticia = (from promoNoticia in db.promo_promonoticias
                                 join noticia in db.noticias on promoNoticia.idnoticia equals noticia.idnoticia
                                 join promo in db.promo_promocoes on promoNoticia.idpromocao equals promo.idpromocao
                                 where
                                    noticia.idnoticia_knewin == idNoticiaKnewin
                                 select new TagPromocao()
                                 {
                                     idpromocao = promo.idpromocao,
                                     nome = promo.nome
                                 }).ToList();

            return promosNoticia;
        }


        private List<int?> RetornaListaFontesKnewin()
        {

            ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

            return db.fontes_noticias.Select(x => x.idfonte_knewin).ToList();

        }

        //public List<ItemCombo> RetornaFontesKnewin()
        //{

        //    ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        //    return db.fontes_noticias.Select(x =>
        //                                                            new ItemCombo()
        //                                                            {
        //                                                                idItem = (int) x.idfonte_knewin,
        //                                                                nome = x.nome
        //                                                            }).ToList();


        //}

        public List<ItemCombo> RetornaEmpresaNoticias()
        {
            ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

            var listaEmpresas = (from e in db.empresas
                                 orderby e.nome
                                 select new ItemCombo()
                                 {
                                     idItem = e.idempresa,
                                     nome = e.nome
                                 }).ToList();

            return listaEmpresas;
        }


        public Mensagem.Mensagem AssociaPromocaoNoticia(int idPromocao, int idNoticia, int fontePesquisa)
        {
            /*
            FONTES:
            1 -> knewin
            2 -> interna
            */

            Mensagem.Mensagem msg;

            switch (fontePesquisa)
            {
                case 1:
                    msg = atualizaPromocaoNoticiaKnewin(idPromocao, idNoticia);
                    break;
                case 2:
                    msg = atualizaPromocaoNoticiaInterna(idPromocao, idNoticia);
                    break;
                default:
                    msg =  atualizaPromocaoNoticiaInterna(idPromocao, idNoticia);
                    break;
            }

            AtualizaNoticiasEmPromocaoElastic(idPromocao);

            return msg;
        }

        public Mensagem.Mensagem atualizaPromocaoNoticiaKnewin(int idPromocao, int idNoticiaKnewin)
        {

            var idNoticiaBanco = db.noticias.Where(n => n.idnoticia_knewin == idNoticiaKnewin).Select(r => r.idnoticia).FirstOrDefault();

            var resultado = db.promo_promonoticias.Where(x => x.idpromocao == idPromocao && x.idnoticia == idNoticiaBanco).Select(x => x);

            if (resultado.Any())
            {
                return new Mensagem.Mensagem("A notícia já está associada a promoção.", Mensagem.Mensagem.tipoMensagem.Alerta, 2);
            }
            else
            {
                try
                {
                    //Se a notícia da Knewin não existir, faz o cadastro
                    if (idNoticiaBanco == 0)
                    {
                        var noticiasKnewin = new ZAdmin_RN.Knewin.NoticiasKnewin();
                        idNoticiaBanco = noticiasKnewin.ImportaNoticiaKnewin(idNoticiaKnewin, _server, _indexElastic);
                    }

                    //Associa noticia com a promoção no mysql
                    var promoNoticiaDB = new ZAdmin_DB.Model.promo_promonoticias();

                    promoNoticiaDB.idnoticia = idNoticiaBanco;
                    promoNoticiaDB.idpromocao = idPromocao;
                    db.promo_promonoticias.Add(promoNoticiaDB);
                    db.SaveChanges();

                    //Associa noticia com a promoção no elastic (sempre, independente de notícia recém cadastrada ou antiga)
                    atualizaPromocaoNoticiaElasticSearch(idNoticiaBanco);

                    return new Mensagem.Mensagem("A notícia " + idNoticiaBanco + " foi associada a promoção " + idPromocao + ".", Mensagem.Mensagem.tipoMensagem.Sucesso, 1);
                }
                catch (Exception e)
                {
                    return new Mensagem.Mensagem(e.ToString(), Mensagem.Mensagem.tipoMensagem.Erro, 2);
                }

            }
        }

        private Mensagem.Mensagem atualizaPromocaoNoticiaInterna(int idPromocao, int idNoticia)
        {


            var resultado = db.promo_promonoticias.Where(x => x.idpromocao == idPromocao && x.idnoticia == idNoticia).Select(x => x);

            if (resultado.Any())
            {
                return new Mensagem.Mensagem("A notícia já está associada a promoção.", Mensagem.Mensagem.tipoMensagem.Alerta, 2);
            }
            else
            {

                try
                {
                    // atualiza promoção mysql e elasticsearch
                    var promoNoticiaDB = new ZAdmin_DB.Model.promo_promonoticias();

                    promoNoticiaDB.idnoticia = idNoticia;
                    promoNoticiaDB.idpromocao = idPromocao;

                    db.promo_promonoticias.Add(promoNoticiaDB);

                    db.SaveChanges();

                    atualizaPromocaoNoticiaElasticSearch(idNoticia);
                                                   
                    return new Mensagem.Mensagem("A notícia " + idNoticia + " foi associada a promoção " + idPromocao + ".", Mensagem.Mensagem.tipoMensagem.Sucesso, 1);
                }
                catch (Exception e)
                {
                    return new Mensagem.Mensagem(e.ToString(), Mensagem.Mensagem.tipoMensagem.Erro, 2);
                }
            }
        }


        //public void AssociaPromocoesNoticia(int idNoticia)
        //{

        //    var node = new Uri(_server);
        //    var settings = new ConnectionSettings(node);
        //    settings.DisableDirectStreaming(true);
        //    settings.DefaultIndex(_indexElastic + "noticias");

        //    var client = new ElasticClient(settings);

        //    var listaPromocoes = (from promocao in db.promo_promocoes
        //                          join promoNoticias in db.promo_promonoticias on promocao.idpromocao equals promoNoticias.idpromocao
        //                          where promoNoticias.idnoticia == idNoticia
        //                          select new
        //                          {
        //                              idpromocao = promocao.idpromocao,
        //                              nome = promocao.nome
        //                          }).ToList();

        //    client.Update<Noticias, object>(idNoticia, d => d.Doc(new { promocoes = listaPromocoes }));


        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idNoticia"></param>
        /// <returns></returns>
        public Mensagem.Mensagem atualizaPromocaoNoticiaElasticSearch(int idNoticia)
        {

            ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();


            try
            {
                var node = new Uri(_server);
                var settings = new ConnectionSettings(node);
                settings.DisableDirectStreaming(true);
                settings.DefaultIndex(_indexElastic + "noticias");
                var client = new ElasticClient(settings);

                var listaPromocoes = (from promocao in db.promo_promocoes
                                      join promoNoticias in db.promo_promonoticias on promocao.idpromocao equals promoNoticias.idpromocao
                                      where promoNoticias.idnoticia == idNoticia
                                      select new
                                      {
                                          idpromocao = promocao.idpromocao,
                                          nome = promocao.nome
                                      }).ToList();

                client.Update<Noticias, object>(idNoticia, d => d.Doc(new { promocoes = listaPromocoes })
                                               );

                return new Mensagem.Mensagem("A notícia " + idNoticia + " foi associada a promoção.", Mensagem.Mensagem.tipoMensagem.Sucesso, 1);

            }
            catch (Exception e)
            {

                return new Mensagem.Mensagem(e.ToString(), Mensagem.Mensagem.tipoMensagem.Erro, 2);

            }

        }

        public void AtualizaNoticiasEmPromocaoElastic(int idPromocao) {

            var noticiasPromo = (from np in db.promo_promonoticias
                                 join n in db.noticias on np.idnoticia equals n.idnoticia
                                 where np.idpromocao == idPromocao
                                 select new
                                 {
                                     idnoticia = n.idnoticia,
                                     autor = n.autor,
                                     conteudo = n.conteudo,
                                     titulo = n.titulo,
                                     datapublicacao = n.datapublicacao,
                                     url = n.url,
                                     nomefonte = n.fontes_noticias.nome
                                 }).ToList();
                                                
            var noticias = noticiasPromo.Select(np => new ZAdmin_RN.Promocao.Noticia()
                                 {
                                     idnoticia = np.idnoticia,
                                     autor = np.autor,
                                     conteudo = np.conteudo,
                                     titulo = np.titulo,
                                     datapublicacao = np.datapublicacao != null ? np.datapublicacao.Value.ToString("yyyy-MM-dd") : null,
                                     url = np.url,
                                     nomefonte = np.nomefonte
                                 }).ToList();


            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            client.Update<Promocao, object>(idPromocao, d => d.Doc(new { Noticias = noticias }));


        }


        public void DesassociarPromocaoNoticia(TagPromocao tag)
        {
            ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

            /*
           FONTES:
           1 -> knewin
           2 -> interna
           */

            var idNoticia = 0;

            if (tag.fonteNoticia == 1)
            {
                idNoticia = db.noticias.Where(n => n.idnoticia_knewin == tag.idNoticia).Select(r => r.idnoticia).FirstOrDefault();
            }
            else
            {
                idNoticia = tag.idNoticia;
            }

            var noticiaExiste = db.promo_promonoticias.Where(x => x.idnoticia == idNoticia && x.idpromocao == tag.idpromocao).FirstOrDefault();

            if (noticiaExiste != null)
            {
                db.promo_promonoticias.Attach(noticiaExiste);
                db.promo_promonoticias.Remove(noticiaExiste);
                db.SaveChanges();
            }

            var listaNoticPromo = (from pro in db.promo_promocoes
                                   join n in db.promo_promonoticias on pro.idpromocao equals n.idpromocao
                                   where n.idnoticia == idNoticia
                                   select new { idpromocao = pro.idpromocao, nome = pro.nome }).ToList();


            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            client.Update<Noticias, object>(
                                                idNoticia,
                                                d => d.Doc(new { promocoes = listaNoticPromo })
                                            );

            AtualizaNoticiasEmPromocaoElastic(tag.idpromocao);

        }

        public List<Noticias> retornaNoticiasPromocao(int idPromocao, int pagina, int quantidade)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);

            int posicao = pagina > 1 ? ((pagina * quantidade) - 10) : pagina - 1;


            ISearchResponse<Noticias> resposta;

            resposta = client.Search<Noticias>(s => s
                                .From(posicao)
                                .Query(q => q
                                .Terms(x => x.Field("promocoes.idpromocao").Terms(idPromocao))
                                 ));

            if (resposta.Documents.Count == 0)
            {

                resposta = client.Search<Noticias>(s => s
                              .From(posicao)
                               );

            }

            _totalNoticias = Convert.ToInt32(resposta.Total);

            var listaNoticias = resposta.Documents.ToList<Noticias>();

            return listaNoticias;
        }

        public ElasticClient ConectaElastic()
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "noticias");

            var client = new ElasticClient(settings);
            return client;
        }

    }
}


public class SourceLocality
{
    public string country { get; set; }
    public string state { get; set; }
}

public class ImageHit
{
    public string url { get; set; }
    public string caption { get; set; }
    public string credit { get; set; }
}

public class Hit
{
    public string content { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public string domain { get; set; }
    public int id { get; set; }
    public int source_id { get; set; }
    public string source { get; set; }
    public string crawled_date { get; set; }
    public string published_date { get; set; }
    public string lang { get; set; }
    public List<SourceLocality> source_locality { get; set; }
    public List<ImageHit> image_hits { get; set; }
    public string author { get; set; }
    public string subtitle { get; set; }
    public string hat { get; set; }
    public string category { get; set; }
}

public class RootObject
{
    public int num_docs { get; set; }
    public int start { get; set; }
    public int count { get; set; }
    public int elapsedTime { get; set; }
    public List<Hit> hits { get; set; }
}
