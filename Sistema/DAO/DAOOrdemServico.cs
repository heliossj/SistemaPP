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
    public class DAOOrdemServico : Sistema.DAO.DAO
    {

        public List<OrdemServico> GetOrdemServicos()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<OrdemServico>();

                while (reader.Read())
                {
                    var OS = new OrdemServico
                    {
                        codigo = Convert.ToInt32(reader["OrdemServico_ID"]),
                        situacao = Convert.ToString(reader["OrdemServico_Situacao"]),
                        dtAbertura = Convert.ToDateTime(reader["OrdemServico_DataAbertura"]),
                        dtValidade = Convert.ToDateTime(reader["OrdemServico_DataValidade"]),
                        observacao = Convert.ToString(reader["OrdemServco_Observacao"]),
                        Cliente = new Select.Clientes.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_ID"]),
                            text = Convert.ToString(reader["Cliente_Nome"])
                        },
                        Funcionario = new Select.Funcionarios.Select
                        {
                            id = Convert.ToInt32(reader["Funcionario_ID"]),
                            text = Convert.ToString(reader["Funcionario_Nome"])
                        }
                    };
                    list.Add(OS);
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

        public bool Insert(Models.OrdemServico OS)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbordemservicos ( situacao, dtabertura, dtvalidade, codcliente, codcondpagamento, observacao, codfuncionario, dtultalteracao, dtexecucao ) VALUES ('{0}', {1}, {2}, {3}, {4}, '{5}', {6}, {7}, {8} ); SELECT SCOPE_IDENTITY()",
                    OS.situacao,
                    this.FormatDateTime(DateTime.Now),
                    this.FormatDate(OS.dtValidade),
                    OS.Cliente.id,
                    OS.CondicaoPagamento.id,
                    this.FormatString(OS.observacao),
                    OS.Funcionario.id,
                    this.FormatDate(DateTime.Now),
                    this.FormatDate(OS.dtExecucao)
                    );
                string sqlServico = "INSERT INTO tbservicosos ( codordemservico, codservico, unidade, qtservico, vlservico, codexecutante) VALUES ({0}, {1}, '{2}', {3}, {4}, {5})";
                string sqlProduto = "INSERT INTO tbprodutosos ( codordemservico, codproduto, unidade, qtproduto, vlproduto) VALUES ({0}, {1}, '{2}', {3}, {4})";
                string sqlParcela = "INSERT INTO tbcontasreceber (codfornecedor, codforma, nrparcela, vlparcela, dtvencimento, situacao, codcompra) VALUES ({0}, {1}, {2}, {3}, {4}, '{5}', {6} )";
                using (con)
                {
                    OpenConnection();

                    SqlTransaction sqlTrans = con.BeginTransaction();
                    SqlCommand command = con.CreateCommand();
                    command.Transaction = sqlTrans;
                    try
                    {
                        command.CommandText = sql;
                        var codOS = Convert.ToInt32(command.ExecuteScalar());
                        foreach (var item in OS.ServicosOS)
                        {
                            var Servico = string.Format(sqlServico, codOS, item.codServico, this.FormatString(item.unidade), this.FormatDecimal(item.qtServico), this.FormatDecimal(item.vlServico), item.codExecutante);
                            command.CommandText = Servico;
                            command.ExecuteNonQuery();
                        }

                        foreach (var item in OS.ProdutosOS)
                        {
                            var produto = string.Format(sqlProduto, codOS, item.codProduto, this.FormatString(item.unidade), this.FormatDecimal(item.qtProduto), this.FormatDecimal(item.vlProduto));
                            command.CommandText = produto;
                            command.ExecuteNonQuery();
                        }

                        //se a situação for autorizada, aí gera as vendas e insere...
                        //if (OS.situacao == "T")

                        //foreach (var item in OS.ParcelasOS)
                        //{
                        //    var parcela = string.Format(sqlParcela, compra.Fornecedor.id, item.idFormaPagamento, item.nrParcela, this.FormatDecimal(item.vlParcela), this.FormatDate(item.dtVencimento), "P", codCompra);
                        //    command.CommandText = parcela;
                        //    command.ExecuteNonQuery();
                        //}

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
            throw new Exception("Não implementado");
        }

        public OrdemServico GetOrdemServico(int codOrdemServico)
        {
            try
            {
                var model = new Models.OrdemServico();
                OpenConnection();
                var sql = this.Search(codOrdemServico, null);
                var sqlServicos = this.SearchServicos(codOrdemServico);
                var sqlProdutos = this.SearchProdutos(codOrdemServico);
                var sqlParcelas = this.SearchParcelas(codOrdemServico);
                //var sqlParcelas = this.SearchParcelas(codOrdemServico);
                var listServicos = new List<OrdemServico.ServicosVM>();
                var listProdutos = new List<OrdemServico.ProdutosVM>();
                var listParcelas = new List<Shared.ParcelasVM>();

                //SqlQuery = new SqlCommand(sql + sqlProdutos + sqlParcelas, con);
                SqlQuery = new SqlCommand(sql + sqlServicos + sqlProdutos, con);
                reader = SqlQuery.ExecuteReader();
                while (reader.Read())
                {
                    model.codigo = Convert.ToInt32(reader["OrdemServico_ID"]);
                    model.situacao = Convert.ToString(reader["OrdemServico_Situacao"]);
                    model.dtAbertura = Convert.ToDateTime(reader["OrdemServico_DataAbertura"]);
                    model.dtValidade = Convert.ToDateTime(reader["OrdemServico_DataValidade"]);
                    model.observacao = Convert.ToString(reader["OrdemServco_Observacao"]);
                    model.dtUltAlteracao = Convert.ToDateTime(reader["OrdemServico_DataUltAlteracao"]);
                    model.dtExecucao = Convert.ToDateTime(reader["OrdemServico_DataExecucao"]);
                    model.Cliente = new Select.Clientes.Select
                    {
                        id = Convert.ToInt32(reader["Cliente_ID"]),
                        text = Convert.ToString(reader["Cliente_Nome"])
                    };
                    model.Funcionario = new Select.Funcionarios.Select
                    {
                        id = Convert.ToInt32(reader["Funcionario_ID"]),
                        text = Convert.ToString(reader["Funcionario_Nome"])
                    };
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
                        var servico = new OrdemServico.ServicosVM
                        {
                            codServico = Convert.ToInt32(reader["Servico_ID"]),
                            nomeServico = Convert.ToString(reader["Servico_Nome"]),
                            unidade = Convert.ToString(reader["Servico_Unidade"]),
                            qtServico = Convert.ToDecimal(reader["Servico_Quantidade"]),
                            vlServico = Convert.ToDecimal(reader["Servico_Valor"]),
                            codExecutante = Convert.ToInt32(reader["Servico_Executante_ID"]),
                            nomeExecutante = Convert.ToString(reader["Servico_Executante_Nome"])
                        };
                        listServicos.Add(servico);
                    }
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var produto = new OrdemServico.ProdutosVM
                        {
                            codProduto = Convert.ToInt32(reader["Produto_ID"]),
                            nomeProduto = Convert.ToString(reader["Produto_Nome"]),
                            unidade = Convert.ToString(reader["Produto_Unidade"]),
                            qtProduto = Convert.ToDecimal(reader["Produto_Quantidade"]),
                            vlProduto = Convert.ToDecimal(reader["Produto_Valor"])
                        };
                        listProdutos.Add(produto);
                    }
                }

                //if (reader.NextResult())
                //{
                //    while (reader.Read())
                //    {
                //        var parcela = new Shared.ParcelasVM
                //        {
                //            idFormaPagamento = Convert.ToInt32(reader["FormaPagamento_ID"]),
                //            nmFormaPagamento = Convert.ToString(reader["FormaPagamento_Nome"]),
                //            nrParcela = Convert.ToDouble(reader["ContaPagar_NrParcela"]),
                //            vlParcela = Convert.ToDecimal(reader["ContaPagar_VlParcela"]),
                //            dtVencimento = Convert.ToDateTime(reader["ContaPagar_DataVencimento"]),
                //            situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["ContaPagar_Situacao"]))
                //        };
                //        listParcelas.Add(parcela);
                //    }
                //}

                model.ServicosOS = listServicos;
                model.ProdutosOS = listProdutos;
                //model.ParcelasOS = listParcelas;
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
                swhere = " WHERE tbordemservicos.codordemservico = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbfuncionarios.nomefuncionario LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
            SELECT
                tbordemservicos.codordemservico AS OrdemServico_ID,
                tbordemservicos.situacao AS OrdemServico_Situacao,
                tbordemservicos.dtabertura AS OrdemServico_DataAbertura,
                tbordemservicos.dtvalidade AS OrdemServico_DataValidade,
                tbordemservicos.observacao AS OrdemServco_Observacao,
                tbordemservicos.dtultalteracao AS OrdemServico_DataUltAlteracao,
                tbordemservicos.dtexecucao AS OrdemServico_DataExecucao,
                tbordemservicos.codcliente AS Cliente_ID,
                tbclientes.nomerazaosocial AS Cliente_Nome,
                tbordemservicos.codfuncionario AS Funcionario_ID,
                tbfuncionarios.nomefuncionario AS Funcionario_Nome,
                tbordemservicos.codcondpagamento AS CondicaoPagamento_ID,
                tbcondpagamentos.nomecondicao AS CondicaoPagamento_Nome
                FROM tbordemservicos
            INNER JOIN tbclientes ON tbordemservicos.codcliente = tbclientes.codcliente
            INNER JOIN tbfuncionarios ON tbordemservicos.codfuncionario = tbfuncionarios.codfuncionario
            INNER JOIN tbcondpagamentos ON tbordemservicos.codcondpagamento = tbcondpagamentos.codcondicao
            " + swhere + ";";
            return sql;
        }

        private string SearchServicos(int? id)
        {
            var sql = string.Empty;
            sql = @"
                    SELECT
	                    tbservicosos.codordemservico AS OrdemServico_ID,
	                    tbservicosos.codservico AS Servico_ID,
	                    tbservicos.nomeservico AS Servico_Nome,
	                    tbservicosos.unidade AS Servico_Unidade,
	                    tbservicosos.qtservico AS Servico_Quantidade,
	                    tbservicosos.vlservico AS Servico_Valor,
	                    tbservicosos.codexecutante AS Servico_Executante_ID,
	                    tbfuncionarios.nomefuncionario AS Servico_Executante_Nome
                    FROM tbservicosos
                    INNER JOIN tbservicos ON tbservicosos.codservico = tbservicos.codservico
                    INNER JOIN tbfuncionarios ON tbservicosos.codexecutante = tbfuncionarios.codfuncionario
                    WHERE tbservicosos.codordemservico = " + id + ";"
            ;
            return sql;
        }

        private string SearchProdutos(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
	                    tbprodutosos.codordemservico AS OrdemServico_ID,
	                    tbprodutosos.codproduto AS Produto_ID,
	                    tbprodutos.nomeproduto AS Produto_Nome,
	                    tbprodutosos.unidade AS Produto_Unidade,
	                    tbprodutosos.qtproduto AS Produto_Quantidade,
	                    tbprodutosos.vlproduto AS Produto_Valor
                    FROM tbprodutosos
                    INNER JOIN tbprodutos ON tbprodutosos.codproduto = tbprodutos.codproduto
                    WHERE tbprodutosos.codordemservico = " + id + ";"
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
                        tbcontaspagar.situacao AS ContaPagar_Situacao
                    FROM tbcontaspagar
                    INNER JOIN tbformapagamento on tbcontaspagar.codforma = tbformapagamento.codforma
                    WHERE tbcontaspagar.codcompra = " + id
            ;
            return sql;
        }
    }
}