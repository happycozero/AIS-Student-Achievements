using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Student_Achievements.Classes
{
    public class InputValidator
    {
        public bool Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Поле не должно быть пустым.", "Ошибка добавления записи", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверяем наличие русских символов
            Regex regex = new Regex("[А-Яа-яЁё]");
            bool hasRussianLetters = regex.IsMatch(input);

            if (!hasRussianLetters)
            {
                MessageBox.Show("Название должно содержать хотя бы одну русскую букву.", "Ошибка добавления записи", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверяем отсутствие цифр и английской раскладки
            regex = new Regex("[0-9a-zA-Z]");
            bool hasEnglishOrDigits = regex.IsMatch(input);

            if (hasEnglishOrDigits)
            {
                MessageBox.Show("Название не должно содержать цифры или символы латинского алфавита.", "Ошибка добавления записи", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}