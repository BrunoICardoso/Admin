using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class MapaDadosCaptura
    {
        
        public int idDadosCaputura { get; set; }
        public int? idmaoacaptura { get; set; }
        public DateTime? dataHoraCaptura { get; set; }

        public string uf { get; set; }
        public string area { get; set; }
        public string especie { get; set; }
        public string subEspecie { get; set; }
        public string @base { get; set; }
        public string caracteristica { get; set; }
        public string atributo { get; set; }
        public string complemento { get; set; }
        public string estabelecimento { get; set; }
        public string cnpj { get; set; }
        public string produto { get; set; }
        public DateTime? dataConcessao { get; set; }
        public string registro { get; set; }
        public string marca { get; set; }
        public string origem { get; set; }
        public string modoAplicacao { get; set; }
        public string status { get; set; }

        public bool StatusMapa { get; set; }
        public bool ValidarRegistro { get; set;}

        public List<int> listaEmpresas = new List<int>();
    }


        public class MapaDadosCapturaAnterior
     {

        public int idDadosCaputura { get; set; }
        public int? idmaoacaptura { get; set; }
        public DateTime? dataHoraCaptura { get; set; }

        public string uf { get; set; }
        public string area { get; set; }
        public string especie { get; set; }
        public string subEspecie { get; set; }
        public string @base { get; set; }
        public string caracteristica { get; set; }
        public string atributo { get; set; }
        public string complemento { get; set; }
        public string estabelecimento { get; set; }
        public string cnpj { get; set; }
        public string produto { get; set; }
        public DateTime? dataConcessao { get; set; }
        public string registro { get; set; }
        public string marca { get; set; }
        public string origem { get; set; }
        public string modoAplicacao { get; set; }
        public string status { get; set; }
        
    }

    public class Registro {

        public string registro { get; set; }

    }
}
