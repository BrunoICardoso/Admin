using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ZAdmin_RN.Listas;
using ZAdmin_RN.Promocao;
using ZAdmin.Utils;

namespace ZAdmin.Controllers
{
    public class PromocaoRedeSocialAPIController : ApiController
    {
        private string servidorElastic = Utils.Configuracoes.ServidorElastic;

        public ZAdmin_RN.PromocaoRedesSociais.PromocaoRedeSocial Pesquisar(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {
            var RN = new ZAdmin_RN.PromocaoRedesSociais.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.retornaRedesSocias(filtros);
        }
        
        public void DesassociarPromocaoPost(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {
            var RN = new ZAdmin_RN.PromocaoRedesSociais.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            RN.desassociarPromocaoPost(filtros);
        }

        public string AssociarPromocaoPost(ZAdmin_RN.PromocaoRedesSociais.ProcessoRedeSocialFiltros filtros)
        {
            var RN = new ZAdmin_RN.PromocaoRedesSociais.PromocaoRedesSociais(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return RN.associarPromocaoPost(filtros.idpromocao, filtros.idpost, filtros.nomeRede);
        }

        public List<itemLista> GetRetornaPromocoesTotalPosts(int idEmpresa)
        {
            var promoRN = new Promocoes();
            var promos = promoRN.retornaPromocoesEmpresaTotalPosts(idEmpresa);     
            return promos;
        }
    }
}