using System.Collections.Generic;
using ZAdmin_RN.MapaDadosCaptura;
using ZAdmin_RN.MapaRegistros;

namespace ZAdmin.ViewModel
{
    public class MapaCapturaEditar
    {
        public MapaDadosCaptura Captura { get; set; }
        public List<ItemCombo> Empresas { get; set; }
        public List<ItemCombo> Estados { get; set; }        
    }
}