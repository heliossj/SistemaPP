using Sistema.DAO;
using Sistema.DataTables;
using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class ContasPagarController : Controller
    {
        DAOContasPagar daoPaises = new DAOContasPagar();

        public ActionResult Index()
        {
            var k = ViewBag.filter;
            try
            {
                var daoContasPagar = new DAOContasPagar();
                List<Models.ContasPagar> list = daoContasPagar.GetContasPagar();
                return View(list);
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        public ActionResult Pagar(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Pagar(int id, Sistema.Models.ContasPagar model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var daoContasPagar = new DAOContasPagar();
                    daoContasPagar.Pagar(id);
                    this.AddFlashMessage("Parcela paga com sucesso!");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Cancelar(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codContaPagar)
        {
            try
            {
                var daoContasPagar = new DAOContasPagar();
                var model = daoContasPagar.GetContaPagar(codContaPagar);
                return View(model);
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }
    }
}