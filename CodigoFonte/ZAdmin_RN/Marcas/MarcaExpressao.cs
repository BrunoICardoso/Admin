using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Marcas
{
    public class MarcaExpressao
    {
        public int? idExpressao { get; set; }
        public int idMarca { get; set; }
        public string expressao { get; set; }
        public bool ativo { get; set; }
        public bool excluido { get; set; }
    }
}
