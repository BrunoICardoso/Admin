using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.Listas;
using ZAdmin_RN.Utils;

namespace ZAdmin_RN.Promocao
{
    public class Promocoes
    {

        public string _server = "";
        private string _indexElastic = "";
        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public int TotalRegistros = 0;

        public Promocoes(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        public Promocoes() { }

        private int _totalPromocoes;

        public int totalPromocoes
        {
            get { return _totalPromocoes; }
            set { _totalPromocoes = value; }
        }

        public List<itemTabelaPromocao> RetornaPromocoes(filtroIndexPromocoes filtro)
        {

            if (filtro.dataInicialCadastrada.HasValue)
            {
                filtro.dataInicialCadastrada = DateTime.Parse(filtro.dataInicialCadastrada.Value.ToShortDateString());
            }

            if (filtro.dataFinalCadastrada.HasValue)
            {
                filtro.dataFinalCadastrada = DateTime.Parse(filtro.dataFinalCadastrada.Value.ToShortDateString());
            }

            if (filtro.dataInicialVigencia.HasValue)
            {
                filtro.dataInicialVigencia = DateTime.Parse(filtro.dataInicialVigencia.Value.ToShortDateString());
            }

            if (filtro.dataFinalVigencia.HasValue)
            {
                filtro.dataFinalVigencia = DateTime.Parse(filtro.dataFinalVigencia.Value.ToShortDateString());
            }

            var promocoes = (from promocao in db.promo_promocoes
                             join promoempresa in db.promo_promoempresas on promocao.idpromocao equals promoempresa.idpromocao into pe
                             from promoempresa in pe.DefaultIfEmpty()
                             join empresas in db.empresas on promoempresa.idempresa equals empresas.idempresa into e
                             from empresa in e.DefaultIfEmpty()
                             join modalidade in db.promo_modalidades on promocao.idmodalidade equals modalidade.idpromomodalidade into m
                             from modalidade in m.DefaultIfEmpty()
                             where
                                (promocao.excluido == false)
                                &&
                                (filtro.empresa == empresa.idempresa || filtro.empresa == 0 || filtro.empresa == null)
                                &&
                                (
                                    promocao.nome.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.certificadoautorizacao.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.outrosinteressados.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.mecanicapromo.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.produtosparticipantes.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.premiospromo.ToLower().Contains(filtro.termos.ToLower()) ||
                                    promocao.textoregulamento.ToLower().Contains(filtro.termos.ToLower()) ||
                                    filtro.termos == "" || filtro.termos == null
                                )
                                &&
                                (filtro.modalidade == null || filtro.modalidade == 0 || promocao.idmodalidade == filtro.modalidade)
                                &&
                                (filtro.dataInicialCadastrada == null || promocao.dtcadastro >= filtro.dataInicialCadastrada)
                                &&
                                (filtro.dataFinalCadastrada == null || promocao.dtcadastro <= filtro.dataFinalCadastrada)
                                &&
                                (filtro.dataInicialVigencia == null || promocao.dtvigenciaini >= filtro.dataInicialVigencia)
                                &&
                                (filtro.dataFinalVigencia == null || promocao.dtvigenciafim <= filtro.dataFinalVigencia)

                             select new itemTabelaPromocao()
                             {
                                 idPromocao = promocao.idpromocao,
                                 promocao = promocao.nome,
                                 empresa = empresa == null ? "" : empresa.nome,
                                 modalidade = modalidade == null ? "" : modalidade.nome,
                                 dataCadastro = promocao.dtcadastro,
                                 vigenciaInicial = promocao.dtvigenciaini,
                                 vigenciaFinal = promocao.dtvigenciafim
                             }
                            );

            //var promocoes = (from promocao in db.promo_promocoes
            //                 join empresa in db.promo_promoempresas on promocao.idpromocao equals empresa.idpromocao
            //                 join emp in db.empresas on empresa.idempresa equals emp.idempresa
            //                 join modalidade in db.promo_modalidades on promocao.idmodalidade equals modalidade.idpromomodalidade
            //                 where

            //                        (promocao.excluido == false)

            //                        &&

            //                        (filtro.empresa == empresa.idempresa || filtro.empresa == 0 || filtro.empresa == null)
            //                        &&

            //                        (

            //                         promocao.nome.ToLower().Contains(filtro.termos.ToLower()) ||
            //                          promocao.certificadoautorizacao.ToLower().Contains(filtro.termos.ToLower()) ||
            //                           promocao.outrosinteressados.ToLower().Contains(filtro.termos.ToLower()) ||
            //                            promocao.mecanicapromo.ToLower().Contains(filtro.termos.ToLower()) ||
            //                             promocao.produtosparticipantes.ToLower().Contains(filtro.termos.ToLower()) ||
            //                              promocao.premiospromo.ToLower().Contains(filtro.termos.ToLower()) ||
            //                               promocao.textoregulamento.ToLower().Contains(filtro.termos.ToLower()) ||
            //                                filtro.termos == "" || filtro.termos == null
            //                                 ) &&

            //                        (promocao.idmodalidade == filtro.modalidade || filtro.modalidade == 0 || filtro.modalidade == null)
            //                        &&

            //                        (
            //                         promocao.dtcadastro >= filtro.dataInicialCadastrada &&
            //                         promocao.dtcadastro <= filtro.dataFinalCadastrada ||
            //                              (
            //                              filtro.dataInicialCadastrada == null ||
            //                              filtro.dataFinalCadastrada == null
            //                              )
            //                         ) &&

            //                         (
            //                             (
            //                             promocao.dtvigenciaini >= filtro.dataInicialVigencia &&
            //                             promocao.dtvigenciafim <= filtro.dataFinalVigencia
            //                             ) ||
            //                                 (
            //                                 filtro.dataInicialVigencia == null ||
            //                                 filtro.dataFinalVigencia == null
            //                                 )
            //                         )

            //                 select new itemTabelaPromocao()
            //                 {
            //                     idPromocao = promocao.idpromocao,
            //                     promocao = promocao.nome,
            //                     empresa = emp.nome,
            //                     modalidade = modalidade.nome,
            //                     dataCadastro = promocao.dtcadastro,
            //                     vigenciaInicial = promocao.dtvigenciaini,
            //                     vigenciaFinal = promocao.dtvigenciafim
            //                 }
            //                );


            switch (filtro.nomeColuna)
            {
                case "empresa":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.empresa);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.empresa);

                    break;

                case "modalidade":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.modalidade);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.modalidade);

                    break;

                case "promocao":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.promocao);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.promocao);

                    break;

                case "dataCadastro":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.dataCadastro);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.dataCadastro);

                    break;

                case "vigenciaInicial":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.vigenciaInicial);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.vigenciaInicial);

                    break;

                case "vigenciaFinal":

                    if (filtro.asc)
                        promocoes = promocoes.OrderBy(x => x.vigenciaFinal);
                    else
                        promocoes = promocoes.OrderByDescending(x => x.vigenciaFinal);

                    break;

                default:

                    promocoes = promocoes.OrderBy(x => x.empresa);

                    break;
            }

            _totalPromocoes = promocoes.Count();

            //var xx = promocoes.OrderBy(x => x.promocao).ToList();

            var resultado = promocoes.Skip((filtro.pagina - 1) * filtro.quantidade).Take(filtro.quantidade).ToList();

            return resultado;
        }

        public void remover(int idPromocao)
        {

            var promocaoDB = (from promocao in db.promo_promocoes
                              where promocao.idpromocao == idPromocao &&
                                    promocao.excluido == false
                              select promocao).SingleOrDefault();

            promocaoDB.excluido = true;
            db.SaveChanges();

        }

        public List<ItemCombo> retornaModalidades()
        {

            var modalidades = (from modalidade in db.promo_modalidades
                               where modalidade.excluido == false
                               select new ItemCombo()
                               {
                                   idItem = modalidade.idpromomodalidade,
                                   nome = modalidade.nome
                               }).ToList();

            return modalidades;
        }

        public List<ItemCombo> retornaEmpresas()
        {
            var empresas = (from empresa in db.empresas
                            where empresa.excluido == false
                            orderby empresa.nome ascending
                            select new ItemCombo()
                            {
                                idItem = empresa.idempresa,
                                nome = empresa.nome
                            }).ToList();

            return empresas;
        }

        public int EditarPromocao(Promocao dados)
        {
            int idpromocao;

            idpromocao = EdicaoSalvarPromocaoMySQL(dados);
            EdicaoSalvarPromocaoElasticSearch(idpromocao);

            return idpromocao;
        }

        public int EdicaoSalvarPromocaoMySQL(Promocao dados)
        {
            var promoDB = new ZAdmin_DB.Model.promo_promocoes();

            promoDB = db.promo_promocoes.Where(x => x.idpromocao == dados.idPromocao).FirstOrDefault();

            promoDB.idorgaoregulador = dados.idOrgaoregulador != null ? dados.idOrgaoregulador : null;
            promoDB.idmodalidade = dados.idModalidade != null ? dados.idModalidade : 1; // 1 - modalidade outros
            promoDB.nome = dados.nome;
            promoDB.certificadoautorizacao = dados.certificadoAutorizacao;
            promoDB.outrosinteressados = dados.outrosInteressados;
            promoDB.abrangencianacional = dados.abrangenciaNacional;

            if (dados.dtCadastro == null && promoDB.dtcadastro == null)
                promoDB.dtcadastro = DateTime.Now;
            else if (dados.dtCadastro != null && promoDB.dtcadastro == null)
                promoDB.dtcadastro = dados.dtCadastro;
            else if (dados.dtCadastro == null && promoDB.dtcadastro != null)
                promoDB.dtcadastro = promoDB.dtcadastro;
            else if (dados.dtCadastro != null && promoDB.dtcadastro != null)
                promoDB.dtcadastro = dados.dtCadastro;
            else
                promoDB.dtcadastro = promoDB.dtcadastro;

            if (dados.dtVigenciaIni == null && promoDB.dtvigenciaini == null)
                promoDB.dtvigenciaini = null;
            else if (dados.dtVigenciaIni != null && promoDB.dtvigenciaini == null)
                promoDB.dtvigenciaini = dados.dtVigenciaIni;
            else if (dados.dtVigenciaIni == null && promoDB.dtvigenciaini != null)
                promoDB.dtvigenciaini = promoDB.dtvigenciaini;
            else if (dados.dtVigenciaIni != null && promoDB.dtvigenciaini != null)
                promoDB.dtvigenciaini = dados.dtVigenciaIni;
            else
                promoDB.dtvigenciaini = promoDB.dtvigenciaini;

            if (dados.dtVigenciaFim == null && promoDB.dtvigenciafim == null)
                promoDB.dtvigenciafim = null;
            else if (dados.dtVigenciaFim != null && promoDB.dtvigenciafim == null)
                promoDB.dtvigenciafim = dados.dtVigenciaFim;
            else if (dados.dtVigenciaFim == null && promoDB.dtvigenciafim != null)
                promoDB.dtvigenciafim = promoDB.dtvigenciafim;
            else if (dados.dtVigenciaFim != null && promoDB.dtvigenciafim != null)
                promoDB.dtvigenciafim = dados.dtVigenciaFim;
            else
                promoDB.dtvigenciafim = promoDB.dtvigenciafim;

            promoDB.valorpremios = dados.valorPremio;
            promoDB.linksitepromocao = dados.linkSitePromocao;
            promoDB.linkfacebook = dados.linkFacebook;
            promoDB.linkinstagram = dados.linkInstagram;
            promoDB.linktwitter = dados.linkTwitter;
            promoDB.linkyoutube = dados.linkYoutube;
            promoDB.mecanicapromo = dados.mecanicaPromo;
            promoDB.produtosparticipantes = dados.produtosParticipantes;
            promoDB.premiospromo = dados.premiosPromo;
            promoDB.linkregulamento = dados.linkRegulamento;
            promoDB.textoregulamento = dados.textoRegulamento;
            promoDB.excluido = false;

            // Salvar empresas
            // =======================================
            if (dados.ListaEmpresas != null && dados.ListaEmpresas.Count() > 0)
            {
                foreach (var idempresa in dados.ListaEmpresas)
                {
                    var existeEmp = promoDB.promo_promoempresas.Where(x => x.idempresa == idempresa).FirstOrDefault();
                    if (existeEmp == null)
                    {
                        var empDB = new ZAdmin_DB.Model.promo_promoempresas();
                        empDB.idempresa = idempresa;
                        empDB.idpromocao = promoDB.idpromocao;

                        promoDB.promo_promoempresas.Add(empDB);
                    }
                }
            }

            //Exclui as empresas retiradas da promoção no MySQL
            var empresasExcluir = (from e in promoDB.promo_promoempresas
                                   where (dados.ListaEmpresas == null) || !(from ev in dados.ListaEmpresas select ev).Contains(e.idempresa.Value)
                                   select e).ToList();

            foreach (var e in empresasExcluir)
            {
                db.promo_promoempresas.Remove(e);
            }

            //Salvar Estados
            if (dados.ListaEstados != null && dados.ListaEstados.Count() > 0)
            {
                foreach (var idestado in dados.ListaEstados)
                {
                    var existeEmp = db.promo_estadosabrangencia.Where(x => x.idestado == idestado && x.idpromocao == promoDB.idpromocao).FirstOrDefault();
                    if (existeEmp == null)
                    {
                        var empDB = new ZAdmin_DB.Model.promo_estadosabrangencia();
                        empDB.idestado = idestado;
                        empDB.idpromocao = promoDB.idpromocao;

                        promoDB.promo_estadosabrangencia.Add(empDB);
                    }
                }
            }

            //Excluir estados
            var estadosExcluir = (from e in promoDB.promo_estadosabrangencia
                                   where (dados.ListaEstados == null) || !(from ev in dados.ListaEstados select ev).Contains(e.idestado.Value)
                                   select e).ToList();

            foreach (var e in estadosExcluir)
            {
                db.promo_estadosabrangencia.Remove(e);
            }

            //Salvar Municipios
            if (dados.ListaMunicipios != null && dados.ListaMunicipios.Count() > 0)
            {
                foreach (var idmunicipio in dados.ListaMunicipios)
                {
                    var existeEmp = db.promo_municipiosabrangencia.Where(x => x.idmunicipio == idmunicipio && x.idpromocao == promoDB.idpromocao).FirstOrDefault();
                    if (existeEmp == null)
                    {
                        var empDB = new ZAdmin_DB.Model.promo_municipiosabrangencia();
                        empDB.idmunicipio = idmunicipio;
                        empDB.idpromocao = promoDB.idpromocao;

                        promoDB.promo_municipiosabrangencia.Add(empDB);
                    }
                }
            }

            //Excluir municipios
            var municipiosExcluir = (from e in promoDB.promo_municipiosabrangencia
                                  where (dados.ListaMunicipios == null) || !(from ev in dados.ListaMunicipios select ev).Contains(e.idmunicipio.Value)
                                  select e).ToList();

            foreach (var e in municipiosExcluir)
            {
                db.promo_municipiosabrangencia.Remove(e);
            }

            if (promoDB.idpromocao == 0)
                db.promo_promocoes.Add(promoDB);

            db.SaveChanges();

            return promoDB.idpromocao;
        }


        public int EdicaoSalvarPromocaoElasticSearch(int idpromocao)
        {
            var promoDB = new ZAdmin_DB.Model.promo_promocoes();

            promoDB = db.promo_promocoes.Include("promo_estadosabrangencia.estados").Include("promo_promoempresas.empresas").
                Include("promo_municipiosabrangencia.municipios").Where(x => x.idpromocao == idpromocao).FirstOrDefault();

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            //var resposta = client.Update<Promocao, object>(idpromocao, d => d.Doc(promo));

            var resposta = client.Update<Promocao, object>(idpromocao, d => d.Doc(
                                    new
                                    {
                                        idorgaoregulador = promoDB.idorgaoregulador,
                                        idmodalidade = promoDB.idmodalidade,
                                        nomepromocao = promoDB.nome,
                                        certificadoautorizacao = promoDB.certificadoautorizacao,
                                        outrosinteressados = promoDB.outrosinteressados,
                                        abrangencia_nacional = promoDB.abrangencianacional,
                                        dtcadastro = promoDB.dtcadastro != null ? promoDB.dtcadastro.Value.ToString("yyyy-MM-dd") : null,
                                        dtvigenciaini = promoDB.dtvigenciaini != null ? promoDB.dtvigenciaini.Value.ToString("yyyy-MM-dd") : null,
                                        dtvigenciafim = promoDB.dtvigenciafim != null ? promoDB.dtvigenciafim.Value.ToString("yyyy-MM-dd") : null,
                                        valorpremio = promoDB.valorpremios,
                                        linksitepromocao = promoDB.linksitepromocao,
                                        linkfacebook = promoDB.linkfacebook,
                                        linkinstagram = promoDB.linkinstagram,
                                        linktwitter = promoDB.linktwitter,
                                        linkyoutube = promoDB.linkyoutube,
                                        mecanicapromo = promoDB.mecanicapromo,
                                        nomemodalidade = db.promo_modalidades.Where(x => x.idpromomodalidade == promoDB.idmodalidade).Select(x => x.nome).FirstOrDefault(),
                                        produtosparticipantes = promoDB.produtosparticipantes,
                                        premiospromo = promoDB.premiospromo,
                                        linkregulamento = promoDB.linkregulamento,
                                        textoregulamento = promoDB.textoregulamento,
                                        excluido = false,
                                        abrangestados = promoDB.promo_estadosabrangencia.Select(x => new AbrangEstados { idestado = x.idestado.Value, nome = x.estados.nome, uf = x.estados.uf }).ToList(),
                                        empresas = promoDB.promo_promoempresas.Select(x => new Empresa { idempresa = (int)x.idempresa, nome = x.empresas.nome }).ToList(),
                                        abrangmunicipios = promoDB.promo_municipiosabrangencia.Select(x => new AbrangMunicipio { idmunicipio = (int)x.idmunicipio, nome = x.municipios.nome, idestado = (int)x.municipios.idestado, uf = x.municipios.estados.uf }).ToList(),
                                        arquivosregulamento = promoDB.promo_regulamentoarquivos.Select(x => new Arquivo { nomearquivo = x.nome, tipo = x.tipo }),
                                        arquivosrelacionados = promoDB.promo_arquivos.Select(x => new Arquivo { nomearquivo = x.nome, tipo = x.tipo, url = x.url })
                                    }                
                ));

            return idpromocao;
        }

        public int CadastrarPromocao(Promocao dados)
        {
            int idpromocao;

            idpromocao = CadastroSalvarPromocaoMySQL(dados);
            CadastroSalvarPromocaoElasticSearch(idpromocao, dados);

            return idpromocao;
        }

        private int CadastroSalvarPromocaoMySQL(Promocao dados)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var promoDB = new ZAdmin_DB.Model.promo_promocoes();

            promoDB.idorgaoregulador = dados.idOrgaoregulador != null ? dados.idOrgaoregulador : null;
            promoDB.idmodalidade = dados.idModalidade != null ? dados.idModalidade : 1; //1 - outros
            promoDB.nome = dados.nome;
            promoDB.certificadoautorizacao = dados.certificadoAutorizacao;
            promoDB.outrosinteressados = dados.outrosInteressados;
            promoDB.abrangencianacional = dados.abrangenciaNacional;
            promoDB.dtcadastro = DateTime.Now;
            promoDB.dtvigenciaini = dados.dtVigenciaIni != null ? dados.dtVigenciaIni : null;
            promoDB.dtvigenciafim = dados.dtVigenciaFim != null ? dados.dtVigenciaFim : null;
            promoDB.valorpremios = dados.valorPremio;
            promoDB.linksitepromocao = dados.linkSitePromocao;
            promoDB.linkfacebook = dados.linkFacebook;
            promoDB.linkinstagram = dados.linkInstagram;
            promoDB.linktwitter = dados.linkTwitter;
            promoDB.linkyoutube = dados.linkYoutube;
            promoDB.mecanicapromo = dados.mecanicaPromo;
            promoDB.produtosparticipantes = dados.produtosParticipantes;
            promoDB.premiospromo = dados.premiosPromo;
            promoDB.linkregulamento = dados.linkRegulamento;
            promoDB.textoregulamento = dados.textoRegulamento;
            promoDB.excluido = false;

            db.promo_promocoes.Add(promoDB);
            db.SaveChanges();

            // Caso o cadastro esteja sendo feito a partir de notícia
            if (dados.idNoticia > 0)
            {
                var rnPromocaoNoticia = new PromocaoNoticia(_server, _indexElastic);
                rnPromocaoNoticia.AssociaPromocaoNoticia(promoDB.idpromocao, dados.idNoticia, dados.fontePesquisa);
            }

            // Caso o cadastro esteja sendo feito a partir de um post
            if (dados.idRedeSocial > 0)
            {
                var rnPromocaoRedesSociais = new PromocaoRedesSociais.PromocaoRedesSociais(_server, _indexElastic);
                rnPromocaoRedesSociais.associarPromocaoPost(promoDB.idpromocao, dados.idRedeSocial, dados.nomeRedeSocial);
            }

            //var dadosProc = db.seae_processos.Where(x => x.idprocesso == dados.idProcesso).FirstOrDefault();
            return promoDB.idpromocao;
        }

        private void CadastroSalvarPromocaoElasticSearch(int idpromocao, Promocao dados)
        {
            var orgao = db.promo_orgaosreguladores.Where(x => x.idorgao == dados.idOrgaoregulador).FirstOrDefault();

            ZAdmin_RN.Promocao.PromocaoElastic promoElastic = new PromocaoElastic()
            {
                idpromocao = idpromocao,
                idorgaoregulador = dados.idOrgaoregulador,
                nomeorgaoregulador = orgao.nome,
                nome = dados.nome,
                certificadoautorizacao = dados.certificadoAutorizacao,
                dtcadastro = DateTime.Now.ToString("yyyy-MM-dd")
            };

            if (dados.idNoticia > 0)
            {
                var noticiadb = (dados.fontePesquisa == 1) ? db.noticias.Where(x => x.idnoticia_knewin == dados.idNoticia).FirstOrDefault() :
                    db.noticias.Where(x => x.idnoticia == dados.idNoticia).FirstOrDefault();

                promoElastic.noticias = new List<Noticia>();

                Noticia noticia = new Noticia()
                {
                    idnoticia = noticiadb.idnoticia,
                    autor = noticiadb.autor,
                    conteudo = noticiadb.conteudo,
                    titulo = noticiadb.titulo,
                    datapublicacao = noticiadb.datapublicacao != null ? noticiadb.datapublicacao.Value.ToString("yyyy-MM-dd") : null,
                    url = noticiadb.url,
                    nomefonte = db.fontes_noticias.Where(x => x.idfonte == noticiadb.idfonte).FirstOrDefault().nome
                };
                promoElastic.noticias.Add(noticia);
            }

            if (dados.idRedeSocial > 0)
            {
                switch (dados.nomeRedeSocial)
                {
                    case "facebook":
                        {
                            var postdb = db.fb_posts.Where(x => x.idpost == dados.idRedeSocial).FirstOrDefault();

                            promoElastic.postsfacebook = new List<FacePost>();

                            FacePost post = new FacePost()
                            {
                                idpost = (int)postdb.idpost,
                                compartilhamentos = (int)postdb.compartilhamentos,
                                curtidas = postdb.reacoes,
                                datahora = postdb.datahora.ToString("dd/MM/yyyy HH:mm:ss"),
                                postagem = postdb.postagem,
                                nomeimagem = postdb.nomeimagem,
                                qtdcomentarios = (int)postdb.comentarios
                            };

                            promoElastic.postsfacebook.Add(post);

                            break;
                        }
                    case "twitter":
                        {
                            var postdb = db.tw_posts.Where(x => x.idpost == dados.idRedeSocial).FirstOrDefault();

                            promoElastic.poststwitter = new List<TwPost>();

                            TwPost post = new TwPost()
                            {
                                idpost = postdb.idpost,
                                curtidas = (int)postdb.qtdfavoritado,
                                retweets = (int)postdb.qtdretweets,
                                datahora = postdb.datahora.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                nomeimagem = postdb.nomeimagem,
                                postagem = postdb.postagem
                            };

                            promoElastic.poststwitter.Add(post);
                            break;
                        }
                    case "instagram":
                        {
                            var postdb = db.insta_posts.Where(x => x.idpost == dados.idRedeSocial).FirstOrDefault();

                            promoElastic.postsinstagram = new List<InstaPost>();

                            InstaPost post = new InstaPost()
                            {
                                idpost = postdb.idpost,
                                curtidas = (int)postdb.qtdcurtidas,
                                datahora = postdb.datahora.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                                nomeimagem = postdb.nomeimagem,
                                postagem = postdb.postagem,
                                qtdcomentarios = (int)postdb.qtdcomentarios
                            };

                            promoElastic.postsinstagram.Add(post);
                            break;
                        }
                    case "youtube":
                        {
                            var postdb = db.yt_videos.Where(x => x.idvideo == dados.idRedeSocial).FirstOrDefault();

                            promoElastic.videosyoutube = new List<VideoYt>();

                            VideoYt post = new VideoYt()
                            {
                                idvideo = postdb.idvideo,
                                curtidas = postdb.qtdcurtidas,
                                datahora = postdb.datahora.ToString("dd/MM/yyyy HH:mm:ss"),
                                nomeimagem = postdb.nomeimagem,
                                descricao = postdb.descricao,
                                qtdcomentarios = postdb.qtdcomentarios,
                                visualizacoes = postdb.qtdvisualizacoes,
                                descurtidas = postdb.qtddescurtidas
                            };

                            promoElastic.videosyoutube.Add(post);
                            break;
                        }
                }
            }

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            var resposta = client.Index(promoElastic, i => i.Type("promocao").Id(promoElastic.idpromocao));

            var promoNoticia = new PromocaoNoticia(_server, _indexElastic);
            promoNoticia.atualizaPromocaoNoticiaElasticSearch(dados.idNoticia);

        }

        public Promocao retornaPromocao(int? id)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var promo = (
                            from p in db.promo_promocoes
                            where p.idpromocao == id
                            select new Promocao()
                            {
                                idPromocao = p.idpromocao,
                                idOrgaoregulador = p.idorgaoregulador,
                                idModalidade = p.idmodalidade,
                                nome = p.nome,
                                certificadoAutorizacao = p.certificadoautorizacao,
                                outrosInteressados = p.outrosinteressados,
                                abrangenciaNacional = p.abrangencianacional == true ? true : false,
                                dtCadastro = p.dtcadastro,
                                dtVigenciaIni = p.dtvigenciaini,
                                dtVigenciaFim = p.dtvigenciafim,
                                valorPremio = p.valorpremios,
                                linkSitePromocao = p.linksitepromocao,
                                linkFacebook = p.linkfacebook,
                                linkInstagram = p.linkinstagram,
                                linkTwitter = p.linktwitter,
                                linkYoutube = p.linkyoutube,
                                mecanicaPromo = p.mecanicapromo,
                                produtosParticipantes = p.produtosparticipantes,
                                premiosPromo = p.premiospromo,
                                linkRegulamento = p.linkregulamento,
                                textoRegulamento = p.textoregulamento,
                                excluido = p.excluido,
                                Empresas = (
                                                p.promo_promoempresas.Select(x => new ZAdmin_RN.Promocao.PromocaoEmpresa()
                                                {
                                                    idempresa = x.idempresa,
                                                    nome = x.empresas.nome
                                                })
                                            ),
                                Estados = (
                                                p.promo_estadosabrangencia.Select(x => new ZAdmin_RN.Promocao.PromocaoEstado()
                                                {
                                                    id = x.idestado
                                                })
                                            ),
                                Municipios = (
                                                p.promo_municipiosabrangencia.Select(x => new ZAdmin_RN.Promocao.PromocaoMunicipio()
                                                {
                                                    id = x.idmunicipio
                                                })
                                            ),
                            }
                        ).FirstOrDefault();


            return promo;

        }

        public IEnumerable<Processos_Seae> RetornaProcessosSEAEPromocoes(FiltroPromocoes filtro)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "promocoes");

            var client = new ElasticClient(settings);

            var response = client.Search<Processos_Seae>(s =>
                                    s.Query(q =>
                                    (q.MultiMatch(m => m.
                                        Fields(f => f
                                                .Field("numprocesso")
                                                .Field("comoparticipar")
                                                .Field("interessados")
                                                .Field("modalidade")
                                                .Field("nome")
                                                .Field("premios")
                                                .Field("situacaoatual")
                                                .Field("solicitantes")
                                                .Field("numprocesso")

                                                ).Query(filtro.pesquisa)
                                                 .Operator(Operator.Or)
                                            )
                                        )

                                         && q.DateRange(d => d.
                                            Field(f => f.dtprocesso)
                                            .GreaterThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataInicial))
                                            .LessThanOrEquals(string.Format("{0:yyyy-MM-dd}", filtro.dataFinal))
                                        )
                                        )

                                        .Sort(x => x.Ascending(c => c.dtprocesso)));

            this.TotalRegistros = Convert.ToInt32(response.Total);
            return response.Documents.ToList().Skip(filtro.pag).Take(filtro.quantidade);
        }

        public int RetornaTotalProcessos()
        {
            return this.TotalRegistros;
        }

        public Promocao RetornaPromocaoProSeaeCadastro(int idPromo, int? idProcesso)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var promo = db.promo_promocoes.Where(x => x.idpromocao == idPromo).FirstOrDefault();

            var promoseae = (
                  from p in db.seae_processos
                  where p.idprocesso == idProcesso

                  select new Promocao
                  {
                      nome = promo.nome,
                      idOrgaoregulador = promo.idorgaoregulador,
                      //numeroProcesso = p.numprocesso,
                      outrosInteressados = p.interessados,
                      abrangenciaNacional = p.abrangencia_nacional,
                      textoRegulamento = p.resumo,
                      dtVigenciaIni = p.dtvigenciaini,
                      dtVigenciaFim = p.dtvigenciafim,
                      valorPremio = p.valortotalpremios,
                      premiosPromo = p.premios,
                      certificadoAutorizacao = p.numprocesso != "" && p.numprocesso != null ? p.numprocesso : promo.certificadoautorizacao,
                      dtCadastro = promo.dtcadastro,

                      Empresas = (from ep in p.seae_empresa_processos
                                  join e in db.empresas on ep.idempresa equals e.idempresa
                                  select new PromocaoEmpresa()
                                  {
                                      idempresa = e.idempresa,
                                      nome = e.nome
                                  }
                                  ),

                      Municipios = (from em in p.seae_abrang_municipio
                                    join m in db.municipios on em.idmunicipio equals m.idmunicipio
                                    select new PromocaoMunicipio
                                    {

                                        id = m.idmunicipio,
                                        nome = m.nome
                                    }
                     ),


                      Estados = (from pe in p.seae_abrang_estado
                                 join e in db.estados on pe.idestado equals e.idestado
                                 select new PromocaoEstado
                                 {

                                     id = e.idestado,
                                     nome = e.nome

                                 }

                              ),

                      ArquivoSeae = (from a in p.seae_arquivos_proc
                                     select new PromocaoArquivoSeae
                                     {
                                         idarquivo = a.idarquivo,
                                         link = a.link,
                                         nomearquivo = a.nomearquivo,
                                         textoarquivo = a.textoarquivo
                                     }
                     )
                  }).FirstOrDefault();

            // Salvar os arquivos de processo SEAE para promoção
            // ========================================================================
            if (promoseae.ArquivoSeae.Count() > 0 && promoseae.ArquivoSeae != null)
                foreach (var arqSeae in promoseae.ArquivoSeae)
                {
                    var existePromoArq = db.promo_arquivos.Where(x => x.idpromocao == idPromo && x.nome == arqSeae.nomearquivo && x.url == arqSeae.link).FirstOrDefault();
                    if (existePromoArq == null)
                    {
                        var promoArq = new ZAdmin_DB.Model.promo_arquivos();
                        promoArq.idpromocao = idPromo;
                        promoArq.nome = arqSeae.nomearquivo;
                        promoArq.url = arqSeae.link;
                        promoArq.conteudo = arqSeae.textoarquivo;
                        promoArq.tipo = "Link";
                        promoArq.excluido = false;

                        db.promo_arquivos.Add(promoArq);
                        db.SaveChanges();
                    }
                }

            return promoseae;

        }

        public bool CapturaImagemUrl(string url, int idPromo, string dirExecutaveis, string dirArquivos)
        {
            string nomeArquivo = idPromo + ".jpg";

            var cap = new Captura();

            if (cap.GeraImagemUrl(url, nomeArquivo, dirExecutaveis, dirArquivos))
            {
                var objArq = new ZAdmin_RN.Promocao.PromocaoArquivo();
                objArq.idpromocao = idPromo;
                objArq.nome = nomeArquivo;
                objArq.tipo = "Imagem";
                objArq.excluido = false;

                var arqPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();
                arqPromo.salvarArquivoRelacionado(objArq);

                return true;
            }
            else
            {
                return false;
            }

        }

        public List<itemLista> retornaPromocoesEmpresaTotalNoticias(int idEmpresa)
        {
            var promos = (from p in db.promo_promocoes
                          join pe in db.promo_promoempresas on p.idpromocao equals pe.idpromocao
                          where
                            p.excluido == false &&
                            pe.idempresa == idEmpresa
                          orderby p.nome
                          select new itemLista()
                          {
                              idItem = p.idpromocao,
                              nome = p.nome,
                              total = p.promo_promonoticias.Count()
                          }).ToList();

            return promos;
        }

        public List<itemLista> retornaPromocoesEmpresaTotalPosts(int idEmpresa)
        {
            var promos = (from p in db.promo_promocoes
                          join pe in db.promo_promoempresas on p.idpromocao equals pe.idpromocao
                          where
                            pe.idempresa == idEmpresa
                          orderby p.nome
                          select new itemLista()
                          {
                              idItem = p.idpromocao,
                              nome = p.nome,
                              total = p.promo_fb_posts.Count() + p.promo_tw_posts.Count() + p.promo_insta_posts.Count() + p.promo_yt_videos.Count()
                          }).ToList();

            return promos;
        }

        
    }
}