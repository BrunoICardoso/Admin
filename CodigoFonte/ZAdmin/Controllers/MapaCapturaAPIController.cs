using System;
using System.Collections.Generic;
using System.Web.Http;
using ZAdmin.ViewModel;
using ZAdmin_RN.MapaDadosCaptura;
using ZAdmin_RN.MapaRegistros;

namespace ZAdmin.Controllers
{

    public class MapaCapturaAPIController : ApiController
    {

        public MapaCapturaListar Capturas(FiltroCaptura filtro)
        {

            var RNMapaDadosCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            var capturas = RNMapaDadosCaptura.RetornaListaDeDadosMapaCaptura(filtro);

            var mapaCapturaListar = new MapaCapturaListar();
            mapaCapturaListar.Capturas = capturas;
            mapaCapturaListar.TotalDeCapturas = RNMapaDadosCaptura.TotalCapturas;

            return mapaCapturaListar;
        }

        public MapaCapturaCadastro Get(int id)
        {

            var RNMapaCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            var capturaCadastro = new MapaCapturaCadastro();
            capturaCadastro.Captura = RNMapaCaptura.RetornaCapturaParaRegitros(id);
            capturaCadastro.Filtros = RNMapaCaptura.RetornaCombos();

            return capturaCadastro;
        }

        public MapaCapturaEditar GetRetornaCaptura(int id)
        {
            var RNMapaCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            var MapaCapturaEditar = new MapaCapturaEditar();

            MapaCapturaEditar.Captura = RNMapaCaptura.RetornaCaptura(id);
            MapaCapturaEditar.Empresas = RNMapaCaptura.RetornaListaDeEmpresas();
            MapaCapturaEditar.Estados = RNMapaCaptura.RetornaListaDeEstados();

            return MapaCapturaEditar;
        }

        public List<ItemCombo> GetRetornaMarcas(int id)
        {
            var RNMapaDadosCaputura = new MapaDadosCapturas();
            return RNMapaDadosCaputura.RetornaListaDeMarcas(id);
        }

        public List<ItemCombo> GetRetornaMarcasByEmpresas(string strIds)
        {
            List<int> Ids = new List<int>();
            if (strIds == "0")
            {
                Ids = null;
            }
            else
            {
                string[] listaIds = new string[] { };
                listaIds = strIds.Split('|');

                foreach (var id in listaIds)
                {
                    Ids.Add(Convert.ToInt32(id));
                }
            }
            
            var RNMapaDadosCaputura = new MapaDadosCapturas();
            return RNMapaDadosCaputura.RetornaListaMarcasByEmpresas(Ids);
        }

        public void Put(MapaDadosCaptura captura)
        {
            var RNMapaCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            RNMapaCaptura.Editar(captura);
        }

        public void Delete(int id)
        {
            var RNMapaDadosCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            RNMapaDadosCaptura.Deletar(id);
        }

        public void Post(MapaDadosCapturaImportado registro)
        {

            var RNMapaCapturas = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            RNMapaCapturas.SalvaCapturaParaRegitros(registro);

        }

        public List<ItemCombo> GetRetornaListaDeEmpresas()
        {
            var RNMapaCaptura = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            return RNMapaCaptura.RetornaListaDeEmpresas();
        }

        public List<ItemCombo> GetRetornaListaCapturas()
        {
            var RNcapturas = new ZAdmin_RN.MapaDadosCaptura.MapaCapturas();
            return RNcapturas.RetornaListaCapturas();
        }

        public int VerificaAssociacaoCnpjEmpresaMapa(ZAdmin_RN.MapaRegistros.MapaRegistro filtro)
        {
            var rn = new ZAdmin_RN.MapaDadosCaptura.MapaDadosCapturas();
            return rn.RetornaAssociacaoCnpjEmpresaMapa(filtro);
        }
    }

}
