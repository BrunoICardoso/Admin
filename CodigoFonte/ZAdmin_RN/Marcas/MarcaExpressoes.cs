using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Marcas
{
    public class MarcaExpressoes
    {

        public List<MarcaExpressao> RetornaListaDeExpressoes(int idMarca)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();


            var expressoes = (from e in db.marca_expressoes_noticias
                              where e.idmarca == idMarca &&
                                     e.excluido == false
                              select new MarcaExpressao()
                              {
                                  idExpressao = e.idexpressoesnoticias,
                                  idMarca = e.idmarca,
                                  expressao = e.expressao,
                                  ativo = e.ativo
                              }).ToList();

            return expressoes;
        }

        public int Cadastrar(MarcaExpressao expressao)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            if (expressao.idExpressao == null || expressao.idExpressao == 0)
            {

                var expressaoDB = new ZAdmin_DB.Model.marca_expressoes_noticias();

                expressaoDB.idmarca = expressao.idMarca;
                expressaoDB.expressao = expressao.expressao != null ? expressao.expressao.Trim() : null;
                expressaoDB.ativo = true;
                expressaoDB.excluido = false;

                db.marca_expressoes_noticias.Add(expressaoDB);
                db.SaveChanges();
                return expressaoDB.idexpressoesnoticias;

            }
            else
            {

                var expressaoDB = (from e in db.marca_expressoes_noticias
                                   where e.idexpressoesnoticias == expressao.idExpressao
                                   select e).FirstOrDefault();

                expressaoDB.expressao = expressao.expressao != null ? expressao.expressao.Trim() : null;
                expressaoDB.ativo = expressao.ativo;
                db.SaveChanges();
                return expressaoDB.idexpressoesnoticias;
            }
        }
        
        public void Excluir(int idExpressao)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var expressaoDB = (from e in db.marca_expressoes_noticias
                               where e.idexpressoesnoticias == idExpressao
                               select e).FirstOrDefault();

            expressaoDB.excluido = true;

            db.SaveChanges();
        }
        
    }
}
