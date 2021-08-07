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
    public class ClientesController : Controller
    {
        DAOClientes daoClientes = new DAOClientes();

        public ActionResult Index()
        {
            try
            {
                var daoClientes = new DAOClientes();
                List<Models.Clientes> list = daoClientes.GetClientes();
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
        public ActionResult Create(Sistema.Models.Clientes model)
        {
            this.validForm(model);
            if (ModelState.IsValid)
            {
                try
                {
                    daoClientes = new DAOClientes();
                    daoClientes.Insert(model);
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
        public ActionResult Edit(Sistema.Models.Clientes model)
        {
            this.validForm(model);
            if (ModelState.IsValid)
            {
                try
                {
                    daoClientes = new DAOClientes();
                    daoClientes.Update(model);
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
                daoClientes = new DAOClientes();
                daoClientes.Delete(id);
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

        private ActionResult GetView(int? codCliente)
        {
            var daoClientes = new DAOClientes();
            var model = daoClientes.GetCliente(codCliente);
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
            var daoClientes = new DAOClientes();
            var list = daoClientes.GetClientesSelect(id, q);
            var select = list.Select(u => new
            {
                id = u.id,
                text = u.text,
            }).OrderBy(u => u.text).ToList();
            return select.AsQueryable();
        }

        public JsonResult JsCreate(Clientes model)
        {
            var daoClientes = new DAOClientes();
            daoClientes.Insert(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro adicionado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult JsUpdate(Clientes model)
        {
            var daoClientes = new DAOClientes();
            daoClientes.Update(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private Models.Clientes validForm(Models.Clientes model)
        {
            if (string.IsNullOrEmpty(model.situacao))
            {
                ModelState.AddModelError("situacao", "Informe a situação");
            }
            if (model.tipo == "F")
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
            }
            else
            {
                model.email = model.emailJuridica;
                if (string.IsNullOrWhiteSpace(model.razaoSocial))
                {
                    ModelState.AddModelError("razaoSocial", "Informe a razão social");
                }
                if (string.IsNullOrWhiteSpace(model.nomeFantasia))
                {
                    ModelState.AddModelError("nomeFantasia", "Informe o nome fantasia");
                }
                if (string.IsNullOrWhiteSpace(model.cnpj))
                {
                    ModelState.AddModelError("cnpj", "Informe o CNPJ");
                }
                if (string.IsNullOrWhiteSpace(model.ie))
                {
                    ModelState.AddModelError("ie", "Informe a Inscrição Estadual");
                }
                if (string.IsNullOrWhiteSpace(model.emailJuridica))
                {
                    ModelState.AddModelError("emailJuridica", "Informe o email");
                }
            }

            //Dados em comum
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
            if (model.CondicaoPagamento.id == null)
            {
                ModelState.AddModelError("CondicaoPagamento.id", "Informe a condição de pagamento");
            }
            return model;
        }
    }
}