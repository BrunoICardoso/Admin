using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.ProcessoElastic
{
    public class Processos_Seae
    {

        public int idprocesso { get; set; }
        public bool abrangencia_nacional { get; set; }
        public List<AbrangeEstado> abrangestados { get; set; }
        public List<AbrangeMunicipio> abrangmunicipios { get; set; }
        public List<Arquivo> arquivos { get; set; }
        public string comoparticipar { get; set; }

        public DateTime? dtprocesso { get; set; }
        public DateTime? dtvigenciafim { get; set; }
        public DateTime? dtvigenciaini { get; set; }
        public DateTime? datasituacaoatual { get; set; }

        public List<Empresa> empresas { get; set; }
        public string formacontemplacao { get; set; }
        public string interessados { get; set; }
        public string modalidade { get; set; }
        public string nome { get; set; }
        public int numdocs { get; set; }
        public string numprocesso { get; set; }
        public string premios { get; set; }
        public string resumo { get; set; }
        public List<Setor> setores { get; set; }
        public string situacaoatual { get; set; } // Última situção ordenado por data
        //public List<Situacao> situacoes { get; set; }
        public List<Solicitante> solicitantes { get; set; }
        public double valortotalpremios { get; set; }

    }
}
