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
                        codigo = Convert.ToInt32(reader["CondicaoPagamento_ID"]),
                        nomeCondicao = Convert.ToString(reader["CondicaoPagamento_Nome"]),
                        situacao = Sistema.Util.FormatFlag.Situacao(Convert.ToString(reader["CondicaoPagamento_Situacao"])),
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
                var sql = string.Format("INSERT INTO tbcondpagamentos ( nomecondicao, txjuros, multa, desconto, situacao, dtcadastro, dtultalteracao) VALUES ('{0}', {1}, {2}, {3}, '{4}', '{5}', '{6}'); SELECT SCOPE_IDENTITY()",
                    this.FormatString(condicaPagamento.nomeCondicao),
                    condicaPagamento.txJuros != null ? condicaPagamento.txJuros : 0,
                    condicaPagamento.multa != null ? condicaPagamento.multa : 0,
                    condicaPagamento.desconto != null ? condicaPagamento.desconto : 0,
                    this.FormatString(condicaPagamento.situacao),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd")
                    );
                string sqlItem = "INSERT INTO tbparcelascondicao (codcondicao, codforma, nrparcela, qtdias, txpercentual) VALUES ({0}, {1}, {2}, {3}, {4} )";
                using (con)
                {
                    OpenConnection();

                    SqlTransaction sqlTrans = con.BeginTransaction();
                    SqlCommand command = con.CreateCommand();
                    command.Transaction = sqlTrans;
                    try
                    {
                        command.CommandText = sql;
                        //command.ExecuteNonQuery();
                        var codCondicao = Convert.ToInt32(command.ExecuteScalar());
                        foreach (var item in condicaPagamento.ListCondicao)
                        {
                            var Item = string.Format(sqlItem, codCondicao, item.codFormaPagamento, item.nrParcela, item.qtDias, item.txPercentual);
                            command.CommandText = Item;
                            command.ExecuteNonQuery();
                        }
                        sqlTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTrans.Rollback();
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return true;
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
                    var sqlParcelas = this.SearchParcelas(codCondicaoPagamento);
                    var list = new List<CondicaoPagamento.CondicaoPagamentoVM>();
                    SqlQuery = new SqlCommand(sql + sqlParcelas, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["CondicaoPagamento_ID"]);
                        model.nomeCondicao = Convert.ToString(reader["CondicaoPagamento_Nome"]);
                        model.situacao = Convert.ToString(reader["CondicaoPagamento_Situacao"]);
                        model.txJuros = Convert.ToDecimal(reader["CondicaoPagamento_TaxaJuros"]);
                        model.multa = Convert.ToDecimal(reader["CondicaoPagamento_Multa"]);
                        model.desconto = Convert.ToDecimal(reader["CondicaoPagamento_Desconto"]);
                        model.dtCadastro = Convert.ToDateTime(reader["CondicaoPagamento_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["CondicaoPagamento_DataUltAlteracao"]);
                    };

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var item = new CondicaoPagamento.CondicaoPagamentoVM()
                            {
                                codCondicaoPagamento = Convert.ToInt32(reader["Condicao_ID"]),
                                codFormaPagamento = Convert.ToInt32(reader["Condicao_Forma_ID"]),
                                nomeFormaPagamento = Convert.ToString(reader["Condicao_Forma_Nome"]),
                                nrParcela = Convert.ToInt16(reader["Parcela_Nr"]),
                                qtDias = Convert.ToInt16(reader["Parcela_QtDias"]),
                                txPercentual = Convert.ToDecimal(reader["Parcela_TaxaPercentual"])
                            };
                            list.Add(item);
                        }
                    }
                    model.ListCondicao = list;
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
                string sql = "DELETE FROM tbcondpagamentos WHERE codcondicao = " + codCondicaoPagamento;
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
                    swhere += " OR tbcondpagamentos.nomedocondicao LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                SELECT
                    tbcondpagamentos.codcondicao AS CondicaoPagamento_ID,
                    tbcondpagamentos.nomecondicao AS CondicaoPagamento_Nome,
                    tbcondpagamentos.situacao AS CondicaoPagamento_Situacao,
                    tbcondpagamentos.txjuros AS CondicaoPagamento_TaxaJuros,
                    tbcondpagamentos.multa AS CondicaoPagamento_Multa,
                    tbcondpagamentos.desconto AS CondicaoPagamento_Desconto,
                    tbcondpagamentos.dtcadastro AS CondicaoPagamento_DataCadastro,
                    tbcondpagamentos.dtultalteracao AS CondicaoPagamento_DataUltAlteracao
                FROM tbcondpagamentos" + swhere + ";";
            return sql;
        }

        private string SearchParcelas(int? id)
        {
            var sql = string.Empty;

            sql = @"
                    SELECT
                    tbparcelascondicao.codcondicao AS Condicao_ID,
                    tbparcelascondicao.codforma AS Condicao_Forma_ID,
                    tbformapagamento.nomeforma AS Condicao_Forma_Nome,
                    tbparcelascondicao.nrparcela AS Parcela_Nr,
                    tbparcelascondicao.qtdias AS Parcela_QtDias,
                    tbparcelascondicao.txpercentual AS Parcela_TaxaPercentual
                    FROM tbparcelascondicao
                    INNER JOIN tbformapagamento on tbparcelascondicao.codforma = tbformapagamento.codforma
                    WHERE tbparcelascondicao.codcondicao = " + id
            ;
            return sql;
        }


    }
}