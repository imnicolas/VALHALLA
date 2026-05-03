using System;
using System.IO;
using System.Linq;
using System.Text;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class EntornoService
    {
        private readonly UserMapper _userMapper;
        private readonly EquipoMapper _equipoMapper;
        private readonly NetworkHelper _networkHelper;
        private readonly HerramientaMapper _herramientaMapper;
        private readonly SecretoService _secretoService;

        public EntornoService()
        {
            _userMapper = new UserMapper();
            _equipoMapper = new EquipoMapper();
            _networkHelper = new NetworkHelper();
            _herramientaMapper = new HerramientaMapper();
            _secretoService = new SecretoService();
        }

        // CU-05: Validar Asignación de Hardware
        public void ValidarHardware(int idUsuario)
        {
            string macFisica = _networkHelper.GetPhysicalMacAddress();
            
            if (string.IsNullOrEmpty(macFisica))
                throw new Exception("No se pudo detectar ninguna tarjeta de red física activa en este equipo.");

            // Normalizar MAC eliminando guiones o dos puntos para comparar correctamente
            macFisica = macFisica.Replace(":", "").Replace("-", "").ToUpper();

            // Buscar equipos asignados a este usuario
            var equiposDelUsuario = _equipoMapper.GetAll().Where(e => e.IdUsuario == idUsuario).ToList();
            
            if (!equiposDelUsuario.Any())
                throw new Exception("Flujo FA-01: No tienes ningún equipo de hardware asignado a tu cuenta.");

            bool macCoincide = equiposDelUsuario.Any(e => e.MacAddress != null && 
                                                           e.MacAddress.Replace(":", "").Replace("-", "").ToUpper() == macFisica);

            if (!macCoincide)
            {
                throw new Exception($"Flujo FA-01 (Seguridad Zero Trust): La MAC Address de esta computadora ({macFisica}) no coincide con la asignada en el inventario. Se detiene el proceso de restauración.");
            }
        }

        // CU-01: Restaurar Entorno de Desarrollo
        public void GenerarScriptRestauracion(int idUsuario, string outputPath)
        {
            // 1. Validar hardware (CU-05)
            ValidarHardware(idUsuario);

            // 2. Traer el usuario y su perfil
            var user = _userMapper.GetById(idUsuario);
            if (user == null || user.IdPerfilStack == null)
                throw new Exception("El usuario no tiene un Perfil Stack asignado.");

            // 3. Traer herramientas
            var herramientas = _herramientaMapper.GetHerramientasByPerfil(user.IdPerfilStack.Value);

            // 4. Traer y desencriptar secretos
            var secretos = _secretoService.ObtenerSecretosDeUsuario(idUsuario);
            var cryptoHelper = new CryptoHelper();

            // 5. Ensamblar Script PowerShell (.ps1)
            StringBuilder ps1Content = new StringBuilder();
            ps1Content.AppendLine("# ==========================================");
            ps1Content.AppendLine("# SCRIPT DE RESTAURACIÓN DE ENTORNO - VALHALLA");
            ps1Content.AppendLine($"# Usuario: {user.Email}");
            ps1Content.AppendLine("# ==========================================");
            ps1Content.AppendLine("");

            // Instalar herramientas
            ps1Content.AppendLine("Write-Host 'Instalando Herramientas del Stack...' -ForegroundColor Cyan");
            foreach (var h in herramientas)
            {
                ps1Content.AppendLine($"Write-Host 'Ejecutando: {h.Nombre}'");
                ps1Content.AppendLine(h.ScriptBase);
            }
            ps1Content.AppendLine("");

            // Restaurar secretos
            ps1Content.AppendLine("Write-Host 'Restaurando configuraciones y secretos personales...' -ForegroundColor Cyan");
            foreach (var secreto in secretos)
            {
                string contenidoPlano = cryptoHelper.Decrypt(secreto.BlobCifrado);
                // Escapar comillas dobles para PowerShell
                string contenidoEscapado = contenidoPlano.Replace("\"", "`\"");

                ps1Content.AppendLine($"$content = @\"");
                ps1Content.AppendLine(contenidoEscapado);
                ps1Content.AppendLine("\"@");
                ps1Content.AppendLine($"Set-Content -Path \"$env:USERPROFILE\\{secreto.TipoDocumento}\" -Value $content -Encoding UTF8");
                ps1Content.AppendLine($"Write-Host 'Archivo {secreto.TipoDocumento} restaurado con éxito.' -ForegroundColor Green");
            }

            ps1Content.AppendLine("");
            ps1Content.AppendLine("Write-Host '¡Entorno restaurado completamente!' -ForegroundColor Yellow");
            ps1Content.AppendLine("Pause");

            // Guardar en disco local (La clave maestra nunca viaja, el script ya contiene la configuración plana localmente)
            File.WriteAllText(outputPath, ps1Content.ToString(), Encoding.UTF8);
        }
    }
}
