using Sistema.DAO;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class PaisesController : Controller
    {
        PaisesDAO daoPaises = new PaisesDAO();

        public ActionResult Index()
        {
            var daoPaises = new PaisesDAO();
            List<Models.PaisesVM> list = daoPaises.GetPaises();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.PaisesVM model)
        {
            if (string.IsNullOrWhiteSpace(model.nmPais))
            {
                ModelState.AddModelError("nmPais", "Informe um nome de país válido");
            }
            if (string.IsNullOrWhiteSpace(model.DDI))
            {
                ModelState.AddModelError("DDI", "Informe o DDI");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe a sigla");
            }
            if (ModelState.IsValid)
            {
                
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Sistema.Models.PaisesVM model)
        {
            if (string.IsNullOrWhiteSpace(model.nmPais))
            {
                ModelState.AddModelError("nmPais", "Informe um nome de país válido");
            }
            if (string.IsNullOrWhiteSpace(model.DDI))
            {
                ModelState.AddModelError("DDI", "Informe o DDI");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe a sigla");
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }


    }
}