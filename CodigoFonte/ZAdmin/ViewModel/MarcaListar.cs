using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Empresas;
using ZAdmin_RN.Marcas;


namespace ZAdmin.ViewModel
{
    public class MarcaListar
    {

        public List<Marca> Marcas { get; set; }

        public List<ZAdmin_RN.Setores.Setor> Setores { get; set; }
        public List<Empresa> Empresas { get; set; }
        public int TotalMarcas { get; set; }

    }


}