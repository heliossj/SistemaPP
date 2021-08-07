using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class CidadesController : Controller
    {
        DAOCidades daoCidades = new DAOCidades();

        public ActionResult Index()
        {
            var daoCidades = new DAOCidades();
            List<Models.Cidades> list = daoCidades.GetCidades();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Cidades model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeCidade))
            {
                ModelState.AddModelError("nomeCidade", "Informe um nome de cidade válida");
            }
            if (string.IsNullOrWhiteSpace(model.ddd))
            {
                ModelState.AddModelError("ddd", "Informe um ddd válido");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe uma sigla válida");
            }
            if (model.Estado.id == null)
            {
                ModelState.AddModelError("Estado.id", "Informe um estado");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    daoCidades = new DAOCidades();
                    daoCidades.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Cidades model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeCidade))
            {
                ModelState.AddModelError("nomeCidade", "Informe um nome de cidade válida");
            }
            if (string.IsNullOrWhiteSpace(model.ddd))
            {
                ModelState.AddModelError("ddd", "Informe um ddd válido");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe uma sigla válida");
            }
            if (model.Estado.id == null)
            {
                ModelState.AddModelError("Estado.id", "Informe um estado");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoCidades = new DAOCidades();
                    daoCidades.Update(model);
                    this.AddFlashMessage(Util.AlertMessage.EDIT_SUCESS);
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
                daoCidades = new DAOCidades();
                daoCidades.Delete(id);
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

        private ActionResult GetView(int? codCidade)
        {
            try
            {
                var daoCidades = new DAOCidades();
                var model = daoCidades.GetCidade(codCidade);
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
            var daoCidades = new DAOCidades();
            var list = daoCidades.GetCidadesSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Models.Cidades model)
        {
            var daoCidades = new DAOCidades();
            daoCidades.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Models.Cidades model)
        {
            var daoCidades = new DAOCidades();
            daoCidades.Update(model);
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