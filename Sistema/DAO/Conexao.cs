using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Sistema.DAO
{
    public class Conexao
    {
        // variáveis de conexão, execução e resultSet
        protected SqlConnection con;
        protected SqlCommand SqlQuery;
        protected SqlDataReader reader;

        protected void OpenConnection()
        {
            try
            {
                con = new SqlConnection(WebConfigurationManager.ConnectionStrings["MSSQLSERVER"].ConnectionString);
                con.Open();
            }
            catch (Exception error)
            {
                throw new Exception("Erro ao abrir a conexão: " + error.Message);
            }
        }

        protected void CloseConnection()
        {
            try
            {
                if (con != null)
                {
                    con.Close();
                }
            }
            catch (Exception error)
            {
                throw new Exception("Erro ao fechar a conexão: " + error.Message);
            }
        }
    }


}