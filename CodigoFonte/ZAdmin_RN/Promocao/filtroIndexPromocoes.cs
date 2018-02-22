using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class filtroIndexPromocoes
    {

        public int? empresa { get; set; }
        public int? modalidade { get; set; }
        public DateTime? dataInicialCadastrada { get; set; }
        public DateTime? dataFinalCadastrada { get; set; }
        public DateTime? dataInicialVigencia { get; set; }
        public DateTime? dataFinalVigencia { get; set; }
        public string termos { get; set; }

        //ordenacao

        public string nomeColuna { get; set; }
        public bool asc { get; set; }
        
        //paginacao

        public int pagina { get; set; }
        public int quantidade { get; set; }

    }
}
