using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Clientes;

namespace ZAdmin.ViewModel
{
    public class ClienteCadastro
    {

        public Cliente cliente { get; set; }

        public List<Estado> Estados { get; set; }
        

    }
}