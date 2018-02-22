using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZAdmin.Controllers
{
    public class EmpresaController : Controller
    {
        // GET: Empresa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Expressoes(int id)
        {
            ViewBag.idEmpresa = id;
            return View();
        }

        // GET: Empresa/Create
        public ActionResult Cadastrar()
        {
            return View();
        }

        // POST: Empresa/Create
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

        // GET: Empresa/Edit/5
        public ActionResult Editar(int id)
        {
            ViewBag.idEmpresa = id;
            return View();
        }

        // POST: Empresa/Edit/5
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

        // GET: Empresa/Delete/5
        public ActionResult Deletar(int id)
        {
            return View();
        }

      

        // POST: Empresa/Delete/5
        [HttpPost]
        public ActionResult Deletar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
