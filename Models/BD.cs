namespace EscapeDelChacho.Models;
using Microsoft.Data.SqlClient;
using Dapper;

/*
 * ============================================================================
 *  CÓMO DEBERÍA CONSTRUIRSE LA BASE DE DATOS (todavía no está creada)
 * ============================================================================
 *
 *  Base de datos: EscapeDelChacho
 *
 *  Tabla Partidas
 *      id                  int IDENTITY PRIMARY KEY
 *      nombreParticipante  nvarchar(100)  NOT NULL
 *      fechaInicio         datetime       NOT NULL
 *      estado              nvarchar(20)   NOT NULL   -- 'EnCurso' o 'Finalizada'
 *      habitacionActual    int            NOT NULL   -- orden de la sala en la que está el jugador
 *
 *  Tabla Habitaciones
 *      id                  int IDENTITY PRIMARY KEY
 *      orden               int            NOT NULL   -- 1, 2, 3, 4...
 *      nombre              nvarchar(100)  NOT NULL
 *      narrativa           nvarchar(max)  NOT NULL
 *      desafio             nvarchar(max)  NOT NULL
 *      respuestaCorrecta   nvarchar(50)   NOT NULL
 *      contenidoPista      nvarchar(max)  NOT NULL
 *
 *  Tabla Intentos
 *      id                  int IDENTITY PRIMARY KEY
 *      partidaId           int  NOT NULL  FOREIGN KEY REFERENCES Partidas(id)
 *      habitacionId        int  NOT NULL  FOREIGN KEY REFERENCES Habitaciones(id)
 *      respuestaIngresada  nvarchar(50)   NOT NULL
 *      correcto            bit            NOT NULL
 *      fecha               datetime       NOT NULL
 *      numeroIntento       int            NOT NULL
 *
 *  Tabla HabitacionesResueltas   (relación Partida <-> Habitacion resuelta)
 *      partidaId           int  NOT NULL  FOREIGN KEY REFERENCES Partidas(id)
 *      habitacionId        int  NOT NULL  FOREIGN KEY REFERENCES Habitaciones(id)
 *      fechaResolucion     datetime       NOT NULL
 *      PRIMARY KEY (partidaId, habitacionId)
 *
 *  Una vez que la base esté creada en SQL Server, cada bloque marcado con
 *  "TODO DAPPER" de esta clase debe reemplazarse por la consulta real usando
 *  SqlConnection + Dapper, tal como se hizo en el TP del álbum de figuritas
 *  (using (SqlConnection connection = new SqlConnection(_connectionString)) { ... }).
 *
 *  Mientras tanto, esta clase usa listas en memoria (_partidas, _habitaciones,
 *  _intentos, _resueltas) para poder probar el flujo completo del juego sin
 *  tener la base armada. Esas listas NO son la base de datos, son solo un
 *  reemplazo temporal.
 * ============================================================================
 */
public class BD
{

    private string _connectionString = @"Server=localhost;DataBase=EscapeDelChacho;Integrated Security=true;TrustServerCertificate=true";

    // cantidad máxima de intentos permitidos por habitación
    public int intentosMaximos = 5;

    // ------------------------------------------------------------------
    // Datos temporales en memoria. Simulan las tablas de la BD mientras
    // la base no exista. Se pierden cada vez que se reinicia la app.
    // ------------------------------------------------------------------
    private static List<Partida> _partidas = new List<Partida>();
    private static List<Intento> _intentos = new List<Intento>();
    private static List<(int partidaId, int habitacionId)> _resueltas = new List<(int, int)>();
    private static int _proximoIdPartida = 1;
    private static int _proximoIdIntento = 1;

    // Contenido de las 4 habitaciones obligatorias. Cuando exista la base,
    // este listado pasa a estar en la tabla Habitaciones y se consulta con
    // Dapper (SELECT * FROM Habitaciones ORDER BY orden).
    private static List<Habitacion> _habitaciones = new List<Habitacion>
    {
        new Habitacion
        {
            id = 1,
            orden = 1,
            nombre = "El Vestuario del Container",
            narrativa = "Te despertás tirado en lo que parece un vestuario improvisado, adentro de un container en el puerto. Un representante trucho te encerró ahí: te va a vender a un club de Arabia Saudita sin que vos lo sepas ni lo quieras. En la pared, con marcador, alguien que estuvo encerrado antes que vos dejó dos acertijos.",
            desafio = "Acertijo 1: \"Tengo agujas, pero no sé coser. Tengo números, pero no sé contar. ¿Qué soy?\"\nAcertijo 2: \"Si me nombras, desaparezco. ¿Qué soy?\"\nSumá la cantidad de letras de las dos respuestas y escribí ese número para abrir la primera puerta.",
            respuestaCorrecta = "13",
            contenidoPista = "El primer objeto lo mirás antes de entrenar para saber si llegás tarde. El segundo es lo que hay que guardar cuando el DT te está retando en el entretiempo."
        },
        new Habitacion
        {
            id = 2,
            orden = 2,
            nombre = "Los Casilleros Numerados",
            narrativa = "Contra la otra pared hay cuatro casilleros con un candado de combinación de cuatro dígitos. Pegado con cinta encontrás un papel con pistas numéricas que dejó el utilero.",
            desafio = "Soy un número de cuatro cifras.\nLa primera cifra es el doble de la segunda.\nLa segunda cifra es 3.\nLa tercera cifra es la suma de la primera y la segunda.\nLa cuarta cifra es la diferencia entre la primera y la segunda.\n¿Cuál es el código?",
            respuestaCorrecta = "6393",
            contenidoPista = "Arrancá por la segunda cifra, que ya te la dan (es 3). Las demás son solo cuentas a partir de esa."
        },
        new Habitacion
        {
            id = 3,
            orden = 3,
            nombre = "La Sala de los Representantes",
            narrativa = "Del otro lado de la chapa escuchás discutir a tres representantes: A, B y C. Están peleando por quién se queda con la comisión de tu pase a Arabia Saudita. En la puerta hay tres números pegados, uno por cada uno.",
            desafio = "A dice: \"B está mintiendo.\"\nB dice: \"C está mintiendo.\"\nC dice: \"A y B están mintiendo.\"\nSolo uno de los tres dice la verdad. Los números en la puerta son: A = 17, B = 42, C = 86.\nIngresá el número de quien dice la verdad.",
            respuestaCorrecta = "86",
            contenidoPista = "Probá suponer que cada uno dice la verdad, uno a la vez, y fijate en cuál de los tres casos no se genera ninguna contradicción con lo que dicen los otros dos."
        },
        new Habitacion
        {
            id = 4,
            orden = 4,
            nombre = "El Cronograma del Vuelo",
            narrativa = "Encontrás una carpeta con el itinerario de tu \"traspaso\". Hay una secuencia de números, pero una mancha de café tapa el último. Ese número es el código de la puerta de salida del container.",
            desafio = "Entendé el patrón de la secuencia y descubrí el valor de X:\n2, 3, 7, 25, 121, 721, X",
            respuestaCorrecta = "5041",
            contenidoPista = "Fijate qué pasa si multiplicás todos los números anteriores entre sí (factorial) y le sumás 1 al resultado."
        }
    };

    // --------------------------------------------------------------
    // Partidas
    // --------------------------------------------------------------

    public int crearPartida(string nombreParticipante)
    {

        // TODO DAPPER: reemplazar por algo como
        // string query = "INSERT INTO Partidas (nombreParticipante, fechaInicio, estado, habitacionActual) " +
        //                 "VALUES (@pnombre, @pfecha, 'EnCurso', 1); SELECT SCOPE_IDENTITY();";
        // using (SqlConnection connection = new SqlConnection(_connectionString)) {
        //     nuevoId = connection.QuerySingle<int>(query, new { pnombre = nombreParticipante, pfecha = DateTime.Now });
        // }

        Partida nueva = new Partida
        {
            id = _proximoIdPartida++,
            nombreParticipante = nombreParticipante,
            fechaInicio = DateTime.Now,
            estado = "EnCurso",
            habitacionActual = 1
        };
        _partidas.Add(nueva);
        return nueva.id;

    }

    public Partida? buscarPartidaActivaPorParticipante(string nombreParticipante)
    {

        // TODO DAPPER: SELECT TOP 1 * FROM Partidas WHERE nombreParticipante = @pnombre AND estado = 'EnCurso'
        return _partidas.FirstOrDefault(p => p.nombreParticipante == nombreParticipante && p.estado == "EnCurso");

    }

    public Partida? consultarPartida(int id)
    {

        // TODO DAPPER: SELECT * FROM Partidas WHERE id = @pid
        return _partidas.FirstOrDefault(p => p.id == id);

    }

    public void actualizarHabitacionActual(int partidaId, int orden)
    {

        // TODO DAPPER: UPDATE Partidas SET habitacionActual = @porden WHERE id = @pid
        Partida? partida = consultarPartida(partidaId);
        if (partida != null)
        {
            partida.habitacionActual = orden;
        }

    }

    public void marcarPartidaFinalizada(int partidaId)
    {

        // TODO DAPPER: UPDATE Partidas SET estado = 'Finalizada' WHERE id = @pid
        Partida? partida = consultarPartida(partidaId);
        if (partida != null)
        {
            partida.estado = "Finalizada";
        }

    }

    // --------------------------------------------------------------
    // Habitaciones
    // --------------------------------------------------------------

    public List<Habitacion> obtenerHabitaciones()
    {

        // TODO DAPPER: SELECT * FROM Habitaciones ORDER BY orden
        return _habitaciones;

    }

    public Habitacion? obtenerHabitacionPorOrden(int orden)
    {

        // TODO DAPPER: SELECT * FROM Habitaciones WHERE orden = @porden
        return _habitaciones.FirstOrDefault(h => h.orden == orden);

    }

    public bool puedeAccederAHabitacion(int partidaId, int orden)
    {

        // Regla de negocio: para entrar a la sala "orden", la sala anterior
        // (orden - 1) tiene que figurar como resuelta en la base de datos.
        // La primera sala siempre está permitida.
        if (orden == 1)
        {
            return true;
        }

        Habitacion? habitacionAnterior = obtenerHabitacionPorOrden(orden - 1);
        if (habitacionAnterior == null)
        {
            return false;
        }

        return habitacionFueResuelta(partidaId, habitacionAnterior.id);

    }

    public bool habitacionFueResuelta(int partidaId, int habitacionId)
    {

        // TODO DAPPER: SELECT 1 FROM HabitacionesResueltas WHERE partidaId = @p AND habitacionId = @h
        return _resueltas.Contains((partidaId, habitacionId));

    }

    public void marcarHabitacionResuelta(int partidaId, int habitacionId)
    {

        // TODO DAPPER: INSERT INTO HabitacionesResueltas (partidaId, habitacionId, fechaResolucion)
        //              VALUES (@ppartida, @phabitacion, @pfecha)
        if (!habitacionFueResuelta(partidaId, habitacionId))
        {
            _resueltas.Add((partidaId, habitacionId));
        }

    }

    public List<int> obtenerHabitacionesResueltas(int partidaId)
    {

        // TODO DAPPER: SELECT habitacionId FROM HabitacionesResueltas WHERE partidaId = @p
        return _resueltas.Where(r => r.partidaId == partidaId).Select(r => r.habitacionId).ToList();

    }

    // --------------------------------------------------------------
    // Intentos
    // --------------------------------------------------------------

    public void registrarIntento(int partidaId, int habitacionId, string respuestaIngresada, bool correcto)
    {

        // TODO DAPPER: INSERT INTO Intentos (partidaId, habitacionId, respuestaIngresada, correcto, fecha, numeroIntento)
        //              VALUES (@ppartida, @phabitacion, @prespuesta, @pcorrecto, @pfecha, @pnumero)
        Intento nuevo = new Intento
        {
            id = _proximoIdIntento++,
            partidaId = partidaId,
            habitacionId = habitacionId,
            respuestaIngresada = respuestaIngresada,
            correcto = correcto,
            fecha = DateTime.Now,
            numeroIntento = contarIntentos(partidaId, habitacionId) + 1
        };
        _intentos.Add(nuevo);

    }

    public int contarIntentos(int partidaId, int habitacionId)
    {

        // TODO DAPPER: SELECT COUNT(*) FROM Intentos WHERE partidaId = @p AND habitacionId = @h
        return _intentos.Count(i => i.partidaId == partidaId && i.habitacionId == habitacionId);

    }

}
