using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
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
            Fill_Export();
        }

        public void Fill_Export()
        {
            cbTableExp.Items.Clear();

            DB_Connect connect = new DB_Connect();
            connect.OpenConnect();

            string sql = "SHOW TABLES;";
            MySqlCommand com = new MySqlCommand(sql, connect.GetConnect());
            MySqlDataReader reader = null;

            try
            {
                reader = com.ExecuteReader();
                while (reader.Read())
                {
                    cbTableExp.Items.Add(reader[0].ToString());
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                connect.CloseConnect();
            }
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
            if (cbTableExp.Text == "")
            {
                MessageBox.Show("Предупреждение! Выберите таблицу для экспорта!", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (cbTableExp.Text == "")
            {
                MessageBox.Show("Предупреждение! Выберите путь для экспорта.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (cbRazd.Text == "")
            {
                MessageBox.Show("Предупреждение! Выберите разделитель для экспорта.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                try
                {
                    string temp = tbExport.Text;
                    tbExport.Text = temp.Replace("/", "\\");
                    DB_Connect connect = new DB_Connect();
                    connect.OpenConnect();

                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + cbTableExp.Text, connect.GetConnect());
                    MySqlDataReader reader = cmd.ExecuteReader();

                    string filePath = tbExport.Text + "//" + cbTableExp.Text + ".csv";
                    StreamWriter sw = new StreamWriter(filePath);

                    // записываем заголовки столбцов в CSV файл
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        sw.Write(reader.GetName(i));
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
                    connect.CloseConnect();

                    cbTableExp.SelectedIndex = -1;
                    tbExport.Clear();
                    cbRazd.SelectedIndex = -1;

                    MessageBox.Show("Успешно! Экспорт завершен. Файл сохранен по выбранному пути.", "Экспорт БД", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                catch (Exception msg)
                {
                    MessageBox.Show("Возникла ошибка!" + msg.Message, "Ошибка программы", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
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
