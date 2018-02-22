using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Empresas
{
    public class EmpresaExpressoes
    {

        public List<EmpresaExpressao> RetornaListaDeExpressoes(int idEmpresa)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();


            var expressoes = (from e in db.empresa_expressoes_noticias
                              where e.idempresa == idEmpresa &&
                              e.excluido == false
                              select new EmpresaExpressao()
                              {
                                  idExpressao = e.idexpressoesnoticias,
                                  idEmpresa = e.idempresa,
                                  expressao = e.expressao,
                                  ativo = e.ativo
                              }).ToList();

            return expressoes;
        }

        public int Cadastrar(EmpresaExpressao expressao)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            if(expressao.idExpressao == null || expressao.idExpressao == 0)
            {

                var expressaoDB = new ZAdmin_DB.Model.empresa_expressoes_noticias();

                expressaoDB.idempresa = expressao.idEmpresa;
                expressaoDB.expressao = expressao.expressao != null ? expressao.expressao.Trim() : null;
                expressaoDB.ativo = true;
                expressaoDB.excluido = false;

                db.empresa_expressoes_noticias.Add(expressaoDB);
                db.SaveChanges();

                return expressaoDB.idexpressoesnoticias;

            }else
            {
                var expressaoDB = (from e in db.empresa_expressoes_noticias
                                   where e.idexpressoesnoticias == expressao.idExpressao
                                   select e).FirstOrDefault();
                expressaoDB.ativo = true;
                expressaoDB.expressao = expressao.expressao != null ? expressao.expressao.Trim() : null;
                db.SaveChanges();
                return expressaoDB.idexpressoesnoticias;
            }
        }

        public void Excluir(int idExpressao)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var expressaoDB = (from e in db.empresa_expressoes_noticias
                               where e.idexpressoesnoticias == idExpressao
                               select e).FirstOrDefault();

            expressaoDB.excluido = true;

            db.SaveChanges();
        }


    }
}
