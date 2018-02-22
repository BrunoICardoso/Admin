using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.Empresas;
using ZAdmin_RN.Marcas;

namespace ZAdmin_RN.Produtos
{
    public class Produtos
    {

        private int _totalProdutos;
        
        public int TotalProdutos
        {
            get { return _totalProdutos; }
            set { _totalProdutos = value; }
        }

        public List<Produto> RetornaProdutos(int idEmpresa, int idMarca, int pagina, int qtdRegistros)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            _totalProdutos = (from p in db.produtos
                              where ((p.excluido == false) &&
                                      (p.idmarca == idMarca || idMarca == 0) &&
                                      (p.marcas.empresas.idempresa == idEmpresa || idEmpresa == 0))
                              select p).Count();

            var produtos = (from p in db.produtos
                            where ((p.excluido == false) &&
                                   (p.idmarca == idMarca || idMarca == 0) &&
                                   (p.marcas.empresas.idempresa == idEmpresa || idEmpresa == 0))
                            select p).OrderBy(x => x.nome).Skip((pagina - 1) * qtdRegistros).Take(qtdRegistros);

            var listaDeProdutos = (from p in produtos
                                   where p.excluido == false
                                   select new Produto()
                                   {
                                       idmarca = p.idmarca,
                                       idproduto = p.idproduto,
                                       nome = p.nome,
                                       marcanome = p.marcas.nome,
                                       descricao = p.descricao,
                                       urlsite = p.urlsite,
                                       redessocias = (p.produtoredessociais.Select(x => new RedeSocial()
                                       {
                                           idProduto = x.idproduto,
                                           idRedeSocial = x.idredesocial,
                                           urlRedeSocial = x.urlredesocial
                                       }))
                                   }
                                   ).ToList();


            return listaDeProdutos;

        }

        public List<Produto> RetornaListaNomeProdutos()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaNomesDeProdutos = (from p in db.produtos
                                        select new Produto()
                                        {
                                            nome = p.nome,
                                            idproduto = p.idproduto

                                        }).ToList();
            
            return listaNomesDeProdutos;
        }

        public Produto RetornaProduto(int idProduto)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = (from p in db.produtos
                             where p.idproduto == idProduto
                             select new Produto()
                             {
                                 idproduto = p.idproduto,
                                 idmarca = p.idmarca,
                                 idempresa = p.marcas.idempresa,
                                 nome = p.nome,
                                 descricao = p.descricao,
                                 urlsite = p.urlsite,
                                 caminhoimagem = p.imagem,
                                 redessocias = (p.produtoredessociais.Select(x => new RedeSocial()
                                 {
                                     idProduto = x.idproduto,
                                     idRedeSocial = x.idredesocial,
                                     urlRedeSocial = x.urlredesocial
                                 }))


                             }).FirstOrDefault();

            return produtoDB;
        }

        public int Cadastrar(Produto produto)
            {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = new ZAdmin_DB.Model.produtos();

            produtoDB.idmarca = produto.idmarca;
            produtoDB.nome = produto.nome.Trim();
            produtoDB.urlsite = produto.urlsite.Trim();
            produtoDB.descricao = produto.descricao.Trim();
            produtoDB.excluido = false;
            produtoDB.imagem = produto.caminhoimagem.Trim();

            foreach (var p in produto.redessocias)
            {
                if(p.urlRedeSocial != null && p.urlRedeSocial != null) { 

                    var redesocialprodutoDB = new ZAdmin_DB.Model.produtoredessociais();
                    redesocialprodutoDB.idredesocial = p.idRedeSocial;
                    redesocialprodutoDB.urlredesocial = p.urlRedeSocial;
                    produtoDB.produtoredessociais.Add(redesocialprodutoDB);
                }
            }

            db.produtos.Add(produtoDB);
            db.SaveChanges();

            return produtoDB.idproduto;
            }

        public void AtualizaCaminhoImagem(Produto produto)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = (from p in db.produtos
                             where p.idproduto == produto.idproduto
                             select p).FirstOrDefault();
            if(produtoDB != null) { 
            produtoDB.imagem = produto.caminhoimagem.Trim();

            db.SaveChanges();
            }

        }

        public void Editar(Produto produto)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = (from p in db.produtos
                             where p.idproduto == produto.idproduto
                             select p).FirstOrDefault();
            produtoDB.nome = produto.nome.Trim();
            produtoDB.urlsite = produto.urlsite.Trim();
            produtoDB.descricao = produto.descricao.Trim();
            produtoDB.idmarca = produto.idmarca;
            if(produto.caminhoimagem != "" && produto.caminhoimagem != null)
            {
                produtoDB.imagem = produto.caminhoimagem.Trim();
            }

            foreach(var s in produto.redessocias)
            {
                var redesocialprodutoDB = (from r in produtoDB.produtoredessociais
                                           where r.idredesocial == s.idRedeSocial
                                           select r).FirstOrDefault();
                if(redesocialprodutoDB == null)
                {
                    var novaRedeSocialDB = new ZAdmin_DB.Model.produtoredessociais()
                    {
                        idredesocial = s.idRedeSocial,
                        urlredesocial = s.urlRedeSocial
                    };
                        produtoDB.produtoredessociais.Add(novaRedeSocialDB);
                }
                else
                {
                     redesocialprodutoDB.idredesocial = s.idRedeSocial;
                     redesocialprodutoDB.urlredesocial = s.urlRedeSocial.Trim();
                }

                db.SaveChanges();

            }

        }

        public void Deletar(int idProduto)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = (from p in db.produtos
                             where p.idproduto == idProduto
                             select p).FirstOrDefault();

            if(produtoDB != null)
            {
                produtoDB.excluido = true;
                db.SaveChanges();
            }
            
        }

        public List<Marca> RetornaMarcas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcas = (from m in db.marcas
                          where m.excluido == false
                          select new Marca()
                          {
                              idMarca = m.idmarca,
                              idEmpresa = m.idempresa,
                              Nome = m.nome
                           }).ToList();
            return marcas;

        }

        public List<Marca> RetornaMarcas(int? idEmpresa)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcas = (from m in db.marcas
                          where ((m.excluido == false) &&
                          (m.idempresa == idEmpresa || idEmpresa == 0))
                          orderby m.nome ascending
                          select new Marca()
                          {
                              idMarca = m.idmarca,
                              idEmpresa = m.idempresa,
                              Nome = m.nome
                          }).ToList();
            return marcas;

        }

        public List<Empresa> RetornaEmpresas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresas = (from e in db.empresas
                            where e.excluido == false
                            orderby e.nome ascending
                            select new Empresa()
                            {
                                idempresa = e.idempresa,
                                nome = e.nome,
                            }).ToList();

            return empresas;
        }


        public bool VerificaProdutoExistente(string nomeProduto, int idmarcaProduto)
        {
            bool resultado;

            var db = new ZAdmin_DB.Model.zeengEntities();

            var produtoDB = (from p in db.produtos
                             where (p.excluido == false) &&
                             (p.nome.ToLower().Equals(nomeProduto.ToLower())) &&
                             (p.idmarca == idmarcaProduto)
                             select p);

            if (produtoDB.Any())
            {
                resultado = true;
            }else
            {
                resultado = false;
            }
            
            return resultado;
        }

    }
}
