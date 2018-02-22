using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAdmin_RN.Usuarios
{
    public class Usuario
    {

        public int idusuario { get; set; }
        public int? idcliente { get; set; }
        public string nomeCliente { get; set; }
        public string nome { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string email { get; set; }
        public bool? ativo { get; set; }
        
    }
}
