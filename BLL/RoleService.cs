using System;
using System.Linq;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class RoleService
    {
        private readonly RoleMapper _roleMapper;

        public RoleService()
        {
            _roleMapper = new RoleMapper();
        }

        public Role GetRoleById(int id)
        {
            return _roleMapper.GetById(id);
        }

        public string GetRoleNameById(int id)
        {
            var role = _roleMapper.GetById(id);
            return role != null ? role.NombreRol : "Desconocido";
        }

        public Role GetRoleByName(string name)
        {
            return _roleMapper.GetAll().FirstOrDefault(r => r.NombreRol.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
