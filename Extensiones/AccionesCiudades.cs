using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using GlobalCityManager.Models;
using Microsoft.AspNetCore.Http;

namespace GlobalCityManager.Extensiones
{
    public static partial class AccionesCiudades
    {
        private static IDictionary<int, City> _listaCity;

        static AccionesCiudades()
        {
            var sqlCiudades = WorldContext.Instance.Ciudades.OrderBy(c => c.CountryCode).Select(c => c.Id);

            AccionesPaises.obtenerPais("AAA");  // Activar los Países.
            _listaCity = new Dictionary<int, City>();
            foreach (var cId in sqlCiudades)
            {
                var ciudad = WorldContext.Instance.Ciudades.FirstOrDefault(c => c.Id == cId);

                _listaCity.Add(cId, ciudad);
            }
        }

        static public City obtenerCiudad(int cId)
        {
            if (_listaCity.ContainsKey(cId))
            {
                return _listaCity[cId];
            }

            return null;
        }

        static public IList<City> obtenerListaTodasCiudades() { return _listaCity.Values.OrderBy(c => c.CountryCode).ToList(); }

        static public IList<City> obtenerListaCiudadesPais(string code)
        {
            var salida = new List<City>();

            foreach (var parciudad in _listaCity)
            {
                if (parciudad.Value.CountryCode == code)
                {
                    var city = new City();

                    city = parciudad.Value;
                    salida.Add(city);
                }
            }

            return salida;
        }

        static public void modificarCiudad(City ciudad)
        {
            if (_listaCity.ContainsKey(ciudad.Id))
            {
                var ciudadOrigen = WorldContext.Instance.Ciudades.FirstOrDefault(c => c.Id == ciudad.Id);

                ciudadOrigen.Name = ciudad.Name;
                ciudadOrigen.District = ciudad.District;
                ciudadOrigen.Population = ciudad.Population;
                _listaCity[ciudad.Id] = ciudadOrigen;
                WorldContext.Instance.SaveChanges();
            }
        }

        static public City crearCiudad(City ciudad)
        {
            int cId = _listaCity.Keys.Max() + 1;

            ciudad.Id = cId;
            _listaCity.Add(cId, ciudad);
            WorldContext.Instance.Add<City>(ciudad);
            WorldContext.Instance.SaveChanges();

            return ciudad;
        }

/*
*   borrarPais produce un borrado en cascada, es decir,
*   borrar el país y sus ciudades.
*   Por eso sólo activo el método para los países creados
*   con código 'AAA'.
 */
        static public void borrarCiudad(int cId)
        {
            if (_listaCity.ContainsKey(cId))
            {
                WorldContext.Instance.Remove<City>(obtenerCiudad(cId));
                _listaCity.Remove(cId);
                WorldContext.Instance.SaveChanges();
            }
        }
    }
}
