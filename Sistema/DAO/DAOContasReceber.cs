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
    public class DAOContasReceber : Sistema.DAO.DAO
    {

        public List<ContasReceber> GetContasPagar()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<ContasReceber>();

                while (reader.Read())
                {
                    var contaReceber = new ContasReceber
                    {
                        codigo = Convert.ToInt32(reader["ContaReceber_ID"]),
                        Cliente = new Select.Clientes.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_ID"]),
                            text = Convert.ToString(reader["Cliente_Nome"])
                        },
                        FormaPagamento = new Select.FormaPagamento.Select
                        {
                            id = Convert.ToInt32(reader["FormaPagamento_ID"]),
                            text = Convert.ToString(reader["FormaPagamento_Nome"])
                        },
                        nrParcela = Convert.ToInt16(reader["ContaReceber_NrParcela"]),
                        vlParcela = Convert.ToDecimal(reader["ContaReceber_Valor"]),
                        dtVencimento = Convert.ToDateTime(reader["ContaReceber_DataVencimento"]),
                        dtPagamento = !string.IsNullOrEmpty(reader["ContaReceber_DataPagamento"].ToString()) ? Convert.ToDateTime(reader["ContaReceber_DataPagamento"]) : (DateTime?)null,
                        situacao = Convert.ToString(reader["ContaReceber_Situacao"]),
                        txJuros = Convert.ToDecimal(reader["ContaReceber_Juros"]),
                        multa = Convert.ToDecimal(reader["ContaReceber_Multa"]),
                        desconto = Convert.ToDecimal(reader["ContaReceber_Desconto"]),
                    };
                    if (DateTime.Now.Date > contaReceber.dtVencimento.Date)
                    {
                        var dtBase = (DateTime.Now - contaReceber.dtVencimento).Days;
                        decimal txJusto = decimal.Round((contaReceber.txJuros * contaReceber.vlParcela) / 100, 2);
                        decimal multaDiaria = decimal.Round(((contaReceber.multa * contaReceber.vlParcela) / 100) * dtBase, 2);
                        contaReceber.vlParcela = contaReceber.vlParcela + multaDiaria + txJusto;
                    }
                    else
                    {
                        var txDesconto = decimal.Round((contaReceber.desconto * contaReceber.vlParcela) / 100, 2);
                        contaReceber.vlParcela = contaReceber.vlParcela - txDesconto;
                    }
                    list.Add(contaReceber);
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

        public ContasReceber GetContaPagar(int? codContaReceber)
        {
            try
            {
                var model = new Models.ContasReceber();
                if (codContaReceber != null)
                {
                    OpenConnection();
                    var sql = this.Search(codContaReceber, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["ContaReceber_ID"]);
                        model.codVenda = Convert.ToInt32(reader["Venda_ID"]);
                        model.Cliente = new Select.Clientes.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_ID"]),
                            text = Convert.ToString(reader["Cliente_Nome"])
                        };
                        model.FormaPagamento = new Select.FormaPagamento.Select
                        {
                            id = Convert.ToInt32(reader["FormaPagamento_ID"]),
                            text = Convert.ToString(reader["FormaPagamento_Nome"])
                        };
                        model.ContaContabil = new Select.ContasContabeis.Select
                        {
                            id = !string.IsNullOrEmpty(reader["ContaContabil_ID"].ToString()) ? Convert.ToInt32(reader["ContaContabil_ID"]) : (int?)null,
                            text = !string.IsNullOrEmpty(reader["ContaContabil_Nome"].ToString()) ? Convert.ToString(reader["ContaContabil_Nome"]) : string.Empty
                        };
                        model.nrParcela = Convert.ToInt16(reader["ContaReceber_NrParcela"]);
                        model.vlParcela = Convert.ToDecimal(reader["ContaReceber_Valor"]);
                        model.dtVencimento = Convert.ToDateTime(reader["ContaReceber_DataVencimento"]);
                        model.dtPagamento = !string.IsNullOrEmpty(reader["ContaReceber_DataPagamento"].ToString()) ? Convert.ToDateTime(reader["ContaReceber_DataPagamento"]) : (DateTime?)null;
                        model.situacao = (reader["ContaReceber_Situacao"].ToString() == "P" ? "PENDENTE" : "PAGA");
                        model.txJuros = Convert.ToDecimal(reader["ContaReceber_Juros"]);
                        model.multa = Convert.ToDecimal(reader["ContaReceber_Multa"]);
                        model.desconto = Convert.ToDecimal(reader["ContaReceber_Desconto"]);

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

        public void Receber(int id, Models.ContasReceber model)
        {
            var sql = "UPDATE tbcontasreceber set dtpagamento = " + this.FormatDate(DateTime.Now) + ", situacao = 'G', codconta = " + model.ContaContabil.id + " WHERE tbcontasreceber.codcontareceber = " + model.codigo;

            string lancamento = "O CLIENTE " + model.Cliente.id + " - " + this.FormatString(model.Cliente.text) + " REALIZOU O PAGAMENTO DA PARCELA Nº" + model.nrParcela + " REFERENTE A VENDA Nº " + model.codVenda + ".";

            var sqlLancamento = string.Format("INSERT INTO tblancamentos (codconta, dtmovimento, vllancamento, tipo, descricao) VALUES ({0}, {1}, {2}, '{3}', '{4}')",
                                model.ContaContabil.id,
                                this.FormatDate(DateTime.Now),
                                this.FormatDecimal(model.vlParcela),
                                "D",
                                this.FormatString(lancamento)
                );

            var sqlSaldoConta = "UPDATE tbcontascontabeis set vlsaldo += " + this.FormatDecimal(model.vlParcela) + " WHERE tbcontascontabeis.codconta = " + model.ContaContabil.id;
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

        private string Search(int? id, string filter)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE codcontareceber = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbclientes.nomerazaosocial LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                SELECT
	                tbcontasreceber.codcontareceber AS ContaReceber_ID,
                    tbcontasreceber.codvenda AS Venda_ID,
	                tbcontasreceber.nrparcela AS ContaReceber_NrParcela,
	                tbcontasreceber.vlparcela AS ContaReceber_Valor,
	                tbcontasreceber.dtvencimento AS ContaReceber_DataVencimento,
	                tbcontasreceber.situacao AS ContaReceber_Situacao,
	                tbcontasreceber.dtpagamento AS ContaReceber_DataPagamento,
	                tbcontasreceber.codforma AS FormaPagamento_ID,
	                tbformapagamento.nomeforma AS FormaPagamento_Nome,
	                tbcontasreceber.codvenda AS ContaReceber_Venda_ID,
	                tbcontasreceber.codcliente AS Cliente_ID,
	                tbclientes.nomerazaosocial AS Cliente_Nome,
                    tbcontasreceber.codconta AS ContaContabil_ID,
                    tbcontascontabeis.nomeconta AS ContaContabil_Nome,
					tbcontasreceber.juros AS ContaReceber_Juros,
					tbcontasreceber.multa AS ContaReceber_Multa,
					tbcontasreceber.desconto AS ContaReceber_Desconto
                FROM tbcontasreceber
                INNER JOIN tbformapagamento ON tbcontasreceber.codforma = tbformapagamento.codforma
                INNER JOIN tbclientes ON tbcontasreceber.codcliente = tbclientes.codcliente
                LEFT JOIN tbcontascontabeis ON tbcontasreceber.codconta = tbcontascontabeis.codconta"
                + swhere;
            return sql;
        }
    }
}