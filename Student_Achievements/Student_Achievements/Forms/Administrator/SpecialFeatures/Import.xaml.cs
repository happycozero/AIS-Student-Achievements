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
    public partial class Import : Window
    {
        private string path_csv = "";

        public Import()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Import();
        }

        private void ButSpecFeat_Click(object sender, RoutedEventArgs e)
        {
            var SpecialFeatForm = new SpecialFeature();
            Application.Current.MainWindow = SpecialFeatForm;
            this.Hide();
            SpecialFeatForm.Show();
        }

        public void Fill_Import()
        {
            cbTable.Items.Clear();

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
                    cbTable.Items.Add(reader[0].ToString());
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

        private void ImportData(MySqlConnection connection, string sql)
        {
            MySqlCommand com = new MySqlCommand(sql, connection);
            com.ExecuteNonQuery();
        }

        private void ButImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog upload = new Microsoft.Win32.OpenFileDialog();
            upload.Filter = "csv Files |*.csv";

            if (upload.ShowDialog() == true)
            {
                path_csv = upload.FileName;
                string[] temp = path_csv.Split('.');
                string[] temp2 = temp[0].Split('\\');
            }
            else
            {
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(path_csv))
                {
                    MessageBox.Show("Выберите файл для импорта.", "Импорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(cbTable.Text))
                {
                    MessageBox.Show("Выберите таблицу для импорта.", "Импорт БД", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string temp = path_csv;
                path_csv = temp.Replace("\\", "//");

                string sql = string.Format("LOAD DATA INFILE '{0}' IGNORE INTO TABLE {1} FIELDS TERMINATED BY '{2}';", tbImport.Text, cbTable.Text, cbRazd.Text);

                using (var connection = new DB_Connect().GetConnect())
                {
                    ImportData(connection, sql);

                    cbTable.SelectedIndex = -1;
                    path_csv = "";
                }

                MessageBox.Show("Импорт завершен.", "Импорт БД", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Возникла ошибка! " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
