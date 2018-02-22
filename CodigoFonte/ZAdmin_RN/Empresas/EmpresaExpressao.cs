using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Empresas
{
    public class EmpresaExpressao
    {

        public int? idExpressao { get; set; }
        public int idEmpresa { get; set; }
        public string expressao { get; set; }
        public bool ativo { get; set; }
        public bool excluido { get; set; }

    }
}
