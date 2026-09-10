namespace EscapeDelChacho.Models;

public class Intento
{

    public int id { get; set; }
    public int partidaId { get; set; }
    public int habitacionId { get; set; }
    public string respuestaIngresada { get; set; } = string.Empty;
    public bool correcto { get; set; }
    public DateTime fecha { get; set; }
    public int numeroIntento { get; set; }

}
