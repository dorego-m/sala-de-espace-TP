CREATE DATABASE EscapeDelChacho;
GO

USE EscapeDelChacho;
GO

CREATE TABLE Partidas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreParticipante VARCHAR(100) NOT NULL,
    SalaActual INT NOT NULL DEFAULT 1,
    FechaInicio DATETIME NOT NULL
);
GO

CREATE TABLE Habitaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Orden INT NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Narrativa VARCHAR(MAX) NOT NULL,
    Desafio VARCHAR(MAX) NOT NULL,
    RespuestaCorrecta VARCHAR(100) NOT NULL,
    ContenidoPista VARCHAR(MAX) NOT NULL
);
GO

INSERT INTO Habitaciones (Orden, Nombre, Narrativa, Desafio, RespuestaCorrecta, ContenidoPista)
VALUES
    (
        1,
        'Sala 1: El contenedor',
        'Te despertás en un contenedor frío y oscuro. Hay una puerta cerrada y una nota en la pared.',
        'La nota dice: “La primera clave es la palabra que abre la puerta sin ser llave.”',
        'puerta',
        'La pista: pensá en la palabra que se usa para abrir algo sin usar una llave.'
    ),
    (
        2,
        'Sala 2: El almacén',
        'La segunda sala tiene cajas apiladas y una máquina con un cartel: “Solo la verdad abre este cierre.”',
        'Resolvé el acertijo para activar el mecanismo.',
        'verdad',
        'La pista: el cartel habla de “verdad”, no de números ni de objetos.'
    ),
    (
        3,
        'Sala 3: La pasarela',
        'Hay una pasarela con luces parpadeantes y una frase escrita en el metal: “Sigue el camino correcto.”',
        'Ingresá la respuesta que representa el camino correcto para continuar.',
        'correcto',
        'La pista: la frase repite la palabra clave de la solución.'
    ),
    (
        4,
        'Sala 4: La salida',
        'La última habitación tiene la salida frente a vos, pero antes tenés que decir la palabra final para liberar el candado.',
        'Descubrí la clave final para escapar.',
        'libertad',
        'La pista: la salida se abre con libertad, no con miedo ni con prisa.'
    );
GO

CREATE PROCEDURE sp_IniciarPartida
    @Nombre VARCHAR(100)
AS
BEGIN
    INSERT INTO Partidas (NombreParticipante, SalaActual, FechaInicio)
    VALUES (@Nombre, 1, GETDATE())
END
GO

CREATE PROCEDURE sp_ActualizarSala
    @Id INT,
    @Sala INT
AS
BEGIN
    UPDATE Partidas 
    SET SalaActual = @Sala 
    WHERE Id = @Id
END
GO

CREATE PROCEDURE sp_ObtenerPartida
    @Id INT
AS
BEGIN
    SELECT * FROM Partidas 
    WHERE Id = @Id
END
GO
