using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class FiltroCaptura
    {

        public string pesquisafiltro { get; set; }
        
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public int idEmpresa { get; set; }
        public int pagina { get; set; }
        public int qtdRegistros { get; set; }
        public string vinculo { get; set; }
        public int? captura { get; set; }
        public string status { get; set; }
    }
}
