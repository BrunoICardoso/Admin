using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Editar(int id)
        {
            ViewBag.idUsuario = id;
            return View();
        }

    }
}