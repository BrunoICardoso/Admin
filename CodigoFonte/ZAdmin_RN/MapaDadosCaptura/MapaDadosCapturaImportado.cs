using System;
using System.Collections.Generic;

namespace ZAdmin_RN.MapaDadosCaptura
{
    public class MapaDadosCapturaImportado
    {
        public int idDadosCaputura { get; set; }

        public int idRegistro { get; set; }
        public int? idEstado { get; set; }
        public int? idArea { get; set; }
        public int? idEspecie { get; set; }
        public int? idSubEspecie { get; set; }
        public int? idBase { get; set; }
        public int? idCaracteristica { get; set; }
        public int? idAtributo { get; set; }
        public int? idComplemento { get; set; }
        public int? idOrigem { get; set; }
        public int? idMarca { get; set; }
        public int? idEmpresa { get; set; }

        public List<int> listaEmpresas = new List<int>();

        public DateTime? dataHoraCaptura { get; set; }
        public DateTime? dataConcessao { get; set; }

        public string uf { get; set; }
        public string nomearea { get; set; }
        public string nomeEspecie { get; set; }
        public string nomeSubespecie { get; set; }
        public string nomebase { get; set; }
        public string nomecaracteristica { get; set; }
        public string nomeatributo { get; set; }
        public string nomecomplemento { get; set; }
        public string nomeestabelecimento { get; set; }
        public string cnpj { get; set; }
        public string nomeProduto { get; set; }
        public string numeroRegistro { get; set; }
        public string nomemarca { get; set; }
        public string nomeorigem { get; set; }
        public string modoAplicacao { get; set; }
        public string status { get; set; }


    }
}
