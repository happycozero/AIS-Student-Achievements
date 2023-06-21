using System;

namespace Student_Achievements.Classes
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?";
            var password = new char[length];

            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                password[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(password);
        }
    }
}
