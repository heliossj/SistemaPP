using Sistema.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class ComprasController : Controller
    {
        DAOCompras daoCompra = new DAOCompras();

        public ActionResult Index()
        {
            try
            {
                var daoCompra = new DAOCompras();
                List<Models.Compras> list = daoCompra.GetCompras();
                return View(list);
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Compras model)
        {
            model.dtEmissao = !string.IsNullOrEmpty(model.dtEmissaoAux) ? Convert.ToDateTime(model.dtEmissaoAux) : model.dtEmissao;

            if (string.IsNullOrWhiteSpace(model.modelo))
            {
                ModelState.AddModelError("modelo", "Informe o modelo");
            }
            if (string.IsNullOrWhiteSpace(model.serie))
            {
                ModelState.AddModelError("serie", "Informe a série");
            }
            if (model.nrNota == null || model.nrNota == 0)
            {
                ModelState.AddModelError("nrNota", "Informe o número da nota");
            }
            if (model.Fornecedor.id == null)
            {
                ModelState.AddModelError("Fornecedor.id", "Informe o fornecedor");
            }
            if (model.dtEmissao == null)
            {
                ModelState.AddModelError("dtEmissao", "Informe a data de emissão");
            }
            if (model.dtEntrega == null)
            {
                ModelState.AddModelError("dtEntrega", "Informe a data de entrega");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    daoCompra = new DAOCompras();
                    //var validNF = daoCompra.validNota(model.modelo, model.serie, model.nrNota.GetValueOrDefault(), model.Fornecedor.id.GetValueOrDefault());
                    if (model.finalizar != "S")
                    {
                        throw new Exception("É preciso verificar antes de continuar");
                    }
                    daoCompra.Insert(model);
                    this.AddFlashMessage(Util.AlertMessage.INSERT_SUCESS);
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

        public ActionResult Details(int id)
        {
            return this.GetView(id);
        }

        public ActionResult Cancelar(int id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        [ActionName("Cancelar")]
        public ActionResult CancelarCompra(int id, Models.Compras model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    daoCompra = new DAOCompras();
                    daoCompra.CancelarCompra(id);
                    this.AddFlashMessage("Registro cancelado com sucesso!");
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
                var daoCompra = new DAOCompras();
                var model = daoCompra.GetCompra(id);
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