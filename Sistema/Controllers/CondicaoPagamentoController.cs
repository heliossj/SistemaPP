using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            try
            {
                var daoCondicaoPagamento = new DAOCondicaoPagamento();
                List<Models.CondicaoPagamento> list = daoCondicaoPagamento.GetCondicaoPagamentos();
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
        public ActionResult Create(Sistema.Models.CondicaoPagamento model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeCondicao))
            {
                ModelState.AddModelError("nomeCondicao", "Informe um nome de condição de pagamento válido");
            }
            if (model.ListCondicao == null || !model.ListCondicao.Any())
            {
                ModelState.AddModelError("ListCondicao", "Informe ao menos um item na lista");
            }
            if (model.txJuros == null)
            {
                ModelState.AddModelError("txJuros", "Informe a taxa de juros");
            }
            if (model.multa == null)
            {
                ModelState.AddModelError("multa", "Informe a multa");
            }
            if (model.txPercentualTotalAux != 100)
            {
                ModelState.AddModelError("txPercentualTotal", "A porcentagem total das parcelas deve ser igual a 100%, verifique");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoCondicaoPagamento = new DAOCondicaoPagamento();
                    daoCondicaoPagamento.Insert(model);
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
            if (model.ListCondicao == null || !model.ListCondicao.Any())
            {
                ModelState.AddModelError("ListCondicao", "Informe ao menos um item na lista");
            }
            if (model.txJuros == null)
            {
                ModelState.AddModelError("txJuros", "Informe a taxa de juros");
            }
            if (model.multa == null)
            {
                ModelState.AddModelError("multa", "Informe a multa");
            }
            if (model.txPercentualTotalAux != 100)
            {
                ModelState.AddModelError("txPercentualTotal", "A porcentagem total das parcelas deve ser igual a 100%, verifique");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoCondicaoPagamento = new DAOCondicaoPagamento();
                    daoCondicaoPagamento.Update(model);
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

        public ActionResult Delete(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                daoCondicaoPagamento = new DAOCondicaoPagamento();
                daoCondicaoPagamento.Delete(id);
                this.AddFlashMessage(Util.AlertMessage.DELETE_SUCESS);
                return RedirectToAction("Index");
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

        private ActionResult GetView(int? codCondicaoPagamento)
        {
            try
            {
                var daoCondicaoPagamento = new DAOCondicaoPagamento();
                var model = daoCondicaoPagamento.GetCondicaoPagamento(codCondicaoPagamento);
                return View(model);
            } catch (Exception ex)
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

        public JsonResult JsGetParcelas(int idCondicaoPagamento, decimal? vlTotal, DateTime? dtIiniParcela)
        {
            var daoConPag = new DAOCondicaoPagamento();
            var cond = daoConPag.GetCondicaoPagamento(idCondicaoPagamento);
            var ListCondicao = cond.ListCondicao.OrderBy(k => k.nrParcela);

            var ListParcelas = new List<Models.Shared.ParcelasVM>();
            var dtInicio = DateTime.Now;
            if (dtIiniParcela != null)
            {
                dtInicio = dtIiniParcela.GetValueOrDefault();
            }
            foreach (var parcela in ListCondicao)
            {
                var itemParcela = new Models.Shared.ParcelasVM
                {
                    nrParcela = parcela.nrParcela,
                    dtVencimento = dtInicio.AddDays((double)parcela.qtDias),
                    idFormaPagamento = parcela.codFormaPagamento,
                    nmFormaPagamento = parcela.nomeFormaPagamento,
                    vlParcela = (parcela.txPercentual / 100 ) * vlTotal
                };
                ListParcelas.Add(itemParcela);
            }

            var result = new
            {
                type = "success",
                message = "Parcelas geradas com sucesso!",
                parcelas = ListParcelas
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }





    }
}