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