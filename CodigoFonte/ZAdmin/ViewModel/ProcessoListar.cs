using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Processo;

namespace ZAdmin.ViewModel
{
    public class ProcessoListar
    {
        public List<ZAdmin_RN.ProcessoElastic.Processos_Seae> Processo { get; set; }
        public int TotalRegistros { get; set; }
    }
}