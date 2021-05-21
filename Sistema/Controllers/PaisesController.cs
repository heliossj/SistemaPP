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
    public class PaisesController : Controller
    {
        DAOPaises daoPaises = new DAOPaises();

        public ActionResult Index()
        {
            var daoPaises = new DAOPaises();
            List<Models.Paises> list = daoPaises.GetPaises();
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sistema.Models.Paises model)
        {
            if (string.IsNullOrWhiteSpace(model.nomePais))
            {
                ModelState.AddModelError("nomePais", "Informe um nome de país válido");
            }
            if (string.IsNullOrWhiteSpace(model.DDI))
            {
                ModelState.AddModelError("DDI", "Informe o DDI");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe a sigla");
            }
            if (ModelState.IsValid)
            {
                daoPaises = new DAOPaises();
                daoPaises.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Paises model)
        {
            if (string.IsNullOrWhiteSpace(model.nomePais))
            {
                ModelState.AddModelError("nomePais", "Informe um nome de país válido");
            }
            if (string.IsNullOrWhiteSpace(model.DDI))
            {
                ModelState.AddModelError("DDI", "Informe o DDI");
            }
            if (string.IsNullOrWhiteSpace(model.sigla))
            {
                ModelState.AddModelError("sigla", "Informe a sigla");
            }
            if (ModelState.IsValid)
            {

                daoPaises = new DAOPaises();
                daoPaises.Update(model);
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
            daoPaises = new DAOPaises();
            daoPaises.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codPais)
        {
            var daoPaises = new DAOPaises();
            var model = daoPaises.GetPais(codPais);
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
            }).OrderBy(u => u.text).ToList();

            //if (!string.IsNullOrEmpty(q))
            //{
            //    var filter = q.Split(' ');
            //    foreach (var word in filter)
            //    {
            //        select.Where(u => u.text.Contains(word));
            //    }
            //}



            return select.AsQueryable();
        }

        public JsonResult JsCreate(Paises model)
        {
            var daoPaises = new DAOPaises();
            var create = daoPaises.Insert(model);
            model.codPais = create.id;
            //model.idMarca = bean.idMarca;
            var result = new
            {
                type = "success",
                field = "",
                message = "Pais adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}