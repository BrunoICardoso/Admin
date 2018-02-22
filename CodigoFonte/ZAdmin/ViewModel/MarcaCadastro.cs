using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{
    public class MarcaCadastro
    {

        public ZAdmin_RN.Marcas.Marca Marca { get; set; }

        public List<ZAdmin_RN.Setores.Setor> Setores { get; set; }

        public List<ZAdmin_RN.Empresas.Empresa> Empresas { get; set; }

    }
}