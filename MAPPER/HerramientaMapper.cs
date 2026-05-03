using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class HerramientaMapper : AbstractMapper<Herramienta>
    {
        protected override string GetTableName() => "Herramientas";

        protected override string GetPrimaryKeyName() => "idHerramienta";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Herramientas (nombre, version, scriptBase) VALUES (@nombre, @version, @scriptBase)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Herramientas SET nombre=@nombre, version=@version, scriptBase=@scriptBase WHERE idHerramienta=@idHerramienta";
        }

        protected override Herramienta Map(SqlDataReader reader)
        {
            return new Herramienta
            {
                Id = Convert.ToInt32(reader["idHerramienta"]),
                Nombre = reader["nombre"].ToString(),
                Version = reader["version"] != DBNull.Value ? reader["version"].ToString() : null,
                ScriptBase = reader["scriptBase"] != DBNull.Value ? reader["scriptBase"].ToString() : null
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, Herramienta entity)
        {
            command.Parameters.AddWithValue("@nombre", entity.Nombre);
            command.Parameters.AddWithValue("@version", entity.Version ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@scriptBase", entity.ScriptBase ?? (object)DBNull.Value);
        }

        protected override void AddParametersForUpdate(SqlCommand command, Herramienta entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idHerramienta", entity.Id);
        }

        public System.Collections.Generic.List<Herramienta> GetHerramientasByPerfil(int idPerfilStack)
        {
            var herramientas = new System.Collections.Generic.List<Herramienta>();
            string query = @"
                SELECT h.idHerramienta, h.nombre, h.version, h.scriptBase
                FROM Herramientas h
                INNER JOIN Perfil_Herramienta ph ON h.idHerramienta = ph.idHerramienta
                WHERE ph.idPerfilStack = @idPerfil";

            using (var connection = _dbConnection.GetConnection())
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idPerfil", idPerfilStack);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            herramientas.Add(Map(reader));
                        }
                    }
                }
            }
            return herramientas;
        }
    }
}
