using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class MapaRegistroController : Controller
    {
        // GET: MapaRegistro
        public ActionResult Index()
        {
            return View();
        }

        // GET: MapaRegistro/Create
        public ActionResult Cadastrar()
        {
            return View();
        }

        // GET: MapaRegistro/Edit
        public ActionResult Editar(int id)
        {
            ViewBag.idRegistro = id;
            return View();
        }


    }
}