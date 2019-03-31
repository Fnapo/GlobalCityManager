using Microsoft.AspNetCore.Mvc;
using GlobalCityManager.Models;
using GlobalCityManager.Extensiones;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;

namespace GlobalCityManager.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            var listaPaises = AccionesPaises.obtenerListaPaises();

            return View(listaPaises);
        }

        [HttpGet]
        public IActionResult Edit(string pcode)
        {
            var pais = AccionesPaises.obtenerPais(pcode);

            return View(pais);
        }

        [HttpPost]
        public IActionResult Edit(Country pais, IFormFile fichero)
        {
            if (ModelState.IsValid)
            {
                AccionesPaises.modificarPais(pais, fichero);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Country pais, IFormFile fichero)
        {
            if (ModelState.IsValid)
            {
                pais.Code = pais.Code.ToUpper();
                AccionesPaises.crearPais(pais, fichero);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Borrar(string pcode)
        {
            AccionesPaises.borrarPais(pcode);

            return RedirectToAction("Index");
        }
    }
}
