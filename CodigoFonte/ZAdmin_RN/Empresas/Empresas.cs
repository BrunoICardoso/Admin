using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Empresas
{
    public class Empresas
    {
        private int _totalEmpresas;

        public int TotalEmpresas
        {
            get { return _totalEmpresas; }
            set { _totalEmpresas = value; }
        }

        public List<Empresa> retornaEmpresas(filtroEmpresas filtro)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            _totalEmpresas = (from e in db.empresas
                              where e.excluido == false
                              select e).Count();


            var empresasDB = (from empresa in db.empresas
                              where empresa.excluido == false &&
                              (empresa.idsetor == filtro.setor || filtro.setor == 0) &&
                              (empresa.nome.ToLower() == filtro.pesquisa.ToLower() ||
                              empresa.nome.Contains(filtro.pesquisa.ToLower()) ||
                              filtro.pesquisa == "" ||
                              filtro.pesquisa == null)

                              select new Empresa() {
                                  nome = empresa.nome,
                                  setorNome = empresa.setoresempresa.nome,
                                  urlsite = empresa.urlsite,
                                  idempresa = empresa.idempresa,
                                  descricao = empresa.descricao
                              }).ToList();


            _totalEmpresas = empresasDB.Count();


            var empresas = empresasDB.OrderBy(x => x.nome).Skip((filtro.pagina - 1) * filtro.regporpagina).Take(filtro.regporpagina).ToList();
            
            //var resultado = (empresas.Select(empresa => new Empresa()
            //{
            //    nome = empresa.nome,
            //    setorNome = empresa.setoresempresa.nome,
            //    urlsite = empresa.urlsite,
            //    idempresa = empresa.idempresa,
            //    descricao = empresa.descricao
            //})).ToList();
            

            return empresas;
        }


        public List<Empresa> RetornaEmpresasLista(int pagina, int regporpagina)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();


            _totalEmpresas = (from e in db.empresas
                              where e.excluido == false
                              select e).Count();

            var empresas = (from e in db.empresas
                            where e.excluido == false
                            select e).OrderBy(x => x.nome).Skip((pagina - 1) * regporpagina).Take(regporpagina);

            var emp = (from e in empresas

                       select new Empresa()
                       {
                           idempresa = e.idempresa,
                           idsetor = e.idsetor,
                           setorNome = e.setoresempresa.nome,
                           nome = e.nome,
                           urlsite = e.urlsite,
                           descricao = e.descricao,
                           redessociais = (e.empresaredessociais.Select(x => new RedeSocial() { idempresa = x.idempresa, idredesocial = x.idredesocial, urlredesocial = x.urlredesocial })),
                           cnpjs = (e.cnpjempresa.Select(x => new Cnpj() { idcnpjempresa = x.idcnpjempresa, cnpj = x.cnpj }))


                       }).ToList();

            return emp;

        }

        public List<Empresa> RetornaListaNomeDeEmpresas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var listaNomeEmpresas = (from e in db.empresas
                                     orderby e.nome ascending
                                     select new Empresa()
                                     {
                                         idempresa = e.idempresa,
                                         nome = e.nome
                                     }).ToList();

            return listaNomeEmpresas;
        }

        public List<Setor> RetornaSetores()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var setores = (from s in db.setoresempresa
                           select new Setor()
                           {
                               idsetor = s.idsetoresempresa,
                               nome = s.nome
                           }

                           ).ToList();

            return setores;

        }

        public Empresa retornaEmpresa(int id)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresaDB = (from e in db.empresas
                             where e.idempresa == id
                             select new Empresa()
                             {
                                 nome = e.nome,
                                 idempresa = e.idempresa,
                                 urlsite = e.urlsite,
                                 idsetor = e.idsetor,
                                 descricao = e.descricao,
                                 caminhoImagem = e.imagem,
                                 redessociais = (e.empresaredessociais.Select(x => new RedeSocial() { idempresa = x.idempresa, idredesocial = x.idredesocial, urlredesocial = x.urlredesocial })),
                                 cnpjs = (e.cnpjempresa.Select(x => new Cnpj() { idcnpjempresa = x.idcnpjempresa, cnpj = x.cnpj }))

                             }).FirstOrDefault();


            return empresaDB;

        }

        public int Cadastrar(Empresa empresa)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresaDB = new ZAdmin_DB.Model.empresas();

            empresaDB.nome = empresa.nome;
            empresaDB.idsetor = empresa.idsetor;
            empresaDB.urlsite = empresa.urlsite != null ? empresa.urlsite.Trim() : null;
            empresaDB.descricao = empresa.descricao != null ? empresa.descricao.Trim() : null;
            empresaDB.imagem = empresa.caminhoImagem != null ? empresa.caminhoImagem.Trim() : null;

            foreach (var c in empresa.cnpjs)
            {
                if (c.cnpj != "" && c.cnpj != null)
                {
                    var cnpjDB = new ZAdmin_DB.Model.cnpjempresa();
                    cnpjDB.cnpj = c.cnpj;
                    empresaDB.cnpjempresa.Add(cnpjDB);
                }

            }

            foreach (var r in empresa.redessociais)
            {
                if (r.urlredesocial != "" && r.urlredesocial != null)
                {
                    var rede = new ZAdmin_DB.Model.empresaredessociais()
                    {
                        idredesocial = r.idredesocial,
                        urlredesocial = r.urlredesocial
                    };
                    empresaDB.empresaredessociais.Add(rede);
                }
            }

            db.empresas.Add(empresaDB);
            db.SaveChanges();
            return empresaDB.idempresa;
        }

        public void AtualizaCaminhoImagem(Empresa empresa)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var emp = (from e in db.empresas
                       where e.idempresa == empresa.idempresa
                       select e).FirstOrDefault();

            emp.imagem = empresa.caminhoImagem != null ? empresa.caminhoImagem.Trim() : null;

            db.SaveChanges();

        }

        public void Editar(Empresa empresa)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresaDB = (from e in db.empresas
                             where e.idempresa == empresa.idempresa
                             select e).FirstOrDefault();

            empresaDB.nome = empresa.nome;
            empresaDB.idsetor = empresa.idsetor;
            empresaDB.descricao = empresa.descricao != null ? empresa.descricao.Trim() : null;
            empresaDB.urlsite = empresa.urlsite != null ? empresa.urlsite.Trim() : null;
            if (empresa.caminhoImagem != "" && empresa.caminhoImagem != null)
            {
                empresaDB.imagem = empresa.caminhoImagem.Trim();
            }


            foreach (var c in empresa.cnpjs)
            {
                if (c.excluir)
                {
                    var cnpjDB = (from cnpj in empresaDB.cnpjempresa
                                  where cnpj.idcnpjempresa == c.idcnpjempresa
                                  select cnpj).FirstOrDefault();

                    if (cnpjDB != null)
                    {           
                        db.cnpjempresa.Remove(cnpjDB);
                        db.SaveChanges();
                    }

                }
                else if (c.idcnpjempresa == null && (c.cnpj != null || c.cnpj != ""))
                {
                    var cnpjDB = new ZAdmin_DB.Model.cnpjempresa();
                    cnpjDB.cnpj = c.cnpj;
                    empresaDB.cnpjempresa.Add(cnpjDB);
                }
                else
                {
                    var cnpjDB = (from cnpj in empresaDB.cnpjempresa
                                  where cnpj.idcnpjempresa == c.idcnpjempresa
                                  select cnpj).FirstOrDefault();

                    if (c.cnpj != "" && c.cnpj != null)
                    {
                        if (cnpjDB != null)
                        {
                            cnpjDB.cnpj = c.cnpj;
                        }
                    }
                }
            }




            foreach (var r in empresa.redessociais)
            {
                var redeDB = (from rede in empresaDB.empresaredessociais
                              where rede.idredesocial == r.idredesocial
                              select rede).FirstOrDefault();

                if (redeDB == null && r.urlredesocial != "" && r.urlredesocial != null)
                {

                    var rede = new ZAdmin_DB.Model.empresaredessociais()
                    {
                        idredesocial = r.idredesocial,
                        urlredesocial = r.urlredesocial
                    };
                    empresaDB.empresaredessociais.Add(rede);
                }

                if (redeDB != null)
                {
                    redeDB.idredesocial = r.idredesocial;
                    redeDB.urlredesocial = r.urlredesocial;
                }


            }
            

            db.SaveChanges();
        }

        public void Deletar(int idempresa)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresaDB = (from e in db.empresas
                             where e.idempresa == idempresa
                             select e).FirstOrDefault();

            if (empresaDB != null)
            {
                empresaDB.excluido = true;
                db.SaveChanges();
            }

        }

        public bool VerificaEmpresaExistente(string nomeEmpresa)
        {

            bool resultado;

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresaDB = (from e in db.empresas
                             where (e.excluido == false) &&
                             (e.nome.ToLower().Equals(nomeEmpresa.ToLower()))
                             select e);


            if (empresaDB.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }

            return resultado;
        }

        public bool verificaCnpjExistente(string cnpjempresa)
        {
            bool resultado;
            var db = new ZAdmin_DB.Model.zeengEntities();

            var cnpjEmpresaDB = (from cnpj in db.cnpjempresa
                                 join empresa in db.empresas on cnpj.idempresa equals empresa.idempresa
                                 where cnpj.cnpj.Equals(cnpjempresa) && empresa.excluido == false
                                 select cnpj);

            if (cnpjEmpresaDB.Any())
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }

            return resultado;
        }

        public bool ExisteCNPJBancoEditar(string cnpjempresa, int idempresa)
        {
            bool resultado;
            var db = new ZAdmin_DB.Model.zeengEntities();

            //var cnpjEmpresaDB = (from cnpj in db.cnpjempresa
            //                     join empresa in db.empresas on cnpj.idempresa equals empresa.idempresa
            //                     where cnpj.cnpj.Equals(cnpjempresa) && empresa.excluido == false
            //                     select cnpj);

            var verificacnpj = (from cnpj in db.cnpjempresa
                                where cnpj.idempresa == idempresa && cnpj.cnpj.Equals(cnpjempresa)
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


        public List<Combos.ItemCombo> RetornaItensComboEmpresas()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresasDB = (from empresa in db.empresas
                          where empresa.excluido == false
                          orderby empresa.nome ascending
                          select new Combos.ItemCombo()
                          {
                              idItem = empresa.idempresa,
                              nome = empresa.nome
                          }).ToList();


            return empresasDB;
        }


        public List<Combos.ItemCombo> RetornaEmpresasSetor(int idSetor)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var empresas = (from empresa in db.empresas
                            where empresa.excluido == false && (empresa.idsetor == idSetor || idSetor == 0)
                            select new Combos.ItemCombo()
                            {
                                idItem = empresa.idempresa,
                                nome = empresa.nome
                            }).ToList();

            return empresas;
        }

    }
}
