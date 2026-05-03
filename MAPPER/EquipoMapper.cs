using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class EquipoMapper : AbstractMapper<Equipo>
    {
        protected override string GetTableName() => "Equipos";

        protected override string GetPrimaryKeyName() => "idEquipo";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Equipos (macAddress, nroSerie, estado, idUsuario) VALUES (@macAddress, @nroSerie, @estado, @idUsuario)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Equipos SET macAddress=@macAddress, nroSerie=@nroSerie, estado=@estado, idUsuario=@idUsuario WHERE idEquipo=@idEquipo";
        }

        protected override Equipo Map(SqlDataReader reader)
        {
            return new Equipo
            {
                Id = Convert.ToInt32(reader["idEquipo"]),
                MacAddress = reader["macAddress"]?.ToString(),
                NroSerie = reader["nroSerie"]?.ToString(),
                Estado = reader["estado"].ToString(),
                IdUsuario = reader["idUsuario"] != DBNull.Value ? Convert.ToInt32(reader["idUsuario"]) : (int?)null
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, Equipo entity)
        {
            command.Parameters.AddWithValue("@macAddress", entity.MacAddress ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@nroSerie", entity.NroSerie ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@estado", entity.Estado);
            command.Parameters.AddWithValue("@idUsuario", entity.IdUsuario.HasValue ? (object)entity.IdUsuario.Value : DBNull.Value);
        }

        protected override void AddParametersForUpdate(SqlCommand command, Equipo entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idEquipo", entity.Id);
        }

        public Equipo GetByMacAddress(string macAddress)
        {
            return GetAll().FirstOrDefault(e => e.MacAddress != null && e.MacAddress.Equals(macAddress, StringComparison.OrdinalIgnoreCase));
        }
    }
}
