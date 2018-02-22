using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class itemTabelaPromocao
    {

        public int idPromocao { get; set; }
        public string empresa { get; set; }
        public string modalidade { get; set; }
        public string promocao { get; set; }
        public DateTime? dataCadastro { get; set; }
        public DateTime? vigenciaInicial { get; set; }
        public DateTime? vigenciaFinal { get; set; }

    }
}
