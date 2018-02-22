using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class FiltroPromocoes
    {

        public string pesquisa { get; set; }
        public string numproc { get; set; }
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public bool ordenacao { get; set; }
        public int pag { get; set; }
        public int quantidade { get; set; }

    }
}


