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
    public class ServicosController : Controller
    {
        DAOServicos daoServicos = new DAOServicos();

        public ActionResult Index()
        {
            try
            {
                var daoServicos = new DAOServicos();
                List<Models.Servicos> list = daoServicos.GetServicos();
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
        public ActionResult Create(Sistema.Models.Servicos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeServico))
            {
                ModelState.AddModelError("nomeServico", "Informe um nome do serviço");
            }
            if (model.vlServico == null || model.vlServico == 0)
            {
                ModelState.AddModelError("vlServico", "Informe o valor do serviço");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (string.IsNullOrWhiteSpace(model.unidade))
            {
                ModelState.AddModelError("unidade", "Informe a unidade");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoServicos = new DAOServicos();
                    daoServicos.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Servicos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeServico))
            {
                ModelState.AddModelError("nomeServico", "Informe um nome do serviço");
            }
            if (model.vlServico == null || model.vlServico == 0)
            {
                ModelState.AddModelError("vlServico", "Informe o valor do serviço");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (string.IsNullOrWhiteSpace(model.unidade))
            {
                ModelState.AddModelError("unidade", "Informe a unidade");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoServicos = new DAOServicos();
                    daoServicos.Update(model);
                    this.AddFlashMessage(Util.AlertMessage.EDIT_SUCESS);
                    return RedirectToAction("Index");
                } catch (Exception ex)
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
                daoServicos = new DAOServicos();
                daoServicos.Delete(id);
                this.AddFlashMessage(Util.AlertMessage.DELETE_SUCESS);
                return RedirectToAction("Index");
            } catch ( Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codServico)
        {
            try
            {
                var daoServicos = new DAOServicos();
                var model = daoServicos.GetServico(codServico);
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
            var DAOServicos = new DAOServicos();
            var list = DAOServicos.GetServicosSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
                vlServico = u.vlServico,
                unidade = u.unidade
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Servicos model)
        {
            var daoServicos = new DAOServicos();
            daoServicos.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Servicos model)
        {
            var daoServicos = new DAOServicos();
            daoServicos.Update(model);
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