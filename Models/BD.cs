using System.Data;
using Microsoft.Data.SqlClient; 
using Dapper;

namespace EscapeDelChacho.Models 
{
    public class BD
    {
        private static string _connectionString = @"Server=localhost;Database=EscapeDelChacho;Trusted_Connection=True;TrustServerCertificate=True;";

        public static void IniciarPartida(string nombre)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string storedProcedure = "sp_IniciarPartida";
                db.Execute(storedProcedure, new { Nombre = nombre }, commandType: CommandType.StoredProcedure);
            }
        }
        
        public static void ActualizarSala(int partidaId, int nuevaSala)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string storedProcedure = "sp_ActualizarSala";
                db.Execute(storedProcedure, new { Id = partidaId, Sala = nuevaSala }, commandType: CommandType.StoredProcedure);
            }
        }

        public static Partida ObtenerPartida(int partidaId)
        {
            using (SqlConnection db = new SqlConnection(_connectionString))
            {
                string storedProcedure = "sp_ObtenerPartida";
                return db.QueryFirstOrDefault<Partida>(storedProcedure, new { Id = partidaId }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}