namespace EscapeDelChacho.Models
{
    public class Partida
    {
        public int Id { get; set; }
        public string? NombreParticipante { get; set; }
        public int SalaActual { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}