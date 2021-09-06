using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class OrdemServicoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new Sistema.Models.OrdemServico
            {
                dtAbertura = DateTime.Now,
            };
            return View(model);
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