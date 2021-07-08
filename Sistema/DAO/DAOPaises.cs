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

        public List<Paises> GetPaises()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Paises>();

                while (reader.Read())
                {
                    var pais = new Paises
                    {
                        codigo = Convert.ToInt32(reader["Pais_ID"]),
                        nomePais = Convert.ToString(reader["Pais_Nome"]),
                        sigla = Convert.ToString(reader["Pais_Sigla"]),
                        DDI = Convert.ToString(reader["Pais_DDI"]),
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
                    this.FormatString(pais.nomePais),
                    this.FormatString(pais.DDI),
                    this.FormatString(pais.sigla),
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

        public bool Update(Models.Paises pais)
        {
            try
            {
                string sql = "UPDATE tbpaises SET nomepais = '"
                    + this.FormatString(pais.nomePais) + "'," +
                    " ddi = '" + this.FormatString(pais.DDI) + "'," +
                    " sigla = '" + this.FormatString(pais.sigla) + "'," +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "' WHERE codpais = " + pais.codigo;
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
                    var sql = this.Search(codPais, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Pais_ID"]);
                        model.nomePais = Convert.ToString(reader["Pais_Nome"]);
                        model.DDI = Convert.ToString(reader["Pais_DDI"]);
                        model.sigla = Convert.ToString(reader["Pais_Sigla"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Pais_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Pais_DataUltAlteracao"]);
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
                string sql = "DELETE FROM tbpaises WHERE codpais = " + codPais;
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

        public List<Select.Paises.Select> GetPaisesSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Paises.Select>();

                while (reader.Read())
                {
                    var pais = new Select.Paises.Select
                    {
                        id = Convert.ToInt32(reader["Pais_ID"]),
                        text = Convert.ToString(reader["Pais_Nome"]),
                        ddi = Convert.ToString(reader["Pais_DDI"]),
                        sigla = Convert.ToString(reader["Pais_Sigla"]),
                        dtCadastro = Convert.ToDateTime(reader["Pais_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Pais_DataUltAlteracao"]),
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

        private string Search(int? id, string filter)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere = " WHERE codpais = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbpaises.nomepais LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
                        codpais AS Pais_ID,
                        nomepais AS Pais_Nome,
                        ddi AS Pais_DDI,
                        sigla AS Pais_Sigla,
                        dtcadastro AS Pais_DataCadastro,
                        dtultalteracao AS Pais_DataUltAlteracao
                    FROM tbpaises" + swhere;
            return sql;
        }


    }
}