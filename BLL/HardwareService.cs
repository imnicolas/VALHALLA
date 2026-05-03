using System;
using System.Linq;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class HardwareService
    {
        private readonly EquipoMapper _equipoMapper;
        private readonly UserMapper _userMapper;

        private readonly RoleService _roleService;

        public HardwareService()
        {
            _equipoMapper = new EquipoMapper();
            _userMapper = new UserMapper();
            _roleService = new RoleService();
        }

        public List<User> ObtenerDesarrolladores()
        {
            var roleDesarrollador = _roleService.GetRoleByName("Desarrollador");
            if (roleDesarrollador == null) return new List<User>();

            return _userMapper.GetAll().Where(u => u.IdRol == roleDesarrollador.Id).ToList();
        }

        public List<Equipo> ObtenerEquiposDisponibles()
        {
            return _equipoMapper.GetAll().Where(e => e.Estado == "Disponible").ToList();
        }

        public void RegistrarEquipo(string macAddress, string nroSerie)
        {
            if (string.IsNullOrWhiteSpace(macAddress))
                throw new Exception("La MAC Address es obligatoria.");

            var equipoExistente = _equipoMapper.GetByMacAddress(macAddress);
            if (equipoExistente != null)
                throw new Exception("Ya existe un equipo registrado con esa MAC Address.");

            var nuevoEquipo = new Equipo
            {
                MacAddress = macAddress,
                NroSerie = nroSerie,
                Estado = "Disponible",
                IdUsuario = null
            };

            _equipoMapper.Add(nuevoEquipo);
        }

        public void AsignarHardware(string legajo, string macAddress)
        {
            if (string.IsNullOrWhiteSpace(legajo) || string.IsNullOrWhiteSpace(macAddress))
                throw new Exception("El legajo y la MAC Address son obligatorios.");

            // Buscar al usuario
            var user = _userMapper.GetAll().FirstOrDefault(u => u.Legajo == legajo);
            if (user == null)
                throw new Exception("No se encontró ningún desarrollador con ese legajo.");

            // RN-03: Un desarrollador no puede tener más de un equipo principal asignado en simultáneo
            var equipoDelUsuario = _equipoMapper.GetAll().FirstOrDefault(e => e.IdUsuario == user.Id);
            if (equipoDelUsuario != null)
                throw new Exception($"Regla RN-03: El desarrollador {legajo} ya tiene asignado el equipo con MAC {equipoDelUsuario.MacAddress}.");

            // FA-01: Validar que el equipo exista y esté disponible
            var equipoDestino = _equipoMapper.GetByMacAddress(macAddress);
            if (equipoDestino == null)
                throw new Exception("No se encontró ningún equipo con esa MAC Address en el inventario.");

            if (equipoDestino.IdUsuario != null || equipoDestino.Estado != "Disponible")
            {
                var usuarioOcupante = _userMapper.GetById(equipoDestino.IdUsuario.Value);
                string legajoOcupante = usuarioOcupante != null ? usuarioOcupante.Legajo : "Desconocido";
                throw new Exception($"Flujo FA-01: El equipo ya se encuentra asignado actualmente al legajo {legajoOcupante}.");
            }

            // Proceder con la asignación
            equipoDestino.IdUsuario = user.Id;
            equipoDestino.Estado = "Asignado";
            
            _equipoMapper.Update(equipoDestino);
        }
    }
}
