using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class CondicaoPagamentoController : Controller
    {
        DAOCondicaoPagamento daoCondicaoPagamento = new DAOCondicaoPagamento();

        public ActionResult Index()
        {
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            List<Models.CondicaoPagamento> list = daoCondicaoPagamento.GetCondicaoPagamentos();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.CondicaoPagamento model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeCondicao))
            {
                ModelState.AddModelError("nomeCondicao", "Informe um nome de condição de pagamento válido");
            }
            //if (model.ListCondicao.Get == null)
            //{
            //    ModelState.AddModelError("ListCondicao", "Informe ao menos um item na lista");
            //}
            if (ModelState.IsValid)
            {
                daoCondicaoPagamento = new DAOCondicaoPagamento();
                daoCondicaoPagamento.Insert(model);
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
        public ActionResult Edit(Sistema.Models.CondicaoPagamento model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeCondicao))
            {
                ModelState.AddModelError("nomeCondicao", "Informe um nome de condição de pagamento válido");
            }
            if (ModelState.IsValid)
            {
                daoCondicaoPagamento = new DAOCondicaoPagamento();
                //daoCondicaoPagamento.Update(model);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            daoCondicaoPagamento = new DAOCondicaoPagamento();
            daoCondicaoPagamento.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codCondicaoPagamento)
        {
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            var model = daoCondicaoPagamento.GetCondicaoPagamento(codCondicaoPagamento);
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
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            var list = daoCondicaoPagamento.GetCondicaoPagamentoSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,

            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Models.CondicaoPagamento model)
        {
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            daoCondicaoPagamento.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Models.CondicaoPagamento model)
        {
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            //daoCondicaoPagamento.Update(model);
            //model.idMarca = bean.idMarca;
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