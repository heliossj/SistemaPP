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
    public class DAOFormaPagamento : Sistema.DAO.DAO
    {
        public List<FormaPagamento> GetFormasPagamentos()
        {
            try
            {
                var sql = this.Search(null, null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<FormaPagamento>();

                while (reader.Read())
                {
                    var formaPagamento = new FormaPagamento
                    {
                        codigo = Convert.ToInt32(reader["FormaPagamento_ID"]),
                        nomeForma = Convert.ToString(reader["FormaPagamento_Nome"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["FormaPagamento_Situacao"])),
                        dtCadastro = Convert.ToDateTime(reader["FormaPagamento_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["FormaPagamento_DataUltAlteracao"]),
                    };
                    list.Add(formaPagamento);

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

       

        public bool Insert(Models.FormaPagamento formaPagamento)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbformapagamento ( nomeforma, situacao, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', '{2}', '{3}')",
                    this.FormatString(formaPagamento.nomeForma),
                    this.FormatString(formaPagamento.situacao),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd")
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

        public bool Update(Models.FormaPagamento formaPagamento)
        {
            try
            {
                string sql = "UPDATE tbformapagamento SET nomeforma = '"
                    + this.FormatString(formaPagamento.nomeForma) + "'," +
                    " situacao = '" + this.FormatString(formaPagamento.situacao) + "'," +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "' WHERE tbformapagamento.codforma = " + formaPagamento.codigo;
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

        public FormaPagamento GetFormaPagamento(int? codForma)
        {
            try
            {
                var model = new Models.FormaPagamento();
                if (codForma != null)
                {
                    OpenConnection();
                    var sql = this.Search(codForma, null, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["FormaPagamento_ID"]);
                        model.nomeForma = Convert.ToString(reader["FormaPagamento_Nome"]);
                        model.situacao = Convert.ToString(reader["FormaPagamento_Situacao"]);
                        model.dtCadastro = Convert.ToDateTime(reader["FormaPagamento_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["FormaPagamento_DataUltAlteracao"]);
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

        public bool Delete(int? codForma)
        {
            try
            {
                string sql = "DELETE FROM tbformapagamento WHERE codforma = " + codForma;
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

        public List<Select.FormaPagamento.Select> GetFormasPagamentosSelect(int? id, string filter)
        {
            try
            {
                var sql = this.Search(id, filter, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.FormaPagamento.Select>();

                while (reader.Read())
                {
                    var formaPagamento = new Select.FormaPagamento.Select
                    {
                        id = Convert.ToInt32(reader["FormaPagamento_ID"]),
                        text = Convert.ToString(reader["FormaPagamento_Nome"]),
                    };

                    list.Add(formaPagamento);
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

        private string Search(int? id, string filter, string[] flSituacao)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE codforma = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbformapagamento.nomeforma LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            //if (flSituacao != null && flSituacao.Any())
            //{
            //    swhere += "AND tbformapagamento IN ('" + string.Join("','", flSituacao) + "')";
            //}
            sql = @"
                    SELECT
                        tbformapagamento.codforma AS FormaPagamento_ID,
                        tbformapagamento.nomeforma AS FormaPagamento_Nome,
                        tbformapagamento.situacao AS FormaPagamento_Situacao,
                        tbformapagamento.dtcadastro AS FormaPagamento_DataCadastro,
                        tbformapagamento.dtultalteracao AS FormaPagamento_DataUltAlteracao
                    FROM tbformapagamento" + swhere;
            return sql;
        }


    }
}