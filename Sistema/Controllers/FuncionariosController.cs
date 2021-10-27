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
    public class FuncionariosController : Controller
    {
        DAOFuncionarios daoFuncionarios = new DAOFuncionarios();

        public ActionResult Index()
        {
            try
            {
                var daoFuncionarios = new DAOFuncionarios();
                List<Models.Funcionarios> list = daoFuncionarios.GetFuncionarios();
                return View(list);
            } catch (Exception ex)
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
        public ActionResult Create(Sistema.Models.Funcionarios model)
        {
            this.validForm(model);
            if (model.dtAdmissao != null && model.dtAdmissao > DateTime.Now)
            {
                ModelState.AddModelError("dtAdmissao", "A data de admissão não pode ser maior que o dia de hoje");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    daoFuncionarios = new DAOFuncionarios();
                    daoFuncionarios.Insert(model);
                    this.AddFlashMessage(Util.AlertMessage.INSERT_SUCESS);
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

        public ActionResult Edit(int? id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Edit(Sistema.Models.Funcionarios model)
        {
            model.dtAdmissao = model.dtAdmissaoAux != null ? model.dtAdmissaoAux : model.dtAdmissao;
            this.validForm(model);
            if (ModelState.IsValid)
            {
                try
                {
                    daoFuncionarios = new DAOFuncionarios();
                    daoFuncionarios.Update(model);
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
                daoFuncionarios = new DAOFuncionarios();
                daoFuncionarios.Delete(id);
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

        private ActionResult GetView(int? codFuncionario)
        {
            try
            {
                var daoFuncionarios = new DAOFuncionarios();
                var model = daoFuncionarios.GetFuncionario(codFuncionario);
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
            var daoFuncionarios = new DAOFuncionarios();
            var list = daoFuncionarios.GetFuncionariosSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Funcionarios model)
        {
            var daoFuncionarios = new DAOFuncionarios();
            daoFuncionarios.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Funcionarios model)
        {
            var daoFuncionarios = new DAOFuncionarios();
            daoFuncionarios.Update(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Models.Funcionarios validForm(Models.Funcionarios model)
        {
            if (string.IsNullOrWhiteSpace(model.nomePessoa))
            {
                ModelState.AddModelError("nomePessoa", "Informe o nome");
            }
            if (string.IsNullOrWhiteSpace(model.apelidoPessoa))
            {
                ModelState.AddModelError("apelidoPessoa", "Informe o apelido");
            }
            if (string.IsNullOrEmpty(model.sexo))
            {
                ModelState.AddModelError("sexo", "Informe o sexo");
            }
            if (string.IsNullOrWhiteSpace(model.cpf))
            {
                ModelState.AddModelError("cpf", "Informe o CPF");
            }
            if (string.IsNullOrWhiteSpace(model.rg))
            {
                ModelState.AddModelError("rg", "Informe o RG");
            }
            if (model.dtNascimento == null)
            {
                ModelState.AddModelError("dtNascimento", "Informe a data de nascimento");
            }
            if (string.IsNullOrWhiteSpace(model.dsLogradouro))
            {
                ModelState.AddModelError("dsLogradouro", "Informe o logradouro");
            }
            if (string.IsNullOrWhiteSpace(model.numero))
            {
                ModelState.AddModelError("numero", "Informe o número");
            }
            if (string.IsNullOrWhiteSpace(model.complemento))
            {
                ModelState.AddModelError("complemento", "Informe o complemento");
            }
            if (string.IsNullOrWhiteSpace(model.bairro))
            {
                ModelState.AddModelError("bairro", "Informe o bairro");
            }
            if (string.IsNullOrEmpty(model.telefoneFixo) && string.IsNullOrEmpty(model.telefoneCelular))
            {
                ModelState.AddModelError("telefoneFixo", "Informe ao menos um telefone");
                ModelState.AddModelError("telefoneCelular", "Informe ao menos telefone");
            }
            if (model.Cidade.id == null)
            {
                ModelState.AddModelError("Cidade.id", "Informe a cidade");
            }
            if (string.IsNullOrWhiteSpace(model.cep))
            {
                ModelState.AddModelError("cep", "Informe o CEP");
            }
            if (model.dtAdmissao == null)
            {
                ModelState.AddModelError("dtAdmissao", "Informe a data de admissão");
            }
            if (model.vlSalario == null || model.vlSalario == 0)
            {
                ModelState.AddModelError("vlSalario", "Informe um valor de salário válido");
            }
            return model;
        }
    }
}