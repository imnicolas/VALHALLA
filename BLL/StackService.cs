using System;
using System.Collections.Generic;
using System.Linq;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class StackService
    {
        private readonly HerramientaMapper _herramientaMapper;
        private readonly PerfilStackMapper _perfilMapper;

        public StackService()
        {
            _herramientaMapper = new HerramientaMapper();
            _perfilMapper = new PerfilStackMapper();
        }

        public List<Herramienta> ObtenerHerramientasDisponibles()
        {
            return _herramientaMapper.GetAll().ToList();
        }

        public void CrearPerfilStack(string nombrePerfil, List<Herramienta> herramientasSeleccionadas)
        {
            // RN-02: Validar que la lista de herramientas seleccionadas no esté vacía
            if (string.IsNullOrWhiteSpace(nombrePerfil))
            {
                throw new Exception("El nombre del perfil no puede estar vacío.");
            }

            if (herramientasSeleccionadas == null || herramientasSeleccionadas.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos una herramienta para el perfil.");
            }

            var nuevoPerfil = new PerfilStack
            {
                NombrePerfil = nombrePerfil,
                Herramientas = herramientasSeleccionadas
            };

            // Guarda el perfil y sus relaciones en la base de datos
            _perfilMapper.Add(nuevoPerfil);
        }
    }
}
