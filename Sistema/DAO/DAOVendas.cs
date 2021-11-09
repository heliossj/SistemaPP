using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Sistema.Select;

namespace Sistema.DAO
{
    public class DAOVendas : Sistema.DAO.DAO
    {

        public List<Vendas> GetVendas(string modelo)
        {
            try
            {
                var sql = this.Search(null, null, modelo);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Vendas>();

                while (reader.Read())
                {
                    var Venda = new Vendas
                    {
                        codigo = Convert.ToInt32(reader["Venda_ID"]),
                        situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Venda_Situacao"])),
                        dtVenda = Convert.ToDateTime(reader["Venda_Data"]),
                        Funcionario = new Select.Funcionarios.Select
                        {
                            id  = Convert.ToInt32(reader["Vendedor_ID"]),
                            text = Convert.ToString(reader["Vendedor_Nome"])
                        },
                        Cliente = new Select.Clientes.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_ID"]),
                            text = Convert.ToString(reader["Cliente_Nome"])
                        },
                        CondicaoPagamento = new Select.CondicaoPagamento.Select
                        {
                            id = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                            text = Convert.ToString(reader["CondicaoPagamento_Nome"])
                        }
                    };
                    list.Add(Venda);
                }
                return list;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool InsertProduto(Models.Vendas venda)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbvendas ( situacao, dtvenda, codvendedor, codcliente, codcondicao, codordemservico, modelo) VALUES ( '{0}', {1}, {2}, {3}, {4}, {5}, '{6}' ); SELECT SCOPE_IDENTITY()",
                    "N",
                    this.FormatDateTime(DateTime.Now),
                    venda.Funcionario.id,
                    venda.Cliente.id,
                    venda.CondicaoPagamento.id,
                    venda.codOrdemServico != null ? venda.codOrdemServico.ToString() : "null",
                    venda.modelo
                    );
                string sqlProduto = "INSERT INTO tbprodutosvenda ( codvenda, codproduto, unidade, qtproduto, vlproduto, txdesconto ) VALUES ( {0}, {1}, '{2}', {3}, {4}, {5} )";
                string sqlParcela = "INSERT INTO tbcontasreceber ( codvenda, codforma, nrparcela, vlparcela, dtvencimento, situacao, codcliente, juros, multa, desconto ) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}', {6}, {7}, {8}, {9} )";
                string sqlProdutoEstoque = "UPDATE tbprodutos set qtestoque -= {0} WHERE tbprodutos.codproduto = {1}";
                using (con)
                {
                    OpenConnection();

                    SqlTransaction sqlTrans = con.BeginTransaction();
                    SqlCommand command = con.CreateCommand();
                    command.Transaction = sqlTrans;
                    try
                    {
                        command.CommandText = sql;
                        var codVenda = Convert.ToInt32(command.ExecuteScalar());

                        foreach (var item in venda.ProdutosVenda)
                        {
                            var produto = string.Format(sqlProduto, codVenda, item.codProduto, this.FormatString(item.unidade), this.FormatDecimal(item.qtProduto), this.FormatDecimal(item.vlVenda), this.FormatDecimal(item.txDesconto));
                            command.CommandText = produto;
                            command.ExecuteNonQuery();

                            //VERIFICAR SE EXISTE A QUANTIDADE DO ITEM EM ESTOQUE ANTES DE VENDER

                            var prodEstoque = string.Format(sqlProdutoEstoque, this.FormatDecimal(item.vlVenda), item.codProduto);
                            command.CommandText = prodEstoque;
                            command.ExecuteNonQuery();
                        }
                        foreach (var item in venda.ParcelasVenda)
                        {
                            var parcela = string.Format(sqlParcela, codVenda, item.idFormaPagamento, item.nrParcela, this.FormatDecimal(item.vlParcela), this.FormatDate(item.dtVencimento), "P", venda.Cliente.id, this.FormatDecimal(venda.CondicaoPagamento.txJuros), this.FormatDecimal(venda.CondicaoPagamento.multa), this.FormatDecimal(venda.CondicaoPagamento.desconto));
                            command.CommandText = parcela;
                            command.ExecuteNonQuery();
                        }
                        sqlTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTrans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void CancelarVenda(int? codVenda)
        {
            throw new Exception("Não implementado");
        }

        public Vendas GetVenda(int codVenda, string modelo)
        {
            try
            {
                var model = new Models.Vendas();
                OpenConnection();
                var sql = this.Search(codVenda, null, modelo);
                var sqlProdutos = this.SearchProdutos(codVenda);
                var sqlParcelas = this.SearchParcelas(codVenda);
                var listProdutos = new List<Vendas.ProdutosVM>();
                var listParcelas = new List<Shared.ParcelasVM>();

                SqlQuery = new SqlCommand(sql + sqlProdutos + sqlParcelas, con);
                reader = SqlQuery.ExecuteReader();
                while (reader.Read())
                {
                    model.codigo = Convert.ToInt32(reader["Venda_ID"]);
                    model.situacao = Convert.ToString(reader["Venda_Situacao"]);
                    model.dtVenda = Convert.ToDateTime(reader["Venda_Data"]);
                    model.CondicaoPagamento = new Select.CondicaoPagamento.Select
                    {
                        id = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                        text = Convert.ToString(reader["CondicaoPagamento_Nome"])
                    };
                    model.Cliente = new Select.Clientes.Select
                    {
                        id = Convert.ToInt32(reader["Cliente_ID"]),
                        text = Convert.ToString(reader["Cliente_Nome"])
                    };
                    model.Funcionario = new Select.Funcionarios.Select
                    {
                        id = Convert.ToInt32(reader["Vendedor_ID"]),
                        text = Convert.ToString(reader["Vendedor_Nome"])
                    };
                };
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var produto = new Vendas.ProdutosVM
                        {
                            codProduto = Convert.ToInt32(reader["Produto_ID"]),
                            nomeProduto = Convert.ToString(reader["Produto_Nome"]),
                            unidade = Convert.ToString(reader["Produto_Unidade"]),
                            qtProduto = Convert.ToDecimal(reader["Produto_Quantidade"]),
                            vlVenda = Convert.ToDecimal(reader["Produto_Valor"]),
                            txDesconto = Convert.ToDecimal(reader["Produto_TaxaDesconto"])                       
                        };
                        if (produto.txDesconto != null && produto.txDesconto != 0)
                        {
                            decimal txDesc;
                            txDesc = (produto.vlVenda * produto.txDesconto.GetValueOrDefault()) / 100;
                            produto.vlVenda = produto.vlVenda - txDesc;
                        }
                        produto.vlTotal = produto.vlVenda * produto.qtProduto;
                        listProdutos.Add(produto);
                    }
                }
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var parcela = new Shared.ParcelasVM
                        {
                            idFormaPagamento = Convert.ToInt32(reader["FormaPagamento_ID"]),
                            nmFormaPagamento = Convert.ToString(reader["FormaPagamento_Nome"]),
                            nrParcela = Convert.ToDouble(reader["ContaReceber_NrParcela"]),
                            vlParcela = Convert.ToDecimal(reader["ContaReceber_Valor"]),
                            dtVencimento = Convert.ToDateTime(reader["ContaReceber_DataVencimento"]),
                            situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["ContaReceber_Situacao"])),
                        };
                        listParcelas.Add(parcela);
                    }
                }
                model.ProdutosVenda = listProdutos;
                model.ParcelasVenda = listParcelas;
                return model;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        private string Search(int? id, string filter, string modelo)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " AND (tbvendas.codvenda = " + id + ") ";
            }
            if (!string.IsNullOrEmpty(modelo))
            {
                swhere += " AND (tbvendas.modelo = '" + modelo + "') ";
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbclientes.nomerazaosocial LIKE'%" + word + "%'";
                }
            }
            if (!string.IsNullOrEmpty(swhere))
                swhere = " WHERE " + swhere.Remove(0, 4);
            sql = @"
                    SELECT
	                    tbvendas.codvenda AS Venda_ID,
	                    tbvendas.situacao AS Venda_Situacao,
	                    tbvendas.dtvenda AS Venda_Data,
	                    tbvendas.modelo AS Venda_Modelo,
	                    tbvendas.codvendedor AS Vendedor_ID,
	                    tbfuncionarios.nomefuncionario AS Vendedor_Nome,
	                    tbvendas.codcliente AS Cliente_ID,
	                    tbclientes.nomerazaosocial AS Cliente_Nome,
	                    tbvendas.codcondicao AS CondicaoPagamento_ID,
	                    tbcondpagamentos.nomecondicao AS CondicaoPagamento_Nome
                    FROM tbvendas
                    INNER JOIN tbfuncionarios ON tbvendas.codvendedor = tbfuncionarios.codfuncionario
                    INNER JOIN tbclientes ON tbvendas.codcliente = tbclientes.codcliente
                    INNER JOIN tbcondpagamentos ON tbvendas.codcondicao = tbcondpagamentos.codcondicao
            " + swhere + ";";
            return sql;
        }

        private string SearchProdutos(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbprodutosvenda.codvenda AS Venda_ID,
	                    tbprodutosvenda.codproduto AS Produto_ID,
	                    tbprodutos.nomeproduto AS Produto_Nome,
	                    tbprodutosvenda.unidade AS Produto_Unidade,
	                    tbprodutosvenda.qtproduto AS Produto_Quantidade,
	                    tbprodutosvenda.vlproduto AS Produto_Valor,
	                    tbprodutosvenda.txdesconto AS Produto_TaxaDesconto
                    FROM tbprodutosvenda
                    INNER JOIN tbprodutos ON tbprodutosvenda.codproduto = tbprodutos.codproduto
                    WHERE tbprodutosvenda.codvenda = " + id + ";"
            ;
            return sql;
        }

        private string SearchParcelas(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbcontasreceber.codcontareceber AS ContaReceber_ID,
	                    tbcontasreceber.nrparcela AS ContaReceber_NrParcela,
	                    tbcontasreceber.vlparcela AS ContaReceber_Valor,
	                    tbcontasreceber.dtvencimento AS ContaReceber_DataVencimento,
	                    tbcontasreceber.situacao AS ContaReceber_Situacao,
	                    tbcontasreceber.dtpagamento AS ContaReceber_DataPagamento,
	                    tbcontasreceber.codforma AS FormaPagamento_ID,
	                    tbformapagamento.nomeforma AS FormaPagamento_Nome,
	                    tbcontasreceber.codvenda AS ContaReceber_Venda_ID
                    FROM tbcontasreceber
                    INNER JOIN tbformapagamento ON tbcontasreceber.codforma = tbformapagamento.codforma
                    WHERE tbcontasreceber.codvenda = " + id
            ;
            return sql;
        }
    }
}