using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Sistema.DAO
{
    public class DAO
    {
        // variáveis de conexão, execução e resultSet
        protected SqlConnection con;
        protected SqlCommand SqlQuery;
        protected SqlDataReader reader;

        protected void OpenConnection()
        {
            try
            {
                con = new SqlConnection(WebConfigurationManager.ConnectionStrings["HELIO"].ConnectionString);
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

        protected string FormatString(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.ToUpper().Trim();
            }
            return text;
        }

        protected string FormatRG(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(".", "").Replace("-", "");
            }
            return text;
        }

        protected string FormatCPF(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(".", "").Replace("-", "");
            }
            return text;
        }

        protected string FormatCNPJ(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(".", "").Replace("/", "").Replace("-", "");
            }
            return text;
        }

        protected string FormatCEP(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("-", "");
            }
            return text;
        }

        protected string FormatPhone(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }
            return text;
        }





    }
}