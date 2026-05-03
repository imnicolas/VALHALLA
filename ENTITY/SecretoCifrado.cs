using System;

namespace ENTITY
{
    public class SecretoCifrado : BaseEntity
    {
        public int Id { get; set; }
        public string TipoDocumento { get; set; }
        public byte[] BlobCifrado { get; set; }
        public int IdUsuario { get; set; }
    }
}
