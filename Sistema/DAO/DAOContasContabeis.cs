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
    public class DAOContasContabeis : Sistema.DAO.DAO
    {

        public List<ContasContabeis> GetContasContabeis()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<ContasContabeis>();
                while (reader.Read())
                {
                    var servico = new ContasContabeis
                    {
                        codigo = Convert.ToInt32(reader["ContasContabeis_ID"]),
                        nomeConta = Convert.ToString(reader["ContasContabeis_Nome"]),
                        classificacao = Convert.ToString(reader["ContasContabeis_Classificacao"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["ContasContabeis_Situacao"])),
                        vlSaldo = Convert.ToDecimal(reader["ContasContabeis_Saldo"]),
                        dtCadastro = Convert.ToDateTime(reader["ContasContabeis_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["ContasContabeis_DataUltAlteracao"]),
                    };
                    list.Add(servico);
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

        public bool Insert(Models.ContasContabeis contaContabil)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbcontascontabeis ( nomeconta, classificacao, vlsaldo, situacao, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', {2}, '{3}', {4}, {5})",
                    this.FormatString(contaContabil.nomeConta),
                    !string.IsNullOrEmpty(contaContabil.classificacao) ? this.FormatString(contaContabil.classificacao) : "",
                    0,
                    this.FormatString(contaContabil.situacao),
                    this.FormatDate(DateTime.Now),
                    this.FormatDate(DateTime.Now)
                    );
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                int i = SqlQuery.ExecuteNonQuery();

                if (i > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public bool Update(Models.ContasContabeis contaContabil)
        {
            try
            {
                string sql = "UPDATE tbcontascontabeis SET nomeconta = '"
                    + this.FormatString(contaContabil.nomeConta) + "'," +
                    " classificacao = '" + (!string.IsNullOrEmpty(contaContabil.classificacao) ? this.FormatString(contaContabil.classificacao) : "") + "'," +
                    " situacao = '" + this.FormatString(contaContabil.situacao) + "'," +
                    " dtultalteracao = " + this.FormatDate(DateTime.Now) +
                    " WHERE codconta = " + contaContabil.codigo;
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);

                int i = SqlQuery.ExecuteNonQuery();

                if (i > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public ContasContabeis GetContaContabil(int? codConta)
        {
            try
            {
                var model = new Models.ContasContabeis();
                if (codConta != null)
                {
                    OpenConnection();
                    var sql = this.Search(codConta, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["ContasContabeis_ID"]);
                        model.nomeConta = Convert.ToString(reader["ContasContabeis_Nome"]);
                        model.classificacao = Convert.ToString(reader["ContasContabeis_Classificacao"]);
                        model.vlSaldo = Convert.ToDecimal(reader["ContasContabeis_Saldo"]);
                        model.situacao = Convert.ToString(reader["ContasContabeis_Situacao"]);
                        model.dtCadastro = Convert.ToDateTime(reader["ContasContabeis_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["ContasContabeis_DataUltAlteracao"]);
                        
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

        public bool Delete(int? codConta)
        {
            try
            {
                string sql = "DELETE FROM tbcontascontabeis WHERE codconta = " + codConta;
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);

                int i = SqlQuery.ExecuteNonQuery();

                if (i > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public List<Select.ContasContabeis.Select> GetContaContabilSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.ContasContabeis.Select>();

                while (reader.Read())
                {
                    var contaContabil = new Select.ContasContabeis.Select
                    {
                        id = Convert.ToInt32(reader["ContasContabeis_ID"]),
                        text = Convert.ToString(reader["ContasContabeis_Nome"]),
                        classificacao = Convert.ToString(reader["ContasContabeis_Classificacao"]),
                        vlSaldo = Convert.ToDecimal(reader["ContasContabeis_Saldo"]),
                        situacao = Convert.ToString(reader["ContasContabeis_Situacao"]),
                        dtCadastro = Convert.ToDateTime(reader["ContasContabeis_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["ContasContabeis_DataUltAlteracao"])
                    };
                    list.Add(contaContabil);
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

        private string Search(int? id, string filter)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE codconta = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbcontascontabeis.nomeconta LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
	                    tbcontascontabeis.codconta AS ContasContabeis_ID,
	                    tbcontascontabeis.nomeconta AS ContasContabeis_Nome,
	                    tbcontascontabeis.classificacao AS ContasContabeis_Classificacao,
	                    tbcontascontabeis.vlSaldo AS ContasContabeis_Saldo,
	                    tbcontascontabeis.situacao AS ContasContabeis_Situacao,
	                    tbcontascontabeis.dtcadastro AS ContasContabeis_DataCadastro,
	                    tbcontascontabeis.dtultalteracao AS ContasContabeis_DataUltAlteracao
                    FROM tbcontascontabeis" + swhere;
            return sql;
        }


    }
}