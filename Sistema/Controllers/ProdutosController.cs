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
    public class ProdutosController : Controller
    {
        DAOProdutos daoProdutos = new DAOProdutos();

        public ActionResult Index()
        {
            try
            {
                var daoProdutos = new DAOProdutos();
                List<Models.Produtos> list = daoProdutos.GetProdutos();
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
        public ActionResult Create(Sistema.Models.Produtos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeProduto))
            {
                ModelState.AddModelError("nomeProduto", "Informe o nome do produto");
            }
            if (model.Grupo.id == null)
            {
                ModelState.AddModelError("Grupo.id", "Informe o grupo");
            }
            if (model.Fornecedor.id == null)
            {
                ModelState.AddModelError("Fornecedor.id", "Informe o fornecedor");
            }
            if (string.IsNullOrWhiteSpace(model.ncm))
            {
                ModelState.AddModelError("ncm", "Informe o NCM");
            }
            if (string.IsNullOrWhiteSpace(model.cfop))
            {
                ModelState.AddModelError("cfop", "Informe o CFOP");
            }
            if (model.vlCusto == null || model.vlCusto <= 0)
            {
                ModelState.AddModelError("vlCusto", "Informe o valor de custo");
            }
            if (model.vlVenda == null || model.vlVenda <= 0)
            {
                ModelState.AddModelError("vlVenda", "Informe o valor de venda");
            }
            if (model.unidade == "M" && string.IsNullOrWhiteSpace(model.largura))
            {
                ModelState.AddModelError("largura", "Informe a largura");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoProdutos = new DAOProdutos();
                    daoProdutos.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Produtos model)
        {
            if (string.IsNullOrWhiteSpace(model.nomeProduto))
            {
                ModelState.AddModelError("nomeProduto", "Informe o nome do produto");
            }
            if (model.Grupo.id == null)
            {
                ModelState.AddModelError("Grupo.id", "Informe o grupo");
            }
            if (model.Fornecedor.id == null)
            {
                ModelState.AddModelError("Fornecedor.id", "Informe o fornecedor");
            }
            if (string.IsNullOrWhiteSpace(model.ncm))
            {
                ModelState.AddModelError("ncm", "Informe o NCM");
            }
            if (string.IsNullOrWhiteSpace(model.cfop))
            {
                ModelState.AddModelError("cfop", "Informe o CFOP");
            }
            if (model.vlCusto == null || model.vlCusto <= 0)
            {
                ModelState.AddModelError("vlCusto", "Informe o valor de custo");
            }
            if (model.vlVenda == null || model.vlVenda <= 0)
            {
                ModelState.AddModelError("vlVenda", "Informe o valor de venda");
            }
            if (model.unidade == "M" && string.IsNullOrWhiteSpace(model.largura))
            {
                ModelState.AddModelError("largura", "Informe a largura");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoProdutos = new DAOProdutos();
                    daoProdutos.Update(model);
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
                daoProdutos = new DAOProdutos();
                daoProdutos.Delete(id);
                this.AddFlashMessage(Util.AlertMessage.DELETE_SUCESS);
                return RedirectToAction("Index");
            } catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public ActionResult Details(int? id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int? codProduto)
        {
            try
            {
                var daoProdutos = new DAOProdutos();
                var model = daoProdutos.GetProduto(codProduto);
                return View(model);
            } catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? idFornecedor)
        {
            try
            {
                idFornecedor = idFornecedor == 0 ? null : idFornecedor;
                var select = this.Find(null, requestModel.Search.Value, idFornecedor);

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
                var select = this.Find(null, q, null);
                return Json(new JsonSelect<object>(select, page, pageSize), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }

        public JsonResult JsDetails(int? id, string q, int? idFornecedor)
        {
            try
            {
                var result = this.Find(id, q, idFornecedor).FirstOrDefault();
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

        private IQueryable<dynamic> Find(int? id, string q, int? idFornecedor)
        {
            var daoProdutos = new DAOProdutos();
            var list = daoProdutos.GetProdutoSelect(id, q, idFornecedor);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
                vlVenda = u.vlVenda,
                unidade = u.unidade
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Produtos model)
        {
            var daoProdutos = new DAOProdutos();
            daoProdutos.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Produtos model)
        {
            var daoProdutos = new DAOProdutos();
            daoProdutos.Update(model);
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