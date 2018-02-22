using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class ProcessoController : Controller
    {
        // GET: Processo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Editar(int id)
        {           
            ViewBag.idProcesso = id;
            return View();
        }

    }
}