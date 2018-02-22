using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZAdmin_RN.Promocao;
using ZAdmin.ViewModel;
using ZAdmin.Utils;
using ZAdmin_RN.Listas;
using ZAdmin_RN.Mensagem;

namespace ZAdmin.Controllers
{
    public class PromocaoNoticiaAPIController : ApiController
    {
        public IndexPromocaoNoticia noticias(filtroPromocaoNoticia filtro)
        {
            
            var PromocoesRN = new ZAdmin_RN.Promocao.PromocaoNoticia(Configuracoes.ServidorElastic,Configuracoes.IndexElastic);

            var indexPromocaoNoticia = new IndexPromocaoNoticia();
            
            indexPromocaoNoticia.noticias = PromocoesRN.RetornaNoticias(filtro);
            indexPromocaoNoticia.totalNoticias = PromocoesRN.totalNoticias;
            
            return indexPromocaoNoticia;
        }

        public IndexPromocaoNoticia retornaNoticiasPromocao(int idPromocao, int pagina, int quantidade)
        {

            var PromocoesRN = new ZAdmin_RN.Promocao.PromocaoNoticia(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            var indexPromocaoNoticia = new IndexPromocaoNoticia();

            indexPromocaoNoticia.noticias = PromocoesRN.retornaNoticiasPromocao(idPromocao,pagina,quantidade);
            indexPromocaoNoticia.totalNoticias = PromocoesRN.totalNoticias;

            return indexPromocaoNoticia;


        }

        public CombosIndexPromocaoNoticia GetCombos()
        {

            var RNPromocoesNoticias = new ZAdmin_RN.Promocao.PromocaoNoticia(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);

            var combos = new CombosIndexPromocaoNoticia();

            combos.empresas = RNPromocoesNoticias.RetornaEmpresaNoticias();
           
            return combos;

        }

        public List<itemLista> GetRetornaPromocoesTotalNoticia(int idEmpresa)
        {
            var promoRN = new Promocoes();

            var promos = promoRN.retornaPromocoesEmpresaTotalNoticias(idEmpresa);

            return promos;

        }
        
        [HttpGet]
        public Mensagem atualizaPromocaoNoticia(int idPromocao, int idFontePesquisa, int idNoticia)
        {
            var RNPromoNoticias = new ZAdmin_RN.Promocao.PromocaoNoticia(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RNPromoNoticias.AssociaPromocaoNoticia(idPromocao, idNoticia, idFontePesquisa);
        }

        public void DesassociarPromocaoNoticia(ZAdmin_RN.Promocao.TagPromocao tag)
        {
            var RN = new ZAdmin_RN.Promocao.PromocaoNoticia(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            RN.DesassociarPromocaoNoticia(tag);
        }
    }
}