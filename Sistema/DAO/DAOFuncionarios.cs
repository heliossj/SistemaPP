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
    public class DAOFuncionarios : Sistema.DAO.DAO
    {

        public List<Funcionarios> GetFuncionarios()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Funcionarios>();
                while (reader.Read())
                {
                    var Funcionarios = new Funcionarios
                    {
                        codigo = Convert.ToInt32(reader["Funcionario_ID"]),
                        situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Funcionario_Situacao"])),
                        dsLogradouro = Convert.ToString(reader["Funcionario_Logradouro"]),
                        numero = Convert.ToString(reader["Funcionario_Numero"]),
                        complemento = Convert.ToString(reader["Funcionario_Complemento"]),
                        bairro = Convert.ToString(reader["Funcionario_Bairro"]),
                        telefoneFixo = Convert.ToString(reader["Funcionario_TelefoneFixo"]),
                        telefoneCelular = Convert.ToString(reader["Funcionario_TelefoneCelular"]),
                        email = Convert.ToString(reader["Funcionario_Email"]),
                        Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Funcionario_Cidade_ID"]),
                            text = Convert.ToString(reader["Funcionario_Cidade_Nome"])
                        },
                        cep = Convert.ToString(reader["Funcionario_CEP"]),
                        dtCadastro = Convert.ToDateTime(reader["Funcionario_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Funcionario_DataUltAlteracao"]),
                        //Física
                        nomePessoa = Convert.ToString(reader["Funcionario_Nome"]),
                        apelidoPessoa = Convert.ToString(reader["Funcionario_Apelido"]),
                        sexo =Convert.ToString(reader["Funcionario_Sexo"]),
                        cpf = Convert.ToString(reader["Funcionario_CPF"]),
                        rg = Convert.ToString(reader["Funcionario_RG"]),
                        dtNascimento = Convert.ToDateTime(reader["Funcionario_DataNascimento"]),
                        vlSalario = Convert.ToDecimal(reader["Funcionario_Salario"]),
                        dtAdmissao = Convert.ToDateTime(reader["Funcionario_DataAdmissao"]),
                        dtDemissao = Convert.ToDateTime(reader["Funcionario_DataDemissao"]) != null ? Convert.ToDateTime(reader["Funcionario_DataDemissao"]) : (DateTime?)null,
                    };
                    list.Add(Funcionarios);
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

        public bool Insert(Models.Funcionarios funcionario)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbfuncionarios ( situacao, nomefuncionario, sexo, logradouro, numero, complemento, bairro, telfixo, telcelular, email, codcidade, cep, cpf, rg, dtnascimento, dtadmissao, vlsalario, dtdemissao, dtcadastro, dtultalteracao, apelido )" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}', '{20}' )",
                    funcionario.situacao.ToUpper().Trim(),
                    funcionario.nomePessoa.ToUpper().Trim(),
                    funcionario.sexo.ToUpper().Trim(),
                    funcionario.dsLogradouro.ToUpper().Trim(),
                    funcionario.numero.ToUpper().Trim(),
                    funcionario.complemento.ToUpper().Trim(),
                    funcionario.bairro.ToUpper().Trim(),
                    funcionario.telefoneFixo,
                    funcionario.telefoneCelular,
                    funcionario.email.ToUpper().Trim(),
                    funcionario.Cidade.id,
                    funcionario.cep,
                    funcionario.cpf,
                    funcionario.rg,
                    funcionario.dtNascimento.Value.ToString("yyyy-MM-dd"),
                    funcionario.dtAdmissao.Value.ToString("yyyy-MM-dd"),
                    funcionario.vlSalario,
                    funcionario.dtDemissao != null ? funcionario.dtDemissao.Value.ToString("yyyy-MM-dd") : null ,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    funcionario.apelidoPessoa.ToUpper().Trim()
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

        public bool Update(Models.Funcionarios funcionario)
        {
            try
            {
                string sql = "UPDATE tbfuncionarios SET situacao = '"
                    + funcionario.tipo.ToUpper().Trim() + "', " +
                    " nomefuncionario = '" + funcionario.nomePessoa.ToUpper().Trim() + "'," +
                    " sexo = '" + funcionario.sexo.ToUpper().Trim() + "', " +
                    " logradouro = '" + funcionario.dsLogradouro.ToUpper().Trim() + "', " +
                    " numero = '" + funcionario.numero + "', " +
                    " complemento '" + funcionario.complemento.ToUpper().Trim() + "', " +
                    " bairro = '" + funcionario.bairro.ToUpper().Trim() + "', " +
                    " telfixo = '" + funcionario.telefoneFixo + "', " +
                    " telcelular = '" + funcionario.telefoneCelular + "', " +
                    " email = '" + funcionario.email.ToUpper().Trim() + "', " +
                    " codcidade = " + funcionario.Cidade.id + ", " +
                    " cep = '" + funcionario.cep + "', " +
                    " cpf = '" + funcionario.cpf + "', " +
                    " rg = '" + funcionario.rg + "', " +
                    " dtnascimento = '" + funcionario.dtNascimento.Value.ToString("yyyy-MM-dd") + "', " +
                    " dtadmissao = '" + funcionario.dtAdmissao.Value.ToString("yyyy-MM-dd") + "', " +
                    " vlsalario = " + funcionario.vlSalario + ", " +
                    " dtdemissao = '" + funcionario.dtDemissao.Value.ToString("yyyy-MM-dd") != null ? funcionario.dtDemissao.Value.ToString("yyyy-MM-dd") : null + "', " +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " apelido = '" + funcionario.apelidoPessoa.ToUpper().ToString() + "' " +
                    " WHERE codfuncionario = " + funcionario.codigo;
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

        public Funcionarios GetFuncionario(int? codFuncionario)
        {
            try
            {
                var model = new Models.Funcionarios();
                if (codFuncionario != null)
                {
                    OpenConnection();
                    var sql = this.Search(codFuncionario, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Funcionario_ID"]);
                        model.situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Funcionario_Situacao"]));
                        model.dsLogradouro = Convert.ToString(reader["Funcionario_Logradouro"]);
                        model.numero = Convert.ToString(reader["Funcionario_Numero"]);
                        model.complemento = Convert.ToString(reader["Funcionario_Complemento"]);
                        model.bairro = Convert.ToString(reader["Funcionario_Bairro"]);
                        model.telefoneFixo = Convert.ToString(reader["Funcionario_TelefoneFixo"]);
                        model.telefoneCelular = Convert.ToString(reader["Funcionario_TelefoneCelular"]);
                        model.email = Convert.ToString(reader["Funcionario_Email"]);
                        model.Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Funcionario_Cidade_ID"]),
                            text = Convert.ToString(reader["Funcionario_Cidade_Nome"])
                        };
                        model.cep = Convert.ToString(reader["Funcionario_CEP"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Funcionario_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Funcionario_DataUltAlteracao"]);
                        //Física
                        model.nomePessoa = Convert.ToString(reader["Funcionario_Nome"]);
                        model.apelidoPessoa = Convert.ToString(reader["Funcionario_Apelido"]);
                        model.sexo = Convert.ToString(reader["Funcionario_Sexo"]);
                        model.cpf = Convert.ToString(reader["Funcionario_CPF"]);
                        model.rg = Convert.ToString(reader["Funcionario_RG"]);
                        model.dtNascimento = Convert.ToDateTime(reader["Funcionario_DataNascimento"]);
                        model.vlSalario = Convert.ToDecimal(reader["Funcionario_Salario"]);
                        model.dtAdmissao = Convert.ToDateTime(reader["Funcionario_DataAdmissao"]);
                        model.dtDemissao = Convert.ToDateTime(reader["Funcionario_DataDemissao"]) != null ? Convert.ToDateTime(reader["Funcionario_DataDemissao"]) : (DateTime?)null;
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

        public bool Delete(int? codFuncionario)
        {
            try
            {
                string sql = "DELETE FROM tbfuncionarios WHERE codfuncionario = " + codFuncionario;
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

        public List<Select.Funcionarios.Select> GetFuncionariosSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Funcionarios.Select>();
                while (reader.Read())
                {
                    var funcionario = new Select.Funcionarios.Select
                    {
                        id = Convert.ToInt32(reader["Funcionario_ID"]),
                        text = Convert.ToString(reader["Funcionario_Nome"]),
                    };

                    list.Add(funcionario);
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
                swhere = " WHERE tbfuncionarios.codfuncionario = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbfuncionarios.nomefuncionario LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT
                        tbfuncionarios.codfuncionario AS Funcionario_ID,
                        tbfuncionarios.situacao AS Funcionario_Situacao,
                        tbfuncionarios.nomefuncionario AS Funcionario_Nome,
                        tbfuncionarios.apelido AS Funcionario_Apelido,
                        tbfuncionarios.sexo AS Funcionario_Sexo,
                        tbfuncionarios.logradouro AS Funcionario_Logradouro,
                        tbfuncionarios.numero AS Funcionario_Numero,
                        tbfuncionarios.complemento AS Funcionario_Complemento,
                        tbfuncionarios.bairro AS Funcionario_Bairro,
                        tbfuncionarios.telfixo AS Funcionario_TelefoneFixo,
                        tbfuncionarios.telcelular AS Funcionario_TelefoneCelular,
                        tbfuncionarios.email AS Funcionario_Email,
                        tbcidades.codcidade AS Funcionario_Cidade_ID,
                        tbcidades.nomecidade AS Funcionario_Cidade_Nome,
                        tbfuncionarios.cep AS Funcionario_CEP,
                        tbfuncionarios.cpf AS Funcionario_CPF,
                        tbfuncionarios.rg AS Funcionario_RG,
                        tbfuncionarios.dtnascimento AS Funcionario_DataNascimento,
                        tbfuncionarios.dtadmissao AS Funcionario_DataAdmissao,
                        tbfuncionarios.vlsalario AS Funcionario_Salario,
                        tbfuncionarios.dtdemissao AS Funcionario_DataDemissao,
                        tbfuncionarios.dtcadastro AS Funcionario_DataCadastro,
                        tbfuncionarios.dtultalteracao AS Funcionario_DataUltAlteracao
                    FROM tbfuncionarios
                    INNER JOIN tbcidades on tbfuncionarios.codcidade = tbcidades.codcidade
                    " + swhere;
            return sql;
        }


    }
}