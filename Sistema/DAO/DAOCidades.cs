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
    public class DAOCidades : Sistema.DAO.DAO
    {

        public List<Cidades> GetCidades()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Cidades>();

                while (reader.Read())
                {
                    var cidade = new Cidades
                    {
                        codigo = Convert.ToInt32(reader["Cidade_ID"]),
                        nomeCidade = Convert.ToString(reader["Cidade_Nome"]),
                        ddd = Convert.ToString(reader["Cidade_DDD"]),
                        sigla = Convert.ToString(reader["Cidade_Sigla"]),
                        Estado = new Select.Estados.Select
                        {
                            id = Convert.ToInt32(reader["Estado_ID"]),
                            text = Convert.ToString(reader["Estado_Nome"])
                        },
                    };

                    list.Add(cidade);
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

        public bool Insert(Models.Cidades cidade)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbcidades ( nomecidade, ddd, sigla, codestado, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', '{2}', {3}, '{4}', '{5}')",
                    cidade.nomeCidade.ToUpper().Trim(),
                    cidade.ddd.ToUpper().Trim(),
                    cidade.sigla.ToUpper().Trim(),
                    cidade.Estado.id,
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

        public bool Update(Models.Cidades cidade)
        {
            try
            {
                string sql = "UPDATE tbcidades SET nomecidade = '"
                    + cidade.nomeCidade.ToUpper().Trim() + "'," +
                    " ddd = '" + cidade.ddd.ToUpper().Trim() + "'," +
                    " sigla = '" + cidade.sigla.ToUpper().Trim() + "',"+
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "',"+
                    " codestado = " + cidade.Estado.id +
                    " WHERE codcidade = " + cidade.codigo;
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

        public Cidades GetCidade(int? codCidade)
        {
            try
            {
                var model = new Models.Cidades();
                if (codCidade != null)
                {
                    OpenConnection();
                    var sql = this.Search(codCidade, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Cidade_ID"]);
                        model.nomeCidade = Convert.ToString(reader["Cidade_Nome"]);
                        model.ddd = Convert.ToString(reader["Cidade_DDD"]);
                        model.sigla = Convert.ToString(reader["Cidade_Sigla"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Cidade_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Cidade_DataUltAlteracao"]);
                        model.Estado = new Select.Estados.Select
                        {
                            id = Convert.ToInt32(reader["Estado_ID"]),
                            text = Convert.ToString(reader["Estado_Nome"]),
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

        public bool Delete(int? codCidade)
        {
            try
            {
                string sql = "DELETE FROM tbcidades WHERE codcidade = " + codCidade;
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

        public List<Select.Cidades.Select> GetCidadesSelect(int? id, string filter)
        {
            try
            {
                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Cidades.Select>();
                while (reader.Read())
                {
                    var cidade = new Select.Cidades.Select
                    {
                        id = Convert.ToInt32(reader["Cidade_ID"]),
                        text = Convert.ToString(reader["Cidade_Nome"]),
                    };

                    list.Add(cidade);
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
                swhere = " WHERE codcidade = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbcidades.nomecidade LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                SELECT
                    tbcidades.codcidade AS Cidade_ID,
                    tbcidades.nomecidade AS Cidade_Nome,
                    tbcidades.ddd AS Cidade_DDD,
                    tbcidades.sigla AS Cidade_Sigla,
                    tbcidades.dtcadastro AS Cidade_DataCadastro,
                    tbcidades.dtultalteracao AS Cidade_DataUltAlteracao,
                    tbestados.codestado AS Estado_ID,
                    tbestados.nomeestado AS Estado_Nome
                FROM tbcidades
                INNER JOIN tbestados on tbcidades.codestado = tbestados.codestado " + swhere;
            return sql;
        }


    }
}