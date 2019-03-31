using Microsoft.AspNetCore.Mvc;
using GlobalCityManager.Models;
using GlobalCityManager.Extensiones;

namespace GlobalCityManager.Controllers
{
    public class CityController : Controller
    {

        public IActionResult Index()
        {
            var listaCiudades = AccionesCiudades.obtenerListaTodasCiudades();

            return View(listaCiudades);
        }

        public IActionResult IndexPais(string pcode)
        {
            var ciudades = AccionesCiudades.obtenerListaCiudadesPais(pcode);

            return View("Index", ciudades);
        }

        [HttpGet]
        public IActionResult Edit(int cId)
        {
            var ciudad = AccionesCiudades.obtenerCiudad(cId);

            return View(ciudad);
        }

        [HttpPost]
        public IActionResult Edit(City ciudad)
        {
            if (ModelState.IsValid)
            {
                AccionesCiudades.modificarCiudad(ciudad);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(City ciudad)
        {
            if (ModelState.IsValid)
            {
                AccionesCiudades.crearCiudad(ciudad);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Borrar(int cId)
        {
            AccionesCiudades.borrarCiudad(cId);

            return RedirectToAction("Index");
        }
    }
}
