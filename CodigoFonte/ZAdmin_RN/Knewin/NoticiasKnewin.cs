using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Knewin
{
    public class NoticiasKnewin
    {
        public int ImportaNoticiaKnewin(int idKnewin, string elasticServer, string elasticIndex)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();
            var noticiaK = CapturaNoticia(idKnewin);

            //Cria um objeto de noticia para o banco
            var n = new ZAdmin_DB.Model.noticias()
            {
                autor = noticiaK.autor,
                categoria = noticiaK.categoria,
                conteudo = noticiaK.conteudo,
                datacaptura_knewin = noticiaK.datacapturaknewin,
                datacaptura_zeeng = DateTime.Now,
                datapublicacao = noticiaK.datapublicacao,
                dominio = noticiaK.dominio,
                idfonte = RetornaIdFonte(noticiaK.idfonte, noticiaK.nomefonte),
                idnoticia_knewin = noticiaK.idnoticia_knewin,
                linguagem = noticiaK.linguagem,
                localidade = noticiaK.localidade,
                subtitulo = noticiaK.subtitulo,
                titulo = noticiaK.titulo,
                url = noticiaK.url,
            };
            db.noticias.Add(n);

            //Buscar as imagens das notícias
            foreach (var img in noticiaK.imagens)
            {
                var imag = new ZAdmin_DB.Model.noticia_imagens()
                {
                    url = img.url,
                    titulo = img.titulo,
                    creditos = img.creditos
                };
                db.noticia_imagens.Add(imag);
            }
            db.SaveChanges();

            ZAdmin_RN.NoticiasElastic.Noticia noticiaE = ConverteKnewinElastic(n.idnoticia, noticiaK);
            InsereElastic(noticiaE, elasticServer, elasticIndex);

            //Retorna o id da nova notícia cadastrada
            return n.idnoticia;
        }

        public ZAdmin_RN.NoticiasElastic.Noticia ConverteKnewinElastic(int idnoticia, NoticiaKnewin noticiaK)
        {
            ZAdmin_RN.NoticiasElastic.Noticia noticiaE = new NoticiasElastic.Noticia()
            {
                idnoticia = idnoticia,
                idnoticiaknewin = noticiaK.idnoticia_knewin,
                idfonte = noticiaK.idfonte,
                nomefonte = noticiaK.nomefonte,
                titulo = noticiaK.titulo,
                subtitulo = noticiaK.subtitulo,
                dominio = noticiaK.dominio,
                autor = noticiaK.autor,
                conteudo = noticiaK.conteudo,
                url = noticiaK.url,
                localidade = noticiaK.localidade,
                linguagem = noticiaK.linguagem,
                datapublicacao = noticiaK.datapublicacao.ToString("dd/MM/yyyy HH:mm:ss"),
                datacaptura_knewin = noticiaK.datacapturaknewin.ToString("dd/MM/yyyy HH:mm:ss"),
                datacaptura_zeeng = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            };

            noticiaE.imagens = new List<ZAdmin_RN.NoticiasElastic.NoticiaImagem>();

            foreach (var img in noticiaK.imagens)
            {
                var imag = new ZAdmin_RN.NoticiasElastic.NoticiaImagem()
                {
                    url = img.url,
                    titulo = img.titulo,
                    creditos = img.creditos
                };
                noticiaE.imagens.Add(imag);
            }

            return noticiaE;
        }


        public NoticiaKnewin CapturaNoticia(int idKnewin)
        {
            string Key = "2e9ee79e-7a4f-4f2f-bca3-25402e113997";

            //try
            //{
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://data.knewin.com/news");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "";

                json = "{" +
                          "\"key\":\"" + Key + "\"," +
                          "\"newsId\": [" + idKnewin + "]}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var resposta = JsonConvert.DeserializeObject<RootObject>(responseString);
            var n = resposta.hits[0];

            DateTime dtTest;

            NoticiaKnewin noticia = new NoticiaKnewin()
            {
                idfonte = n.source_id,
                autor = n.author,
                conteudo = n.content,
                datacapturaknewin = DateTime.Parse(n.crawled_date),
                datapublicacao = DateTime.TryParse(n.published_date, out dtTest) ? DateTime.Parse(n.published_date) : DateTime.Parse(n.crawled_date),
                idnoticia_knewin = n.id,
                nomefonte = n.source,
                titulo = n.title,
                subtitulo = n.subtitle,
                linguagem = n.lang,
                url = n.url,
                categoria = n.category,
            };

            noticia.imagens = new List<NoticiaImagem>();
            if (n.image_hits != null)
            {
                foreach (var img in n.image_hits)
                {
                    NoticiaImagem imagem = new NoticiaImagem()
                    {
                        url = img.url,
                        titulo = img.caption,
                        creditos = img.credit
                    };
                    noticia.imagens.Add(imagem);
                }
            }
            return noticia;

            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}

        }


        public void InsereElastic(ZAdmin_RN.NoticiasElastic.Noticia noticia, string elasticServer, string elasticIndex)
        {
            var node = new Uri(elasticServer);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(elasticIndex + "noticias");

            var client = new ElasticClient(settings);

            var resposta = client.Index(noticia, i => i.Type("noticias").Id(noticia.idnoticia));
        }


        public int RetornaIdFonte(int idfonteknewin, string nomefonte)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var idfonte = db.fontes_noticias.Where(fonte => fonte.idfonte_knewin == idfonteknewin).Select(f => f.idfonte).FirstOrDefault();
            if (idfonte == 0)
            {
                idfonte = InsereFonte(idfonteknewin, nomefonte);
            }

            return idfonte;
        }

        public int InsereFonte(int idfonteknewin, string nomefonte)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();
            var fonteKnewin = new ZAdmin_DB.Model.fontes_noticias();

            fonteKnewin.idfonte_knewin = idfonteknewin;
            fonteKnewin.nome = nomefonte;

            db.fontes_noticias.Add(fonteKnewin);

            db.SaveChanges();

            return fonteKnewin.idfonte;

        }
    }
}
