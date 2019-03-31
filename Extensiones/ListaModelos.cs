using System.Linq;
using System.Collections.Generic;
using System;
using GlobalCityManager.Models;

namespace GlobalCityManager.Extensiones
{
    public static partial class ListaModelos
    {
        private static IList<Region> _listaRegiones;
        private static IList<Distrito> _listaDistritos;

        static ListaModelos()
        {
            var sqlRegiones = WorldContext.Instance.Paises.OrderBy(p => p.Region).Select(p => p.Region).Distinct();
            var sqlDistritos = WorldContext.Instance.Ciudades.OrderBy(p => p.District).Select(p => p.District).Distinct();

            _listaRegiones = new List<Region>();
            _listaDistritos = new List<Distrito>();

            foreach (var reg in sqlRegiones)
            {
                Region region = new Region(reg);

                _listaRegiones.Add(region);
            }
            foreach (var dis in sqlDistritos)
            {
                Distrito distrito = new Distrito(dis);

                _listaDistritos.Add(distrito);
            }
        }

        static public IList<Region> obtenerListaRegiones()
        {
            return _listaRegiones;
        }

        static public IList<Distrito> obtenerListaDistritos()
        {
            return _listaDistritos;
        }
    }
}
