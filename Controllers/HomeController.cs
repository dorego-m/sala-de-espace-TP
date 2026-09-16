using Microsoft.AspNetCore.Mvc;
using EscapeDelChacho.Models; 

namespace EscapeDelChacho.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Tutorial(){
        return View();
        }
        public IActionResult Integrantes(){
        return View();
        }


        [HttpPost]
        public IActionResult Iniciar(string nombreParticipante)
        {
            BD.IniciarPartida(nombreParticipante);
            
            HttpContext.Session.SetString("NombreParticipante", nombreParticipante);
            HttpContext.Session.SetString("SalaActual", "1");
            
            return RedirectToAction("Sala");
        }

        public IActionResult Sala()
        {
            string? nombreSession = HttpContext.Session.GetString("NombreParticipante");
            string? salaSession = HttpContext.Session.GetString("SalaActual");
            
            if (string.IsNullOrEmpty(nombreSession) || string.IsNullOrEmpty(salaSession))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Nombre = nombreSession;
            ViewBag.SalaActual = int.Parse(salaSession);
            
            return View();
        }

        public IActionResult Terminar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}