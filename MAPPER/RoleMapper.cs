using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class RoleMapper : AbstractMapper<Role>
    {
        protected override string GetTableName() => "Roles";

        protected override string GetPrimaryKeyName() => "idRol";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Roles (nombreRol) VALUES (@nombreRol)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Roles SET nombreRol=@nombreRol WHERE idRol=@idRol";
        }

        protected override Role Map(SqlDataReader reader)
        {
            return new Role
            {
                Id = Convert.ToInt32(reader["idRol"]),
                NombreRol = reader["nombreRol"].ToString()
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, Role entity)
        {
            command.Parameters.AddWithValue("@nombreRol", entity.NombreRol);
        }

        protected override void AddParametersForUpdate(SqlCommand command, Role entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idRol", entity.Id);
        }
    }
}
