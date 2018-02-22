using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZAdmin_RN.Setores;

namespace ZAdmin.Controllers
{
    public class SetorAPIController : ApiController
    {

        public IEnumerable<Setor> Get() {
            var RNSetores = new ZAdmin_RN.Setores.Setores();
            var setores = RNSetores.RetornaSetores();
            return setores;
        }



    }
}
