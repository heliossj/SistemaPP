using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Sistema.DAO
{
    public class DAOEstados : Sistema.DAO.DAO
    {
        public List<Select.Estados.Select> GetEstadosSelect()
        {
            try
            {
                var sql = "SELECT* FROM tbestados";
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Estados.Select>();

                while (reader.Read())
                {
                    var estado = new Select.Estados.Select
                    {
                        id = Convert.ToInt32(reader["codestado"]),
                        text = Convert.ToString(reader["nomeestado"]),
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


    }
}