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
    public class DAOProdutos : Sistema.DAO.DAO
    {

        public List<Produtos> GetProdutos()
        {
            try
            {
                var sql = this.Search(null, null, null);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Produtos>();

                while (reader.Read())
                {
                    var produto = new Produtos
                    {
                        codigo = Convert.ToInt32(reader["Produto_ID"]),
                        situacao = Util.FormatFlag.Situacao(Convert.ToString(reader["Produto_Situacao"])),
                        nomeProduto = Convert.ToString(reader["Produto_Nome"]),
                        unidade = Convert.ToString(reader["Produto_Unidade"]),
                        largura = Convert.ToString(reader["Produto_Largura"]),
                        Grupo = new Select.Grupos.Select
                        {
                            id = Convert.ToInt32(reader["Produto_Grupo_ID"]),
                            text = Convert.ToString(reader["Produto_Grupo_Nome"])
                        },
                        Fornecedor = new Select.Fornecedores.Select
                        {
                            id = Convert.ToInt32(reader["Produto_Fornecedor_ID"]),
                            text = Convert.ToString(reader["Produto_Fornecedor_Nome"])
                        },
                        ncm = Convert.ToString(reader["Produto_NCM"]),
                        qtEstoque = Convert.ToDecimal(reader["Produto_QtEstoque"]),
                        vlCusto = Convert.ToDecimal(reader["Produto_VlCusto"]),
                        vlUltCompra = Convert.ToDecimal(reader["Produto_VlUltimaCompra"]),
                        vlVenda = Convert.ToDecimal(reader["Produto_VlVenda"]),
                        observacao = Convert.ToString(reader["Produto_Observacao"]),
                        dtCadastro = Convert.ToDateTime(reader["Produto_DataCadastro"]),
                        dtUltAlteracao = Convert.ToDateTime(reader["Produto_DataUltAlteracao"])                        
                    };

                    list.Add(produto);
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

        public bool Insert(Models.Produtos produto)
        {
            try
            {
                var sql = string.Format("INSERT INTO tbprodutos ( situacao, nomeproduto, unidade, largura, codgrupo, codfornecedor, ncm, qtestoque, vlcusto, vlultcompra, vlvenda, observacao, cfop, dtcadastro, dtultalteracao )" +
                    " VALUES ('{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}', {7}, {8}, {9}, {10}, '{11}', '{12}', '{13}', '{14}' )",
                    this.FormatString(produto.situacao),
                    this.FormatString(produto.nomeProduto),
                    produto.unidade,
                    produto.largura,
                    produto.Grupo.id,
                    produto.Fornecedor.id,
                    produto.ncm,
                    produto.qtEstoque != null ? produto.qtEstoque : 0,
                    0,
                    produto.vlUltCompra != null ? produto.vlUltCompra : 0,
                    produto.vlVenda.ToString().Replace(",", "."),
                    !string.IsNullOrWhiteSpace(produto.observacao) ? this.FormatString(produto.observacao) : "",
                    produto.cfop,
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

        public bool Update(Models.Produtos produto)
        {
            try
            {
                //string sql = "teste";
                var sql = "UPDATE tbprodutos SET situacao = '"
                    + this.FormatString(produto.situacao) + "', " +
                " nomeproduto = '" + this.FormatString(produto.nomeProduto) + "', " +
                " unidade = '" + produto.unidade + "', " +
                " largura = '" + produto.largura + "', " +
                " codgrupo = " + produto.Grupo.id + ", " +
                " codfornecedor = " + produto.Fornecedor.id + ", " +
                " ncm = '" + produto.ncm + "', " +
                " vlcusto = " + produto.vlCusto.ToString().Replace(",", ".") + ", " +
                " vlvenda = " + produto.vlVenda.ToString().Replace(",", ".") + ", " +
                " observacao = '" + this.FormatString(produto.observacao) + "', " +
                " cfop = '" + produto.cfop + "', " +
                " dtultalteracao = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'" +
                " WHERE codproduto = " + produto.codigo;
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

        public Produtos GetProduto(int? codProduto)
        {
            try
            {
                var model = new Models.Produtos();
                if (codProduto != null)
                {
                    OpenConnection();
                    var sql = this.Search(codProduto, null, null);
                    SqlQuery = new SqlCommand(sql, con);
                    reader = SqlQuery.ExecuteReader();
                    while (reader.Read())
                    {
                        model.codigo = Convert.ToInt32(reader["Produto_ID"]);
                        model.situacao = Convert.ToString(reader["Produto_Situacao"]);
                        model.nomeProduto = Convert.ToString(reader["Produto_Nome"]);
                        model.unidade = Convert.ToString(reader["Produto_Unidade"]);
                        model.largura = Convert.ToString(reader["Produto_Largura"]);
                        model.Grupo = new Select.Grupos.Select
                        {
                            id = Convert.ToInt32(reader["Produto_Grupo_ID"]),
                            text = Convert.ToString(reader["Produto_Grupo_Nome"])
                        };
                        model.Fornecedor = new Select.Fornecedores.Select
                        {
                            id = Convert.ToInt32(reader["Produto_Fornecedor_ID"]),
                            text = Convert.ToString(reader["Produto_Fornecedor_Nome"])
                        };
                        model.ncm = Convert.ToString(reader["Produto_NCM"]);
                        model.cfop = Convert.ToString(reader["Produto_CFOP"]);
                        model.qtEstoque = Convert.ToDecimal(reader["Produto_QtEstoque"]);
                        model.vlCusto = Convert.ToDecimal(reader["Produto_VlCusto"]);
                        model.vlUltCompra = Convert.ToDecimal(reader["Produto_VlUltimaCompra"]);
                        model.vlVenda = Convert.ToDecimal(reader["Produto_VlVenda"]);
                        model.observacao = Convert.ToString(reader["Produto_Observacao"]);
                        model.dtCadastro = Convert.ToDateTime(reader["Produto_DataCadastro"]);
                        model.dtUltAlteracao = Convert.ToDateTime(reader["Produto_DataUltAlteracao"]);
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

        public bool Delete(int? codProduto)
        {
            try
            {
                string sql = "DELETE FROM tbprodutos WHERE codproduto = " + codProduto;
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

        public List<Select.Produtos.Select> GetProdutoSelect(int? id, string filter, int? idFornecedor)
        {
            try
            {

                var sql = this.Search(id, filter, idFornecedor);
                OpenConnection();
                SqlQuery = new SqlCommand(sql, con);
                reader = SqlQuery.ExecuteReader();
                var list = new List<Select.Produtos.Select>();

                while (reader.Read())
                {
                    var produto = new Select.Produtos.Select
                    {
                        id = Convert.ToInt32(reader["Produto_ID"]),
                        text = Convert.ToString(reader["Produto_Nome"]),
                        vlVenda = Convert.ToDecimal(reader["Produto_VlVenda"]),
                        unidade = Convert.ToString(reader["Produto_Unidade"])
                    };
                    list.Add(produto);
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

        private string Search(int? id, string filter, int? idFornecedor)
        {
            var sql = string.Empty;
            var swhere = string.Empty;
            if (id != null)
            {
                swhere += " AND codproduto = " + id;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                var filterQ = filter.Split(' ');
                foreach (var word in filterQ)
                {
                    swhere += " OR tbprodutos.nomeproduto LIKE'%" + word + "%'";
                }
            }
            if (idFornecedor != null)
            {
                swhere += " AND tbfornecedores.codfornecedor = " + idFornecedor; 
            }
            if (!string.IsNullOrEmpty(swhere))
                swhere = " WHERE " + swhere.Remove(0, 4);
            sql = @"
                    SELECT
                        tbprodutos.codproduto AS Produto_ID,
                        tbprodutos.situacao AS Produto_Situacao,
                        tbprodutos.nomeproduto AS Produto_Nome,
                        tbprodutos.unidade AS Produto_Unidade,
                        tbprodutos.largura AS Produto_Largura,
                        tbgrupos.codgrupo AS Produto_Grupo_ID,
                        tbgrupos.nomegrupo AS Produto_Grupo_Nome,
                        tbfornecedores.codfornecedor AS Produto_Fornecedor_ID,
                        tbfornecedores.nomerazaosocial AS Produto_Fornecedor_Nome,
                        tbprodutos.ncm AS Produto_NCM,
                        tbprodutos.qtestoque AS Produto_QtEstoque,
                        tbprodutos.vlcusto AS Produto_VlCusto,
                        tbprodutos.vlultcompra AS Produto_VlUltimaCompra,
                        tbprodutos.vlvenda AS Produto_VlVenda,
                        tbprodutos.observacao AS Produto_Observacao,
                        tbprodutos.cfop AS Produto_CFOP,
                        tbprodutos.dtcadastro AS Produto_DataCadastro,
                        tbprodutos.dtultalteracao AS Produto_DataUltAlteracao
                    FROM tbprodutos
                    INNER JOIN tbgrupos on tbprodutos.codgrupo = tbgrupos.codgrupo
                    INNER JOIN tbfornecedores on tbprodutos.codfornecedor = tbfornecedores.codfornecedor" + swhere;
            return sql;
        }


    }
}