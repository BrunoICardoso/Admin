using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Clientes;

namespace ZAdmin.ViewModel
{
    public class ClienteListar
    {

        public List<Cliente> Clientes { get; set; }
        public int TotalClientes { get; set; }

    }
}