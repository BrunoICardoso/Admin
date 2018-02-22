using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Subsetores
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<Subsetor> RetornaSubsetores(int idSetor)
        {
            return (from s in db.seae_subsetores_proc
                    where s.idsetor == idSetor
                    orderby s.descricao ascending
                    select new Subsetor()
                    {
                        codsubsetor = s.codsubsetor,
                        descricao = s.descricao,
                        idsetor = s.idsetor,
                        idsubsetor = s.idsubsetor
                    }
                    ).ToList();
        }

    }
}
