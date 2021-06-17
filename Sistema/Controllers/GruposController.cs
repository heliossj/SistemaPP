using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class GruposController : Controller
    {
        DAOGrupos daoGrupos = new DAOGrupos();

        public ActionResult Index()
        {
            var daoGrupos = new DAOGrupos();
            List<Models.Grupos> list = daoGrupos.GetGrupos();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Grupos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeGrupo))
            {
                ModelState.AddModelError("nomeGrupo", "Informe um nome de grupo válido");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe uma situação");
            }
            if (ModelState.IsValid)
            {
                daoGrupos = new DAOGrupos();
                daoGrupos.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Grupos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeGrupo))
            {
                ModelState.AddModelError("nomeGrupo", "Informe um nome de grupo válido");
            }
            if (string.IsNullOrWhiteSpace(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe uma situação");
            }
            if (ModelState.IsValid)
            {
                daoGrupos = new DAOGrupos();
                daoGrupos.Update(model);
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
            daoGrupos = new DAOGrupos();
            daoGrupos.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codGrupo)
        {
            var daoGrupo = new DAOGrupos();
            var model = daoGrupo.GetGrupo(codGrupo);
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
            var daoGrupos = new DAOGrupos();
            var list = daoGrupos.GetGruposSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Models.Grupos model)
        {
            var daoGrupos = new DAOGrupos();
            daoGrupos.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Models.Grupos model)
        {
            var daoGrupos = new DAOGrupos();
            daoGrupos.Update(model);
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