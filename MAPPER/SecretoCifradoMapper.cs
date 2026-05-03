using System;
using Microsoft.Data.SqlClient;
using ENTITY;

namespace MAPPER
{
    public class SecretoCifradoMapper : AbstractMapper<SecretoCifrado>
    {
        protected override string GetTableName() => "Secretos_Cifrados";

        protected override string GetPrimaryKeyName() => "idSecreto";

        protected override string GetInsertQuery()
        {
            return "INSERT INTO Secretos_Cifrados (tipoDocumento, blobCifrado, idUsuario) VALUES (@tipoDocumento, @blobCifrado, @idUsuario)";
        }

        protected override string GetUpdateQuery()
        {
            return "UPDATE Secretos_Cifrados SET tipoDocumento=@tipoDocumento, blobCifrado=@blobCifrado, idUsuario=@idUsuario WHERE idSecreto=@idSecreto";
        }

        protected override SecretoCifrado Map(SqlDataReader reader)
        {
            return new SecretoCifrado
            {
                Id = Convert.ToInt32(reader["idSecreto"]),
                TipoDocumento = reader["tipoDocumento"].ToString(),
                BlobCifrado = (byte[])reader["blobCifrado"],
                IdUsuario = Convert.ToInt32(reader["idUsuario"])
            };
        }

        protected override void AddParametersForInsert(SqlCommand command, SecretoCifrado entity)
        {
            command.Parameters.AddWithValue("@tipoDocumento", entity.TipoDocumento);
            command.Parameters.AddWithValue("@blobCifrado", entity.BlobCifrado);
            command.Parameters.AddWithValue("@idUsuario", entity.IdUsuario);
        }

        protected override void AddParametersForUpdate(SqlCommand command, SecretoCifrado entity)
        {
            AddParametersForInsert(command, entity);
            command.Parameters.AddWithValue("@idSecreto", entity.Id);
        }
    }
}
