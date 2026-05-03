using System;

namespace ENTITY
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Legajo { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int IdRol { get; set; }
        public int? IdPerfilStack { get; set; }
        
        // Helper property for backward compatibility with AuthService
        public string Username 
        { 
            get { return Legajo; } 
            set { Legajo = value; } 
        }
    }
}
