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
    public class DAOEstados : Sistema.DAO.DAO
    {

        public List<Estados> GetEstados()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Estados>();

                while (reader.Read())
                {
                    var estado = new Estados
                    {
                        codEstado = Convert.ToInt32(reader["Estado_ID"]),
                        nomeEstado = Convert.ToString(reader["Estado_Nome"]),
                        uf = Convert.ToString(reader["Estado_UF"]),
                        Pais = new Select.Paises.Select
                        {
                            id = Convert.ToInt32(reader["Pais_ID"]),
                            text = Convert.ToString(reader["Pais_Nome"])
                        },
                    };

                    list.Add(estado);
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

        public bool Insert(Models.Estados estado)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbestados ( nomeestado, uf, codpais, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', {2}, '{3}', '{4}')",
                    estado.nomeEstado.ToUpper().Trim(),
                    estado.uf.ToUpper().Trim(),
                    estado.Pais.id,
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

        public bool Update(Models.Estados estado)
        {
            try
            {
                string sql = "UPDATE tbestados SET nomeestado = '"
                    + estado.nomeEstado.ToUpper().Trim() + "'," +
                    " uf = '" + estado.uf.ToUpper().Trim() + "'," +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "',"+
                    " codpais = " + estado.Pais.id +
                    " WHERE codestado = " + estado.codEstado;
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

        public Estados GetEstado(int? codEstado)
        {
            try
            {
                var model = new Models.Estados();
                if (codEstado != null)
                {
                    OpenConnection();
                    var sql = this.Search(codEstado, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codEstado = Convert.ToInt32(reader["Estado_ID"]);
                        model.nomeEstado = Convert.ToString(reader["Estado_Nome"]);
                        model.uf = Convert.ToString(reader["Estado_UF"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Estado_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Estado_DataUltAlteracao"]);
                        model.Pais = new Select.Paises.Select
                        {
                            id = Convert.ToInt32(reader["Pais_ID"]),
                            text = Convert.ToString(reader["Pais_Nome"]),
                        };
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

        public bool Delete(int? codEstado)
        {
            try
            {
                string sql = "DELETE FROM tbestados WHERE codestado = " + codEstado;
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

        public List<Select.Estados.Select> GetEstadosSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Estados.Select>();
                while (reader.Read())
                {
                    var estado = new Select.Estados.Select
                    {
                        id = Convert.ToInt32(reader["Estado_ID"]),
                        text = Convert.ToString(reader["Estado_Nome"]),
                    };

                    list.Add(estado);
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
                swhere = " WHERE codestado = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbestados.nomeestado LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
                        tbestados.codestado AS Estado_ID,
                        tbestados.nomeestado AS Estado_Nome,
                        tbestados.uf AS Estado_UF,
                        tbestados.dtcadastro AS Estado_DataCadastro,
                        tbestados.dtultalteracao AS Estado_DataUltAlteracao,
                        tbpaises.codpais AS Pais_ID,
                        tbpaises.nomepais AS Pais_Nome
                    FROM tbestados
                    INNER JOIN tbpaises on tbestados.codpais = tbpaises.codpais" + swhere;
            return sql;
        }


    }
}