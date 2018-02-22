using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class PromocaoModalidades
    {

        public List<PromocaoModalidade> retornaModalidades()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            return (
                        from m in db.promo_modalidades
                        where m.excluido == false
                        select new PromocaoModalidade()
                        {
                            idpromomodalidade = m.idpromomodalidade,
                            nome = m.nome,
                            excluido = m.excluido
                        }
                ).ToList();
        }

    }
}
