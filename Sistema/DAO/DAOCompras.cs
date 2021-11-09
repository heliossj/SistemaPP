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
            var list = new List<Compras>();
            var sql = this.Search(null, null, null, null, null);

            using (con)
            {
                OpenConnection();
                SqlTransaction sqlTrans = con.BeginTransaction();
                SqlCommand command = con.CreateCommand();
                command.Transaction = sqlTrans;
                try
                {
                    command.CommandText = sql;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var compra = new Compras
                        {
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
                        list.Add(compra);
                    }
                    con.Close();
                    OpenConnection();
                    foreach (var item in list)
                    {
                        var listComp = new List<Shared.ParcelasVM>();
                        using (var details = new SqlCommand(this.SearchParcelas(item.modelo, item.serie, item.nrNota, item.Fornecedor.id), con))
                        {
                            using (var detReader = details.ExecuteReader())
                            {
                                while (detReader.Read())
                                {
                                    var parcela = new Shared.ParcelasVM
                                    {
                                        idFormaPagamento = Convert.ToInt32(detReader["FormaPagamento_ID"]),
                                        nmFormaPagamento = Convert.ToString(detReader["FormaPagamento_Nome"]),
                                        nrParcela = Convert.ToDouble(detReader["ContaPagar_NrParcela"]),
                                        vlParcela = Convert.ToDecimal(detReader["ContaPagar_VlParcela"]),
                                        dtVencimento = Convert.ToDateTime(detReader["ContaPagar_DataVencimento"]),
                                        situacao = Convert.ToString(detReader["ContaPagar_Situacao"])
                                    };
                                    listComp.Add(parcela);
                                }
                            }
                        }
                        item.ParcelasCompra = listComp;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    sqlTrans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            return list;
        }

        public bool Insert(Models.Compras compra)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbcompras ( modelo, serie, numero, dtemissao, dtentrega, codfornecedor, observacao, dtcadastro, situacao, codcondicao, vlfrete, vlseguro, vldespesas ) VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, '{8}', {9}, {10}, {11}, {12} ); ",
                    compra.modelo,
                    compra.serie,
                    compra.nrNota,
                    compra.dtEmissao != null ? this.FormatDate(compra.dtEmissao.Value) : null,
                    compra.dtEntrega != null ? this.FormatDate(compra.dtEntrega) : null,
                    compra.Fornecedor.id,
                    this.FormatString(compra.observacao),
                    this.FormatDate(DateTime.Now),
                    "N",
                    compra.CondicaoPagamento.id,
                    compra.vlFrete != null ? this.FormatDecimal(compra.vlFrete).ToString() : "null",
                    compra.vlSeguro != null ? this.FormatDecimal(compra.vlSeguro).ToString() : "null",
                    compra.vlDespesas != null ? this.FormatDecimal(compra.vlDespesas).ToString() : "null"
                    );
                string sqlProduto = "INSERT INTO tbprodutoscompra ( codproduto, unidade, qtproduto, vlcompra, txdesconto, vlvenda, modelo, serie, numero, codfornecedor) VALUES ( {0}, '{1}', {2}, {3}, {4}, {5}, '{6}', '{7}', {8}, {9})";
                string sqlParcela = "INSERT INTO tbcontaspagar (codfornecedor, codforma, nrparcela, vlparcela, dtvencimento, situacao, modelo, serie, numero, juros, multa, desconto) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}', '{6}', '{7}', {8}, {9}, {10}, {11})";
                string sqlUpdateProduto = "UPDATE tbprodutos set qtestoque += {0}, vlultcompra += {1} WHERE codproduto = {2}";
                using (con)
                {
                    OpenConnection();

                    SqlTransaction sqlTrans = con.BeginTransaction();
                    SqlCommand command = con.CreateCommand();
                    command.Transaction = sqlTrans;
                    try
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        //var codCompra = Convert.ToInt32(command.ExecuteScalar());
                        foreach (var item in compra.ProdutosCompra)
                        {
                            var Item = string.Format(sqlProduto, item.codProduto, item.unidade, this.FormatDecimal(item.qtProduto), this.FormatDecimal(item.vlCompra), this.FormatDecimal(item.txDesconto), this.FormatDecimal(item.vlVenda), compra.modelo, compra.serie, compra.nrNota, compra.Fornecedor.id);
                            var test = Item;
                            command.CommandText = Item;
                            command.ExecuteNonQuery();

                            var upProd = string.Format(sqlUpdateProduto, this.FormatDecimal(item.qtProduto), this.FormatDecimal(item.vlCompra), item.codProduto);
                            command.CommandText = upProd;
                            command.ExecuteNonQuery();
                        }

                        foreach (var item in compra.ParcelasCompra)
                        {
                            var parcela = string.Format(sqlParcela, compra.Fornecedor.id, item.idFormaPagamento, item.nrParcela, this.FormatDecimal(item.vlParcela), this.FormatDate(item.dtVencimento), "P", compra.modelo, compra.serie, compra.nrNota, this.FormatDecimal(compra.CondicaoPagamento.txJuros), this.FormatDecimal(compra.CondicaoPagamento.multa), this.FormatDecimal(compra.CondicaoPagamento.desconto));
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

        public void CancelarCompra(string modelo, string serie, int numero, int codFornecedor)
        {
            throw new Exception("Não implementado");
        }

        public Compras GetCompra(string filter, string modelo, string serie, int numero, int codFornecedor)
        {
            try
            {
                var model = new Models.Compras();
                OpenConnection();
                var sql = this.Search(filter, modelo, serie, numero, codFornecedor);
                var sqlProdutos = this.SearchProdutos(modelo, serie, numero, codFornecedor);
                var sqlParcelas = this.SearchParcelas(modelo, serie, numero, codFornecedor);
                var listProdutos = new List<Compras.ProdutosVM>();
                var listParcelas = new List<Shared.ParcelasVM>();

                SqlQuery = new SqlCommand(sql + sqlProdutos + sqlParcelas, con);
                reader = SqlQuery.ExecuteReader();
                while (reader.Read())
                {
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
                    model.vlFrete = !string.IsNullOrEmpty(reader["Compra_Frete"].ToString()) ? Convert.ToDecimal(reader["Compra_Frete"]) : (decimal?)null;
                    model.vlSeguro = !string.IsNullOrEmpty(reader["Compra_Seguro"].ToString()) ? Convert.ToDecimal(reader["Compra_Seguro"]) : (decimal?)null;
                    model.vlDespesas = !string.IsNullOrEmpty(reader["Compra_Despesas"].ToString()) ? Convert.ToDecimal(reader["Compra_Despesas"]) : (decimal?)null;

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

        private string Search(string filter, string modelo, string serie, int? numero, int? codFornecedor)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (!string.IsNullOrEmpty(modelo))
            {
                swhere += " AND tbcompras.modelo = '" + modelo + "'";
            }
            if (!string.IsNullOrEmpty(serie))
            {
                swhere += " AND tbcompras.serie = '" + serie + "'";
            }
            if (numero != null)
            {
                swhere += " AND tbcompras.numero = " + numero;
            }
            if (numero != null)
            {
                swhere += " AND tbcompras.codfornecedor = " + codFornecedor;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbfornecedores.nomerazaosocial LIKE'%" + word + "%'";
                }
            }

            if (!string.IsNullOrEmpty(swhere))
                swhere = " WHERE " + swhere.Remove(0, 4);
                sql = @"
                    SELECT
	                    tbcompras.situacao AS Compra_Situacao,
	                    tbcompras.modelo AS Compra_Modelo,
	                    tbcompras.serie AS Compra_Serie,
	                    tbcompras.numero AS Compra_Numero,
	                    tbcompras.dtemissao AS Compra_DataEmissao,
	                    tbcompras.dtentrega AS Compra_DataEntrega,
	                    tbcompras.observacao AS Compra_Observacao,
	                    tbcompras.dtcadastro AS Compra_DataEntrada,
	                    tbcompras.vlfrete AS Compra_Frete,
	                    tbcompras.vlseguro AS Compra_Seguro,
	                    tbcompras.vldespesas AS Compra_Despesas,
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

        private string SearchProdutos(string modelo, string serie, int numero, int codFornecedor)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbprodutoscompra.modelo AS Compra_Modelo,
	                    tbprodutoscompra.serie AS Compra_Serie,
	                    tbprodutoscompra.numero AS Compra_Numero,
	                    tbprodutoscompra.codfornecedor AS Compra_Fornecedor_ID,
	                    tbprodutoscompra.codproduto AS ProdutoCompra_Produto_ID,
	                    tbprodutos.nomeproduto AS ProdutoCompra_Produto_Nome,
	                    tbprodutoscompra.unidade AS ProdutoCompra_Unidade,
	                    tbprodutoscompra.qtproduto AS ProdutoCompra_QtProduto,
	                    tbprodutoscompra.vlcompra AS ProdutoCompra_VlCompra,
	                    tbprodutoscompra.txdesconto AS ProdutoCompra_TxDesconto,
	                    tbprodutoscompra.vlvenda AS ProdutoCompra_VlVenda
                    FROM tbprodutoscompra
                    INNER JOIN tbprodutos on tbprodutoscompra.codproduto = tbprodutos.codproduto
                    WHERE tbprodutoscompra.modelo = '" + modelo + "' AND tbprodutoscompra.serie = '" + serie + "' AND tbprodutoscompra.numero = " + numero + " AND tbprodutoscompra.codfornecedor = " + codFornecedor + ";"
            ;
            return sql;
        }

        private string SearchParcelas(string modelo, string serie, int? numero, int? codFornecedor)
        {
            var sql = string.Empty;
            sql = @"
                    SELECT
                        tbcontaspagar.codfornecedor AS ContaPagar_Fornecedor_ID,
	                    tbcontaspagar.codforma AS FormaPagamento_ID,
	                    tbformapagamento.nomeforma AS FormaPagamento_Nome,
	                    tbcontaspagar.nrparcela AS ContaPagar_NrParcela,
	                    tbcontaspagar.vlparcela AS ContaPagar_VlParcela,
	                    tbcontaspagar.dtvencimento AS ContaPagar_DataVencimento,
                        tbcontaspagar.situacao AS ContaPagar_Situacao,
	                    tbcontaspagar.modelo AS Compra_Modelo,
	                    tbcontaspagar.serie AS Compra_Serie,
	                    tbcontaspagar.numero AS Compra_Numero,
	                    tbcontaspagar.codfornecedor AS Compra_Fornecedor_ID
                    FROM tbcontaspagar
                    INNER JOIN tbformapagamento on tbcontaspagar.codforma = tbformapagamento.codforma
                    WHERE tbcontaspagar.modelo = '" + modelo + "' AND tbcontaspagar.serie = '" + serie + "' AND tbcontaspagar.numero = " + numero + " AND tbcontaspagar.codfornecedor = "+ codFornecedor;
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
                return false;
            }
        }
    }
}