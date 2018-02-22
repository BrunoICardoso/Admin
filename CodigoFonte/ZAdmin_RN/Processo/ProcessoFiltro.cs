using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class ProcessoFiltro
    {
        public string palavrasChave { get; set; }
        public DateTime? dataIni { get; set; }
        public DateTime? dataFim { get; set; }
        public int idempresa { get; set; }
        public string numProcesso { get; set; }
        public string ordenacao { get; set; }
        public int pagina { get; set; }
        public int qtdRegistros { get; set; }
    }
}
