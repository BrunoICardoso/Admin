using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class StatusDadosCaptura
    {
        public int? idStatusCaptura { get; set; }
        public int? idMapaDadosCaputura { get; set; }
        public int? idUsuario { get; set; }
        public DateTime? data { get; set; }
        public string status { get; set; }
    }
}
