using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Processo
{
    public class Processo
    {

        public int idprocesso { get; set; }
        public string numprocesso { get; set; }
        public DateTime? dtprocesso { get; set; }
        public DateTime? dtVigenciaIni { get; set; }
        public DateTime? dtVigenciaFim { get; set; }
        public string interessados { get; set; }
        public string premios { get; set; }
        public decimal? valorTotalPremios { get; set; }
        public string modalidade { get; set; }
        public string formaContemplacao { get; set; }
        public bool? abrangenciaNacional { get; set; }
        public string nome { get; set; }
        public string resumo { get; set; }
        public string comoParticipar { get; set; }
        public bool excluido { get; set; }

        public IEnumerable<int?> empresas { get; set; }
        public IEnumerable<int?> estados { get; set; }
        public IEnumerable<int?> municipios { get; set; }

        public int numdocs { get; set; }

        public int? numdoc { get; set; }
        public string coordenacao { get; set; }
        
        public Situacao SituacaoAtual
        {
            get
            {
                if (situacoes == null)
                    return null;
                else
                    return situacoes.OrderByDescending(z => z.data).FirstOrDefault();
            }
        }
        
        public string descricao { get; set; }
        public IEnumerable<Situacao> situacoes { get; set; }
        public IEnumerable<ZAdmin_RN.Processo.Arquivo> arquivos { get; set; }
        public IEnumerable<ZAdmin_RN.Processo.Setor> setores { get; set; }
        public IEnumerable<ZAdmin_RN.Processo.Subsetor> subsetores { get; set; }
        public IEnumerable<ZAdmin_RN.Processo.SituacaoMovimento> SituacaoMovimento { get; set; }
    }
}
