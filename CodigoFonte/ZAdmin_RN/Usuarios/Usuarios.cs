using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAdmin_RN.Seguranca;

namespace ZAdmin_RN.Usuarios
{
    public class Usuarios
    {

        private int _totalUsuarios;

        public int TotalUsuarios
        {
            get { return _totalUsuarios; }
            set { _totalUsuarios = value; }
        }


        public List<Usuario> RetornaListaDeUsuarios(int idcliente, int pagina, int qtdporpagina)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            _totalUsuarios = (from u in db.usuarios
                              where (u.excluido == false) &&
                              (u.idcliente == idcliente || idcliente == 0)
                              select u).Count();

            var usuarios = (from u in db.usuarios
                            where u.excluido == false &&
                            (u.idcliente == idcliente || idcliente == 0)
                            select u).OrderBy(x => x.nome).Skip((pagina - 1) * qtdporpagina).Take(qtdporpagina);

            var listaDeUsuarios = (from u in usuarios
                                   select new Usuario()
                                   {
                                       nome = u.nome,
                                       email = u.email,
                                       idcliente = u.idcliente,
                                       nomeCliente = u.clientes.nome,
                                       idusuario = u.idusuario,
                                       login = u.login,
                                       ativo = u.ativo
                                   }).ToList();

            return listaDeUsuarios;
        }

        public Usuario RetornaUsuario(int id)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var usuarioDB = (from u in db.usuarios
                         where u.idusuario == id
                         select new Usuario()
                         {
                             nome = u.nome,
                             email = u.email,
                             login = u.login,
                             idcliente = u.idcliente,
                             idusuario = u.idusuario,
                             ativo = u.ativo
                         }).FirstOrDefault();
            
            return usuarioDB;
        }
        
        public void Cadastrar(Usuario usuario)
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var usuarioDB = new ZAdmin_DB.Model.usuarios();
            
            usuarioDB.nome = usuario.nome;
            usuarioDB.login = usuario.login;
            
            usuarioDB.senha = Criptografia.Criptografar(usuario.senha);
            usuarioDB.idcliente = usuario.idcliente;
            usuarioDB.email = usuario.email.Trim();
            usuarioDB.ativo = true;
            usuarioDB.excluido = false;

            db.usuarios.Add(usuarioDB);
            db.SaveChanges();

        }
        
        public void Editar(Usuario usuario)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            var usuarioDB = (from u in db.usuarios
                             where u.idusuario == usuario.idusuario
                             select u).FirstOrDefault();

            usuarioDB.ativo = usuario.ativo;
            usuarioDB.nome = usuario.nome.Trim();
            usuarioDB.login = usuario.login.Trim();
            if(usuario.senha != null)
            {
             usuarioDB.senha = Criptografia.Criptografar(usuario.senha);
            }
            usuarioDB.idcliente = usuario.idcliente;
            usuarioDB.email = usuario.email;

            db.SaveChanges();

        }

        public void Deletar(int idusuario)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();


            var usuarioDB = (from u in db.usuarios
                             where u.idusuario == idusuario
                             select u).FirstOrDefault();

            if (usuarioDB != null)
            {
                usuarioDB.excluido = true;
                db.SaveChanges();
            }

        }

        public List<Clientes.Cliente> RetornaListaDeClientes()
        {
            var db = new ZAdmin_DB.Model.zeengEntities();

            var clientes = (from c in db.clientes
                            where c.excluido == false
                            orderby c.nome ascending
                            select new Clientes.Cliente()
                            {
                                nome = c.nome,
                                idcliente = c.idclientes
                            }).ToList();

            return clientes;
        }

        public bool VerificaUsuarioExistente(string email)
        {

            var db = new ZAdmin_DB.Model.zeengEntities();

            bool resultado = (from user in db.usuarios
                             where user.email.Equals(email)
                             select user).Any();

            return resultado;

        }
        
    }
}
