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
    public class DAOClientes : Sistema.DAO.DAO
    {

        public List<Clientes> GetClientes()
        {
            try
            {
                var sql = this.Search(null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Clientes>();
                var tipoPessoa = string.Empty;
                while (reader.Read())
                {
                    tipoPessoa = Convert.ToString(reader["Cliente_Tipo"]);
                    var Clientes = new Clientes
                    {
                        codigo = Convert.ToInt32(reader["Cliente_ID"]),
                        tipo = Util.FormatFlag.TipoPessoa(Convert.ToString(reader["Cliente_Tipo"])),
                        situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Cliente_Situacao"])),
                        dsLogradouro = Convert.ToString(reader["Cliente_Logradouro"]),
                        numero = Convert.ToString(reader["Cliente_Numero"]),
                        complemento = Convert.ToString(reader["Cliente_Complemento"]),
                        bairro = Convert.ToString(reader["Cliente_Bairro"]),
                        telefoneFixo = Convert.ToString(reader["Cliente_TelefoneFixo"]),
                        telefoneCelular = Convert.ToString(reader["Cliente_TelefoneCelular"]),
                        email = Convert.ToString(reader["Cliente_Email"]),
                        Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_Cidade_ID"]),
                            text = Convert.ToString(reader["Cliente_Cidade_Nome"])
                        },
                        cep = Convert.ToString(reader["Cliente_CEP"]),
                        CondicaoPagamento = new Select.CondicaoPagamento.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_CondicaoPagamento_ID"]),
                            text = Convert.ToString(reader["Cliente_CondicaoPagamento_Nome"])
                        },
                        dtCadastro = Convert.ToDateTime(reader["Cliente_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Cliente_DataUltAlteracao"]),
                        //Física
                        nomePessoa = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_RazaoSocial_NomeCliente"]) : "",
                        apelidoPessoa = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_NomeFantasia_ApelidoCliente"]) : "",
                        sexo = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_Sexo"]) : "",
                        cpf = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_CNPJ_CPF"]) : "",
                        rg = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_IE_RG"]) : "",
                        dtNascimento = tipoPessoa == "F" ? Convert.ToDateTime(reader["Cliente_DataFundacao_DataNascimento"]) : (DateTime?)null,
                        //Jurídica
                        razaoSocial = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_RazaoSocial_NomeCliente"]) : "",
                        nomeFantasia = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_NomeFantasia_ApelidoCliente"]) : "",
                        cnpj = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_CNPJ_CPF"]) : "",
                        ie = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_IE_RG"]) : "",
                        dtFundacao = tipoPessoa == "J" ? Convert.ToDateTime(reader["Cliente_DataFundacao_DataNascimento"]) : (DateTime?)null,
                    };

                    list.Add(Clientes);
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

        public bool Insert(Models.Clientes cliente)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbclientes ( tipo, nomerazaosocial, sexo, logradouro, numero, complemento, bairro, telfixo, telcelular, email, codcidade, cep, cpfcnpj, rgie, dtnascimentofundacao, situacao, codcondicao, dtcadastro, dtultalteracao, apelidonomefantasia)" +
                    "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', {10}, '{11}', '{12}', '{13}', '{14}', '{15}', {16}, '{17}', '{18}', '{19}' )",
                    this.FormatString(cliente.tipo),
                    cliente.tipo == "F" ? this.FormatString(cliente.nomePessoa) : this.FormatString(cliente.razaoSocial),
                    cliente.tipo == "F" ? this.FormatString(cliente.sexo) : "",
                    this.FormatString(cliente.dsLogradouro),
                    this.FormatString(cliente.numero),
                    this.FormatString(cliente.complemento),
                    this.FormatString(cliente.bairro),
                    this.FormatPhone(cliente.telefoneFixo),
                    this.FormatPhone(cliente.telefoneCelular),
                    this.FormatString(cliente.email),
                    cliente.Cidade.id,
                    this.FormatCEP(cliente.cep),
                    cliente.tipo == "F" ? this.FormatCPF(cliente.cpf) : this.FormatCNPJ(cliente.cnpj),
                    cliente.tipo == "F" ? this.FormatRG(cliente.rg) : cliente.ie,
                    cliente.tipo == "F" ? cliente.dtNascimento.Value.ToString("yyyy-MM-dd") : cliente.dtFundacao.Value.ToString("yyyy-MM-dd"),
                    cliente.situacao.ToUpper().Trim(),
                    cliente.CondicaoPagamento.id,
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    DateTime.Now.ToString("yyyy-MM-dd"),
                    cliente.tipo == "F" ? this.FormatString(cliente.apelidoPessoa) : this.FormatString(cliente.nomeFantasia)
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

        public bool Update(Models.Clientes cliente)
        {
            try
            {
                string sql = "UPDATE tbclientes SET tipo = '" +
                    this.FormatString(cliente.tipo) + "', " +
                    " nomerazaosocial = '" + (cliente.tipo == "F" ? this.FormatString(cliente.nomePessoa) : this.FormatString(cliente.razaoSocial)) + "', " +
                    " sexo = '" + (cliente.tipo == "F" ? this.FormatString(cliente.sexo) : "") + "', " +
                    " logradouro = '" + this.FormatString(cliente.dsLogradouro) + "', " +
                    " numero = '" + cliente.numero + "', " +
                    " complemento ='" + this.FormatString(cliente.complemento) + "', " +
                    " bairro = '" + this.FormatString(cliente.bairro) + "', " +
                    " telfixo = '" + this.FormatPhone(cliente.telefoneFixo) + "', " +
                    " telcelular = '" + this.FormatPhone(cliente.telefoneCelular) + "', " +
                    " email = '" + this.FormatString(cliente.email) + "', " +
                    " codcidade = " + cliente.Cidade.id + ", " +
                    " cep = '" + this.FormatCEP(cliente.cep) + "', " +
                    " cpfcnpj = '" + ( cliente.tipo == "F" ? this.FormatCPF(cliente.cpf) : this.FormatCNPJ(cliente.cnpj) )+ "', " +
                    " rgie = '" + ( cliente.tipo == "F" ? this.FormatRG(cliente.rg) : cliente.ie )+ "'," +
                    " dtnascimentofundacao = '" + (cliente.tipo == "F" ? cliente.dtNascimento.Value.ToString("yyyy-MM-dd") : cliente.dtFundacao.Value.ToString("yyyy-MM-dd")) + "', " +
                    " situacao = '" + this.FormatString(cliente.situacao) + "', " +
                    " codcondicao = " + cliente.CondicaoPagamento.id + ", " +
                    " dtultalteracao = '" + ( DateTime.Now.ToString("yyyy-MM-dd") )+ "'," +
                    " apelidonomefantasia = '" + ( cliente.tipo == "F" ? this.FormatString(cliente.apelidoPessoa) : this.FormatString(cliente.nomeFantasia) )+ "'" +
                    " WHERE codcliente = " + cliente.codigo;
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

        public Clientes GetCliente(int? codCliente)
        {
            try
            {
                var model = new Models.Clientes();
                if (codCliente != null)
                {
                    OpenConnection();
                    var sql = this.Search(codCliente, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    var tipoPessoa = string.Empty;
                    while (reader.Read())
                    {
                        //dsTipo = tipoPessoa == "F" ? "FÍSICA" : "JURÍDICA",
                        tipoPessoa = Convert.ToString(reader["Cliente_Tipo"]);
                        model.dsTipo = tipoPessoa == "F" ? "FÍSICA" : "JURÍDICA";

                        model.codigo = Convert.ToInt32(reader["Cliente_ID"]);
                        model.tipo = Convert.ToString(reader["Cliente_Tipo"]);
                        model.situacao = Convert.ToString(reader["Cliente_Situacao"]);
                        model.dsLogradouro = Convert.ToString(reader["Cliente_Logradouro"]);
                        model.numero = Convert.ToString(reader["Cliente_Numero"]);
                        model.complemento = Convert.ToString(reader["Cliente_Complemento"]);
                        model.bairro = Convert.ToString(reader["Cliente_Bairro"]);
                        model.telefoneFixo = Convert.ToString(reader["Cliente_TelefoneFixo"]);
                        model.telefoneCelular = Convert.ToString(reader["Cliente_TelefoneCelular"]);
                        model.email = Convert.ToString(reader["Cliente_Email"]);
                        model.Cidade = new Select.Cidades.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_Cidade_ID"]),
                            text = Convert.ToString(reader["Cliente_Cidade_Nome"])
                        };
                        model.cep = Convert.ToString(reader["Cliente_CEP"]);
                        model.CondicaoPagamento = new Select.CondicaoPagamento.Select
                        {
                            id = Convert.ToInt32(reader["Cliente_CondicaoPagamento_ID"]),
                            text = Convert.ToString(reader["Cliente_CondicaoPagamento_Nome"])
                        };
                        model.dtCadastro = Convert.ToDateTime(reader["Cliente_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Cliente_DataUltAlteracao"]);
                        //Física
                        model.nomePessoa = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_RazaoSocial_NomeCliente"]) : "";
                        model.apelidoPessoa = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_NomeFantasia_ApelidoCliente"]) : "";
                        model.sexo = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_Sexo"]) : "";
                        model.cpf = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_CNPJ_CPF"]) : "";
                        model.rg = tipoPessoa == "F" ? Convert.ToString(reader["Cliente_IE_RG"]) : "";
                        model.dtNascimento = tipoPessoa == "F" ? Convert.ToDateTime(reader["Cliente_DataFundacao_DataNascimento"]) : (DateTime?)null;
                        //Jurídica
                        model.razaoSocial = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_RazaoSocial_NomeCliente"]) : "";
                        model.nomeFantasia = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_NomeFantasia_ApelidoCliente"]) : "";
                        model.cnpj = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_CNPJ_CPF"]) : "";
                        model.ie = tipoPessoa == "J" ? Convert.ToString(reader["Cliente_IE_RG"]) : "";
                        model.dtFundacao = tipoPessoa == "J" ? Convert.ToDateTime(reader["Cliente_DataFundacao_DataNascimento"]) : (DateTime?)null;
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

        public bool Delete(int? codCliente)
        {
            try
            {
                string sql = "DELETE FROM tbclientes WHERE codcliente = " + codCliente;
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

        public List<Select.Clientes.Select> GetClientesSelect(int? id, string filter)
        {
            try
            {

                var sql = this.Search(id, filter);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Clientes.Select>();
                while (reader.Read())
                {
                    var cliente = new Select.Clientes.Select
                    {
                        id = Convert.ToInt32(reader["Cliente_ID"]),
                        text = Convert.ToString(reader["Cliente_Nome"]),
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
                swhere = " WHERE tbclientes.codcliente = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbclientes.nomerazaosocial LIKE'%" + word + "%'";
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