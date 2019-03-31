using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using GlobalCityManager.Models;
using Microsoft.AspNetCore.Http;

namespace GlobalCityManager.Extensiones
{
    public static partial class AccionesPaises
    {
        private static IDictionary<string, Country> _listaCountry;

        static AccionesPaises()
        {
            var sqlPaises = WorldContext.Instance.Paises.OrderBy(p => p.Code).Select(p => p.Code);

            _listaCountry = new Dictionary<string, Country>();
            foreach (var pCode in sqlPaises)
            {
                var pais = WorldContext.Instance.Paises.FirstOrDefault(p => p.Code == pCode);

                _listaCountry.Add(pCode, pais);
            }
        }

        static public IList<Country> obtenerListaPaises() { return _listaCountry.Values.OrderBy(p => p.Code).ToList(); }

        static public Country obtenerPais(string pcode)
        {
            if (_listaCountry.ContainsKey(pcode))
            {
                return _listaCountry[pcode];
            }

            return null;
        }

        static public void modificarPais(Country pais, IFormFile fichero)
        {
            if (_listaCountry.ContainsKey(pais.Code))
            {
                var paisOrigen = WorldContext.Instance.Paises.FirstOrDefault(p => p.Code == pais.Code);

                paisOrigen.Name = pais.Name;
                paisOrigen.Region = pais.Region;
                _listaCountry[pais.Code] = paisOrigen;
                WorldContext.Instance.SaveChanges();
                modificarBandera(pais.Code, fichero);
            }
        }

        static private void modificarBandera(string pcode, IFormFile fichero)
        {
            if (fichero == null || fichero.Length == 0) { return; }
            if (!fichero.ContentType.Contains("image/")) { return; }

            var nombreFicheroDestino = $"{pcode}{Path.GetExtension(fichero.FileName)}";
            var caminoRelativo = Path.Combine("images", nombreFicheroDestino);
            var caminoAbsoluto = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var paisOrigen = WorldContext.Instance.Paises.FirstOrDefault(c => c.Code == pcode);

            caminoAbsoluto = Path.Combine(caminoAbsoluto, caminoRelativo);
            paisOrigen.NationalFlag = caminoRelativo;
            using (var stream = new FileStream(caminoAbsoluto, FileMode.Create))
            {
                fichero.CopyTo(stream);
            }
            _listaCountry[pcode].NationalFlag = caminoRelativo;
            WorldContext.Instance.SaveChanges();
        }

        static public Country crearPais(Country pais, IFormFile fichero)
        {
            if (_listaCountry.ContainsKey(pais.Code)) { return null; }

            _listaCountry.Add(pais.Code, pais);
            WorldContext.Instance.Add<Country>(pais);
            WorldContext.Instance.SaveChanges();
            modificarBandera(pais.Code, fichero);
            if (_listaCountry[pais.Code].NationalFlag == null || _listaCountry[pais.Code].NationalFlag.Length == 0) { _listaCountry[pais.Code].NationalFlag = "images\\default.png"; }
            WorldContext.Instance.SaveChanges();

            return pais;
        }

        /*
        *   borrarPais produce un borrado en cascada, es decir,
        *   borrar el país y sus ciudades.
        *   Por eso sólo activo el método para los países creados
        *   con código 'AAA'.
         */
        static public void borrarPais(string pCode)
        {
            if (_listaCountry.ContainsKey(pCode))
            {
                IList<City> listaCiudades = AccionesCiudades.obtenerListaCiudadesPais(pCode);

                foreach (var ciudad in listaCiudades)
                {
                    AccionesCiudades.borrarCiudad(ciudad.Id);
                }
                WorldContext.Instance.Remove<Country>(obtenerPais(pCode));
                _listaCountry.Remove(pCode);
                WorldContext.Instance.SaveChanges();
            }
        }
    }
}
