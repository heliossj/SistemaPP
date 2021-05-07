using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

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
                        //dtCadastro = Convert.ToDateTime(reader["dtCadastro"]),
                        //dtAtualizacao = Convert.ToDateTime(reader["dtAtualizacao"])
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
                var sql = string.Format("INSERT INTO tbpaises ( nomepais, ddi, sigla) VALUES ('{0}', '{1}', '{2}')",
                    pais.nomePais.ToUpper().Trim(),
                    pais.DDI.ToUpper().Trim(),
                    pais.sigla.ToUpper()).Trim();

                //string sql = "INSERT INTO tbpaises ( nomepais, ddi, sigla ) VALUES ('" + pais.nomePais + "', '" + pais.DDI + "', '" + pais.sigla + "')";

                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);

                //verifica se houve alteração
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
                string sql = "UPDATE tbpaises SET nomepais = '" + pais.nomePais.ToUpper().Trim() + "', ddi = '" + pais.DDI.ToUpper().Trim() + "', sigla = '" + pais.sigla.ToUpper().Trim() + "' WHERE codpais = " + pais.codPais;
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


    }
}