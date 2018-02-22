using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class ClienteController : Controller
    {

        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cliente/Create
        public ActionResult Cadastrar()
        {
            return View();
        }

        public ActionResult Editar(int id)
        {
            ViewBag.idCliente = id;
            return View();
        }



    }
}