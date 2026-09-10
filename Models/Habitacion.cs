namespace EscapeDelChacho.Models;

public class Habitacion
{

    public int id { get; set; }
    public int orden { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string narrativa { get; set; } = string.Empty;
    public string desafio { get; set; } = string.Empty;
    public string respuestaCorrecta { get; set; } = string.Empty;
    public string contenidoPista { get; set; } = string.Empty;

}
