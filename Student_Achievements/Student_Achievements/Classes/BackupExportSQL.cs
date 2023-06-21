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
        //Метод getBackup
        public void getBackup()
        {
            //Получение даты на данный момент
            DateTime dt = DateTime.Now;
            //Путь к сохранению резервного копирования БД
            string file = AppDomain.CurrentDomain.BaseDirectory + "\\backup_database_sql\\backup_sql" + dt.ToString("yyyy-MM-dd_HH-mm-ss") + ".sql";
            //Создаем экземпляр класса подключения к БД
            DB_Connect db = new DB_Connect();
            //Открываем соединение
            db.OpenConnect();
            //Выполнение команды
            using (MySqlCommand cmd = new MySqlCommand())
            {
                //Открытие соедиения, выполнение резервного копирования и сохранения по пути, закрытие соединения с БД
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = db.GetConnect();
                    mb.ExportToFile(file);
                }
            }
            //Закрываем соединение
            db.CloseConnect();

            //Выводим сообщение об успешном экспорте базы данных 
            MessageBox.Show("Экспорт всей базы данных произошел успешно! Резервное копирование сохранено по пути: " + file, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
