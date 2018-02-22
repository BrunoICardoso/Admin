using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Clientes;
using ZAdmin_RN.Usuarios;

namespace ZAdmin.ViewModel
{
    public class UsuarioCadastro
    {

        public Usuario usuario { get; set; }
        public List<Cliente> clientes { get; set; }
        
    }
}