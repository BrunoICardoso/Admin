using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.PromocaoRedesSociais
{
    public class PromocaoRedesSociais
    {

        private int _totalFace;
        private int _totalTw;
        private int _totalInsta;
        private int _totalYt;
        private ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();
        string _server = "";
        string _indexElastic = "";

        public PromocaoRedesSociais(string servidorElastic, string indexElastic)
        {
            _server = servidorElastic;
            _indexElastic = indexElastic;
        }

        public ZAdmin_RN.PromocaoRedesSociais.PromocaoRedeSocial retornaRedesSocias(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {
            filtros.dataIni = filtros.dataIni == null ? DateTime.Now.AddYears(-1) : new DateTime(filtros.dataIni.Value.Year, filtros.dataIni.Value.Month, filtros.dataIni.Value.Day, 00, 00, 00, 123);
            filtros.dataFim = filtros.dataFim == null ? DateTime.Now : new DateTime(filtros.dataFim.Value.Year, filtros.dataFim.Value.Month, filtros.dataFim.Value.Day, 23, 59, 59, 123);

            var DadosRedesSociais = new ZAdmin_RN.PromocaoRedesSociais.PromocaoRedeSocial();
            switch (filtros.nomeRede)
            {
                case "facebook":
                    DadosRedesSociais.ListaFacebook = retornaListaFacebook(filtros);
                    DadosRedesSociais.TotalListaFacebook = _totalFace;
                    break;
                case "twitter":
                    DadosRedesSociais.ListaTwitter = retornaListaTwitter(filtros);
                    DadosRedesSociais.TotalListaTwitter = _totalTw;
                    break;
                case "instagram":
                    DadosRedesSociais.ListaInstagram = retornaListaInstagram(filtros);
                    DadosRedesSociais.TotalListaInstagram = _totalInsta;
                    break;
                case "youtube":
                    DadosRedesSociais.ListaYoutube = retornaListaYoutube(filtros);
                    DadosRedesSociais.TotalListaYoutube = _totalYt;
                    break;
            }

            return DadosRedesSociais;
        }

        public List<fb_post> retornaListaFacebook(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "fb_posts");

            var client = new ElasticClient(settings);

            var resultado = client.Search<ZAdmin_RN.PromocaoRedesSociais.fb_post>(s => s
                                .Size(filtros.QtdRegistros)
                                .From(filtros.paginaAtual)
                                .Query(q =>

                                        (filtros.idempresa != 0 && filtros.idempresa != null ? q.Term("idempresa", filtros.idempresa) : null) &&

                                        (filtros.idpromocao != 0 ? q.Match(m => m.Field("promocoes.idpromocao").Query(filtros.idpromocao.ToString())) : null) &&

                                        q.MultiMatch(m => m
                                                        .Fields(f => f.Field(p => p.postagem)
                                                        )
                                                        .Query(filtros.textoPesquisa)
                                                        .Operator(Operator.Or)
                                        ) &&

                                        q.DateRange(d => d
                                            .Field(f => f.datahora)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy 00:00:00}", filtros.dataIni))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy 23:59:59}", filtros.dataFim))
                                        )
                                    )
                                    .Sort(x => x.Descending(p => p.datahora))
            );

            _totalFace = resultado.Documents.Count > 0 ? Convert.ToInt32(resultado.Total) : 0;


            return resultado.Documents.ToList();
        }

        public List<tw_post> retornaListaTwitter(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "tw_posts");

            var client = new ElasticClient(settings);

            var resultado = client.Search<ZAdmin_RN.PromocaoRedesSociais.tw_post>(s => s
                                .Size(filtros.QtdRegistros)
                                .From(filtros.paginaAtual)
                                .Query(q =>

                                        (filtros.idempresa != 0 ? q.Term("idempresa", filtros.idempresa) : null) &&

                                        (filtros.idpromocao != 0 ? q.Match(m => m.Field("promocoes.idpromocao").Query(filtros.idpromocao.ToString())) : null) &&

                                        q.MultiMatch(m => m
                                                        .Fields(f => f.Field(p => p.postagem)
                                                        )
                                                        .Query(filtros.textoPesquisa)
                                                        .Operator(Operator.Or)
                                        ) &&

                                        q.DateRange(d => d
                                            .Field(f => f.datahora)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataIni))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataFim))
                                        )
                                    )
                                    .Sort(x => x.Descending(p => p.datahora))
            );

            _totalTw = resultado.Documents.Count > 0 ? Convert.ToInt32(resultado.Total) : 0;

            return resultado.Documents.ToList();
        }

        public List<insta_post> retornaListaInstagram(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "insta_posts");

            var client = new ElasticClient(settings);

            var resultado = client.Search<ZAdmin_RN.PromocaoRedesSociais.insta_post>(s => s
                                .Size(filtros.QtdRegistros)
                                .From(filtros.paginaAtual)
                                .Query(q =>

                                       (filtros.idempresa != 0 ? q.Term("idempresa", filtros.idempresa) : null) &&

                                        (filtros.idpromocao != 0 ? q.Match(m => m.Field("promocoes.idpromocao").Query(filtros.idpromocao.ToString())) : null) &&

                                        q.MultiMatch(m => m
                                                        .Fields(f => f.Field(p => p.postagem)
                                                        )
                                                        .Query(filtros.textoPesquisa)
                                                        .Operator(Operator.Or)
                                        ) &&

                                        q.DateRange(d => d
                                            .Field(f => f.datahora)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataIni))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataFim))
                                        )
                                    )
                                    .Sort(x => x.Descending(p => p.datahora))
            );

            _totalInsta = resultado.Documents.Count > 0 ? Convert.ToInt32(resultado.Total) : 0;

            return resultado.Documents.ToList();
        }

        public List<yt_video> retornaListaYoutube(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);
            settings.DefaultIndex(_indexElastic + "yt_videos");

            var client = new ElasticClient(settings);

            var resultado = client.Search<ZAdmin_RN.PromocaoRedesSociais.yt_video>(s => s
                                .Size(filtros.QtdRegistros)
                                .From(filtros.paginaAtual)
                                .Query(q =>

                                        (filtros.idempresa != 0 ? q.Term("idempresa", filtros.idempresa) : null) &&

                                        (filtros.idpromocao != 0 ? q.Match(m => m.Field("promocoes.idpromocao").Query(filtros.idpromocao.ToString())) : null) &&

                                        q.MultiMatch(m => m
                                                        .Fields(f => f.Field(p => p.descricao)
                                                        )
                                                        .Query(filtros.textoPesquisa)
                                                        .Operator(Operator.Or)
                                        ) &&

                                        q.DateRange(d => d
                                            .Field(f => f.datahora)
                                            .GreaterThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataIni))
                                            .LessThanOrEquals(string.Format("{0:dd/MM/yy HH:mm:ss}", filtros.dataFim))
                                        )
                                    )
                                    .Sort(x => x.Descending(p => p.datahora))
            );

            _totalYt = resultado.Documents.Count > 0 ? Convert.ToInt32(resultado.Total) : 0;

            return resultado.Documents.ToList();
        }

        public void desassociarPromocaoPost(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {

            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);

            // Desassociar do banco Mysql
            // ----------------------------------------
            switch (filtros.nomeRede)
            {
                case "facebook":
                    var face = db.promo_fb_posts.Where(x => x.idpost == filtros.idpost && x.idpromocao == filtros.idpromocao).FirstOrDefault();
                    if (face != null)
                    {
                        db.promo_fb_posts.Attach(face);
                        db.promo_fb_posts.Remove(face);
                        db.SaveChanges();
                    }
                    var listaFace = (
                                        from pro in db.promo_promocoes
                                        join p in db.promo_fb_posts on pro.idpromocao equals p.idpromocao
                                        where p.idpost == filtros.idpost
                                        select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                     ).ToList();

                    // Desassociar do Elastic de redes social
                    // ----------------------------------------
                    settings.DefaultIndex(_indexElastic + "fb_posts");

                    var clientFace = new ElasticClient(settings);
                    clientFace.Update<ZAdmin_RN.PromocaoRedesSociais.fb_post, object>(
                                                                                        filtros.idpost,
                                                                                        d => d.Doc(new { promocoes = listaFace })
                                                                                    );
                    // Desassociar do Elastic de promoção
                    // ----------------------------------------
                    var listarPosts = (
                                            from posts in db.fb_posts
                                            join pp in db.promo_fb_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == filtros.idpromocao
                                            select posts).ToList();

                    var novosPosts = listarPosts.Select(posts => new ZAdmin_RN.Promocao.FacePost()
                    {
                        datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                        idpost = Convert.ToInt32(posts.idpost),
                        nomeimagem = posts.nomeimagem,
                        postagem = posts.postagem,
                        compartilhamentos = Convert.ToInt32(posts.compartilhamentos),
                        curtidas = posts.reacoes,
                        qtdcomentarios = posts.fb_comentarios.Count(),
                        //comentarios = null,
                        //promocoes = posts.promo_fb_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                    }
                                ).ToList();

                    settings.DefaultIndex(_indexElastic + "promocoes");
                    var clientPromoFace = new ElasticClient(settings);

                    clientPromoFace.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                            filtros.idpromocao,
                                                                            d => d.Doc(
                                                                                    new 
                                                                                    {
                                                                                        postsfacebook = novosPosts
                                                                                    }
                                                                                ).Type("promocao")
                                                                        );

                    break;
                case "twitter":
                    var tw = db.promo_tw_posts.Where(x => x.idpost == filtros.idpost && x.idpromocao == filtros.idpromocao).FirstOrDefault();
                    if (tw != null)
                    {
                        db.promo_tw_posts.Attach(tw);
                        db.promo_tw_posts.Remove(tw);
                        db.SaveChanges();
                    }
                    var listaTw = (
                                        from pro in db.promo_promocoes
                                        join p in db.promo_tw_posts on pro.idpromocao equals p.idpromocao
                                        where p.idpost == filtros.idpost
                                        select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                     ).ToList();

                    // Desassociar do Elastic de redes social
                    // ----------------------------------------
                    settings.DefaultIndex(_indexElastic + "tw_posts");

                    var clientTw = new ElasticClient(settings);
                    clientTw.Update<ZAdmin_RN.PromocaoRedesSociais.tw_post, object>(
                                                                                        filtros.idpost,
                                                                                        d => d.Doc(new { promocoes = listaTw })
                                                                                    );
                    // Desassociar do Elastic de promoção
                    // ----------------------------------------
                    var listarPostsTw = (
                                            from posts in db.tw_posts
                                            join pp in db.promo_tw_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == filtros.idpromocao
                                            select posts).ToList();

                    var novosPostsTw = listarPostsTw.Select(posts => new ZAdmin_RN.Promocao.TwPost()
                    {
                        datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                        idpost = Convert.ToInt32(posts.idpost),
                        nomeimagem = posts.nomeimagem,
                        postagem = posts.postagem,
                        curtidas = posts.qtdfavoritado.Value,
                        retweets = posts.qtdretweets.Value ,
                        //promocoes = posts.promo_tw_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                    }
                                ).ToList();

                    settings.DefaultIndex(_indexElastic + "promocoes");
                    var clientPromoTw = new ElasticClient(settings);

                    clientPromoTw.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                            filtros.idpromocao,
                                                                            d => d.Doc(
                                                                                    new 
                                                                                    {
                                                                                        poststwitter = novosPostsTw
                                                                                    }
                                                                                ).Type("promocao")
                                                                        );

                    break;
                case "instagram":
                    var insta = db.promo_insta_posts.Where(x => x.idpost == filtros.idpost && x.idpromocao == filtros.idpromocao).FirstOrDefault();
                    if (insta != null)
                    {
                        db.promo_insta_posts.Attach(insta);
                        db.promo_insta_posts.Remove(insta);
                        db.SaveChanges();
                    }
                    var listaIns = (
                                       from pro in db.promo_promocoes
                                       join p in db.promo_insta_posts on pro.idpromocao equals p.idpromocao
                                       where p.idpost == filtros.idpost
                                       select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                    ).ToList();

                    // Desassociar do Elastic de redes social
                    // ----------------------------------------
                    settings.DefaultIndex(_indexElastic + "insta_posts");

                    var clientIns = new ElasticClient(settings);
                    clientIns.Update<ZAdmin_RN.PromocaoRedesSociais.insta_post, object>(
                                                                                        filtros.idpost,
                                                                                        d => d.Doc(new { promocoes = listaIns })
                                                                                    );
                    // Desassociar do Elastic de promoção
                    // ----------------------------------------
                    var listarPostsIns = (
                                            from posts in db.insta_posts
                                            join pp in db.promo_insta_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == filtros.idpromocao
                                            select posts).ToList();

                    var novosPostsIns = listarPostsIns.Select(posts => new ZAdmin_RN.Promocao.InstaPost()
                    {
                        datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                        idpost = Convert.ToInt32(posts.idpost),
                        nomeimagem = posts.nomeimagem,
                        postagem = posts.postagem,
                        curtidas = posts.qtdcurtidas.Value,
                        qtdcomentarios = posts.qtdcomentarios.Value ,
                        //promocoes = posts.promo_insta_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                    }
                                ).ToList();

                    settings.DefaultIndex(_indexElastic + "promocoes");
                    var clientPromoIns = new ElasticClient(settings);

                    clientPromoIns.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                            filtros.idpromocao,
                                                                            d => d.Doc(
                                                                                    new 
                                                                                    {
                                                                                        postsinstagram = novosPostsIns
                                                                                    }
                                                                                ).Type("promocao")
                                                                        );

                    break;
                case "youtube":
                    var yt = db.promo_yt_videos.Where(x => x.idvideo == filtros.idpost && x.idpromocao == filtros.idpromocao).FirstOrDefault();
                    if (yt != null)
                    {
                        db.promo_yt_videos.Attach(yt);
                        db.promo_yt_videos.Remove(yt);
                        db.SaveChanges();
                    }
                    var listaYt = (
                                       from pro in db.promo_promocoes
                                       join p in db.promo_yt_videos on pro.idpromocao equals p.idpromocao
                                       where p.idvideo == filtros.idpost
                                       select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                    ).ToList();

                    // Desassociar do Elastic de redes social
                    // ----------------------------------------
                    settings.DefaultIndex(_indexElastic + "yt_posts");

                    var clientYt = new ElasticClient(settings);
                    clientYt.Update<ZAdmin_RN.PromocaoRedesSociais.yt_video, object>(
                                                                                        filtros.idpost,
                                                                                        d => d.Doc(new { promocoes = listaYt })
                                                                                    );

                    // Desassociar do Elastic de promoção
                    // ----------------------------------------
                    var listarPostsYt = (
                                            from posts in db.yt_videos
                                            join pp in db.promo_yt_videos on posts.idvideo equals pp.idvideo
                                            where pp.idpromocao == filtros.idpromocao
                                            select posts).ToList();

                    var novosPostsYt = listarPostsYt.Select(posts => new ZAdmin_RN.Promocao.VideoYt()
                    {
                        datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                        idvideo = Convert.ToInt32(posts.idvideo),
                        nomeimagem = posts.nomeimagem,
                        descricao = posts.descricao,
                        curtidas = posts.qtdcurtidas,
                        qtdcomentarios = posts.qtdcomentarios,
                        visualizacoes = posts.qtdvisualizacoes,
                        descurtidas = posts.qtddescurtidas,
                        //promocoes = posts.promo_yt_videos.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                    }
                                ).ToList();

                    settings.DefaultIndex(_indexElastic + "promocoes");
                    var clientPromoYt = new ElasticClient(settings);

                    clientPromoYt.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                            filtros.idpromocao,
                                                                            d => d.Doc(
                                                                                    new 
                                                                                    {
                                                                                        videosyoutube = novosPostsYt
                                                                                    }
                                                                                ).Type("promocao")
                                                                        );
                    break;
            }
        }

        public string associarPromocaoPost(int idpromocao, int idpost, string nomeRede)
        {
            var node = new Uri(_server);
            var settings = new ConnectionSettings(node);
            settings.DisableDirectStreaming(true);

            bool retornaNomePromo = true;

            try
            {
                switch (nomeRede)
                {
                    case "facebook":
                        // Associar MySQL
                        // --------------------------------------------
                        var existeFb = db.promo_fb_posts.Where(x => x.idpost == idpost && x.idpromocao == idpromocao).FirstOrDefault();
                        if (existeFb == null)
                        {
                            var faceBD = new ZAdmin_DB.Model.promo_fb_posts();
                            faceBD.idpost = idpost;
                            faceBD.idpromocao = idpromocao;

                            db.promo_fb_posts.Add(faceBD);
                            db.SaveChanges();

                            // Associar promoções ao índice fb_post no Elastic
                            // ----------------------------------------------------------------------
                            var listaFace = (
                                            from pro in db.promo_promocoes
                                            join p in db.promo_fb_posts on pro.idpromocao equals p.idpromocao
                                            where p.idpost == idpost
                                            select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                         ).ToList();

                            settings.DefaultIndex(_indexElastic + "fb_posts");

                            var clientFace = new ElasticClient(settings);
                            clientFace.Update<ZAdmin_RN.PromocaoRedesSociais.fb_post, object>(
                                                                                                idpost,
                                                                                                d => d.Doc(new { promocoes = listaFace })
                                                                                            );

                            // Associar posts facebook ao índice promoções no Elastic
                            // ----------------------------------------------------------------------
                            var listarPosts = (
                                            from posts in db.fb_posts
                                            join pp in db.promo_fb_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == idpromocao
                                            select posts).ToList();

                            var novosPosts = listarPosts.Select(posts => new ZAdmin_RN.Promocao.FacePost()
                                            {
                                                datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                                                idpost = Convert.ToInt32(posts.idpost),
                                                nomeimagem = posts.nomeimagem,
                                                postagem = posts.postagem,
                                                compartilhamentos = Convert.ToInt32(posts.compartilhamentos),
                                                curtidas = posts.reacoes,
                                                qtdcomentarios = posts.fb_comentarios.Count(),
                                                //comentarios = null,
                                                //promocoes = posts.promo_fb_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome}).ToList()
                                            }
                                        ).ToList();                                                        

                            settings.DefaultIndex(_indexElastic + "promocoes");
                            var clientPromoFace = new ElasticClient(settings);

                            clientPromoFace.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                                    idpromocao,
                                                                                    d => d.Doc(
                                                                                            new 
                                                                                            {
                                                                                                postsfacebook = novosPosts
                                                                                            }
                                                                                        ).Type("promocao")
                                                                                );
                            retornaNomePromo = true;
                        }
                        else
                        {
                            retornaNomePromo = false;
                        }
                        break;
                    case "twitter":
                        // Associar MySQL
                        // --------------------------------------------
                        var existeTW = db.promo_tw_posts.Where(x => x.idpost == idpost && x.idpromocao == idpromocao).FirstOrDefault();
                        if (existeTW == null)
                        {
                            var twBD = new ZAdmin_DB.Model.promo_tw_posts();
                            twBD.idpost = idpost;
                            twBD.idpromocao = idpromocao;

                            db.promo_tw_posts.Add(twBD);
                            db.SaveChanges();

                            // Associar promoções ao índice tw_post no Elastic
                            // ----------------------------------------------------------------------
                            var listaTw = (
                                            from pro in db.promo_promocoes
                                            join p in db.promo_tw_posts on pro.idpromocao equals p.idpromocao
                                            where p.idpost == idpost
                                            select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                         ).ToList();

                            settings.DefaultIndex(_indexElastic + "tw_posts");

                            var clientTw = new ElasticClient(settings);
                            clientTw.Update<ZAdmin_RN.PromocaoRedesSociais.tw_post, object>(
                                                                                                idpost,
                                                                                                d => d.Doc(new { promocoes = listaTw })
                                                                                            );

                            // Associar posts twitter ao índice promoções no Elastic
                            // ----------------------------------------------------------------------
                            var listarPosts = (
                                            from posts in db.tw_posts
                                            join pp in db.promo_tw_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == idpromocao
                                            select posts).ToList();

                            var novosPosts = listarPosts.Select(posts => new ZAdmin_RN.Promocao.TwPost()
                            {
                                datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                                idpost = Convert.ToInt32(posts.idpost),
                                nomeimagem = posts.nomeimagem,
                                postagem = posts.postagem,
                                curtidas = posts.qtdfavoritado.Value,
                                retweets = posts.qtdretweets.Value,
                                //promocoes = posts.promo_tw_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                            }
                                        ).ToList();

                            settings.DefaultIndex(_indexElastic + "promocoes");
                            var clientPromoTw = new ElasticClient(settings);

                            clientPromoTw.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                                    idpromocao,
                                                                                    d => d.Doc(
                                                                                            new 
                                                                                            {
                                                                                                poststwitter = novosPosts
                                                                                            }
                                                                                        ).Type("promocao")
                                                                                );
                            retornaNomePromo = true;
                        }
                        else
                        {
                            retornaNomePromo = false;
                        }

                        break;
                    case "instagram":
                        // Associar MySQL
                        // --------------------------------------------
                        var existeInsta = db.promo_insta_posts.Where(x => x.idpost == idpost && x.idpromocao == idpromocao).FirstOrDefault();
                        if (existeInsta == null)
                        {
                            var insBD = new ZAdmin_DB.Model.promo_insta_posts();
                            insBD.idpost = idpost;
                            insBD.idpromocao = idpromocao;

                            db.promo_insta_posts.Add(insBD);
                            db.SaveChanges();

                            // Associar promoções ao índice insta_post no Elastic
                            // ----------------------------------------------------------------------
                            var listaIn = (
                                            from pro in db.promo_promocoes
                                            join p in db.promo_insta_posts on pro.idpromocao equals p.idpromocao
                                            where p.idpost == idpost
                                            select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                         ).ToList();

                            settings.DefaultIndex(_indexElastic + "insta_posts");

                            var clientIns = new ElasticClient(settings);
                            clientIns.Update<ZAdmin_RN.PromocaoRedesSociais.insta_post, object>(
                                                                                                idpost,
                                                                                                d => d.Doc(new { promocoes = listaIn })
                                                                                            );

                            // Associar posts instagram ao índice promoções no Elastic
                            // ----------------------------------------------------------------------
                            var listarPosts = (
                                            from posts in db.insta_posts
                                            join pp in db.promo_insta_posts on posts.idpost equals pp.idpost
                                            where pp.idpromocao == idpromocao
                                            select posts).ToList();

                            var novosPosts = listarPosts.Select(posts => new ZAdmin_RN.Promocao.InstaPost()
                            {
                                datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                                idpost = Convert.ToInt32(posts.idpost),
                                nomeimagem = posts.nomeimagem,
                                postagem = posts.postagem,
                                curtidas = posts.qtdcurtidas.Value,
                                qtdcomentarios = posts.qtdcomentarios.Value //,
                                //promocoes = posts.promo_insta_posts.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                            }
                                        ).ToList();

                            settings.DefaultIndex(_indexElastic + "promocoes");
                            var clientPromoIns = new ElasticClient(settings);

                            clientPromoIns.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                                    idpromocao,
                                                                                    d => d.Doc(
                                                                                            new 
                                                                                            {
                                                                                                postsinstagram = novosPosts
                                                                                            }
                                                                                        ).Type("promocao")
                                                                                );

                            retornaNomePromo = true;
                        }
                        else
                        {
                            retornaNomePromo = false;
                        }

                        break;
                    case "youtube":
                        // Associar MySQL
                        // --------------------------------------------
                        var existeYt = db.promo_yt_videos.Where(x => x.idvideo == idpost && x.idpromocao == idpromocao).FirstOrDefault();
                        if (existeYt == null)
                        {
                            var ytBD = new ZAdmin_DB.Model.promo_yt_videos();
                            ytBD.idvideo = idpost;
                            ytBD.idpromocao = idpromocao;

                            db.promo_yt_videos.Add(ytBD);
                            db.SaveChanges();

                            // Associar promoções ao índice yt_video no Elastic
                            // ----------------------------------------------------------------------
                            var listaYt = (
                                            from pro in db.promo_promocoes
                                            join p in db.promo_yt_videos on pro.idpromocao equals p.idpromocao
                                            where p.idvideo == idpost
                                            select new { idpromocao = pro.idpromocao, nome = pro.nome }
                                         ).ToList();

                            settings.DefaultIndex(_indexElastic + "yt_videos");

                            var clientYt = new ElasticClient(settings);
                            clientYt.Update<ZAdmin_RN.PromocaoRedesSociais.yt_video, object>(
                                                                                                idpost,
                                                                                                d => d.Doc(new { promocoes = listaYt })
                                                                                            );
                            // Associar videos youtube ao índice promoções no Elastic
                            // ----------------------------------------------------------------------
                            var listarPosts = (
                                            from posts in db.yt_videos
                                            join pp in db.promo_yt_videos on posts.idvideo equals pp.idvideo
                                            where pp.idpromocao == idpromocao
                                            select posts).ToList();

                            var novosPosts = listarPosts.Select(posts => new ZAdmin_RN.Promocao.VideoYt()
                            {
                                datahora = string.Format("{0:dd/MM/yyy HH:mm:ss}", posts.datahora),
                                idvideo = Convert.ToInt32(posts.idvideo),
                                nomeimagem = posts.nomeimagem,
                                descricao = posts.descricao,
                                curtidas = posts.qtdcurtidas,
                                qtdcomentarios = posts.qtdcomentarios,
                                visualizacoes = posts.qtdvisualizacoes,
                                descurtidas = posts.qtddescurtidas,
                                //promocoes = posts.promo_yt_videos.Select(promo => new Promocao.Promo() { idpromocao = promo.idpromocao.Value, nomepromocao = promo.promo_promocoes.nome }).ToList()
                            }
                                        ).ToList();

                            settings.DefaultIndex(_indexElastic + "promocoes");
                            var clientPromoYt = new ElasticClient(settings);

                            clientPromoYt.Update<ZAdmin_RN.Promocao.PromocaoElastic, object>(
                                                                                    idpromocao,
                                                                                    d => d.Doc(
                                                                                            new
                                                                                            {
                                                                                                videosyoutube = novosPosts
                                                                                            }
                                                                                        ).Type("promocao")
                                                                                );
                            retornaNomePromo = true;
                        }
                        else
                        {
                            retornaNomePromo = false;
                        }
                        break;
                }

                if (retornaNomePromo == true)
                {
                    var Promocao = db.promo_promocoes.Where(x => x.idpromocao == idpromocao).FirstOrDefault();
                    return Promocao.nome;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //throw;
                return ex.StackTrace + ex.Message;

            }

        }
    }
}
