using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZAdmin.Helpers;
using ZAdmin.ViewModel;
using ZAdmin_RN.Clientes;
using ZAdmin_RN.Usuarios;

namespace ZAdmin.Controllers
{
    public class UsuarioAPIController : ApiController
    {

        public UsuarioListar Get(int idcliente, int pagina, int qtdPorPagina)
        {
            var RNUsuarios = new ZAdmin_RN.Usuarios.Usuarios();
            var usuarios = RNUsuarios.RetornaListaDeUsuarios(idcliente, pagina, qtdPorPagina);

            var UsuarioListar = new UsuarioListar();

            UsuarioListar.Usuarios = usuarios;
            UsuarioListar.TotalUsuarios = RNUsuarios.TotalUsuarios;

            return UsuarioListar;
        }

        public UsuarioCadastro Get(int id)
        {
            var RNUsuarios = new ZAdmin_RN.Usuarios.Usuarios();
            var resultado = RNUsuarios.RetornaUsuario(id);

            var cadastroUsuario = new UsuarioCadastro();
            cadastroUsuario.usuario = resultado;
            cadastroUsuario.clientes = RNUsuarios.RetornaListaDeClientes();

            return cadastroUsuario;
        }

        public List<Cliente> GetRetornaClientes()
        {
            var usuariosRN = new ZAdmin_RN.Usuarios.Usuarios();
            return usuariosRN.RetornaListaDeClientes();
        }

        [HttpPost]
        public Mensagem Post(Usuario usuario)
        {
            var RNUsuarios = new ZAdmin_RN.Usuarios.Usuarios();


            var msg = new Mensagem();

            if (RNUsuarios.VerificaUsuarioExistente(usuario.email))
            {
                msg.erro = true;
                msg.mensagem = "Usuário já Cadastrado!";

            }
            else
            {
                msg.erro = false;

                //string jsonUsuario = HttpContext.Current.Request.Form["json"].ToString();
                //ZAdmin_RN.Usuarios.Usuario usuario = JsonConvert.DeserializeObject<ZAdmin_RN.Usuarios.Usuario>(jsonUsuario);

                RNUsuarios.Cadastrar(usuario);

            }

            return msg;
        }

        public Mensagem Put(int id, [FromBody]Usuario usuario)
        {
            try
            {
                var RNUsuarios = new ZAdmin_RN.Usuarios.Usuarios();
                usuario.idusuario = id;
                RNUsuarios.Editar(usuario);
                
                var msg = new Mensagem();
                msg.erro = false;
                return msg;

            }
            catch (Exception ex)
            {
                var msg = new Mensagem();
                msg.erro = true;
                msg.mensagem = "Erro ao editar: " + ex.Message;
                return msg;
            }

        }

        public void Delete(int id)
        {
            var RNUsuarios = new ZAdmin_RN.Usuarios.Usuarios();
            RNUsuarios.Deletar(id);
        }

    }
}
