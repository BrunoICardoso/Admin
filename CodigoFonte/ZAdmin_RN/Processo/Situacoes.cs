using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Situacoes
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<Situacao> RetornaLista()
        {
            return (from s in db.seae_situacoes
                    select new Situacao()
                    {
                        idsituacao = s.idsituacao,
                        descricao = s.descricao
                    }
                ).ToList();
        }
    }
}
