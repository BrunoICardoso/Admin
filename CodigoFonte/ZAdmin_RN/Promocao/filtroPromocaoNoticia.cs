using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Promocao
{
    public class filtroPromocaoNoticia
    {

        public string pesquisa { get; set; }
        public int fontePesquisa { get; set; }
        public int? empresa { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        
        public List<int?> fontes { get; set; }
        //paginacao

        public int pagina { get; set; }
        public int quantidade { get; set; }
    }
}
