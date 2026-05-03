using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class UserMapper : AbstractMapper<User>
    {
        protected override string GetTableName() => "Usuarios";

        protected override string GetPrimaryKeyName() => "idUsuario";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Usuarios (legajo, email, passwordHash, idRol, idPerfilStack) " +
                   "VALUES (@legajo, @email, @passwordHash, @idRol, @idPerfilStack)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Usuarios SET legajo=@legajo, email=@email, passwordHash=@passwordHash, idRol=@idRol, idPerfilStack=@idPerfilStack " +
                   "WHERE idUsuario=@idUsuario";
        }

        protected override User Map(SqlDataReader reader)
        {
            return new User
            {
                Id = Convert.ToInt32(reader["idUsuario"]),
                Legajo = reader["legajo"].ToString(),
                Email = reader["email"].ToString(),
                PasswordHash = reader["passwordHash"].ToString(),
                IdRol = Convert.ToInt32(reader["idRol"]),
                IdPerfilStack = reader["idPerfilStack"] != DBNull.Value ? Convert.ToInt32(reader["idPerfilStack"]) : (int?)null
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, User entity)
        {
            command.Parameters.AddWithValue("@legajo", entity.Legajo);
            command.Parameters.AddWithValue("@email", entity.Email);
            command.Parameters.AddWithValue("@passwordHash", entity.PasswordHash);
            command.Parameters.AddWithValue("@idRol", entity.IdRol);
            command.Parameters.AddWithValue("@idPerfilStack", entity.IdPerfilStack.HasValue ? (object)entity.IdPerfilStack.Value : DBNull.Value);
        }

        protected override void AddParametersForUpdate(SqlCommand command, User entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idUsuario", entity.Id);
        }
    }
}
