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

        public IActionResult Tutorial()
        {
            return View();
        }

        public IActionResult Integrantes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Iniciar(string nombreParticipante)
        {
            if (string.IsNullOrWhiteSpace(nombreParticipante))
            {
                return RedirectToAction("Index");
            }

            BD.IniciarPartida(nombreParticipante);

            HttpContext.Session.SetString("NombreParticipante", nombreParticipante);
            HttpContext.Session.SetString("SalaActual", "1");

            return RedirectToAction("Sala");
        }

        public IActionResult Sala()
        {
            string? nombreSession = HttpContext.Session.GetString("NombreParticipante");
            string? salaSession = HttpContext.Session.GetString("SalaActual");

            if (string.IsNullOrWhiteSpace(nombreSession) || string.IsNullOrWhiteSpace(salaSession))
            {
                return RedirectToAction("Index");
            }

            if (!int.TryParse(salaSession, out int salaActual))
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
            }

            Habitacion? habitacion = BD.ObtenerHabitacion(salaActual);

            if (habitacion == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.NombreParticipante = nombreSession;
            ViewBag.SalaActual = salaActual;
            ViewBag.Progreso = $"Sala {salaActual}";
            ViewBag.IntentosRestantes = 3;

            return View(habitacion);
        }

        public IActionResult Terminar()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
