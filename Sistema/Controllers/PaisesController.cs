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
        DAOPaises daoPaises = new DAOPaises();

        public ActionResult Index()
        {
            var daoPaises = new DAOPaises();
            List<Models.Paises> list = daoPaises.GetPaises();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Paises model)
        {
            if (string.IsNullOrWhiteSpace(model.nomePais))
            {
                ModelState.AddModelError("nomePais", "Informe um nome de país válido");
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
                daoPaises = new DAOPaises();
                if (daoPaises.Insert(model))
                {
                    ViewBag.Message = "Registro inserido com sucesso";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Edit(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Edit(Sistema.Models.Paises model)
        {
            if (string.IsNullOrWhiteSpace(model.nomePais))
            {
                ModelState.AddModelError("nomePais", "Informe um nome de país válido");
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

                daoPaises = new DAOPaises();
                if (daoPaises.Update(model))
                {
                    ViewBag.Message = "Registro alterado com sucesso";
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]

        public ActionResult Delete(int? id, Sistema.Models.Paises model)
        {
            daoPaises = new DAOPaises();
            if (daoPaises.Delete(id))
            {
                ViewBag.Message = "Registro removido com sucesso";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codPais)
        {
            var daoPaises = new DAOPaises();
            var model = daoPaises.GetPais(codPais);
            return View(model);
            //var model = new Models.Paises(aux)
        }

    }
}