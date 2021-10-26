﻿using Sistema.DAO;
using Sistema.DataTables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Controllers
{
    public class VendasController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                // modelo - 56 serviço / 65 consumidor
                var DAOVendas = new DAOVendas();
                List<Models.Vendas> list = DAOVendas.GetVendas("65");
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
        public ActionResult Create(Sistema.Models.Vendas model)
        {
            if (model.Funcionario.id == null)
            {
                ModelState.AddModelError("Funcionario.id", "Informe o funcionário");
            }
            if (model.Cliente.id == null)
            {
                ModelState.AddModelError("Cliente.id", "Informe o cliente");
            }
            model.modelo = "65";
            if (ModelState.IsValid)
            {
                try
                {
                    var DAOVendas = new DAOVendas();
                    DAOVendas.InsertProduto(model);
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

        public ActionResult Cancelar(int id)
        {
            return this.GetView(id);
        }

        private ActionResult GetView(int id)
        {
            try
            {
                var DAOVendas = new DAOVendas();
                var model = DAOVendas.GetVenda(id, "65");
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