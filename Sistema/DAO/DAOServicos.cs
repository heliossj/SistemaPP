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
    public class DAOServicos : Sistema.DAO.DAO
    {

        public List<Servicos> GetServicos()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Servicos>();

                while (reader.Read())
                {
                    var servico = new Servicos
                    {
                        codigo = Convert.ToInt32(reader["Servico_ID"]),
                        nomeServico = Convert.ToString(reader["Servico_Nome"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["Servico_Situacao"])),
                        vlServico = Convert.ToDecimal(reader["Servico_Valor"]),
                        dtCadastro = Convert.ToDateTime(reader["Servico_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Servicos_DataUltAlteracao"]),
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

        public bool Insert(Models.Servicos servico)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbservicos ( nomeservico, descricao, vlservico, dtcadastro, dtultalteracao, situacao) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}')",
                    servico.nomeServico.ToUpper().Trim(),
                    !string.IsNullOrEmpty(servico.descricao) ? servico.descricao.ToUpper().Trim() : "",
                    servico.vlServico.ToString().Replace(",", "."),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    servico.situacao.ToUpper().Trim()
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

        public bool Update(Models.Servicos servicos)
        {
            try
            {
                string sql = "UPDATE tbservicos SET nomeservico = '"
                    + servicos.nomeServico.ToUpper().Trim() + "'," +
                    " descricao = '" + (!string.IsNullOrEmpty(servicos.descricao) ? servicos.descricao.ToUpper().Trim() : "") + "'," +
                    " vlservico = " + servicos.vlServico.ToString().Replace(",", ".") + ", "+
                    " situacao = '" + servicos.situacao.ToUpper().Trim() + "'," +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "' WHERE codservico = " + servicos.codigo;
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

        public Servicos GetServico(int? codServico)
        {
            try
            {
                var model = new Models.Servicos();
                if (codServico != null)
                {
                    OpenConnection();
                    var sql = this.Search(codServico, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Servico_ID"]);
                        model.nomeServico = Convert.ToString(reader["Servico_Nome"]);
                        model.descricao = Convert.ToString(reader["Servico_Descricao"]);
                        model.vlServico = Convert.ToDecimal(reader["Servico_Valor"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Servico_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Servicos_DataUltAlteracao"]);
                        model.situacao = Convert.ToString(reader["Servico_Situacao"]);
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

        public bool Delete(int? codServico)
        {
            try
            {
                string sql = "DELETE FROM tbservicos WHERE codservico = " + codServico;
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

        public List<Select.Servicos.Select> GetServicosSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Servicos.Select>();

                while (reader.Read())
                {
                    var servico = new Select.Servicos.Select
                    {
                        id = Convert.ToInt32(reader["Servico_ID"]),
                        text = Convert.ToString(reader["Servico_Nome"]),
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

        private string Search(int? id, string filter)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE codservico = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbservicos.nomeservico LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
                        tbservicos.codservico AS Servico_ID,
                        tbservicos.nomeservico AS Servico_Nome,
                        tbservicos.situacao AS Servico_Situacao,
                        tbservicos.vlservico AS Servico_Valor,
                        tbservicos.descricao AS Servico_Descricao,
                        tbservicos.dtcadastro AS Servico_DataCadastro,
                        tbservicos.dtultalteracao AS Servicos_DataUltAlteracao
                    FROM tbservicos" + swhere;
            return sql;
        }


    }
}