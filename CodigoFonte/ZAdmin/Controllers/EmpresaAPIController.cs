using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZAdmin.Helpers;
using ZAdmin.ViewModel;
using ZAdmin_RN.Empresas;

namespace ZAdmin.Controllers
{
    public class EmpresaAPIController : ApiController
    {
        // GET: api/EmpresaAPI
        public EmpresaListar Get(filtroEmpresas filtro)
        {

            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            var empresas = RNEmpresas.retornaEmpresas(filtro);

            var empListar = new EmpresaListar();
            empListar.Empresas = empresas;
            empListar.TotalEmpresas = RNEmpresas.TotalEmpresas;
            return empListar;

        }
        
        public EmpresaListar retornaEmpresas(filtroEmpresas filtro)
        {

            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            var empresas = RNEmpresas.retornaEmpresas(filtro);

            var empListar = new EmpresaListar();
            empListar.Empresas = empresas;
            empListar.TotalEmpresas = RNEmpresas.TotalEmpresas;
            return empListar;

        }

        // GET: api/EmpresaAPI/5
        public EmpresaCadastro Get(int id)
        {

            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            var resultado = RNEmpresas.retornaEmpresa(id);

            var cadEmpresa = new EmpresaCadastro();
            cadEmpresa.Empresa = resultado;
            cadEmpresa.Setores = RNEmpresas.RetornaSetores();

            return cadEmpresa;

        }
        
        public List<Empresa> GetRetornaNomeDeEmpresas()
        {
            var empresasRN = new ZAdmin_RN.Empresas.Empresas();

            return empresasRN.RetornaListaNomeDeEmpresas();
        }

        public bool GetVerificaCnpjExistente(string cnpjempresa)
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();

            return RNEmpresas.verificaCnpjExistente(cnpjempresa);
        }

        public bool GetExisteCNPJBancoEditar(string cnpjempresa, int idempresa)
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();

            return RNEmpresas.ExisteCNPJBancoEditar(cnpjempresa, idempresa);
        }

        // POST: api/EmpresaAPI
        //[FromBody]Empresa empresa
        [HttpPost]
        public Mensagem Post()
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            var msg = new Mensagem();
            msg.erro = false;
            int newidEmpresa = 0;

            string jsonEmpresa = HttpContext.Current.Request.Form[0];
            ZAdmin_RN.Empresas.Empresa empresa = JsonConvert.DeserializeObject<ZAdmin_RN.Empresas.Empresa>(jsonEmpresa);

            if (RNEmpresas.VerificaEmpresaExistente(empresa.nome))
            {
                msg.erro = true;
                msg.mensagem = "Empresa já cadastrada!";
                return msg;
            }
            else
            {   
                newidEmpresa = RNEmpresas.Cadastrar(empresa);
                empresa.idempresa = newidEmpresa;
            }


            var httpPostedFile = HttpContext.Current.Request.Files["ImagemEmpresa"];
            if (httpPostedFile != null)
            {
                var nomeArquivo = newidEmpresa + Path.GetExtension(httpPostedFile.FileName);
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/empresas"), nomeArquivo);
                httpPostedFile.SaveAs(fileSavePath);

                empresa.caminhoImagem = nomeArquivo;
                RNEmpresas.AtualizaCaminhoImagem(empresa);
            }


            //else
            //{

            //    msg.erro = false;

            //    var httpPostedFile = HttpContext.Current.Request.Files["ImagemEmpresa"];
            //    string jsonEmpresa = HttpContext.Current.Request.Form[0];

            //    ZAdmin_RN.Empresas.Empresa empresa = JsonConvert.DeserializeObject<ZAdmin_RN.Empresas.Empresa>(jsonEmpresa);
            //    int newidEmpresa = RNEmpresas.Cadastrar(empresa);
            //    empresa.idempresa = newidEmpresa;
            //    empresa.caminhoImagem = "padrao.png";

            //    if (HttpContext.Current.Request.Files.AllKeys.Any())
            //    {
            //        var nomeArquivo = newidEmpresa + Path.GetExtension(httpPostedFile.FileName);
            //         empresa.caminhoImagem = nomeArquivo;

            //            if (httpPostedFile != null)
            //            {
            //                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/empresas"), nomeArquivo);
            //                httpPostedFile.SaveAs(fileSavePath);
            //            }

            //    }


            //    RNEmpresas.AtualizaCaminhoImagem(empresa);

            //}

            return msg;
        }


        [HttpPost]
        public Mensagem VerificarExtImagem() {

            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            var msg = new Mensagem();
            bool VerificarExt = false;

            var httpPostedFile = HttpContext.Current.Request.Files["ImagemEmpresa"];
            if(httpPostedFile != null)
            {
                string fileExt = System.IO.Path.GetExtension(httpPostedFile.FileName).ToLower();
                List<string> ListaExtensoes = new List<string>() { ".jpg", ".png", ".jpeg" };
                if (!ListaExtensoes.Contains(fileExt))
                {
                    msg.erro = true;
                    msg.mensagem = string.Format("Extensão de arquivo *.{0} não suportada", httpPostedFile.FileName);
                    return msg;
                }
            }                        

            /*
            string jsonEmpresa = HttpContext.Current.Request.Form[0];

            ZAdmin_RN.Empresas.Empresa empresa = JsonConvert.DeserializeObject<ZAdmin_RN.Empresas.Empresa>(jsonEmpresa);
            int newidEmpresa = RNEmpresas.Cadastrar(empresa);

            empresa.idempresa = newidEmpresa;
            */

            msg.erro = false;

            //if (httpPostedFile != null)
            //{
            //    var nomeArquivo = newidEmpresa + Path.GetExtension(httpPostedFile.FileName);
            //    empresa.caminhoImagem = nomeArquivo;

            //    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/empresas"), nomeArquivo);
            //    httpPostedFile.SaveAs(fileSavePath);

            //    RNEmpresas.AtualizaCaminhoImagem(empresa);
            //}            
            
            return msg;
        }


        // PUT: api/EmpresaAPI/5
        //int id, [FromBody]Empresa empresa
        public void Put()
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            
            var httpPostedFile = HttpContext.Current.Request.Files["ImagemEmpresa"];
            string jsonEmpresa = HttpContext.Current.Request.Form[0];

            ZAdmin_RN.Empresas.Empresa empresa = JsonConvert.DeserializeObject<ZAdmin_RN.Empresas.Empresa>(jsonEmpresa);
            RNEmpresas.Editar(empresa);

            if (httpPostedFile != null)
            {
                var nomeArquivo = empresa.idempresa + Path.GetExtension(httpPostedFile.FileName);
                var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/empresas"), nomeArquivo);
                httpPostedFile.SaveAs(fileSavePath);

                empresa.caminhoImagem = nomeArquivo;
                RNEmpresas.AtualizaCaminhoImagem(empresa);
            }

            //if (HttpContext.Current.Request.Files.AllKeys.Any())
            //    {
            //        var nomeArquivo = empresa.idempresa + Path.GetExtension(httpPostedFile.FileName);
            //        empresa.caminhoImagem = "/imagens/empresas/" + nomeArquivo;
            //        if (httpPostedFile != null)
            //        {                       
            //            var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/imagens/empresas"), nomeArquivo);
            //            httpPostedFile.SaveAs(fileSavePath);
            //        }
            //    }            
        }

        // DELETE: api/EmpresaAPI/5
        public void Delete(int id)
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();

            RNEmpresas.Deletar(id);

        }

        public List<EmpresaExpressao> GetRetornaExpressoes(int idEmpresa)
        {
            var RNEmpresa = new ZAdmin_RN.Empresas.EmpresaExpressoes();

            return RNEmpresa.RetornaListaDeExpressoes(idEmpresa);
        }

        public int SalvarExpressao(EmpresaExpressao expressao)
        {
            var RNEmpresa = new ZAdmin_RN.Empresas.EmpresaExpressoes();

            return RNEmpresa.Cadastrar(expressao);
        }

        public void excluirExpressao(int idExpressao)
        {
            var RNEmpresa = new ZAdmin_RN.Empresas.EmpresaExpressoes();

            RNEmpresa.Excluir(idExpressao);
        }
        
        public List<ZAdmin_RN.Combos.ItemCombo> GetItensComboEmpresa()
        {
            var RNEmpresas = new ZAdmin_RN.Empresas.Empresas();
            return RNEmpresas.RetornaItensComboEmpresas();

        }

        public List<Setor> GetItensComboSetor()
        {
            var RNEmpresa = new ZAdmin_RN.Empresas.Empresas();


            return RNEmpresa.RetornaSetores();

		}
        public bool TestarEmpressao(ZAdmin_RN.Knewin.DadosRequestAPI dados)
        {
            var api = new ZAdmin_RN.Knewin.RequestAPI();
            return api.Acessar(dados);
        }

        [HttpGet]
        public List<ZAdmin_RN.Combos.ItemCombo> EmpresasSetor(int idSetor)
        {
            var rnEmpresas = new ZAdmin_RN.Empresas.Empresas();
            return rnEmpresas.RetornaEmpresasSetor(idSetor);
        }

    }
}
