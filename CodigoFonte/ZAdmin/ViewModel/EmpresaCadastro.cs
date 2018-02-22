using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{
    public class EmpresaCadastro
    {

        

        public ZAdmin_RN.Empresas.Empresa Empresa{ get; set; }

        public List<ZAdmin_RN.Empresas.Setor> Setores { get; set; }

    }
}