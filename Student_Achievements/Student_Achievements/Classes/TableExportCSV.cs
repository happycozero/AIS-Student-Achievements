using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Achievements.Classes
{
    class TableExportCSV
    {
        public void ExportAllTablesToCsv(string exportPath)
        {
            string backupFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup_table_csv");

            // Создаем папку backup_table_csv, если её не существует
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            // Создаем имя подпапки с текущей датой и временем в формате yyyy-MM-dd_HH-mm-ss
            string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string subFolderName = string.Format("export_table_{0}", dateTimeString);

            // Путь к подпапке
            string subFolderPath = Path.Combine(backupFolder, subFolderName);

            // Создаем подпапку с текущей датой и временем
            Directory.CreateDirectory(subFolderPath);

            // Получаем список таблиц в БД
            var tables = new List<string> { "cource", "employer", "event", "group", "level_event", "list_result", "prize_place", "specialization", "student", "student_status", "user", "user_role" };

            DB_Connect dbConnect = new DB_Connect();
            dbConnect.GetConnect();

            // Цикл для перебора каждой таблицы
            foreach (string table in tables)
            {
                // SQL-запрос для получения всех данных из таблицы
                string query = string.Format("SELECT * FROM {0};", table);

                using (MySqlCommand command = new MySqlCommand(query, dbConnect.GetConnect()))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Имя файла CSV соответствует имени таблицы
                        string csvFileName = string.Format("{0}.csv", table);

                        // Путь к файлу CSV в подпапке
                        string csvFilePath = Path.Combine(subFolderPath, csvFileName);

                        using (StreamWriter writer = new StreamWriter(csvFilePath))
                        {
                            // Записываем заголовки столбцов в CSV-файл
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                writer.Write(reader.GetName(i));
                                if (i < reader.FieldCount - 1) writer.Write(",");
                            }

                            writer.WriteLine();

                            // Записываем данные в CSV-файл
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    if (!reader.IsDBNull(i))
                                    {
                                        // Делаем экранирование для символов запятых и кавычек
                                        string value = reader.GetString(i).Replace("\"", "\"\"");
                                        writer.Write(string.Format("\"{0}\"", value));
                                    }
                                    else
                                    {
                                        writer.Write("");
                                    }

                                    if (i < reader.FieldCount - 1) writer.Write(",");
                                }

                                writer.WriteLine();
                            }
                        }
                    }
                }
            }

            dbConnect.CloseConnect();
        }
    }
}
