using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using Student_Achievements.Forms.Administrator.SpecialFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Student_Achievements.Forms
{
    /// <summary>
    /// Interaction logic for SpecialFeature.xaml
    /// </summary>
    public partial class SpecialFeature
    {
        public SpecialFeature()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            var menuAdmin = new MenuAdministrator();
            System.Windows.Application.Current.MainWindow = menuAdmin;
            this.Hide();
            menuAdmin.Show();
        }

        private void ButImport_Click(object sender, RoutedEventArgs e)
        {
            var importForm = new Import();
            System.Windows.Application.Current.MainWindow = importForm;
            this.Hide();
            importForm.Show();
        }

        private void ButRecover_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр OpenFileDialog
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            // Устанавливаем фильтр расширений для файлов
            openFileDialog.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";

            // Открываем диалоговое окно и проверяем, что пользователь нажал кнопку OK
            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем выбранный файл
                string file = openFileDialog.FileName;

                try
                {
                    // Создаем экземпляр класса подключения к БД
                    using (var db = new DB_Connect())
                    {
                        // Выполняем команду
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            // Открываем соединение, выполняем восстановление и закрываем соединение с БД
                            using (MySqlBackup mb = new MySqlBackup(cmd))
                            {
                                cmd.Connection = db.GetConnect();
                                mb.ImportFromFile(file);
                            }
                        }

                        System.Windows.MessageBox.Show("База данных была успешно восстановлена из файла " + file, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButBackupCopy_Click(object sender, RoutedEventArgs e)
        {
            BackupExportSQL back = new BackupExportSQL();
            back.getBackup();
        }

        private void ButExport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}