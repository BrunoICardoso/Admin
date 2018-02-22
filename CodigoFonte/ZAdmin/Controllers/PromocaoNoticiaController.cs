using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class PromocaoNoticiaController : Controller
    {
        // GET: PromocaoNoticia
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Associar(int id)
        {
            return View("Index");
        }
    }
}