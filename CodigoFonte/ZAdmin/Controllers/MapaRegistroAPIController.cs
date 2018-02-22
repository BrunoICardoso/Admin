using System;
using System.Collections.Generic;
using System.Web.Http;
using ZAdmin.ViewModel;
using ZAdmin_RN.MapaRegistros;

namespace ZAdmin.Controllers
{
    public class MapaRegistroAPIController : ApiController
    {

        public MapaRegistroListar Filtro(FiltroMapa filtro)
        {

            var RNMapaRegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            var registros = RNMapaRegistros.RetornaListaDeMapaRegistros(filtro);

            var MapaRegistroListar = new MapaRegistroListar();
            MapaRegistroListar.ListaDeMapaRegistros = registros;
            MapaRegistroListar.TotalMapaRegistros = RNMapaRegistros.TotalRegistros;

            return MapaRegistroListar;
        }

        public FiltroMapaRegistro GetFiltros()
        {
            var RNMapaRegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            return RNMapaRegistros.RetornaCombosMapaRegistro();
        }

        public MapaRegistroEditar Get(int id)
        {
            var RNMaparegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            var resultadoRegistro = RNMaparegistros.RetornaMapaRegistro(id);

            var MapaRegistroEditar = new MapaRegistroEditar();
            MapaRegistroEditar.Registro = resultadoRegistro;
            MapaRegistroEditar.Filtros = RNMaparegistros.RetornaCombosMapaRegistro();

            return MapaRegistroEditar;
        }

        public List<ItemCombo> GetRetornaMarcas(int id)
        {
            var RNMapaRegistro = new ZAdmin_RN.MapaRegistros.MapaRegistros();

            return RNMapaRegistro.RetornListaDeMarcasDeUmaEmpresa(id);
        }

        public void Post(MapaRegistro registro)
        {

            var RNMapaRegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            RNMapaRegistros.Cadastrar(registro);

        }

        public void Put(MapaRegistro registro)
        {
            var RNMapaRegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            registro.idRegistro = registro.idRegistro;
            RNMapaRegistros.Editar(registro);
        }

        public void Delete(int id)
        {
            var RNMapaRegistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            RNMapaRegistros.Deletar(id);
        }

        public int VerificaDuplicidadeRegistroEmpresa(ZAdmin_RN.MapaRegistros.FiltroDadosMapaRegistro filtro)
        {
            var RNregistros = new ZAdmin_RN.MapaRegistros.MapaRegistros();
            return RNregistros.VerificaDuplicidadeRegistroEmpresa(filtro);
        }

    }
}
