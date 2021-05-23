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
    public class DAOPaises : Sistema.DAO.DAO
    {

        public List<Paises> GetPaises(int? idPais = null)
        {
            try
            {

                var sql = "SELECT* FROM tbpaises";
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Paises>();

                while (reader.Read())
                {
                    var pais = new Paises
                    {
                        codPais = Convert.ToInt32(reader["codpais"]),
                        nomePais = Convert.ToString(reader["nomepais"]),
                        sigla = Convert.ToString(reader["sigla"]),
                        DDI = Convert.ToString(reader["ddi"]),
                    };

                    list.Add(pais);
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

        public bool Insert(Models.Paises pais)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbpaises ( nomepais, ddi, sigla, dtcadastro, dtultalteracao) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    pais.nomePais.ToUpper().Trim(),
                    pais.DDI.ToUpper().Trim(),
                    pais.sigla.ToUpper().Trim(),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd")
                    );
                //var ultReg = "SELECT * FROM tbpaises where codpais=(SELECT MAX(codpais) FROM tbpaises)";
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                //SqlQuery.ExecuteNonQuery();
                //var reg = new SqlCommand(ultReg, con);
                //reader = reg.ExecuteReader();
                //var model = new Sistema.Select.Paises.Select();
                //while (reader.Read())
                //{
                //    model.id = Convert.ToInt32(reader["codpais"]);
                //    model.text = Convert.ToString(reader["nomepais"]);
                //}

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

        public bool Update(Models.Paises pais)
        {
            try
            {
                string sql = "UPDATE tbpaises SET nomepais = '"
                    + pais.nomePais.ToUpper().Trim() + "'," +
                    "ddi = '" + pais.DDI.ToUpper().Trim() + "'," +
                    " sigla = '" + pais.sigla.ToUpper().Trim() + "'," +
                    "dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "' WHERE codpais = " + pais.codPais;
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

        public Paises GetPais(int? codPais)
        {
            try
            {
                var model = new Models.Paises();
                if (codPais != null)
                {
                    OpenConnection();
                    //var sql = string.Format("SELECT * FROM tbpaises WHERE codpais = {0}", codPais);
                    var sql = "SELECT * FROM tbpaises WHERE codpais = " + codPais;
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codPais = reader.GetInt32(0);
                        model.nomePais = reader.GetString(1);
                        model.DDI = reader.GetString(2);
                        model.sigla = reader.GetString(3);
                        model.dtCadastro = reader.GetDateTime(4);
                        model.dtUltAlteracao = reader.GetDateTime(5);
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

        public bool Delete(int? codPais)
        {
            try
            {
                string sql = "DELETE FROM tbpaises WHERE codPais = " + codPais;
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

        public List<Select.Paises.Select> GetPaisesSelect(int? id, string q)
        {
            try
            {
                var swhere = string.Empty;
                if (id != null)
                {
                    swhere = " WHERE codpais = " + id ;
                }
                if (!string.IsNullOrEmpty(q))
                {
                    //where += " OR ";
                    var filter = q.Split(' ');
                    foreach (var word in filter)
                    {
                        swhere += " OR nomepais LIKE'%" + word + "%'"; 
                    }
                    //swhere = swhere.Remove(0, 3);
                    swhere = " WHERE" + swhere.Remove(0, 3);
                }
                var sql = "SELECT * FROM tbpaises" + swhere;
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Paises.Select>();

                while (reader.Read())
                {
                    var pais = new Select.Paises.Select
                    {
                        id = Convert.ToInt32(reader["codpais"]),
                        text = Convert.ToString(reader["nomepais"]),
                        ddi = Convert.ToString(reader["ddi"]),
                        sigla = Convert.ToString(reader["sigla"]),
                        dtCadastro = Convert.ToDateTime(reader["dtcadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["dtultalteracao"]),
                    };

                    list.Add(pais);
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


    }
}