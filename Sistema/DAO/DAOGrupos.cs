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
    public class DAOGrupos : Sistema.DAO.DAO
    {
        public List<Grupos> GetGrupos()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Grupos>();

                while (reader.Read())
                {
                    var grupo = new Grupos
                    {
                        codigo = Convert.ToInt32(reader["Grupo_ID"]),
                        nomeGrupo = Convert.ToString(reader["Grupo_Nome"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["Grupo_Situacao"])),
                        observacao = Convert.ToString(reader["Grupo_Observacao"]),
                        dtCadastro = Convert.ToDateTime(reader["Grupo_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Grupo_DataUltAlteracao"]),
                    };
                    list.Add(grupo);

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

        public bool Insert(Models.Grupos grupo)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbgrupos ( nomegrupo, situacao, observacao, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    this.FormatString(grupo.nomeGrupo),
                    this.FormatString(grupo.situacao),
                    this.FormatString(grupo.observacao),
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

        public bool Update(Models.Grupos grupo)
        {
            try
            {
                string sql = "UPDATE tbgrupos SET nomegrupo = '"
                    + this.FormatString(grupo.nomeGrupo) + "'," +
                    " situacao = '" + this.FormatString(grupo.situacao) + "'," +
                    " observacao = '" + this.FormatString(grupo.observacao) + "'," +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "' WHERE tbgrupos.codgrupo = " + grupo.codigo;
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

        public Grupos GetGrupo(int? codGrupo)
        {
            try
            {
                var model = new Models.Grupos();
                if (codGrupo != null)
                {
                    OpenConnection();
                    var sql = this.Search(codGrupo, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Grupo_ID"]);
                        model.nomeGrupo = Convert.ToString(reader["Grupo_Nome"]);
                        model.situacao = Convert.ToString(reader["Grupo_Situacao"]);
                        model.observacao = Convert.ToString(reader["Grupo_Observacao"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Grupo_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Grupo_DataUltAlteracao"]);
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

        public bool Delete(int? codGrupo)
        {
            try
            {
                string sql = "DELETE FROM tbgrupos WHERE codgrupo = " + codGrupo;
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

        public List<Select.Grupos.Select> GetGruposSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Grupos.Select>();

                while (reader.Read())
                {
                    var grupo = new Select.Grupos.Select
                    {
                        id = Convert.ToInt32(reader["Grupo_ID"]),
                        text = Convert.ToString(reader["Grupo_Nome"]),
                    };

                    list.Add(grupo);
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
                swhere = " WHERE codgrupo = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbgrupos.nomegrupo LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
                        tbgrupos.codgrupo AS Grupo_ID,
                        tbgrupos.nomegrupo AS Grupo_Nome,
                        tbgrupos.situacao AS Grupo_Situacao,
                        tbgrupos.observacao AS Grupo_Observacao,
                        tbgrupos.dtcadastro AS Grupo_DataCadastro,
                        tbgrupos.dtultalteracao AS Grupo_DataUltAlteracao
                    FROM tbgrupos" + swhere;
            return sql;
        }


    }
}