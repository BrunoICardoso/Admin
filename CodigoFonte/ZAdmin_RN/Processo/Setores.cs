using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Setores
    {
        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<Setor> RetornaSetores()
        {
            return (from s in db.seae_setores_proc
                    orderby s.descricao ascending
                    select new Setor()
                    {
                        codsetor = s.codsetor,
                        descricao = s.descricao,
                        idsetor = s.idsetor
                    }
                    ).ToList();
        }

    }
}
