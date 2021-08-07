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
                        observacao = Convert.ToString(reader["Fornecedor_Observacao"]),
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
                var sql = string.Format("INSERT INTO tbfornecedores ( tipo, nomerazaosocial, sexo, logradouro, numero, complemento, bairro, telfixo, telcelular, email, codcidade, cep, cpfcnpj, rgie, dtnascimentofundacao, situacao, codcondicao, dtcadastro, dtultalteracao, apelidonomefantasia, site, observacao)" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}', '{20}', '{21}' )",
                    this.FormatString(fornecedor.tipo),
                    fornecedor.tipo == "F" ? this.FormatString(fornecedor.nomePessoa) : this.FormatString(fornecedor.razaoSocial),
                    fornecedor.tipo == "F" ? this.FormatString(fornecedor.sexo) : "",
                    this.FormatString(fornecedor.dsLogradouro),
                    fornecedor.numero,
                    this.FormatString(fornecedor.complemento),
                    this.FormatString(fornecedor.bairro),
                    this.FormatPhone(fornecedor.telefoneFixo),
                    this.FormatPhone(fornecedor.telefoneCelular),
                    this.FormatString(fornecedor.email),
                    fornecedor.Cidade.id,
                    this.FormatCEP(fornecedor.cep),
                    fornecedor.tipo == "F" ? this.FormatCPF(fornecedor.cpf) : this.FormatCNPJ(fornecedor.cnpj),
                    fornecedor.tipo == "F" ? this.FormatRG(fornecedor.rg) : fornecedor.ie,
                    fornecedor.tipo == "F" ? ( fornecedor.dtNascimento != null ? fornecedor.dtNascimento.Value.ToString("yyyy-MM-dd") : "" ) : ( fornecedor.dtFundacao != null ? fornecedor.dtFundacao.Value.ToString("yyyy-MM-dd") : ""),
                    this.FormatString(fornecedor.situacao),
                    fornecedor.CondicaoPagamento.id,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    fornecedor.tipo == "F" ? this.FormatString(fornecedor.apelidoPessoa) : this.FormatString(fornecedor.nomeFantasia),
                    this.FormatString(fornecedor.site),
                    this.FormatString(fornecedor.observacao)
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
                string sql = "UPDATE tbfornecedores SET tipo = '"
                    + this.FormatString(fornecedor.tipo) + "', " +
                    " nomerazaosocial = '" + ( fornecedor.tipo == "F" ? this.FormatString(fornecedor.nomePessoa) : this.FormatString(fornecedor.razaoSocial) )+ "'," +
                    " sexo = '" + ( fornecedor.tipo == "F" ? this.FormatString(fornecedor.sexo) : "" ) + "', " +
                    " logradouro = '" + this.FormatString(fornecedor.dsLogradouro) + "', " +
                    " numero = '" + fornecedor.numero + "', " +
                    " complemento = '" + this.FormatString(fornecedor.complemento) + "', " +
                    " bairro = '" + this.FormatString(fornecedor.bairro) + "', " +
                    " telfixo = '" + this.FormatPhone(fornecedor.telefoneFixo) + "', " +
                    " telcelular = '" + this.FormatPhone(fornecedor.telefoneCelular) + "', " +
                    " email = '" + this.FormatString(fornecedor.email) + "', " +
                    " codcidade = " + fornecedor.Cidade.id + ", " +
                    " cep = '" + this.FormatCEP(fornecedor.cep) + "', " +
                    " cpfcnpj = '" + ( fornecedor.tipo == "F" ? this.FormatCPF(fornecedor.cpf) : this.FormatCNPJ(fornecedor.cnpj) ) + "', " +
                    " rgie = '" + ( fornecedor.tipo == "F" ? this.FormatRG(fornecedor.rg) : fornecedor.ie )+ "', " +
                    " dtnascimentofundacao = '" + ( fornecedor.tipo == "F" ? ( fornecedor.dtNascimento != null ?  fornecedor.dtNascimento.Value.ToString("yyyy-MM-dd") : "" ) : ( fornecedor.dtFundacao != null ?  fornecedor.dtFundacao.Value.ToString("yyyy-MM-dd") : "") ) + "', " +
                    " situacao = '" + fornecedor.situacao.ToUpper().ToString() + "', " +
                    " codcondicao = " + fornecedor.CondicaoPagamento.id + ", " +
                    " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'," +
                    " apelidonomefantasia = '" + ( fornecedor.tipo == "F" ? this.FormatString(fornecedor.apelidoPessoa) : this.FormatString(fornecedor.nomeFantasia) ) + "', " +
                    " site = '" + this.FormatString(fornecedor.site) + "', " +
                    " observacao = '" + this.FormatString(fornecedor.observacao) + "' " +
                    " WHERE codfornecedor = " + fornecedor.codigo;
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

        public Fornecedores GetFornecedor(int? codFornecedor)
        {
            try
            {
                var model = new Models.Fornecedores();
                if (codFornecedor != null)
                {
                    OpenConnection();
                    var sql = this.Search(codFornecedor, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    var tipoPessoa = string.Empty;
                    while (reader.Read())
                    {
                        tipoPessoa = Convert.ToString(reader["Fornecedor_Tipo"]);
                        model.dsTipo = tipoPessoa == "F" ? "FÍSICA" : "JURÍDICA";
                        model.codigo = Convert.ToInt32(reader["Fornecedor_ID"]);
                        model.tipo = Convert.ToString(reader["Fornecedor_Tipo"]);
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
                        model.observacao = Convert.ToString(reader["Fornecedor_Observacao"]);
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
                        model.emailJuridica = Convert.ToString(reader["Fornecedor_Email"]);
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
                    var tipoPessoa = Convert.ToString(reader["Fornecedor_Tipo"]);
                    var fornecedor = new Select.Fornecedores.Select
                    {
                        id = Convert.ToInt32(reader["Fornecedor_ID"]),
                        text = Convert.ToString(reader["Fornecedor_NomeFantasia_ApelidoFornecedor"]),
                    };

                    list.Add(fornecedor);
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
                        tbfornecedores.codfornecedor AS Fornecedor_ID,
                        tbfornecedores.tipo AS Fornecedor_Tipo,
                        tbfornecedores.situacao AS Fornecedor_Situacao,
                        tbfornecedores.logradouro AS Fornecedor_Logradouro,
                        tbfornecedores.numero AS Fornecedor_Numero,
                        tbfornecedores.complemento AS Fornecedor_Complemento,
                        tbfornecedores.bairro AS Fornecedor_Bairro,
                        tbfornecedores.telfixo AS Fornecedor_TelefoneFixo,
                        tbfornecedores.telcelular AS Fornecedor_TelefoneCelular,
                        tbfornecedores.email AS Fornecedor_Email,
                        tbcidades.codcidade AS Fornecedor_Cidade_ID,
                        tbcidades.nomecidade AS Fornecedor_Cidade_Nome,
                        tbfornecedores.cep AS Fornecedor_CEP,
                        tbcondpagamentos.codcondicao AS Fornecedor_CondicaoPagamento_ID,
                        tbcondpagamentos.nomecondicao AS Fornecedor_CondicaoPagamento_Nome,
                        tbfornecedores.dtcadastro AS Fornecedor_DataCadastro,
                        tbfornecedores.dtultalteracao AS Fornecedor_DataUltAlteracao,
                        tbfornecedores.nomerazaosocial AS Fornecedor_RazaoSocial_NomeFornecedor,
                        tbfornecedores.apelidonomefantasia AS Fornecedor_NomeFantasia_ApelidoFornecedor,
                        tbfornecedores.sexo AS Fornecedor_Sexo,
                        tbfornecedores.cpfcnpj AS Fornecedor_CNPJ_CPF,
                        tbfornecedores.rgie AS Fornecedor_IE_RG,
                        tbfornecedores.dtnascimentofundacao AS Fornecedor_DataFundacao_DataNascimento,
	                    tbfornecedores.site AS Fornecedor_Site,
	                    tbfornecedores.observacao AS Fornecedor_Observacao
                        FROM tbfornecedores
                    INNER JOIN tbcidades on tbfornecedores.codcidade = tbcidades.codcidade
                    INNER JOIN tbcondpagamentos on tbfornecedores.codcondicao = tbcondpagamentos.codcondicao
                    " + swhere;
            return sql;
        }


    }
}