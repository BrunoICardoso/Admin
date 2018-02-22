using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZAdmin_RN.Usuarios;

namespace ZAdmin.ViewModel
{
    public class UsuarioListar
    {

        public List<Usuario> Usuarios { get; set; }
        public int TotalUsuarios { get; set; }
        
    }
}