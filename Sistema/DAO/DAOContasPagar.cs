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
    public class DAOContasPagar : Sistema.DAO.DAO
    {

        public List<ContasPagar> GetContasPagar()
        {
            try
            {
                var sql = this.Search(null, null, null, null, null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<ContasPagar>();

                while (reader.Read())
                {
                    var contaPagar = new ContasPagar
                    {
                        Fornecedor = new Select.Fornecedores.Select
                        {
                            id = Convert.ToInt32(reader["ContaPagar_Fornecedor_ID"]),
                            text = Convert.ToString(reader["ContaPagar_Fornecedor_Nome"])
                        },
                        FormaPagamento = new Select.FormaPagamento.Select
                        {
                            id = Convert.ToInt32(reader["ContaPagar_FormaPagamento_ID"]),
                            text = Convert.ToString(reader["ContaPagar_FormaPagamento_Nome"])
                        },
                        nrParcela = Convert.ToInt16(reader["ContaPagar_NrParcela"]),
                        vlParcela = Convert.ToDecimal(reader["ContaPagar_vlParcela"]),
                        dtVencimento = Convert.ToDateTime(reader["ContaPagar_DataVencimento"]),
                        dtPagamento = !string.IsNullOrEmpty(reader["ContaPagar_DataPagamento"].ToString()) ? Convert.ToDateTime(reader["ContaPagar_DataPagamento"]) : (DateTime?)null,
                        situacao = Convert.ToString(reader["ContaPagar_Situacao"]),
                        modelo = Convert.ToString(reader["ContaPagar_Modelo"]),
                        serie = Convert.ToString(reader["ContaPagar_Serie"]),
                        numero = Convert.ToInt32(reader["ContaPagar_Numero"]),
                        txJuros = Convert.ToDecimal(reader["ContaPagar_Juros"]),
                        multa = Convert.ToDecimal(reader["ContaPagar_Multa"]),
                        desconto = Convert.ToDecimal(reader["ContaPagar_Desconto"])
                    };

                    if (DateTime.Now.Date > contaPagar.dtVencimento.Date)
                    {
                        var dtBase = (DateTime.Now - contaPagar.dtVencimento).Days;
                        decimal txJusto = decimal.Round((contaPagar.txJuros * contaPagar.vlParcela) / 100, 2);
                        decimal multaDiaria = decimal.Round(((contaPagar.multa * contaPagar.vlParcela) / 100) * dtBase, 2);
                        contaPagar.vlParcela = contaPagar.vlParcela + multaDiaria + txJusto;
                    }
                    else
                    {
                        var txDesconto = decimal.Round((contaPagar.desconto * contaPagar.vlParcela) / 100, 2);
                        contaPagar.vlParcela = contaPagar.vlParcela - txDesconto;
                    }
                    list.Add(contaPagar);
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

        public ContasPagar GetContaPagar(short nrParcela, string modelo, string serie, int numero, int codFornecedor)
        {
            try
            {
                var model = new Models.ContasPagar();
                OpenConnection();
                var sql = this.Search(null, modelo, serie, numero, codFornecedor, nrParcela);
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                while (reader.Read())
                {
                    model.Fornecedor = new Select.Fornecedores.Select
                    {
                        id = Convert.ToInt32(reader["ContaPagar_Fornecedor_ID"]),
                        text = Convert.ToString(reader["ContaPagar_Fornecedor_Nome"])
                    };
                    model.FormaPagamento = new Select.FormaPagamento.Select
                    {
                        id = Convert.ToInt32(reader["ContaPagar_FormaPagamento_ID"]),
                        text = Convert.ToString(reader["ContaPagar_FormaPagamento_Nome"])
                    };
                    model.nrParcela = Convert.ToInt16(reader["ContaPagar_NrParcela"]);
                    model.vlParcela = Convert.ToDecimal(reader["ContaPagar_vlParcela"]);
                    model.dtVencimento = Convert.ToDateTime(reader["ContaPagar_DataVencimento"]);
                    model.dtPagamento = !string.IsNullOrEmpty(reader["ContaPagar_DataPagamento"].ToString()) ? Convert.ToDateTime(reader["ContaPagar_DataPagamento"]) : (DateTime?)null;
                    model.situacao = Convert.ToString(reader["ContaPagar_Situacao"]) == "P" ? "PENDENTE" : "PAGA";
                    model.modelo = Convert.ToString(reader["ContaPagar_Modelo"]);
                    model.serie = Convert.ToString(reader["ContaPagar_Serie"]);
                    model.numero = Convert.ToInt32(reader["ContaPagar_Numero"]);
                    model.ContaContabil = new Select.ContasContabeis.Select
                    {
                        id = !string.IsNullOrEmpty(reader["ContaContabil_ID"].ToString()) ? Convert.ToInt32(reader["ContaContabil_ID"]) : (int?)null,
                        text = !string.IsNullOrEmpty(reader["ContaContabil_Nome"].ToString()) ? Convert.ToString(reader["ContaContabil_Nome"]) : string.Empty
                    };
                    model.txJuros = Convert.ToDecimal(reader["ContaPagar_Juros"]);
                    model.multa = Convert.ToDecimal(reader["ContaPagar_Multa"]);
                    model.desconto = Convert.ToDecimal(reader["ContaPagar_Desconto"]);

                    if (DateTime.Now.Date > model.dtVencimento.Date)
                    {
                        var dtBase = (DateTime.Now - model.dtVencimento).Days;
                        decimal txJusto = decimal.Round((model.txJuros * model.vlParcela) / 100, 2);
                        decimal multaDiaria = decimal.Round(((model.multa * model.vlParcela) / 100) * dtBase, 2);
                        model.vlParcela = model.vlParcela + multaDiaria + txJusto;
                    }
                    else
                    {
                        var txDesconto = decimal.Round((model.desconto * model.vlParcela) / 100, 2);
                        model.vlParcela = model.vlParcela - txDesconto;
                    }
                }
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

        public void Pagar(string modelo, string serie, int numero, int codfornecedor, short nrparcela, Models.ContasPagar model)
        {
            var swhere = " WHERE tbcontaspagar.modelo = '" + modelo + "' AND tbcontaspagar.serie = '" + serie + "' AND tbcontaspagar.numero = " + numero + " AND tbcontaspagar.codfornecedor = " + codfornecedor + " AND tbcontaspagar.nrparcela = " + nrparcela;
            var sql = "UPDATE tbcontaspagar set dtpagamento = " + this.FormatDate(DateTime.Now) + ", situacao = 'G', codconta = " + model.ContaContabil.id + swhere;

            string lancamento = "PAGAMENTO DO FORNECEDOR " + model.Fornecedor.id + " - " + this.FormatString(model.Fornecedor.text) + ", NOTA FISCAL Nº " + model.numero + ", PARCELA Nº " + model.nrParcela + ".";

            var sqlLancamento = string.Format("INSERT INTO tblancamentos (codconta, dtmovimento, vllancamento, tipo, descricao) VALUES ({0}, {1}, {2}, '{3}', '{4}')",
                                model.ContaContabil.id,
                                this.FormatDate(DateTime.Now),
                                this.FormatDecimal(model.vlParcela),
                                "D",
                                this.FormatString(lancamento)
                );

            var sqlSaldoConta = "UPDATE tbcontascontabeis set vlsaldo -= " + this.FormatDecimal(model.vlParcela) + " WHERE tbcontascontabeis.codconta = " + model.ContaContabil.id;
            using (con)
            {
                OpenConnection();
                SqlTransaction trans = con.BeginTransaction();
                SqlCommand command = con.CreateCommand();
                try
                {
                    command.Transaction = trans;

                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                    command.CommandText = sqlLancamento;
                    command.ExecuteNonQuery();

                    command.CommandText = sqlSaldoConta;
                    command.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public void Cancelar(int codContaPagar)
        {
            var sql = "UPDATE tbcontaspagar set dtpagamento = null, situacao = 'P' WHERE codcontapagar = " + codContaPagar;
            using (con)
            {
                OpenConnection();
                SqlTransaction trans = con.BeginTransaction();
                SqlCommand command = con.CreateCommand();
                try
                {
                    command.CommandText = sql;
                    command.Transaction = trans;
                    command.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private string Search(string filter, string modelo, string serie, int? numero, int? codFornecedor, short? nrParcela)
        {
            var sql = string.Empty;
            var swhere = string.Empty;

            if (!string.IsNullOrEmpty(modelo))
            {
                swhere += " AND tbcontaspagar.modelo = '" + modelo + "'";
            }
            if (!string.IsNullOrEmpty(serie))
            {
                swhere += " AND tbcontaspagar.serie = '" + serie + "'";
            }
            if (numero != null)
            {
                swhere += " AND tbcontaspagar.codfornecedor = " + codFornecedor;
            }
            if (nrParcela != null)
            {
                swhere += " AND tbcontaspagar.nrparcela = " + nrParcela;
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
	                tbcontaspagar.codfornecedor AS ContaPagar_Fornecedor_ID,
	                tbfornecedores.nomerazaosocial AS ContaPagar_Fornecedor_Nome,
	                tbcontaspagar.codforma AS ContaPagar_FormaPagamento_ID,
	                tbformapagamento.nomeforma AS ContaPagar_FormaPagamento_Nome,
	                tbcontaspagar.nrparcela AS ContaPagar_NrParcela,
	                tbcontaspagar.vlparcela AS ContaPagar_vlParcela,
	                tbcontaspagar.dtvencimento AS ContaPagar_DataVencimento,
	                tbcontaspagar.dtpagamento AS ContaPagar_DataPagamento,
	                tbcontaspagar.situacao AS ContaPagar_Situacao,
	                tbcontaspagar.modelo AS ContaPagar_Modelo,
	                tbcontaspagar.serie AS ContaPagar_Serie,
	                tbcontaspagar.numero AS ContaPagar_Numero,
	                tbcontaspagar.codconta AS ContaContabil_ID,
	                tbcontascontabeis.nomeconta AS ContaContabil_Nome,
	                tbcontaspagar.juros AS ContaPagar_Juros,
	                tbcontaspagar.multa AS ContaPagar_Multa,
	                tbcontaspagar.desconto AS ContaPagar_Desconto
                FROM tbcontaspagar
                INNER JOIN tbfornecedores ON tbcontaspagar.codfornecedor = tbfornecedores.codfornecedor
                INNER JOIN tbformapagamento ON tbcontaspagar.codforma = tbformapagamento.codforma
                LEFT JOIN tbcontascontabeis ON tbcontaspagar.codconta = tbcontascontabeis.codconta"
                + swhere;
            return sql;
        }
    }
}