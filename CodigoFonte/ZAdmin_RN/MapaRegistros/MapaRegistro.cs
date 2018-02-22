using System;
using System.Collections.Generic;

namespace ZAdmin_RN.MapaRegistros
{
    public class MapaRegistro
    {

        public string cnpj { get; set; }
        public int idRegistro { get; set; }
        public int? idEstado { get; set; }
        public int? idArea { get; set; }
        public int? idEspecie { get; set; }
        public string nomeEspecie { get; set; }
        public int? idSubEspecie { get; set; }
        public string nomeSubespecie { get; set; }
        public int? idBase { get; set; }
        public int? idCaracteristica { get; set; }
        public int? idAtributo { get; set; }
        public int? idComplemento { get; set; }
        public int? idOrigem { get; set; }
        public int? idMarca { get; set; }
        public int? idEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string nomeMarca { get; set; }
        public string nomeProduto { get; set; }
        public DateTime? dataConcessao { get; set; }
        public string numeroRegistro { get; set; }
        public string modoAplicacao{ get; set; }
        public string status { get; set; }

        public List<int> listaEmpresas = new List<int>();

    }
}

