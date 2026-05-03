using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class PerfilStackMapper : AbstractMapper<PerfilStack>
    {
        protected override string GetTableName() => "PerfilesStack";

        protected override string GetPrimaryKeyName() => "idPerfilStack";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO PerfilesStack (nombrePerfil) VALUES (@nombrePerfil)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE PerfilesStack SET nombrePerfil=@nombrePerfil WHERE idPerfilStack=@idPerfilStack";
        }

        protected override PerfilStack Map(SqlDataReader reader)
        {
            return new PerfilStack
            {
                Id = Convert.ToInt32(reader["idPerfilStack"]),
                NombrePerfil = reader["nombrePerfil"].ToString()
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, PerfilStack entity)
        {
            command.Parameters.AddWithValue("@nombrePerfil", entity.NombrePerfil);
        }

        protected override void AddParametersForUpdate(SqlCommand command, PerfilStack entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idPerfilStack", entity.Id);
        }

        // We override Add to also save the tools in the intermediate table
        new public void Add(PerfilStack entity)
        {
            // Use transaction to ensure both Profile and relations are saved together
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Profile
                        string insertQuery = GetInsertQuery() + "; SELECT SCOPE_IDENTITY();";
                        using (var command = new SqlCommand(insertQuery, connection, transaction))
                        {
                            AddParametersForInsert(command, entity);
                            var result = command.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                entity.Id = Convert.ToInt32((decimal)result);
                            }
                        }

                        // 2. Insert relations in Perfil_Herramienta
                        if (entity.Herramientas != null && entity.Herramientas.Count > 0)
                        {
                            string relationQuery = "INSERT INTO Perfil_Herramienta (idPerfilStack, idHerramienta) VALUES (@idPerfil, @idHerramienta)";
                            foreach (var herramienta in entity.Herramientas)
                            {
                                using (var relCommand = new SqlCommand(relationQuery, connection, transaction))
                                {
                                    relCommand.Parameters.AddWithValue("@idPerfil", entity.Id);
                                    relCommand.Parameters.AddWithValue("@idHerramienta", herramienta.Id);
                                    relCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
