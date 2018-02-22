using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class SituacaoMovimento
    {

        public int idmovsituacao { get; set; }
        public int? idprocesso { get; set; }
        public int idsituacao { get; set; }
        public DateTime dtsituacao { get; set; }
        public string descricao { get; set; }
    }
}
