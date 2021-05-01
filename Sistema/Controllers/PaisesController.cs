using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class PaisesController : Controller
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
        public ActionResult Create(Sistema.Models.PaisesVM model)
        {
            if (string.IsNullOrEmpty(model.nmPais))
            {
                ModelState.AddModelError("nmPais", "Informe o País");
            }
            if (ModelState.IsValid)
            {
                return View(model);
            }
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

        public ActionResult Details()
        {
            return View();
        }


    }
}