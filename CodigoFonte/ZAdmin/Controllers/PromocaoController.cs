using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class PromocaoController : Controller
    {
        // GET: Promocao
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Cadastro()
        //{
        //    return View();
        //}

        //public ActionResult Cadastro(int idNoticia)
        //{
        //    ViewBag.idNoticia = idNoticia;
        //    return View();
        //}

        public ActionResult Cadastro(int? idProc = null)
        {
            if (idProc == null)
            {
                ViewBag.idProcesso = "";
                ViewBag.NumeroProcesso = "";
                ViewBag.idPost = "";
                ViewBag.Rede = "";
                ViewBag.idNoticia = "";

                return View();
            }                
            else
                return CadastroProcesso(idProc);
        }
        
        public ActionResult CadastroProcesso(int? idProc)
        {
            idProc = idProc != null ? idProc : null;

            var RNProcessos = new ZAdmin_RN.Processo.Processos();
            var processo = RNProcessos.RetornaProcesso(idProc);

            ViewBag.idProcesso = processo.idprocesso;
            ViewBag.NumeroProcesso = processo.numprocesso;
            ViewBag.idPost = "";
            ViewBag.Rede = "";

            return View("Cadastro");
        }

        [HttpGet]
        public ActionResult CadastroRedeSocial(string rede, string idPost)
        {
            ViewBag.Rede = rede != null ? rede : "";
            ViewBag.idPost = idPost != null && idPost != "0" ? idPost : "";

            return View("Cadastro");
        }

        [HttpGet]
        public ActionResult CadastroNoticia(string idNoticia, string fonte)
        {
            ViewBag.idNoticia = idNoticia != null ? idNoticia : "";
            ViewBag.fontePesquisa = fonte != null ? fonte : "";
            
            return View("Cadastro");
        }

        public ActionResult Editar(int idPromo, int? idProc = null)
        {
            var promoView = new ViewModel.PromocaoEditar();
            var promoDados = new ZAdmin_RN.Promocao.Promocoes();

            if (idProc == null)
            {
                promoView.promocao = promoDados.retornaPromocao(idPromo);
            }
            else
            {
                promoView.promocao = promoDados.RetornaPromocaoProSeaeCadastro(idPromo, idProc);
            }

            ViewBag.idProcesso = idProc;
            ViewBag.idPromocao = idPromo;

            return View(promoView);
        }
    }
}