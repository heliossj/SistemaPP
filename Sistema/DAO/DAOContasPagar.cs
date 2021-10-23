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
                        numero = Convert.ToInt32(reader["ContaPagar_Numero"])
                    };
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
                        model.situacao = Convert.ToString(reader["ContaPagar_Situacao"]);
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

        public void Pagar(string modelo, string serie, int numero, int codfornecedor, short nrparcela)
        {
            var swhere = " WHERE tbcontaspagar.modelo = '" + modelo + "' AND tbcontaspagar.serie = '" + serie + "' AND tbcontaspagar.numero = " + numero + " AND tbcontaspagar.codfornecedor = " + codfornecedor + " AND tbcontaspagar.nrparcela = " + nrparcela;
            var sql = "UPDATE tbcontaspagar set dtpagamento = " + this.FormatDate(DateTime.Now) + ", situacao = 'G'" + swhere;
            OpenConnection();
            SqlQuery = new SqlCommand(sql, con);
            SqlQuery.ExecuteNonQuery();
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
	                tbcontaspagar.numero AS ContaPagar_Numero
                FROM tbcontaspagar
                INNER JOIN tbfornecedores ON tbcontaspagar.codfornecedor = tbfornecedores.codfornecedor
                INNER JOIN tbformapagamento ON tbcontaspagar.codforma = tbformapagamento.codforma"
                + swhere;
            return sql;
        }
    }
}