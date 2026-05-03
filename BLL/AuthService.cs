using System;
using System.Linq;
using DAL.Security;
using ENTITY;
using MAPPER;

namespace BLL
{
    public class AuthService
    {
        private readonly IGenericMapper<User> _userMapper;
        private readonly CryptographyService _cryptoService;

        public AuthService()
        {
            _userMapper = new UserMapper();
            _cryptoService = new CryptographyService();
        }

        public User Authenticate(string username, string password)
        {
            // Note: username is now mapped to legajo in User entity
            var user = _userMapper.GetAll().FirstOrDefault(u => u.Username == username);

            if (user == null)
                return null;

            string hashedPassword = _cryptoService.HashPassword(password);

            if (user.PasswordHash == hashedPassword)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public string CreateUser(string legajo, string email, int idRol)
        {
            string generatedPassword = GenerateRandomPassword();
            string hashedPassword = _cryptoService.HashPassword(generatedPassword);

            var newUser = new User
            {
                Legajo = legajo,
                Email = email,
                PasswordHash = hashedPassword,
                IdRol = idRol
            };

            _userMapper.Add(newUser);

            return generatedPassword; // Return it so the admin can give it to the user
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = _userMapper.GetById(userId);
            if (user == null) throw new Exception("Usuario no encontrado.");

            string newHashedPassword = _cryptoService.HashPassword(newPassword);

            // Update user
            user.PasswordHash = newHashedPassword;
            _userMapper.Update(user);
        }

        private string GenerateRandomPassword()
        {
            return "Temp" + Guid.NewGuid().ToString().Substring(0, 8) + "!";
        }
    }
}
