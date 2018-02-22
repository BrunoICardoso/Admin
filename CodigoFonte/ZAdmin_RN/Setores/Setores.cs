using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Setores
{
    public class Setores
    {

        public List<Setor> RetornaSetores()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var setores = (from s in db.setoresempresa
                           select new Setor() {
                               idsetoresempresa = s.idsetoresempresa,
                               nome = s.nome
                           }).ToList();

            return setores;
        }
    }
}
