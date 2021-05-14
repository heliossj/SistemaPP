using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class EstadosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Estados model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeEstado))
            {
                ModelState.AddModelError("nomeEstado", "Informe um nome de estado válido");
            }
            if (string.IsNullOrWhiteSpace(model.uf))
            {
                ModelState.AddModelError("uf", "Informe uma uf válida");
            }
            if (model.Pais.id == null)
            {
                ModelState.AddModelError("Pais.id", "Informe um país");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}