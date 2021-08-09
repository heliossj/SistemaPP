﻿using Sistema.DAO;
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
            try
            {
                var daoEstados = new DAOEstados();
                List<Models.Estados> list = daoEstados.GetEstados();
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
                try
                {
                    daoEstados = new DAOEstados();
                    daoEstados.Insert(model);
                    this.AddFlashMessage(Util.AlertMessage.INSERT_SUCESS);
                    return RedirectToAction("Index");
                } catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message);
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
        public ActionResult Edit(Sistema.Models.Estados model)
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
                try
                {
                    daoEstados = new DAOEstados();
                    daoEstados.Update(model);
                    this.AddFlashMessage(Util.AlertMessage.EDIT_SUCESS);
                    return RedirectToAction("Index");
                } catch (Exception ex)
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
                daoEstados = new DAOEstados();
                daoEstados.Delete(id);
                this.AddFlashMessage(Util.AlertMessage.DELETE_SUCESS);
                return RedirectToAction("Index");
            } catch(Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codEstado)
        {
            try
            {
                var daoEstados = new DAOEstados();
                var model = daoEstados.GetEstado(codEstado);
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
            var daoEstados = new DAOEstados();
            var list = daoEstados.GetEstadosSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
                uf = u.uf,
                Pais = new Sistema.Select.Paises.Select {
                    id = u.PaisSelect.id,
                    text = u.PaisSelect.text
                },
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtUltAlteracao
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Models.Estados model)
        {
            var daoEstados = new DAOEstados();
            daoEstados.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Models.Estados model)
        {
            var daoEstados = new DAOEstados();
            daoEstados.Update(model);
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