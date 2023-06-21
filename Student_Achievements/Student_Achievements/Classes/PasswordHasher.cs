using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Student_Achievements.Classes
{
    class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hashedPasswordBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedPasswordBytes);
            }
        }
    }
}
