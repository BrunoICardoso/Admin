using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Empresas;

namespace ZAdmin.ViewModel
{
    public class EmpresaListar
    {
        public List<Empresa> Empresas { get; set; }
        public int TotalEmpresas { get; set; }


    }
}