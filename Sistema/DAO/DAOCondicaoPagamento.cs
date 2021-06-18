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
    public class DAOCondicaoPagamento : Sistema.DAO.DAO
    {

        public List<CondicaoPagamento> GetCondicaoPagamentos()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<CondicaoPagamento>();

                while (reader.Read())
                {
                    var condicaoPagamento = new CondicaoPagamento
                    {
                        codCondicao = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                        nomeCondicao = Convert.ToString(reader["CondicaoPagamento_Nome"]),
                        situacao= Convert.ToString(reader["CondicaoPagamento_Situacao"]),
                    };

                    list.Add(condicaoPagamento);
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

        public bool Insert(Models.CondicaoPagamento condicaPagamento)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbcondpagamentos ( nomecondicaopagamento, txjuros, multa, desconto, situacao, dtcadastro, dtultalteracao) VALUES ('{0}', {1}, {2}, {3}, '{4}', '{5}', '{6}')",
                    condicaPagamento.nomeCondicao.ToUpper().Trim(),
                    condicaPagamento.txJuros,
                    condicaPagamento.multa,
                    condicaPagamento.desconto,
                    condicaPagamento.situacao.ToUpper().Trim(),
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

        //public bool Update(Models.Paises pais)
        //{
        //    try
        //    {
        //        string sql = "UPDATE tbpaises SET nomepais = '"
        //            + pais.nomePais.ToUpper().Trim() + "'," +
        //            " ddi = '" + pais.DDI.ToUpper().Trim() + "'," +
        //            " sigla = '" + pais.sigla.ToUpper().Trim() + "'," +
        //            " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd")
        //            + "' WHERE codpais = " + pais.codPais;
        //        OpenConnection();
        //        SqlQuery = new SqlCommand(sql, con);

        //        int i = SqlQuery.ExecuteNonQuery();

        //        if (i > 1)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception error)
        //    {
        //        throw new Exception(error.Message);
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }
        //}

        public CondicaoPagamento GetCondicaoPagamento(int? codCondicaoPagamento)
        {
            try
            {
                var model = new Models.CondicaoPagamento();
                if (codCondicaoPagamento != null)
                {
                    OpenConnection();
                    var sql = this.Search(codCondicaoPagamento, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codCondicao = Convert.ToInt32(reader["CondicaoPagamento_ID"]);
                        model.nomeCondicao = Convert.ToString(reader["CondicaoPagamento_Nome"]);
                        model.situacao = Convert.ToString(reader["CondicaoPagamento_Situacao"]);
                        model.txJuros = Convert.ToDecimal(reader["CondicaoPagamento_TaxaJuros"]);
                        model.multa = Convert.ToDecimal(reader["CondicaoPagamento_Multa"]);
                        model.desconto = Convert.ToDecimal(reader["CondicaoPagamento_Desconto"]);
                        model.dtCadastro = Convert.ToDateTime(reader["CondicaoPagamento_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["CondicaoPagamento_DataUltAlteracao"]);
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

        public bool Delete(int? codCondicaoPagamento)
        {
            try
            {
                string sql = "DELETE FROM tbcondicaopagamento WHERE codcondicao = " + codCondicaoPagamento;
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

        public List<Select.CondicaoPagamento.Select> GetCondicaoPagamentoSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.CondicaoPagamento.Select>();

                while (reader.Read())
                {
                    var condicaoPagamento = new Select.CondicaoPagamento.Select
                    {
                        id = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                        text = Convert.ToString(reader["CondicaoPagamento_Nome"]),
                    };

                    list.Add(condicaoPagamento);
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
                swhere = " WHERE tbcondpagamentos.codcondicao = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbcondpagamentos.nomedocondicaoLIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                SELECT
                    tbcondpagamentos.codcondicao AS CondicaoPagamento_ID,
                    tbcondpagamentos.nomecondicao AS CondicaoPagamento_Nome,
                    tbcondpagamentos.situacao AS CondicaoPagamento_Situacao,
                    tbcondpagamentos.txjuros AS CondicaoPagamento_Juros,
                    tbcondpagamentos.multa AS CondicaoPagamento_Multa,
                    tbcondpagamentos.desconto AS CondicaoPagamento_Desconto,
                    tbcondpagamentos.dtcadastro AS CondicaoPagamento_DataCadastro,
                    tbcondpagamentos.dtultalteracao AS CondicaoPagamento_DataUltAlteracao
                FROM tbcondpagamentos" + swhere;
            return sql;
        }


    }
}