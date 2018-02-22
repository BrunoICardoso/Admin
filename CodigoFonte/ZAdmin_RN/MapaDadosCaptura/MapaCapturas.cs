using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class MapaCapturas
    {

        ZAdmin_DB.Model.zeengEntities db = new ZAdmin_DB.Model.zeengEntities();

        public List<ZAdmin_RN.MapaRegistros.ItemCombo> RetornaListaCapturas()
        {
            var capturas = db.mapa_capturas.OrderByDescending(x => x.datahorainicio).ToList();
                        
            return (from c in capturas
                    select new ZAdmin_RN.MapaRegistros.ItemCombo()
                    {
                            idItem = c.idmapacaptura,
                            nome = c.datahorainicio.Value.ToString("dd/MM/yyyy") +" | " + c.datahorainicio.Value.ToString("HH:mm")
                    }
                    ).ToList();
        }

    }
}
