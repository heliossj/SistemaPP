using Sistema.Models;
using Sistema.DataTables;
using Sistema.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class ContasContabeisController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var daoContasContabeis = new DAOContasContabeis();
                List<ContasContabeis> list = daoContasContabeis.GetContasContabeis();
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
        public ActionResult Create(Sistema.Models.ContasContabeis model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeConta))
            {
                ModelState.AddModelError("nomeConta", "Informe um nome da conta");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (string.IsNullOrWhiteSpace(model.classificacao))
            {
                ModelState.AddModelError("classificacao", "Informe a unidade");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var daoContasContabeis = new DAOContasContabeis();
                    daoContasContabeis.Insert(model);
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

        public ActionResult Edit(int id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Edit(Sistema.Models.ContasContabeis model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeConta))
            {
                ModelState.AddModelError("nomeConta", "Informe um nome da conta");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (string.IsNullOrWhiteSpace(model.classificacao))
            {
                ModelState.AddModelError("classificacao", "Informe a unidade");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var daoContasContabeis = new DAOContasContabeis();
                    daoContasContabeis.Update(model);
                    this.AddFlashMessage(Util.AlertMessage.EDIT_SUCESS);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var daoContasContabeis = new DAOContasContabeis();
                daoContasContabeis.Delete(id);
                this.AddFlashMessage(Util.AlertMessage.DELETE_SUCESS);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Details(int id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int codConta)
        {
            try
            {
                var daoContaContabil = new DAOContasContabeis();
                var model = daoContaContabil.GetContaContabil(codConta);
                return View(model);
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            try
            {
                var select = this.Find(null, requestModel.Search.Value);
                var totalResult = select.Count();
                var result = select.OrderBy(requestModel.Columns, requestModel.Start, requestModel.Length).ToList();
                return Json(new DataTablesResponse(requestModel.Draw, result, totalResult, result.Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }


        public JsonResult JsSelect(string q, int? page, int? pageSize)
        {
            try
            {
                var select = this.Find(null, q);
                return Json(new JsonSelect<object>(select, page, pageSize), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        public JsonResult JsDetails(int? id, string q)
        {
            try
            {
                var result = this.Find(id, q).FirstOrDefault();
                if (result != null)
                    return Json(result, JsonRequestBehavior.AllowGet);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoContaContabil = new DAOContasContabeis();
            var list = daoContaContabil.GetContaContabilSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
                classificacao = u.classificacao,
                vlSaldo = u.vlSaldo,
                situacao = u.situacao,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtUltAlteracao
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(ContasContabeis model)
        {
            var daoContasContabeis = new DAOContasContabeis();
            daoContasContabeis.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(ContasContabeis model)
        {
            var daoContasContabeis = new DAOContasContabeis();
            daoContasContabeis.Update(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}