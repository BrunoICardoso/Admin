using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Arquivos
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<Arquivo> RetornaLista(int idProcesso)
        {

            return (
                        from a in db.seae_arquivos_proc
                            where a.idprocesso == idProcesso &&
                            a.excluido == false                        
                        select new Arquivo()
                        {
                            coordenacao = a.coordenacao,
                            idarquivo = a.idarquivo,
                            idprocesso = a.idprocesso,
                            link = a.link,
                            nomearquivo = a.nomearquivo,
                            numdoc = a.numdoc,
                            situacao = a.situacao
                        }
                    ).ToList();
        }

    }
}
