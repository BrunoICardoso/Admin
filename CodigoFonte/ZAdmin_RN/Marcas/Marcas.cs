using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.Empresas;
using ZAdmin_RN.Setores;

namespace ZAdmin_RN.Marcas
{

    public class Marcas
    {

        private int _totalMarcas;

        public int TotalMarcas
        {
            get { return _totalMarcas; }
            set { _totalMarcas = value; }
        }

        public List<Marca> RetornaMarcas(int idSetor, int idEmpresa, int pagina, int qtdRegistros)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            _totalMarcas = (from m in db.marcas
                            where ((m.excluido == false) &&
                            (m.idempresa == idEmpresa || idEmpresa == 0) &&
                            (m.empresas.idsetor == idSetor || idSetor == 0))
                            select m).Count();

            var marcas = (from m in db.marcas
                          where ((m.excluido == false) &&
                            (m.idempresa == idEmpresa || idEmpresa == 0) &&
                            (m.empresas.idsetor == idSetor || idSetor == 0))
                          select m).OrderBy(x => x.nome).Skip((pagina - 1) * qtdRegistros).Take(qtdRegistros);

            var listaDeMarcas = (from m in marcas
                                 where m.excluido == false
                                 select new Marca()
                                 {
                                     idEmpresa = m.idempresa,
                                     idMarca = m.idmarca,
                                     EmpresaNome = m.empresas.nome,
                                     Nome = m.nome,
                                     Descricao = m.descricao,
                                     UrlSite = m.urlsite,
                                     RedesSociais = (m.marcaredessociais.Select(x => new RedeSocial()
                                     {
                                         idMarca = x.idmarca,
                                         idRedeSocial = x.idredesocial,
                                         urlRedeSocial = x.urlredesocial
                                     })),
                                 }
                                 ).ToList();

            return listaDeMarcas;
        }

        public List<Marca> RetornaListaNomeMarcas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaNomeDeMarcas = (from m in db.marcas
                                     select new Marca()
                                     {
                                         idMarca = m.idmarca,
                                         Nome = m.nome
                                     }).ToList();

            return listaNomeDeMarcas;

        }

        public Marca RetonaMarca(int idMarca)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();


            var marcaDB = (from m in db.marcas
                           where m.idmarca == idMarca
                           select new Marca()
                           {
                               idMarca = m.idmarca,
                               idEmpresa = m.idempresa,
                               Nome = m.nome,
                               Descricao = m.descricao,
                               UrlSite = m.urlsite,
                               caminhoImagem = m.imagem,
                               RedesSociais = (m.marcaredessociais.Select(x => new RedeSocial() { idMarca = x.idmarca, idRedeSocial = x.idredesocial, urlRedeSocial = x.urlredesocial })),
                               cnpjs = (m.cnpjmarca.Select(x => new Cnpj() { cnpj = x.cnpj, idmarca = x.idmarca, idcnpjmarca = x.idcnpjmarca }))

                           }).FirstOrDefault();

            return marcaDB;
        }

        public int Cadastrar(Marca marca)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcaDB = new ZAdmin_DB.Model.marcas();

            marcaDB.nome = marca.Nome;
            marcaDB.descricao = marca.Descricao;
            marcaDB.urlsite = marca.UrlSite;
            marcaDB.idempresa = marca.idEmpresa;
            marcaDB.imagem = marca.caminhoImagem;
            marcaDB.excluido = false;


            foreach (var c in marca.cnpjs)
            {
                if (c.cnpj != "" && c.cnpj != null)
                {

                    var cnpjMarcaDB = new ZAdmin_DB.Model.cnpjmarca();
                    cnpjMarcaDB.cnpj = c.cnpj;
                    marcaDB.cnpjmarca.Add(cnpjMarcaDB);

                }

            }


            foreach (var m in marca.RedesSociais)
            {
                if (m.urlRedeSocial != "" && m.urlRedeSocial != null)
                {
                    var rsDB = new ZAdmin_DB.Model.marcaredessociais();
                    rsDB.idredesocial = m.idRedeSocial;
                    rsDB.urlredesocial = m.urlRedeSocial;

                    marcaDB.marcaredessociais.Add(rsDB);
                }
            }

            db.marcas.Add(marcaDB);
            db.SaveChanges();
            return marcaDB.idmarca;
        }

        public void AtualizaCaminhoImagem(Marca marca)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaDeMarcas = (from m in db.marcas
                                 where m.idmarca == marca.idMarca
                                 select m).FirstOrDefault();

            listaDeMarcas.imagem = marca.caminhoImagem;

            db.SaveChanges();

        }

        public void Editar(Marca marca)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcaDB = (from m in db.marcas
                           where m.idmarca == marca.idMarca
                           select m).FirstOrDefault();

            marcaDB.nome = marca.Nome;
            marcaDB.descricao = marca.Descricao;
            marcaDB.urlsite = marca.UrlSite;
            marcaDB.idempresa = marca.idEmpresa;
            if (marca.caminhoImagem != "" && marca.caminhoImagem != null)
            {
                marcaDB.imagem = marca.caminhoImagem;
            }

            foreach (var c in marca.cnpjs)
            {

                if (c.excluir)
                {
                    var cnpjDB = (from cnpj in marcaDB.cnpjmarca
                                  where cnpj.idcnpjmarca == c.idcnpjmarca
                                  select cnpj).FirstOrDefault();
                    if (cnpjDB != null)
                    {
                        db.cnpjmarca.Remove(cnpjDB);
                        db.SaveChanges();
                    }
                }
                else if (c.idcnpjmarca == null && (c.cnpj != null))
                {
                    var cnpjDB = new ZAdmin_DB.Model.cnpjmarca();
                    cnpjDB.cnpj = c.cnpj;
                    marcaDB.cnpjmarca.Add(cnpjDB);

                }
                else
                {
                    var cnpjDB = (from cnpj in marcaDB.cnpjmarca
                                  where cnpj.idcnpjmarca == c.idcnpjmarca
                                  select cnpj).FirstOrDefault();
                    if (cnpjDB != null)
                    {
                        cnpjDB.cnpj = c.cnpj;
                    }

                }
            }

            foreach (var s in marca.RedesSociais)
            {
                var redeDB = (from r in marcaDB.marcaredessociais
                              where r.idredesocial == s.idRedeSocial
                              select r).FirstOrDefault();
                if (redeDB == null && s.urlRedeSocial != "" && s.urlRedeSocial != null)
                {
                    var rs = new ZAdmin_DB.Model.marcaredessociais()
                    {
                        idredesocial = s.idRedeSocial,
                        urlredesocial = s.urlRedeSocial
                    };

                    marcaDB.marcaredessociais.Add(rs);

                }
                else if (redeDB != null)
                {
                    redeDB.idredesocial = s.idRedeSocial;
                    redeDB.urlredesocial = s.urlRedeSocial;

                }
            }





            db.SaveChanges();
        }


        public void Deletar(int idMarca)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcaDB = (from m in db.marcas
                           where m.idmarca == idMarca
                           select m).FirstOrDefault();

            if (marcaDB != null)
            {
                marcaDB.excluido = true;
                db.SaveChanges();
            }

        }


        public List<Empresa> retornaEmpresas()
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresas = (from e in db.empresas
                            where e.excluido == false
                            orderby e.nome ascending
                            select new Empresa()
                            {
                                idempresa = e.idempresa,
                                idsetor = e.idsetor,
                                nome = e.nome,
                                //setorNome = e.setoresempresa.nome,
                                //urlsite = e.urlsite,
                                //descricao = e.descricao,
                                //redessociais = (e.empresaredessociais.Select(x => new RedeSocial() { idempresa = x.idempresa, idredesocial = x.idredesocial, urlredesocial = x.urlredesocial })),
                                //cnpjs = (e.cnpjempresa.Select(x => new Cnpj() { idcnpjempresa = x.idcnpjempresa, cnpj = x.cnpj }))
                            }).ToList();
            return empresas;
        }


        public List<Setores.Setor> retornaSetores()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var setores = (from s in db.setoresempresa
                           where s.excluido == false
                           orderby s.nome ascending
                           select new Setores.Setor()
                           {
                               idsetoresempresa = s.idsetoresempresa,
                               nome = s.nome
                           }

                           ).ToList();

            return setores;
        }

        public bool VerificaMarcaExistente(string nomeMarca)
        {
            bool resultado;

            var db = new ZAdmin_DB.Model.zeengEntities();

            var marcaDB = (from m in db.marcas
                           where (m.excluido == false) &&
                           (m.nome.ToLower().Equals(nomeMarca.ToLower()))
                           select m);

            if (marcaDB.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }

            return resultado;
        }

        public bool VerificaCnpjExistente(string cnpjmarca)
        {
            bool resultado;
            var db = new ZAdmin_DB.Model.zeengEntities();

            var cnpjMarcaDB = (from cnpj in db.cnpjmarca
                               join marca in db.marcas on cnpj.idmarca equals marca.idmarca
                               where cnpj.cnpj.Equals(cnpjmarca) && marca.excluido == false
                               select cnpj);

            if (cnpjMarcaDB.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }

        public bool ExisteCNPJBancoEditar(string cnpjmarca, int idmarca)
        {
            bool resultado;
            var db = new ZAdmin_DB.Model.zeengEntities();

            //var cnpjEmpresaDB = (from cnpj in db.cnpjempresa
            //                     join empresa in db.empresas on cnpj.idempresa equals empresa.idempresa
            //                     where cnpj.cnpj.Equals(cnpjempresa) && empresa.excluido == false
            //                     select cnpj);

            var verificacnpj = (from cnpj in db.cnpjmarca
                                where cnpj.idmarca == idmarca && cnpj.cnpj.Equals(cnpjmarca)
                                select cnpj);


            if (verificacnpj.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }

            return resultado;
        }

    }
}
