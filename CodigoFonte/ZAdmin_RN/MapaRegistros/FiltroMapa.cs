using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.MapaRegistros
{
    public class FiltroMapa
    {
        public int? Estados { get; set; }
        public int? Areas { get; set; }
        public int? Especies { get; set; }
        public int? Subespecies { get; set; }
        public int? Bases { get; set; }
        public int? Caracteristicas { get; set; }
        public int? Atributos { get; set; }
        public int? Complementos { get; set; }
        public int? Origens { get; set; }
        public int? Empresas { get; set; }
        public string numeroRegistro { get; set; }
        public string nomeMarca { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public int pagina { get; set; }
        public int qtdRegistros { get; set; }
    }
}
