using Sistema.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class OrdemServicoController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var daoOS = new DAOOrdemServico();
                List<Models.OrdemServico> list = daoOS.GetOrdemServicos();
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
        public ActionResult Create(Models.OrdemServico model)
        {
            if (model.dtValidade == null)
            {
                ModelState.AddModelError("dtValidade", "Informe a data de validade");
            }
            if (model.dtValidade != null && model.dtValidade < DateTime.Now.AddDays(-1))
            {
                ModelState.AddModelError("dtValidade", "A data de validade não pode ser menor que o dia de hoje");
            }
            if (model.dtExecucao == null)
            {
                ModelState.AddModelError("dtExecucao", "Informe a data de execução");
            }
            if (model.dtValidade != null && model.dtExecucao != null)
            {
                if (model.dtExecucao < model.dtValidade)
                {
                    ModelState.AddModelError("dtExecucao", "A data de execução não pode ser menor a data de validade ");
                }
            }
            if (model.Cliente.id == null)
            {
                ModelState.AddModelError("Cliente.id", "Informe o cliente");
            }
            if (model.Funcionario.id == null)
            {
                ModelState.AddModelError("Funcionario.id", "Informe o funcionário");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var daoOS = new DAOOrdemServico();
                    daoOS.Insert(model);
                    //if (model.situacao == "T")
                    //{
                    //    RedirectToAction("Index", "Compras");
                    //}
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


        public ActionResult Details(int id)
        {
            return this.GetView(id);
        }

        public ActionResult Edit(int id)
        {
            return this.GetView(id);
        }

        [HttpPost]
        public ActionResult Edit(int id, Models.OrdemServico model)
        {
            if (model.dtValidade == null)
            {
                ModelState.AddModelError("dtValidade", "Informe a data de validade");
            }
            //if (model.dtValidade != null && model.dtValidade < DateTime.Now.AddDays(-1))
            //{
            //    ModelState.AddModelError("dtValidade", "A data de validade não pode ser menor que o dia de hoje");
            //}
            if (model.dtExecucao == null)
            {
                ModelState.AddModelError("dtExecucao", "Informe a data de execução");
            }
            if (model.dtValidade != null && model.dtExecucao != null)
            {
                if (model.dtExecucao < model.dtValidade)
                {
                    ModelState.AddModelError("dtExecucao", "A data de execução não pode ser menor a data de validade ");
                }
            }
            if (model.Cliente.id == null)
            {
                ModelState.AddModelError("Cliente.id", "Informe o cliente");
            }
            if (model.Funcionario.id == null)
            {
                ModelState.AddModelError("Funcionario.id", "Informe o funcionário");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var daoOS = new DAOOrdemServico();
                    if (model.situacao != "F")
                        daoOS.Update(model);
                    else
                    {
                        return RedirectToAction("VendaOS", "Vendas", new { id = id });
                    }
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

        public ActionResult Cancelar(int id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int id)
        {
            try
            {
                var daoOS = new DAOOrdemServico();
                var model = daoOS.GetOrdemServico(id);
                return View(model);
            }
            catch (Exception ex)
            {
                this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                return View();
            }
        }
    }
}