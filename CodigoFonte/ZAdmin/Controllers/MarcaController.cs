using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class MarcaController : Controller
    {
        // GET: Marca
        public ActionResult Index()
        {
            return View();
        }

        // GET: Marca
        public ActionResult Expressoes(int id)
        {
            ViewBag.idMarca = id;
            return View();
        }

        // GET: Marca/Create
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Marca/Create
        [HttpPost]
        public ActionResult Cadastrar(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Marca/Edit/5
        public ActionResult Editar(int id)
        {
            ViewBag.idMarca = id;
            return View();
        }

        // POST: Marca/Edit/5
        [HttpPost]
        public ActionResult Editar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

   
    }
}
