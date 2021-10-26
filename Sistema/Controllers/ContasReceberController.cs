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
    public class ContasReceberController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var DAOContaReceber = new DAOContasReceber();
                List<Models.ContasReceber> list = DAOContaReceber.GetContasPagar();
                return View(list);
            } catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Receber(int id)
        {
            return this.GetView(id);
        }

        public ActionResult Details(int id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Receber(int id, Sistema.Models.ContasReceber model)
        {
            if (model.ContaContabil.id == null)
            {
                ModelState.AddModelError("ContaContabil.id", "Informe a conta");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var DAOContasReceber = new DAOContasReceber();
                    DAOContasReceber.Receber(id, model);
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

        private ActionResult GetView(int id)
        {
            try
            {
                var DAOContaReceber = new DAOContasReceber();
                var model = DAOContaReceber.GetContaPagar(id);
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