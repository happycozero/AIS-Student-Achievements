using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using Student_Achievements.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        int count_auth = 0;
        static public int role_user = 0;
        static public string id_user = "";

        Captch CaptchForm = new Captch();

        public Authorization()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
            string password = TbPassword.Password;
        }

        private string GetPassHash()
        {
            string passhash = "";

            using (var sh2 = SHA256.Create())
            {
                var sh2byte = sh2.ComputeHash(Encoding.UTF8.GetBytes(TbPassword.Password));
                passhash = BitConverter.ToString(sh2byte).Replace("-", "").ToLower();
            }

            return passhash;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButExitProgram_Click(object sender, RoutedEventArgs e)
        {
            BackupExportSQL BackupExportSQL = new BackupExportSQL();
            BackupExportSQL.getBackup();

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение закрытия", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        //private void cbShowPass_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (cbShowPass.IsChecked == true)
        //    {
        //        TbPassword.Visibility = Visibility.Hidden;
        //    }
        //    else
        //    {
        //        TbPassword.Visibility = Visibility.Visible;
        //    }
        //}

        private void ButAuth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = true;
                string dbpass = "";
                string login = TbLogin.Text;

                DB_Connect connection = new DB_Connect();
                connection.OpenConnect();

                string sql = "SELECT user_password FROM user WHERE user_login = '" + login + "';";
                MySqlCommand command = new MySqlCommand(sql, connection.GetConnect());

                try
                {
                    dbpass = command.ExecuteScalar().ToString();
                }

                catch
                {
                    MessageBox.Show("Ошибка! Такого пользователя не существует. Повторите попытку.", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    TbLogin.Clear();
                    flag = false;
                    count_auth++;
                }

                if (flag == true)
                {
                    if (count_auth > 2)
                    {
                        // Create an instance of the CaptchaWindow dialog
                        var captch = new Captch();

                        // Show the dialog window modally and wait for it to be closed
                        bool? result = captch.ShowDialog();
                    }

                    if (GetPassHash() == dbpass)
                    {
                        count_auth = 0;
                        TbPassword.Clear();
                        TbLogin.Clear();

                        string sql_role = "SELECT user_role FROM user WHERE user_login = '" + login + "';";
                        MySqlCommand com = new MySqlCommand(sql_role, connection.GetConnect());
                        role_user = Convert.ToInt32(com.ExecuteScalar());

                        string sql_idemp = "SELECT id_user FROM user WHERE user_login = '" + login + "';";
                        MySqlCommand com1 = new MySqlCommand(sql_idemp, connection.GetConnect());
                        id_user = com1.ExecuteScalar().ToString();

                        if (role_user == 3)
                        {
                            BackupExportSQL backup_sql = new BackupExportSQL();
                            backup_sql.getBackup();

                            var MenuAdminForm = new MenuAdministrator();
                            Application.Current.MainWindow = MenuAdminForm;
                            this.Hide();
                            MenuAdminForm.Show();
                        }

                        else
                        {
                            if (role_user == 2)
                            {
                                var MenuStudentForm = new MenuStudent();
                                Application.Current.MainWindow = MenuStudentForm;
                                this.Hide();
                                MenuStudentForm.Show();
                            }
                            else if (role_user == 1)
                            {
                                var MenuTeacherForm = new MenuTeacher();
                                Application.Current.MainWindow = MenuTeacherForm;
                                this.Hide();
                                MenuTeacherForm.Show();
                            }
                        }
                    }

                    else
                    {
                        MessageBox.Show("Ошибка! Введен неверный пароль. Повторите попытку.", "Информация", MessageBoxButton.OK, MessageBoxImage.Error);
                        TbPassword.Clear();
                        count_auth++;
                    }
                }
            }

            catch (Exception msg)
            {
                MessageBox.Show("Возникла ошибка! " + msg.Message, "Информация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            KeyboardLayout.SetToEnglish();
        }

        private void TbPassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToEnglish();
        }
    }
}
