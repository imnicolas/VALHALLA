using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class PasswordHistoryMapper : AbstractMapper<PasswordHistory>
    {
        protected override string GetTableName() => "Historial_Passwords";

        protected override string GetPrimaryKeyName() => "idHistorial";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Historial_Passwords (idUsuario, passwordHash, fechaCambio) VALUES (@idUsuario, @passwordHash, @fechaCambio)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Historial_Passwords SET idUsuario=@idUsuario, passwordHash=@passwordHash, fechaCambio=@fechaCambio WHERE idHistorial=@idHistorial";
        }

        protected override PasswordHistory Map(SqlDataReader reader)
        {
            return new PasswordHistory
            {
                Id = Convert.ToInt32(reader["idHistorial"]),
                UserId = Convert.ToInt32(reader["idUsuario"]),
                PasswordHash = reader["passwordHash"].ToString(),
                CreatedAt = Convert.ToDateTime(reader["fechaCambio"])
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, PasswordHistory entity)
        {
            command.Parameters.AddWithValue("@idUsuario", entity.UserId);
            command.Parameters.AddWithValue("@passwordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@fechaCambio", entity.CreatedAt);
        }

        protected override void AddParametersForUpdate(SqlCommand command, PasswordHistory entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idHistorial", entity.Id);
        }
    }
}
