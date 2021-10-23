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
                        
                    };
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
                        model.nrParcela = Convert.ToInt16(reader["ContaReceber_NrParcela"]);
                        model.vlParcela = Convert.ToDecimal(reader["ContaReceber_Valor"]);
                        model.dtVencimento = Convert.ToDateTime(reader["ContaReceber_DataVencimento"]);
                        model.dtPagamento = !string.IsNullOrEmpty(reader["ContaReceber_DataPagamento"].ToString()) ? Convert.ToDateTime(reader["ContaReceber_DataPagamento"]) : (DateTime?)null;
                        model.situacao = Convert.ToString(reader["ContaReceber_Situacao"]);
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
	                tbcontasreceber.nrparcela AS ContaReceber_NrParcela,
	                tbcontasreceber.vlparcela AS ContaReceber_Valor,
	                tbcontasreceber.dtvencimento AS ContaReceber_DataVencimento,
	                tbcontasreceber.situacao AS ContaReceber_Situacao,
	                tbcontasreceber.dtpagamento AS ContaReceber_DataPagamento,
	                tbcontasreceber.codforma AS FormaPagamento_ID,
	                tbformapagamento.nomeforma AS FormaPagamento_Nome,
	                tbcontasreceber.codvendaproduto AS ContaReceber_VendaProduto_ID,
	                tbcontasreceber.codvendaservico AS ContaReceber_VendaServico_ID,
	                tbcontasreceber.codcliente AS Cliente_ID,
	                tbclientes.nomerazaosocial AS Cliente_Nome
                FROM tbcontasreceber
                INNER JOIN tbformapagamento ON tbcontasreceber.codforma = tbformapagamento.codforma
                INNER JOIN tbclientes ON tbcontasreceber.codcliente = tbclientes.codcliente"
                + swhere;
            return sql;
        }
    }
}