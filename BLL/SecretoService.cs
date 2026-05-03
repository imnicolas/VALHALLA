using System;
using System.Collections.Generic;
using System.Linq;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class SecretoService
    {
        private readonly SecretoCifradoMapper _secretoMapper;
        private readonly CryptoHelper _cryptoHelper;

        public SecretoService()
        {
            _secretoMapper = new SecretoCifradoMapper();
            _cryptoHelper = new CryptoHelper();
        }

        public void GuardarSecreto(int idUsuario, string tipoDocumento, string contenidoPlano)
        {
            if (string.IsNullOrWhiteSpace(tipoDocumento))
                throw new Exception("El tipo de documento no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(contenidoPlano))
                throw new Exception("El contenido del secreto no puede estar vacío.");

            // Validar si el usuario ya tiene un secreto de ese tipo, si lo tiene lo actualizamos, si no lo creamos.
            var secretoExistente = ObtenerSecretosDeUsuario(idUsuario)
                                    .FirstOrDefault(s => s.TipoDocumento.Equals(tipoDocumento, StringComparison.OrdinalIgnoreCase));

            // Encriptar el contenido plano a VARBINARY
            byte[] blobCifrado = _cryptoHelper.Encrypt(contenidoPlano);

            if (secretoExistente != null)
            {
                secretoExistente.BlobCifrado = blobCifrado;
                _secretoMapper.Update(secretoExistente);
            }
            else
            {
                var nuevoSecreto = new SecretoCifrado
                {
                    TipoDocumento = tipoDocumento,
                    BlobCifrado = blobCifrado,
                    IdUsuario = idUsuario
                };
                _secretoMapper.Add(nuevoSecreto);
            }
        }

        public List<SecretoCifrado> ObtenerSecretosDeUsuario(int idUsuario)
        {
            return _secretoMapper.GetAll().Where(s => s.IdUsuario == idUsuario).ToList();
        }
    }
}
