using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{
    public class PromocaoEditar
    {
        public ZAdmin_RN.Promocao.Promocao promocao { get; set; }
        public List<ZAdmin_RN.Empresas.Empresa> empresas { get; set; }
        public string strEmpresas { get; set; }
        
    }
}