using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.PromocaoRedesSociais
{
    public class ProcessoRedeSocialFiltros
    {
        public int idpost { get; set; }
        public int idpromocao { get; set; }        
        public string textoPesquisa { get; set; }
        public DateTime? dataIni { get; set; }
        public DateTime? dataFim { get; set; }
        public int? idempresa { get; set; } 
        public string nomeRede { get; set; }
        public int paginaAtual { get; set; }
        public int QtdRegistros { get; set; }

    }
}
