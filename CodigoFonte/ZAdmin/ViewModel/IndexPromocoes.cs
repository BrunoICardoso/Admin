using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZAdmin.ViewModel
{

    public class IndexPromocoes
    {

        public List<ZAdmin_RN.Promocao.itemTabelaPromocao> promocoes { get; set; }
        public int TotalMapaRegistros { get; set; }

    }
}