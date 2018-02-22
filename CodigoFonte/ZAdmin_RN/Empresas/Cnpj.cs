using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Empresas
{
    public class Cnpj
    {
        public int? idcnpjempresa { get; set; }
        public int? idempresa { get; set; }
        public string cnpj { get; set; }
        public bool excluir { get; set; }
    }
}
