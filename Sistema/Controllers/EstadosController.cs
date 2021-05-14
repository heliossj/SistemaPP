using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class EstadosController : Controller
    {
        DAOEstados daoEstados = new DAOEstados();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Estados model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeEstado))
            {
                ModelState.AddModelError("nomeEstado", "Informe um nome de estado válido");
            }
            if (string.IsNullOrWhiteSpace(model.uf))
            {
                ModelState.AddModelError("uf", "Informe uma uf válida");
            }
            if (model.Pais.id == null)
            {
                ModelState.AddModelError("Pais.id", "Informe um país");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {

            try
            {
                var select = this.Find();

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
                var select = this.Find();
                return Json(new JsonSelect<object>(select, page, pageSize), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }


        public JsonResult JsDetails()
        {
            try
            {
                var result = this.Find();
                if (result != null)
                    return Json(result, JsonRequestBehavior.AllowGet);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        private IQueryable<dynamic> Find()
        {
            var daoPaises = new DAOEstados();
            var list = daoPaises.GetEstadosSelect();
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }
    }
}