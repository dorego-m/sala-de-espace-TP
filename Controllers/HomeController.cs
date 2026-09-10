using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EscapeDelChacho.Models;

namespace EscapeDelChacho.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private BD bd = new BD();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

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

        List<Integrante> integrantes = new List<Integrante>
        {
            new Integrante { nombre = "Ian", iniciales = "I" },
            new Integrante { nombre = "Alejo", iniciales = "A" },
            new Integrante { nombre = "Manu", iniciales = "M" }
        };

        return View(integrantes);

    }

    public IActionResult Identificacion()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Identificacion(string nombreParticipante)
    {

        if (string.IsNullOrWhiteSpace(nombreParticipante))
        {
            ViewBag.Error = "Tenés que ingresar tu nombre para entrar al container.";
            return View();
        }

        nombreParticipante = nombreParticipante.Trim();

        // Busca en la BD si el participante ya tiene una partida en curso
        Partida? partidaActiva = bd.buscarPartidaActivaPorParticipante(nombreParticipante);

        int partidaId;
        int salaActual;

        if (partidaActiva != null)
        {
            // Ya tenía una partida activa: se retoma desde donde quedó
            partidaId = partidaActiva.id;
            salaActual = partidaActiva.habitacionActual;
        }
        else
        {
            // No tenía partida activa: se crea una nueva arrancando en la sala 1
            partidaId = bd.crearPartida(nombreParticipante);
            salaActual = 1;
        }

        // La Session solo guarda lo mínimo necesario para navegar
        HttpContext.Session.SetString("NombreParticipante", nombreParticipante);
        HttpContext.Session.SetInt32("PartidaId", partidaId);
        HttpContext.Session.SetInt32("SalaActual", salaActual);

        return RedirectToAction("Sala", new { orden = salaActual });

    }

    public IActionResult Sala(int orden)
    {

        int? partidaId = HttpContext.Session.GetInt32("PartidaId");
        string? nombreParticipante = HttpContext.Session.GetString("NombreParticipante");

        if (partidaId == null || string.IsNullOrEmpty(nombreParticipante))
        {
            return RedirectToAction("Identificacion");
        }

        // El acceso a la habitación se valida siempre contra la base de datos,
        // nunca contra lo que haya guardado en Session.
        if (!bd.puedeAccederAHabitacion(partidaId.Value, orden))
        {
            return RedirectToAction("AccesoDenegado");
        }

        Habitacion? habitacion = bd.obtenerHabitacionPorOrden(orden);

        if (habitacion == null)
        {
            // Ya no hay más habitaciones: el jugador completó el juego
            return RedirectToAction("Victoria");
        }

        HttpContext.Session.SetInt32("SalaActual", orden);
        bd.actualizarHabitacionActual(partidaId.Value, orden);

        ViewBag.NombreParticipante = nombreParticipante;
        ViewBag.Progreso = $"Sala {orden} de {bd.obtenerHabitaciones().Count}";
        ViewBag.IntentosRestantes = bd.intentosMaximos - bd.contarIntentos(partidaId.Value, habitacion.id);

        return View(habitacion);

    }

    [HttpPost]
    public IActionResult Resolver(int habitacionId, int orden, string respuesta)
    {

        int? partidaId = HttpContext.Session.GetInt32("PartidaId");
        string? nombreParticipante = HttpContext.Session.GetString("NombreParticipante");

        if (partidaId == null || string.IsNullOrEmpty(nombreParticipante))
        {
            return RedirectToAction("Identificacion");
        }

        Habitacion? habitacion = bd.obtenerHabitacionPorOrden(orden);

        if (habitacion == null)
        {
            return RedirectToAction("Victoria");
        }

        int intentosUsados = bd.contarIntentos(partidaId.Value, habitacionId);

        ViewBag.NombreParticipante = nombreParticipante;
        ViewBag.Progreso = $"Sala {orden} de {bd.obtenerHabitaciones().Count}";

        if (intentosUsados >= bd.intentosMaximos)
        {
            ViewBag.Error = "Ya usaste todos los intentos permitidos en esta sala.";
            ViewBag.IntentosRestantes = 0;
            return View("Sala", habitacion);
        }

        bool esCorrecta = respuesta != null &&
            respuesta.Trim().Equals(habitacion.respuestaCorrecta, StringComparison.OrdinalIgnoreCase);

        // Todos los intentos, correctos e incorrectos, quedan registrados en la BD
        bd.registrarIntento(partidaId.Value, habitacionId, respuesta ?? "", esCorrecta);

        if (!esCorrecta)
        {
            ViewBag.Error = "Clave incorrecta, intentá de nuevo.";
            ViewBag.IntentosRestantes = bd.intentosMaximos - bd.contarIntentos(partidaId.Value, habitacionId);
            return View("Sala", habitacion);
        }

        bd.marcarHabitacionResuelta(partidaId.Value, habitacionId);

        int siguienteOrden = orden + 1;
        Habitacion? siguienteHabitacion = bd.obtenerHabitacionPorOrden(siguienteOrden);

        if (siguienteHabitacion == null)
        {
            // Era la última habitación: se termina la partida
            bd.marcarPartidaFinalizada(partidaId.Value);
            return RedirectToAction("Victoria");
        }

        return RedirectToAction("Sala", new { orden = siguienteOrden });

    }

    public IActionResult Victoria()
    {

        string? nombreParticipante = HttpContext.Session.GetString("NombreParticipante");

        if (string.IsNullOrEmpty(nombreParticipante))
        {
            return RedirectToAction("Identificacion");
        }

        ViewBag.NombreParticipante = nombreParticipante;
        return View();

    }

    public IActionResult AccesoDenegado()
    {
        return View();
    }

    public IActionResult Reiniciar()
    {

        HttpContext.Session.Remove("PartidaId");
        HttpContext.Session.Remove("NombreParticipante");
        HttpContext.Session.Remove("SalaActual");
        return RedirectToAction("Index");

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
