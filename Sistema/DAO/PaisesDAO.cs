using Sistema.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Sistema.DAO
{
    public class PaisesDAO : Sistema.DAO.Conexao
    {

        public List<PaisesVM> GetPaises()
        {
            try
            {
                OpenConnection();
                SqlQuery = new SqlCommand("SELECT * FROM tbpaises", con);
                reader = SqlQuery.ExecuteReader();

                var list = new List<PaisesVM>();

                while (reader.Read())
                {
                    var pais = new PaisesVM
                    {
                        idPais = Convert.ToInt32(reader["codpais"]),
                        nmPais = Convert.ToString(reader["nomepais"]),
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





    }
}