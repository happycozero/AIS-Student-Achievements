using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Student_Achievements.Forms.Administrator.SpecialFeatures
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Export : Window
    {
        public Export()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButWay_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                tbExport.Text = dialog.SelectedPath;
            }
        }

        private void ButExport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbExport.Text))
            {
                MessageBox.Show("Предупреждение! Выберите путь для экспорта.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbRazd.SelectedIndex == -1)
            {
                MessageBox.Show("Предупреждение! Выберите разделитель для экспорта.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string exportFolderPath = tbExport.Text.Replace("/", "\\");
                string backupFolderName = "backup_tables_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFolderPath = System.IO.Path.Combine(exportFolderPath, backupFolderName);
                Directory.CreateDirectory(backupFolderPath);

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();

                    // Получаем список всех таблиц в базе данных
                    DataTable schema = connect.GetConnect().GetSchema("Tables");
                    string[] tableNames = schema.AsEnumerable()
                        .Where(row => row["TABLE_SCHEMA"].ToString() == connect.GetConnect().Database)
                        .Select(row => row["TABLE_NAME"].ToString())
                        .ToArray();

                    // Проверяем, что в базе данных есть таблицы
                    if (tableNames.Length == 0)
                    {
                        MessageBox.Show("Предупреждение! В базе данных отсутствуют таблицы.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    foreach (string tableName in tableNames)
                    {
                        MySqlCommand cmd = new MySqlCommand("SELECT * FROM `" + tableName + "`", connect.GetConnect());
                        MySqlDataReader reader = cmd.ExecuteReader();

                        string filePath = System.IO.Path.Combine(backupFolderPath, tableName + ".csv");
                        StreamWriter sw = new StreamWriter(filePath);

                        // записываем заголовки столбцов в CSV файл
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            sw.Write("`" + reader.GetName(i) + "`");
                            if (i < reader.FieldCount - 1)
                            {
                                sw.Write(cbRazd.Text);
                            }
                        }
                        sw.WriteLine();

                        // записываем данные из таблицы в CSV файл
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {
                                    sw.Write(reader.GetValue(i).ToString());
                                }
                                if (i < reader.FieldCount - 1)
                                {
                                    sw.Write(cbRazd.Text);
                                }
                            }
                            sw.WriteLine();
                        }

                        sw.Close();
                        reader.Close();
                    }
                }

                tbExport.Clear();
                cbRazd.SelectedIndex = -1;

                MessageBox.Show("Успешно! Экспорт завершен. Файлы сохранены по выбранному пути.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка!" + ex.Message, "Ошибка программы", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButSpecFeat_Click(object sender, RoutedEventArgs e)
        {
            var SpecialFeatForm = new SpecialFeature();
            Application.Current.MainWindow = SpecialFeatForm;
            this.Hide();
            SpecialFeatForm.Show();
        }
    }
}
