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
    public class DAOFornecedores : Sistema.DAO.DAO
    {

        public List<Fornecedores> GetFornecedores()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Fornecedores>();
                var tipoPessoa = string.Empty;
                while (reader.Read())
                {
                    tipoPessoa = Convert.ToString(reader["Fornecedor_Tipo"]);
                    var Fornecedores = new Fornecedores
                    {
                        codigo = Convert.ToInt32(reader["Fornecedor_ID"]),
                        tipo = Util.FormatFlag.TipoPessoa(Convert.ToString(reader["Fornecedor_Tipo"])),
                        situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Fornecedor_Situacao"])),
                        dsLogradouro = Convert.ToString(reader["Fornecedor_Logradouro"]),
                        numero = Convert.ToString(reader["Fornecedor_Numero"]),
                        complemento = Convert.ToString(reader["Fornecedor_Complemento"]),
                        bairro = Convert.ToString(reader["Fornecedor_Bairro"]),
                        telefoneFixo = Convert.ToString(reader["Fornecedor_TelefoneFixo"]),
                        telefoneCelular = Convert.ToString(reader["Fornecedor_TelefoneCelular"]),
                        email = Convert.ToString(reader["Fornecedor_Email"]),
                        Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Fornecedor_Cidade_ID"]),
                            text = Convert.ToString(reader["Fornecedor_Cidade_Nome"])
                        },
                        cep = Convert.ToString(reader["Fornecedor_CEP"]),
                        CondicaoPagamento = new Select.CondicaoPagamento.Select
                        {
                            id = Convert.ToInt32(reader["Fornecedor_CondicaoPagamento_ID"]),
                            text = Convert.ToString(reader["Fornecedor_CondicaoPagamento_Nome"])
                        },
                        dtCadastro = Convert.ToDateTime(reader["Fornecedor_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Fornecedor_DataUltAlteracao"]),
                        site = Convert.ToString(reader["Fornecedor_Site"]),
                        observacao = Convert.ToString(reader["Forecedor_Observacao"]),
                        //Física
                        nomePessoa = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_RazaoSocial_NomeFornecedor"]) : "",
                        apelidoPessoa = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_NomeFantasia_ApelidoFornecedor"]) : "",
                        sexo = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_Sexo"]) : "",
                        cpf = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_CNPJ_CPF"]) : "",
                        rg = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_IE_RG"]) : "",
                        dtNascimento = tipoPessoa == "F" ? Convert.ToDateTime(reader["Fornecedor_DataFundacao_DataNascimento"]) : (DateTime?)null,
                        //Jurídica
                        razaoSocial = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_RazaoSocial_NomeFornecedor"]) : "",
                        nomeFantasia = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_NomeFantasia_ApelidoFornecedor"]) : "",
                        cnpj = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_CNPJ_CPF"]) : "",
                        ie = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_IE_RG"]) : "",
                        dtFundacao = tipoPessoa == "J" ? Convert.ToDateTime(reader["Fornecedor_DataFundacao_DataNascimento"]) : (DateTime?)null,
                    };

                    list.Add(Fornecedores);
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

        public bool Insert(Models.Fornecedores fornecedor)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbclientes ( tipo, nomerazaosocial, sexo, logradouro, numero, complemento, bairro, telfixo, telcelular, email, codcidade, cep, cpfcnpj, rgie, dtnascimentofundacao, situacao, codcondicao, dtcadastro, dtultalteracao, apelidonomefantasia, site, observacao)" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}', '{20}', '{21}' )",
                    fornecedor.tipo.ToUpper().Trim(),
                    fornecedor.tipo == "F" ? fornecedor.nomePessoa.ToUpper().Trim() : fornecedor.razaoSocial.ToUpper().Trim(),
                    fornecedor.tipo == "F" ? fornecedor.sexo.ToUpper().Trim() : "",
                    fornecedor.dsLogradouro.ToUpper().Trim(),
                    fornecedor.numero.ToUpper().Trim(),
                    fornecedor.complemento.ToUpper().Trim(),
                    fornecedor.bairro.ToUpper().Trim(),
                    fornecedor.telefoneFixo,
                    fornecedor.telefoneCelular,
                    fornecedor.email.ToUpper().Trim(),
                    fornecedor.Cidade.id,
                    fornecedor.cep,
                    fornecedor.tipo == "F" ? fornecedor.cpf : fornecedor.cnpj,
                    fornecedor.tipo == "F" ? fornecedor.rg : fornecedor.ie,
                    fornecedor.tipo == "F" ? fornecedor.dtNascimento.Value.ToString("yyyy-MM-dd") : fornecedor.dtFundacao.Value.ToString("yyyy-MM-dd"),
                    fornecedor.situacao.ToUpper().Trim(),
                    fornecedor.CondicaoPagamento.id,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    fornecedor.tipo == "F" ? fornecedor.apelidoPessoa : fornecedor.nomeFantasia,
                    fornecedor.site = fornecedor.site,
                    fornecedor.tipo = fornecedor.observacao
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

        public bool Update(Models.Fornecedores fornecedor)
        {
            try
            {
                string sql = "UPDATE tbclientes SET tipo = '"
                    + fornecedor.tipo.ToUpper().Trim() + "', " +
                    " nomerazaosocial = '" + fornecedor.tipo == "F" ? fornecedor.nomePessoa.ToUpper().Trim() : fornecedor.razaoSocial.ToUpper().Trim() + "'," +
                    " sexo = '" + fornecedor.tipo == "F" ? fornecedor.sexo.ToUpper().Trim() : "" + "', " +
                    " logradouro = '" + fornecedor.dsLogradouro.ToUpper().Trim() + "', " +
                    " numero = '" + fornecedor.numero + "', " +
                    " complemento '" + fornecedor.complemento.ToUpper().Trim() + "', " +
                    " bairro = '" + fornecedor.bairro.ToUpper().Trim() + "', " +
                    " telfixo = '" + fornecedor.telefoneFixo + "', " +
                    " telcelular = '" + fornecedor.telefoneCelular + "', " +
                    " email = '" + fornecedor.email.ToUpper().Trim() + "', " +
                    " codcidade = " + fornecedor.Cidade.id + ", " +
                    " cep = '" + fornecedor.cep + "', " +
                    " cpfcnpj = '" + fornecedor.tipo == "F" ? fornecedor.cpf : fornecedor.cnpj + "', " +
                    " rgie = '" + fornecedor.tipo == "F" ? fornecedor.rg : fornecedor.ie + "', " +
                    " dtnascimentofundacao = '" + fornecedor.tipo == "F" ? fornecedor.dtNascimento.Value.ToString("yyyy-MM-dd") : fornecedor.dtFundacao.Value.ToString("yyyy-MM-dd") + "', " +
                    " situacao = '" + fornecedor.situacao.ToUpper().ToString() + "', " +
                    " codcondicao = " + fornecedor.CondicaoPagamento.id + ", " +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " apelidonomefantasia = '" + fornecedor.tipo == "F" ? fornecedor.apelidoPessoa.ToUpper().ToString() : fornecedor.nomeFantasia.ToUpper().ToString() + "' " +
                    " site = '" + fornecedor.site + "', " +
                    " observacao = '" + fornecedor.observacao + "', " +
                    " WHERE codcliente = " + fornecedor.codigo;
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

        public Fornecedores GetFornecedor(int? codCliente)
        {
            try
            {
                var model = new Models.Fornecedores();
                if (codCliente != null)
                {
                    OpenConnection();
                    var sql = this.Search(codCliente, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    var tipoPessoa = string.Empty;
                    tipoPessoa = Convert.ToString(reader["Fornecedor_Tipo"]);
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Fornecedor_ID"]);
                        model.tipo = Util.FormatFlag.TipoPessoa(Convert.ToString(reader["Fornecedor_Tipo"]));
                        model.situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Fornecedor_Situacao"]));
                        model.dsLogradouro = Convert.ToString(reader["Fornecedor_Logradouro"]);
                        model.numero = Convert.ToString(reader["Fornecedor_Numero"]);
                        model.complemento = Convert.ToString(reader["Fornecedor_Complemento"]);
                        model.bairro = Convert.ToString(reader["Fornecedor_Bairro"]);
                        model.telefoneFixo = Convert.ToString(reader["Fornecedor_TelefoneFixo"]);
                        model.telefoneCelular = Convert.ToString(reader["Fornecedor_TelefoneCelular"]);
                        model.email = Convert.ToString(reader["Fornecedor_Email"]);
                        model.Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Fornecedor_Cidade_ID"]),
                            text = Convert.ToString(reader["Fornecedor_Cidade_Nome"])
                        };
                        model.cep = Convert.ToString(reader["Fornecedor_CEP"]);
                        model.CondicaoPagamento = new Select.CondicaoPagamento.Select
                        {
                            id = Convert.ToInt32(reader["Fornecedor_CondicaoPagamento_ID"]),
                            text = Convert.ToString(reader["Fornecedor_CondicaoPagamento_Nome"])
                        };
                        model.dtCadastro = Convert.ToDateTime(reader["Fornecedor_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Fornecedor_DataUltAlteracao"]);
                        model.site = Convert.ToString(reader["Fornecedor_Site"]);
                        model.observacao = Convert.ToString(reader["Forecedor_Observacao"]);
                        //Física
                        model.nomePessoa = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_RazaoSocial_NomeFornecedor"]) : "";
                        model.apelidoPessoa = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_NomeFantasia_ApelidoFornecedor"]) : "";
                        model.sexo = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_Sexo"]) : "";
                        model.cpf = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_CNPJ_CPF"]) : "";
                        model.rg = tipoPessoa == "F" ? Convert.ToString(reader["Fornecedor_IE_RG"]) : "";
                        model.dtNascimento = tipoPessoa == "F" ? Convert.ToDateTime(reader["Fornecedor_DataFundacao_DataNascimento"]) : (DateTime?)null;
                        //Jurídica
                        model.razaoSocial = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_RazaoSocial_NomeFornecedor"]) : "";
                        model.nomeFantasia = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_NomeFantasia_ApelidoFornecedor"]) : "";
                        model.cnpj = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_CNPJ_CPF"]) : "";
                        model.ie = tipoPessoa == "J" ? Convert.ToString(reader["Fornecedor_IE_RG"]) : "";
                        model.dtFundacao = tipoPessoa == "J" ? Convert.ToDateTime(reader["Fornecedor_DataFundacao_DataNascimento"]) : (DateTime?)null;
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

        public bool Delete(int? codFornecedor)
        {
            try
            {
                string sql = "DELETE FROM tbfornecedores WHERE codfornecedor = " + codFornecedor;
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

        public List<Select.Fornecedores.Select> GetFornecedoresSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Fornecedores.Select>();
                while (reader.Read())
                {
                    var cliente = new Select.Fornecedores.Select
                    {
                        id = Convert.ToInt32(reader["Fornecedor_ID"]),
                        text = Convert.ToString(reader["Fornecedor_Nome"]),
                    };

                    list.Add(cliente);
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
                swhere = " WHERE tbfornecedores.codfornecedor = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbfornecedores.nomerazaosocial LIKE'%" + word + "%'";
                }
                swhere = " WHERE " + swhere.Remove(0, 3);
            }
            sql = @"
                    SELECT 
                        tbclientes.codcliente AS Cliente_ID,
                        tbclientes.tipo AS Cliente_Tipo,
                        tbclientes.situacao AS Cliente_Situacao,
                        tbclientes.logradouro AS Cliente_Logradouro,
                        tbclientes.numero AS Cliente_Numero,
                        tbclientes.complemento AS Cliente_Complemento,
                        tbclientes.bairro AS Cliente_Bairro,
                        tbclientes.telfixo AS Cliente_TelefoneFixo,
                        tbclientes.telcelular AS Cliente_TelefoneCelular,
                        tbclientes.email AS Cliente_Email,
                        tbcidades.codcidade AS Cliente_Cidade_ID,
                        tbcidades.nomecidade AS Cliente_Cidade_Nome,
                        tbclientes.cep AS Cliente_CEP,
                        tbcondpagamentos.codcondicao AS Cliente_CondicaoPagamento_ID,
                        tbcondpagamentos.nomecondicao AS Cliente_CondicaoPagamento_Nome,
                        tbclientes.dtcadastro AS Cliente_DataCadastro,
                        tbclientes.dtultalteracao AS Cliente_DataUltAlteracao,
                        tbclientes.nomerazaosocial AS Cliente_RazaoSocial_NomeCliente,
                        tbclientes.apelidonomefantasia AS Cliente_NomeFantasia_ApelidoCliente,
                        tbclientes.sexo AS Cliente_Sexo,
                        tbclientes.cpfcnpj AS Cliente_CNPJ_CPF,
                        tbclientes.rgie AS Cliente_IE_RG,
                        tbclientes.dtnascimentofundacao AS Cliente_DataFundacao_DataNascimento
                        FROM tbclientes
                    INNER JOIN tbcidades on tbclientes.codcidade = tbcidades.codcidade
                    INNER JOIN tbcondpagamentos on tbclientes.codcondicao = tbcondpagamentos.codcondicao
                    " + swhere;
            return sql;
        }


    }
}