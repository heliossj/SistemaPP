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

        protected string FormatDecimal(decimal? valor)
        {
            var result = string.Empty;
            if (valor != null)
            {
                result = valor.Value.ToString().Replace(",", ".");
            }
            return result;
        }

        protected string FormatDate(DateTime? data)
        {
            var result = string.Empty;
            if (data != null)
            {
                result = "'" + data.Value.ToString("yyyy-MM-dd") + "'";
            }
            return result;
        }

        protected bool ValidCPFCNPJ(string campoValor, string tabela, string coluna, string pessoaTipo)
        {
            try
            {
                string sql = "SELECT " + coluna + " FROM " + tabela + " WHERE " + coluna + " = '" + campoValor + "'";
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                SqlQuery.ExecuteNonQuery();
                reader = SqlQuery.ExecuteReader();
                string cpfcnpjAux = string.Empty;
                while (reader.Read())
                {
                    cpfcnpjAux = Convert.ToString(reader["" + coluna + ""]);
                }

                if (string.IsNullOrEmpty(cpfcnpjAux))
                    return true;
                else
                {
                    throw new Exception("Já existe um registro utilizando este "+ ( pessoaTipo == "J" ? "CNPJ" : "CPF" ) +", verifique!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        protected bool ValidName(string campoValor, string tabela, string coluna)
        {
            try
            {
                string sql = "SELECT " + coluna + " FROM " + tabela + " WHERE " + coluna + " = '" + campoValor + "'";
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                SqlQuery.ExecuteNonQuery();
                reader = SqlQuery.ExecuteReader();
                string name = string.Empty;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["cpfcnpj"]);
                }

                if (string.IsNullOrEmpty(name))
                    return true;
                else
                    return false;
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