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

        public ActionResult Details(string modelo, string serie, int numero, int codFornecedor, short nrparcela)
        {
            return this.GetView(modelo, serie, numero, codFornecedor, nrparcela);
        }

        public ActionResult Pagar(string modelo, string serie, int numero, int codFornecedor, short nrparcela)
        {
            return this.GetView(modelo, serie, numero, codFornecedor, nrparcela);
        }

        [HttpPost]
        public ActionResult Pagar(string modelo, string serie, int numero, int codFornecedor, short nrparcela, Sistema.Models.ContasPagar model)
        {
            if (model.ContaContabil.id == null)
            {
                ModelState.AddModelError("ContaContabil.id", "Informe a conta");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var daoContasPagar = new DAOContasPagar();
                    daoContasPagar.Pagar(modelo, serie, numero, codFornecedor, nrparcela, model);
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

        private ActionResult GetView(string modelo, string serie, int numero, int codFornecedor, short nrparcela)
        {
            try
            {
                var daoContasPagar = new DAOContasPagar();
                var model = daoContasPagar.GetContaPagar(nrparcela, modelo, serie, numero, codFornecedor);
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