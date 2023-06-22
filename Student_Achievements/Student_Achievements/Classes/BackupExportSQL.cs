using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Achievements.Classes
{
    class BackupExportSQL
    {
        public void GetBackup()
        {
            try
            {
                string file = GetBackupFilePath();

                using (DB_Connect db = new DB_Connect())
                {
                    db.OpenConnect();
                    ExportDatabase(db, file);
                }

                ShowBackupSuccessMessage(file);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Произошла ошибка при выполнении резервного копирования базы данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("Произошла ошибка ввода-вывода при сохранении резервной копии: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetBackupFilePath()
        {
            DateTime dt = DateTime.Now;
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\backup_database_sql\\backup_sql" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";
            return file;
        }

        private void ExportDatabase(DB_Connect db, string file)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = db.GetConnect();
                    mb.ExportToFile(file);
                }
            }
        }

        private void ShowBackupSuccessMessage(string file)
        {
            MessageBox.Show("Экспорт всей базы данных произошел успешно! Резервное копирование сохранено по пути: " + file, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
