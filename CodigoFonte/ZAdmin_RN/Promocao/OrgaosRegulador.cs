using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class OrgaosRegulador
    {
        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<OrgaoRegulador> retornaOrgaos()
        {
            return (
                        from org in db.promo_orgaosreguladores
                        where org.excluido == false
                        orderby org.nome ascending
                        select new OrgaoRegulador()                        
                        {
                            idorgao = org.idorgao,
                            nome = org.nome
                        }
                    ).ToList();
        }

    }
}
