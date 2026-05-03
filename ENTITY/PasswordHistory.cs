using System;

namespace ENTITY
{
    public class PasswordHistory : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
