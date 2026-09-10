namespace EscapeDelChacho.Models;

public class Partida
{

    public int id { get; set; }
    public string nombreParticipante { get; set; } = string.Empty;
    public DateTime fechaInicio { get; set; }
    public string estado { get; set; } = "EnCurso"; // EnCurso / Finalizada
    public int habitacionActual { get; set; }

}
