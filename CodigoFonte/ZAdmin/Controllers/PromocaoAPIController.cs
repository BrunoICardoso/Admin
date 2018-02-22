using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZAdmin_RN.Promocao;
using ZAdmin.ViewModel;
using System.Web;
using System.IO;
using ZAdmin.Utils;
using ZAdmin_RN.Listas;

namespace ZAdmin.Controllers
{
    public class PromocaoAPIController : ApiController
    {

        public IndexPromocoes promocoes(ZAdmin_RN.Promocao.filtroIndexPromocoes filtro)
        {
            var promocoesRN = new ZAdmin_RN.Promocao.Promocoes();
            var promocoes = new IndexPromocoes();

            promocoes.promocoes = promocoesRN.RetornaPromocoes(filtro);
            promocoes.TotalMapaRegistros = promocoesRN.totalPromocoes;

            return promocoes;
        }

        public void Delete(int id)
        {
            var promocoesRN = new ZAdmin_RN.Promocao.Promocoes();
            promocoesRN.remover(id);
        }

        public List<ZAdmin_RN.Promocao.ItemCombo> GetModalidades()
        {
            var promocoesRN = new ZAdmin_RN.Promocao.Promocoes();
            return promocoesRN.retornaModalidades();
        }

        public List<ZAdmin_RN.Promocao.ItemCombo> GetEmpresas()
        {
            var promocoesRN = new ZAdmin_RN.Promocao.Promocoes();
            return promocoesRN.retornaEmpresas();
        }

        public List<ZAdmin_RN.Promocao.OrgaoRegulador> GetOrgaosRegulador()
        {
            var orgaosRN = new ZAdmin_RN.Promocao.OrgaosRegulador();
            return orgaosRN.retornaOrgaos();
        }

        public int CadastrarPromocao(ZAdmin_RN.Promocao.Promocao dados)
        {
            var promocaoRN = new ZAdmin_RN.Promocao.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return promocaoRN.CadastrarPromocao(dados);
        }

        public int EditarPromocao(ZAdmin_RN.Promocao.Promocao dados)
        {
            var promocaoRN = new ZAdmin_RN.Promocao.Promocoes(Configuracoes.ServidorElastic, Configuracoes.IndexElastic);
            return promocaoRN.EditarPromocao(dados);
        }

        public List<ZAdmin_RN.Promocao.PromocaoArquivo> GetArquivosRegulamento(int idPromocao)
        {
            var arqsRN = new ZAdmin_RN.Promocao.PromocaoArquivos();
            return arqsRN.retornaArquivosRegulamento(idPromocao);
        }

        public List<ZAdmin_RN.Promocao.PromocaoArquivo> GetArquivosRelacionado(int idPromocao)
        {
            var arqsRN = new ZAdmin_RN.Promocao.PromocaoArquivos();
            return arqsRN.retornaArquivosRelacionado(idPromocao);
        }

        public void ExcluirArquvioRegulamento(ZAdmin_RN.Promocao.PromocaoArquivo arquivo)
        {
            var arqRN = new ZAdmin_RN.Promocao.PromocaoArquivos();
            arqRN.ExcluirArquivoRegulamento(arquivo);
        }

        public void ExcluirArquvioRelacionado(ZAdmin_RN.Promocao.PromocaoArquivo arquivo)
        {
            var arqRN = new ZAdmin_RN.Promocao.PromocaoArquivos();
            arqRN.ExcluirArquivoRelacionado(arquivo);
        }

        [HttpPut]
        public void SalvarArquivoRegulamento()
        {
            try
            {
                var httpPostedFile = HttpContext.Current.Request.Files["arquivoRegul"];
                var idPromocao = HttpContext.Current.Request.Form[0];
                string tipo = "";
                if (httpPostedFile != null)
                {
                    tipo = httpPostedFile.ContentType.Contains("image") ? "Imagem" : "Arquivo";

                    var nomeArquivo = idPromocao + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(httpPostedFile.FileName);
                    //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(Configuracoes.DiretorioSalvarArquivos + "\\promocoes\\"), nomeArquivo);
                    var fileSavePath = Path.Combine(Configuracoes.DiretorioSalvarArquivos + "\\promocoes", nomeArquivo);
                    httpPostedFile.SaveAs(fileSavePath);

                    var objArq = new ZAdmin_RN.Promocao.PromocaoArquivo();
                    objArq.idpromocao = Convert.ToInt32(idPromocao);
                    objArq.nome = nomeArquivo;
                    objArq.tipo = tipo;
                    objArq.excluido = false;

                    var arqPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();
                    arqPromo.salvarArquivoRegulamento(objArq);
                }
            }
            catch (Exception ex)
            {                            
                throw ex;
            }

            //var httpPostedFile = HttpContext.Current.Request.Files["arquivoRegul"];
            //var idPromocao = HttpContext.Current.Request.Form[0];
            //string tipo = "";
            //if (httpPostedFile != null)
            //{
            //    tipo = httpPostedFile.ContentType.Contains("image") ? "Imagem" : "Arquivo";

            //    var nomeArquivo = idPromocao + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(httpPostedFile.FileName);
            //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(Configuracoes.DiretorioSalvarArquivos+ "\\promocoes\\"), nomeArquivo);
            //    httpPostedFile.SaveAs(fileSavePath);

            //    var objArq = new ZAdmin_RN.Promocao.PromocaoArquivo();
            //    objArq.idpromocao = Convert.ToInt32(idPromocao);
            //    objArq.nome = nomeArquivo;
            //    objArq.tipo = tipo;
            //    objArq.excluido = false;

            //    var arqPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();
            //    arqPromo.salvarArquivoRegulamento(objArq);
            //}
        }

        [HttpPut]
        public void SalvarArquivoRelacionado()
        {

            try
            {
                var httpPostedFile = HttpContext.Current.Request.Files["arquivoRelacionado"];
                var idPromocao = HttpContext.Current.Request.Form[0];
                string tipo = "";

                if (httpPostedFile != null)
                {
                    tipo = httpPostedFile.ContentType.Contains("image") ? "Imagem" : "Arquivo";

                    var nomeArquivo = idPromocao + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(httpPostedFile.FileName);
                    //var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(Configuracoes.DiretorioSalvarArquivos + "\\promocoes\\"), nomeArquivo);
                    var fileSavePath = Path.Combine(Configuracoes.DiretorioSalvarArquivos + "\\promocoes", nomeArquivo);
                    httpPostedFile.SaveAs(fileSavePath);

                    var objArq = new ZAdmin_RN.Promocao.PromocaoArquivo();
                    objArq.idpromocao = Convert.ToInt32(idPromocao);
                    objArq.nome = nomeArquivo;
                    objArq.tipo = tipo;
                    objArq.excluido = false;

                    var arqPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();

                    arqPromo.salvarArquivoRelacionado(objArq);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
                 
            //var httpPostedFile = HttpContext.Current.Request.Files["arquivoRelacionado"];
            //var idPromocao = HttpContext.Current.Request.Form[0];
            //string tipo = "";

            //if (httpPostedFile != null)
            //{
            //    tipo = httpPostedFile.ContentType.Contains("image") ? "Imagem" : "Arquivo";

            //    var nomeArquivo = idPromocao + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(httpPostedFile.FileName);
            //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath(Configuracoes.DiretorioSalvarArquivos + "\\promocoes\\"), nomeArquivo);
            //    httpPostedFile.SaveAs(fileSavePath);

            //    var objArq = new ZAdmin_RN.Promocao.PromocaoArquivo();
            //    objArq.idpromocao = Convert.ToInt32(idPromocao);
            //    objArq.nome = nomeArquivo;
            //    objArq.tipo = tipo;
            //    objArq.excluido = false;

            //    var arqPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();
                
            //    arqPromo.salvarArquivoRelacionado(objArq);
            //}
        }

        [HttpGet]
        public void SalvarLinkRelacionado(string url, int idPromocao)
        {

            var objLink = new ZAdmin_RN.Promocao.PromocaoArquivo();
            objLink.idpromocao = idPromocao;
            objLink.tipo = "Link";
            objLink.excluido = false;
            objLink.url = url;

            var linkPromo = new ZAdmin_RN.Promocao.PromocaoArquivos();
            linkPromo.salvarLinkRelacionado(objLink);
        }

        public PromocoesPesquisa RetornaPromocaoSeae(FiltroPromocoes filtro)
        {

            var processoRN = new ZAdmin_RN.Promocao.Promocoes(ZAdmin.Utils.Configuracoes.ServidorElastic, Utils.Configuracoes.IndexElastic);

            var processo = new PromocoesPesquisa()
            {
                ListaProcessos = processoRN.RetornaProcessosSEAEPromocoes(filtro),
                TotalProcessos = processoRN.RetornaTotalProcessos()
            };

            return processo;

        }

        public Promocao RetornaPromocaoSeaeCadastro(int idPromo , int idProcesso) {

            var promocaoCadastro = new ZAdmin_RN.Promocao.Promocoes();

            return promocaoCadastro.RetornaPromocaoProSeaeCadastro(idPromo,idProcesso);


        }
        

        public bool PrintSite(string linkSite, int idPromo)
        {
            var promoRN = new ZAdmin_RN.Promocao.Promocoes();
            return promoRN.CapturaImagemUrl(linkSite, idPromo, Configuracoes.DiretorioExecutaveis, Configuracoes.DiretorioSalvarArquivos);
        }
    }
}

