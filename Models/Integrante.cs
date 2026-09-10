namespace EscapeDelChacho.Models;

// Los integrantes del grupo no forman parte de la lógica del juego,
// así que no hace falta guardarlos en la base de datos. Alcanza con
// esta clase simple para mostrarlos en la vista "Integrantes".
public class Integrante
{

    public string nombre { get; set; } = string.Empty;
    public string iniciales { get; set; } = string.Empty;

}
