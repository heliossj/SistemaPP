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
    public class DAOCompras : Sistema.DAO.DAO
    {

        public List<Compras> GetCompras()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Compras>();

                while (reader.Read())
                {
                    var compras = new Compras
                    {
                        codigo = Convert.ToInt32(reader["Compra_ID"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["Compra_Situacao"])),
                        Fornecedor = new Select.Fornecedores.Select
                        {
                            id = Convert.ToInt32(reader["Fornecedor_ID"]),
                            text = Convert.ToString(reader["Fornecedor_Nome"])
                        },
                        modelo = Convert.ToString(reader["Compra_Modelo"]),
                        serie = Convert.ToString(reader["Compra_Serie"]),
                        nrNota = Convert.ToInt32(reader["Compra_Numero"]),
                        dtEmissao = Convert.ToDateTime(reader["Compra_DataEmissao"]),
                        dtEntrega = Convert.ToDateTime(reader["Compra_DataEntrega"]),
                        dtCadastro = Convert.ToDateTime(reader["Compra_DataEntrada"]),
                    };

                    list.Add(compras);
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

        public bool Insert(Models.Compras compra)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbcompras ( modelo, serie, numero, dtemissao, dtentrega, codfornecedor, observacao, dtcadastro, situacao, codcondicao ) VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, '{8}', {9}); SELECT SCOPE_IDENTITY()",
                    compra.modelo,
                    compra.serie,
                    compra.nrNota,
                    compra.dtEmissao != null ? this.FormatDate(compra.dtEmissao) : null,
                    compra.dtEntrega != null ? this.FormatDate(compra.dtEntrega) : null,
                    compra.Fornecedor.id,
                    this.FormatString(compra.observacao),
                    this.FormatDate(DateTime.Now),
                    "N",
                    compra.CondicaoPagamento.id
                    );
                string sqlProduto = "INSERT INTO tbprodutoscompra ( codcompra, codproduto, unidade, qtproduto, vlcompra, txdesconto, vlvenda) VALUES ({0}, {1}, '{2}', {3}, {4}, {5}, {6})";
                string sqlParcela = "INSERT INTO tbcontaspagar (codfornecedor, codforma, nrparcela, vlparcela, dtvencimento, situacao, codcompra) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}', {6} )";
                string sqlUpdateProduto = "UPDATE tbprodutos set qtestoque += {0}, vlultcompra += {1}, unidade = '{2}'";
                using (con)
                {
                    OpenConnection();

                    SqlTransaction sqlTrans = con.BeginTransaction();
                    SqlCommand command = con.CreateCommand();
                    command.Transaction = sqlTrans;
                    try
                    {
                        command.CommandText = sql;
                        var codCompra = Convert.ToInt32(command.ExecuteScalar());
                        foreach (var item in compra.ProdutosCompra)
                        {
                            var Item = string.Format(sqlProduto, codCompra, item.codProduto, item.unidade, this.FormatDecimal(item.qtProduto), this.FormatDecimal(item.vlCompra), this.FormatDecimal(item.txDesconto), this.FormatDecimal(item.vlVenda));
                            command.CommandText = Item;
                            command.ExecuteNonQuery();

                            //var upProd = string.Format(sqlUpdateProduto, item.qtProduto, item.vlCompra, item.unidade);
                            //command.CommandText = upProd;
                            //command.ExecuteNonQuery();
                        }

                        foreach (var item in compra.ParcelasCompra)
                        {
                            var parcela = string.Format(sqlParcela, compra.Fornecedor.id, item.idFormaPagamento, item.nrParcela, this.FormatDecimal(item.vlParcela), this.FormatDate(item.dtVencimento), "P", codCompra);
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

        public void CancelarCompra(int? codCompra)
        {
            throw new Exception("Método ainda não implementado");
        }

        public Compras GetCompra(int codCompra)
        {
            try
            {
                var model = new Models.Compras();
                OpenConnection();
                var sql = this.Search(codCompra, null);
                var sqlProdutos = this.SearchProdutos(codCompra);
                var sqlParcelas = this.SearchParcelas(codCompra);
                var listProdutos = new List<Compras.ProdutosVM>();
                var listParcelas = new List<Shared.ParcelasVM>();

                SqlQuery = new SqlCommand(sql + sqlProdutos + sqlParcelas, con);
                reader = SqlQuery.ExecuteReader();
                while (reader.Read())
                {
                    model.codigo = Convert.ToInt32(reader["Compra_ID"]);
                    model.situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["Compra_Situacao"]));
                    model.Fornecedor = new Select.Fornecedores.Select
                    {
                        id = Convert.ToInt32(reader["Fornecedor_ID"]),
                        text = Convert.ToString(reader["Fornecedor_Nome"])
                    };
                    model.modelo = Convert.ToString(reader["Compra_Modelo"]);
                    model.serie = Convert.ToString(reader["Compra_Serie"]);
                    model.nrNota = Convert.ToInt32(reader["Compra_Numero"]);
                    model.dtEmissao = Convert.ToDateTime(reader["Compra_DataEmissao"]);
                    model.dtEntrega = Convert.ToDateTime(reader["Compra_DataEntrega"]);
                    model.dtCadastro = Convert.ToDateTime(reader["Compra_DataEntrada"]);
                    model.observacao = Convert.ToString(reader["Compra_Observacao"]);
                    model.CondicaoPagamento = new Select.CondicaoPagamento.Select
                    {
                        id = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                        text = Convert.ToString(reader["CondicaoPagamento_Nome"])
                    };
                };

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var produto = new Compras.ProdutosVM
                        {
                            codProduto = Convert.ToInt32(reader["ProdutoCompra_Produto_ID"]),
                            nomeProduto = Convert.ToString(reader["ProdutoCompra_Produto_Nome"]),
                            unidade = Convert.ToString(reader["ProdutoCompra_Unidade"]),
                            qtProduto = Convert.ToDecimal(reader["ProdutoCompra_QtProduto"]),
                            vlCompra = Convert.ToDecimal(reader["ProdutoCompra_VlCompra"]),
                            txDesconto = Convert.ToDecimal(reader["ProdutoCompra_TxDesconto"]),
                            vlVenda = Convert.ToDecimal(reader["ProdutoCompra_VlVenda"]),
                        };
                        var txDesc = (produto.vlCompra * produto.txDesconto) / 100;
                        var vlTotal = produto.vlCompra - txDesc;
                        produto.vlTotal = vlTotal;
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
                            nrParcela = Convert.ToDouble(reader["ContaPagar_NrParcela"]),
                            vlParcela = Convert.ToDecimal(reader["ContaPagar_VlParcela"]),
                            dtVencimento = Convert.ToDateTime(reader["ContaPagar_DataVencimento"]),
                            situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["ContaPagar_Situacao"]))
                        };
                        listParcelas.Add(parcela);
                    }
                }
                model.ProdutosCompra = listProdutos;
                model.ParcelasCompra = listParcelas;
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

        private string Search(int? id, string filter)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE tbcompras.codcompra = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbfornecedores.nomerazaosocial LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
            SELECT
	            tbcompras.codcompra AS Compra_ID,
	            tbcompras.situacao AS Compra_Situacao,
	            tbcompras.modelo AS Compra_Modelo,
	            tbcompras.serie AS Compra_Serie,
	            tbcompras.numero AS Compra_Numero,
	            tbcompras.dtemissao AS Compra_DataEmissao,
	            tbcompras.dtentrega AS Compra_DataEntrega,
	            tbcompras.observacao AS Compra_Observacao,
	            tbcompras.dtcadastro AS Compra_DataEntrada,
	            tbcompras.codfornecedor AS Fornecedor_ID,
	            tbfornecedores.nomerazaosocial AS Fornecedor_Nome,
	            tbcompras.codcondicao AS CondicaoPagamento_ID,
	            tbcondpagamentos.nomecondicao AS CondicaoPagamento_Nome
            FROM tbcompras
            INNER JOIN tbfornecedores on tbcompras.codfornecedor = tbfornecedores.codfornecedor
            INNER JOIN tbcondpagamentos on tbcompras.codcondicao = tbcondpagamentos.codcondicao
            " + swhere + ";";
            return sql;
        }

        private string SearchProdutos(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbprodutoscompra.codcompra AS ProdutosCompra_Compra_ID,
	                    tbprodutoscompra.codproduto AS ProdutoCompra_Produto_ID,
	                    tbprodutos.nomeproduto AS ProdutoCompra_Produto_Nome,
	                    tbprodutoscompra.unidade AS ProdutoCompra_Unidade,
	                    tbprodutoscompra.qtproduto AS ProdutoCompra_QtProduto,
	                    tbprodutoscompra.vlcompra AS ProdutoCompra_VlCompra,
	                    tbprodutoscompra.txdesconto AS ProdutoCompra_TxDesconto,
	                    tbprodutoscompra.vlvenda AS ProdutoCompra_VlVenda
                    FROM tbprodutoscompra
                    INNER JOIN tbprodutos on tbprodutoscompra.codproduto = tbprodutos.codproduto
                    WHERE tbprodutoscompra.codcompra = " + id + ";"
            ;
            return sql;
        }

        private string SearchParcelas(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbcontaspagar.codcontapagar AS ContaPagar_ID,
                        tbcontaspagar.codfornecedor AS ContaPagar_Fornecedor_ID,
	                    tbcontaspagar.codforma AS FormaPagamento_ID,
	                    tbformapagamento.nomeforma AS FormaPagamento_Nome,
	                    tbcontaspagar.nrparcela AS ContaPagar_NrParcela,
	                    tbcontaspagar.vlparcela AS ContaPagar_VlParcela,
	                    tbcontaspagar.dtvencimento AS ContaPagar_DataVencimento,
                        tbcontaspagar.codcompra AS ContaPagar_Compra_ID,
                        tbcontaspagar.situacao AS ContaPagar_Situacao
                    FROM tbcontaspagar
                    INNER JOIN tbformapagamento on tbcontaspagar.codforma = tbformapagamento.codforma
                    WHERE tbcontaspagar.codcompra = " + id
            ;
            return sql;
        }

        public bool validNota(string modelo, string serie, int nrNota, int codFornecedor)
        {
            string sql = "SELECT * FROM tbcompras WHERE modelo = '" + modelo + "' AND serie = '" + serie + "' AND numero = " + nrNota + " AND codfornecedor = " + codFornecedor;
            OpenConnection();
            SqlQuery = new SqlCommand(sql, con);
            var exist = SqlQuery.ExecuteScalar();
            CloseConnection();
            if (exist == null)
            {
                return true;
            }
            else
            {
                throw new Exception("Já existe uma compra registrada com este número e fornecedor, verifique!");
            }
        }
    }
}