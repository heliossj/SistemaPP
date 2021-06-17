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
    public class FormaPagamentoController : Controller
    {
        DAOFormaPagamento daoFormaPagamento = new DAOFormaPagamento();

        public ActionResult Index()
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            List<Models.FormaPagamento> list = daoFormaPagamento.GetFormasPagamentos();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.FormaPagamento model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeForma))
            {
                ModelState.AddModelError("nomeForma", "Informe uma forma de pagamento válida");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe uma situação");
            }
            if (ModelState.IsValid)
            {
                daoFormaPagamento = new DAOFormaPagamento();
                daoFormaPagamento.Insert(model);
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
        public ActionResult Edit(Sistema.Models.FormaPagamento model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeForma))
            {
                ModelState.AddModelError("nomeForma", "Informe um nome de forma de pagamento válido");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (ModelState.IsValid)
            {
                daoFormaPagamento = new DAOFormaPagamento();
                daoFormaPagamento.Update(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Delete(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            daoFormaPagamento = new DAOFormaPagamento();
            daoFormaPagamento.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codFoma)
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            var model = daoFormaPagamento.GetFormaPagamento(codFoma);
            return View(model);
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
            var daoPaises = new DAOPaises();
            var list = daoPaises.GetPaisesSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
                ddi = u.ddi,
                sigla = u.sigla,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtUltAlteracao

            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(FormaPagamento model)
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            daoFormaPagamento.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(FormaPagamento model)
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            daoFormaPagamento.Update(model);
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