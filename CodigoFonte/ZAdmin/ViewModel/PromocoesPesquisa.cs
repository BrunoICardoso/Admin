using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{
    public class PromocoesPesquisa
    {
       public int TotalProcessos { get; set; }
        public IEnumerable<ZAdmin_RN.Promocao.Processos_Seae> ListaProcessos { get; set; }
       
    }
}