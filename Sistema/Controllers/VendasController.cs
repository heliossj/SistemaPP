using Sistema.DAO;
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
                List<Models.Vendas> list = DAOVendas.GetVendas(null);
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

        public ActionResult Details(int id, string modelo)
        {
            return this.GetView(id, modelo);
        }

        public ActionResult Cancelar(int id, string modelo)
        {
            return this.GetView(id, modelo);
        }

        [HttpPost]
        public ActionResult Cancelar(int id, string modelo, Sistema.Models.Vendas model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var DAOVendas = new DAOVendas();
                    DAOVendas.CancelarVenda(id);
                    this.AddFlashMessage(Util.AlertMessage.INSERT_SUCESS);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    
                    return this.GetView(id, modelo);
                }
            }
            else
            {

                return View(model);
            }
        }

        public ActionResult VendaOS(int id)
        {
            var daoOS = new DAOOrdemServico();
            var os = daoOS.GetOrdemServico(id);
            var listServicos = new List<Models.VendasOS.ServicosVM>();
            var listProdutos = new List<Models.VendasOS.ProdutosVM>();

            var model = new Models.VendasOS
            {
                Funcionario = new Select.Funcionarios.Select
                {
                    id = os.Funcionario.id,
                    text = os.Funcionario.text
                },
                Cliente = new Select.Clientes.Select
                {
                    id = os.Cliente.id,
                    text = os.Cliente.text
                },
                CondicaoPagamento = new Select.CondicaoPagamento.Select
                {
                    id = os.CondicaoPagamento.id,
                    text = os.CondicaoPagamento.text,
                    desconto = os.CondicaoPagamento.desconto,
                    multa = os.CondicaoPagamento.multa,
                    txJuros = os.CondicaoPagamento.txJuros
                }
            };
            model.codOrdemServico = id;
            model.codigo = id;
            if (os.ServicosOS != null && os.ServicosOS.Any())
            {
                foreach (var item in os.ServicosOS)
                {
                    var servico = new Models.VendasOS.ServicosVM
                    {
                        codServico = item.codServico,
                        nomeServico = item.nomeServico,
                        codExecutante = item.codExecutante,
                        nomeExecutante = item.nomeExecutante,
                        qtServico = item.qtServico,
                        unidade = item.unidade,
                        vlServico = item.vlServico
                    };
                    listServicos.Add(servico);
                }
                model.ServicosOS = listServicos;
            }

            if (os.ProdutosOS != null && os.ProdutosOS.Any())
            {
                foreach (var item in os.ProdutosOS)
                {
                    var produto = new Models.VendasOS.ProdutosVM
                    {
                        codProduto = item.codProduto,
                        nomeProduto = item.nomeProduto,
                        qtProduto = item.qtProduto.GetValueOrDefault(),
                        txDesconto = 0,
                        unidade = item.unidade,
                        vlTotal = item.qtProduto.GetValueOrDefault() * item.vlProduto.GetValueOrDefault(),
                        vlVenda = item.vlProduto.GetValueOrDefault()
                    };
                    listProdutos.Add(produto);
                }
                model.ProdutosVenda = listProdutos;
                model.CondicaoPagamentoDois = new Select.CondicaoPagamento.Select
                {
                    id = os.CondicaoPagamento.id,
                    text = os.CondicaoPagamento.text,
                    desconto = os.CondicaoPagamento.desconto,
                    multa = os.CondicaoPagamento.multa,
                    txJuros = os.CondicaoPagamento.txJuros
                };
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult VendaOS(int id, Sistema.Models.VendasOS model)
        {
            model.codigo = id;
            model.codOrdemServico = id;
            if (model.Funcionario.id == null)
            {
                ModelState.AddModelError("Funcionario.id", "Informe o vendedor");
            }
            if (model.Cliente.id == null)
            {
                ModelState.AddModelError("Cliente.id", "Informe o cliente");
            }
            if (model.ProdutosVenda != null && model.ProdutosVenda.Any() && model.CondicaoPagamentoDois.id != null && (model.ParcelasVendaProdutos == null || !model.ParcelasVendaProdutos.Any()))
            {
                ModelState.AddModelError("CondicaoPagamentoDois.id", "Informe as parcelas dos produtos");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var DAOVendas = new DAOVendas();
                    var produtosVenda = new List<Models.Vendas.ProdutosVM>();
                    var parcelasProdutos = new List<Models.Shared.ParcelasVM>();

                    //VENDA SERVIÇO
                    DAOVendas.InsertServico(model);

                    //VENDA PRODUTO
                    if (model.ProdutosVenda != null && model.ProdutosVenda.Any())
                    {
                        var vendaProduto = new Models.Vendas
                        {
                            Funcionario = new Select.Funcionarios.Select
                            {
                                id = model.Funcionario.id,
                                nmFuncionario = model.Funcionario.text,
                            },
                            Cliente = new Select.Clientes.Select
                            {
                                id = model.Cliente.id,
                                text = model.Cliente.text,
                            },
                            CondicaoPagamento = new Select.CondicaoPagamento.Select
                            {
                                id = model.CondicaoPagamentoDois.id,
                                text = model.CondicaoPagamentoDois.text,
                                desconto = model.CondicaoPagamentoDois.desconto,
                                multa = model.CondicaoPagamentoDois.multa,
                                txJuros = model.CondicaoPagamentoDois.txJuros
                            },
                            modelo = "65",
                            codOrdemServico = model.codOrdemServico,
                        };
                        foreach (var item in model.ProdutosVenda)
                        {
                            var prod = new Models.Vendas.ProdutosVM
                            {
                                codProduto = item.codProduto,
                                nomeProduto = item.nomeProduto,
                                qtProduto = item.qtProduto,
                                txDesconto = item.txDesconto,
                                unidade = item.unidade,
                                vlVenda = item.vlVenda,                                
                            };
                            produtosVenda.Add(prod);
                        }
                        vendaProduto.ProdutosVenda = produtosVenda;
                        foreach (var item in model.ParcelasVendaProdutos)
                        {
                            var par = new Models.Shared.ParcelasVM
                            {
                                idFormaPagamento = item.idFormaPagamento,
                                nmFormaPagamento = item.nmFormaPagamento,
                                nrParcela = item.nrParcela,
                                vlParcela = item.vlParcela,
                                dtPagamento = item.dtPagamento,
                                dtVencimento = item.dtVencimento,
                                situacao = item.situacao,
                                
                            };
                            parcelasProdutos.Add(par);
                        }
                        vendaProduto.ParcelasVenda = parcelasProdutos;

                        DAOVendas.InsertProduto(vendaProduto);
                    }
                    this.AddFlashMessage("Ordem de Serviço finalizada com sucesso");
                    return RedirectToAction("Index", "OrdemServico");
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

        private ActionResult GetView(int id, string modelo)
        {
            try
            {
                var DAOVendas = new DAOVendas();
                var model = DAOVendas.GetVenda(id, modelo);
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