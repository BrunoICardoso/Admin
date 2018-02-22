using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class PromocaoArquivos
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<PromocaoArquivo> retornaArquivosRegulamento(int idPromocao)
        {
            return (
                        from a in db.promo_regulamentoarquivos
                        where a.excluido == false &&
                        a.idpromocao == idPromocao
                        orderby a.idregulamentoarquivos descending
                        select new PromocaoArquivo()
                        {
                            id = a.idregulamentoarquivos,
                            idpromocao = a.idpromocao,
                            nome = a.nome,
                            tipo = a.tipo,
                            excluido = a.excluido
                        }
                ).ToList();
        }

        public void ExcluirArquivoRegulamento(ZAdmin_RN.Promocao.PromocaoArquivo arquivo)
        {
            try
            {
                var arq = db.promo_regulamentoarquivos.Where(x => x.idregulamentoarquivos == arquivo.id).FirstOrDefault();
                if (arq != null)
                {
                    arq.excluido = true;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void salvarArquivoRegulamento(PromocaoArquivo promoArq)
        {
            var arqDB = new ZAdmin_DB.Model.promo_regulamentoarquivos();
            arqDB.idpromocao = promoArq.idpromocao;
            arqDB.nome = promoArq.nome;
            arqDB.tipo = promoArq.tipo;
            arqDB.excluido = promoArq.excluido;
            db.promo_regulamentoarquivos.Add(arqDB);
            db.SaveChanges();
        }

        public List<PromocaoArquivo> retornaArquivosRelacionado(int idPromocao)
        {
            return (
                        from a in db.promo_arquivos
                        where a.excluido == false &&
                        a.idpromocao == idPromocao
                        orderby a.idpromoarquivo descending
                        select new PromocaoArquivo()
                        {
                            id = a.idpromoarquivo,
                            idpromocao = a.idpromocao,
                            nome = a.nome,
                            tipo = a.tipo,
                            excluido = a.excluido,
                            url = a.url
                        }
                ).ToList();
        }

        public void ExcluirArquivoRelacionado(ZAdmin_RN.Promocao.PromocaoArquivo arquivo)
        {
            try
            {
                var arq = db.promo_arquivos.Where(x => x.idpromoarquivo == arquivo.id).FirstOrDefault();
                if (arq != null)
                {
                    arq.excluido = true;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void salvarArquivoRelacionado(PromocaoArquivo promoArq)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var arqDB = new ZAdmin_DB.Model.promo_arquivos();

            if (db.promo_arquivos.Where(w => w.idpromocao == promoArq.idpromocao).Select(s => s.nome).First() != promoArq.nome && promoArq.tipo == "Imagem")
            {
                arqDB.idpromocao = promoArq.idpromocao;
                arqDB.nome = promoArq.nome;
                arqDB.tipo = promoArq.tipo;
                arqDB.excluido = promoArq.excluido;
                db.promo_arquivos.Add(arqDB);
                db.SaveChanges();
            }


            //var idarquivo = db.promo_arquivos.Where(x => x.idpromocao == promoArq.idpromocao && x.nome == promoArq.nome).Select(a => a.idpromoarquivo).ToList();            
        }

        public void salvarLinkRelacionado(PromocaoArquivo promoLink)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var linkDB = new ZAdmin_DB.Model.promo_arquivos();
            linkDB.idpromocao = promoLink.idpromocao;
            linkDB.nome = Path.GetFileName(new Uri(promoLink.url).AbsolutePath);
            linkDB.tipo = "Link";
            linkDB.excluido = false;
            linkDB.url = promoLink.url;

            db.promo_arquivos.Add(linkDB);
            db.SaveChanges();

            //string NomeArquivo = promoLink.idpromocao + "_" + linkDB

            //linkDB.nome = Path.GetFileName(new Uri(promoLink.url).AbsolutePath);
            //db.promo_arquivos.Add(linkDB);
            //db.SaveChanges();

        }
    }
}
