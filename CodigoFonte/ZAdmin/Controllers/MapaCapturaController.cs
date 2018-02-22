using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class MapaCapturaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // GET: MapaRegistro/Create
        public ActionResult Exportar(int id)
        {
            ViewBag.idCaptura = id;
            return View();
        }

        public ActionResult Editar(int id)
        {
            ViewBag.idCaptura = id;
            return View();
        }

    }

}
