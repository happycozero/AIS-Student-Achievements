using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using Student_Achievements.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        static public int role_user = 0;
        static public string id_user = "";
        int count_auth = 0;
        public static string checkform = "";

        Captch CaptchForm = new Captch();

        public Authorization()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButExitProgram_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение закрытия", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ButAuth_Click(object sender, RoutedEventArgs e)
        {
            if (count_auth > 0)
            {
                MessageBox.Show("Внимание! Теперь для авторизации необходимо ввести капчу.", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                CaptchForm.ShowDialog();
            }

            if (string.IsNullOrEmpty(tb_login.Text) || string.IsNullOrEmpty(tb_password.Text))
            {
                MessageBox.Show("Ошибка! Сначала заполните все поля!", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                count_auth++;
            }

            string login_user = tb_login.Text;
            string password_user = tb_password.Text;

            string dbpass = "";

            using (var dbConnect = new DB_Connect())
            using (MySqlCommand command = dbConnect.GetConnect().CreateCommand())
            {
                command.CommandText = "SELECT user_password FROM user WHERE user_login = @login_user";
                command.Parameters.AddWithValue("@login_user", login_user);

                try
                {
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        dbpass = result.ToString();
                    }
                    if (dbpass == null)
                    {
                        MessageBox.Show("Ошибка! Такого пользователя не существует. Повторите попытку.", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        tb_login.Clear();
                        count_auth++;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb_login.Clear();
                    count_auth++;
                    return;
                }

                if (count_auth > 1)
                {
                    MessageBox.Show("Внимание! Теперь для авторизации необходимо ввести капчу.", "Информация", MessageBoxButton.OK, MessageBoxImage.Warning);
                    CaptchForm.ShowDialog();
                }

                if (password_user == dbpass)
                {
                    count_auth = 0;

                    using (MySqlCommand com = dbConnect.GetConnect().CreateCommand())
                    {
                        com.CommandText = "SELECT user_role FROM user WHERE user_login = @login_user";
                        com.Parameters.AddWithValue("@login_user", login_user);
                        role_user = Convert.ToInt32(com.ExecuteScalar());
                    }

                    using (MySqlCommand com_id = dbConnect.GetConnect().CreateCommand())
                    {
                        com_id.CommandText = "SELECT id_user FROM user WHERE user_login = @login_user";
                        com_id.Parameters.AddWithValue("@login_user", login_user);
                        id_user = com_id.ExecuteScalar().ToString();
                    }

                    tb_password.Clear();
                    tb_login.Clear();

                    switch (role_user)
                    {
                        case 1:
                            checkform = "teacher";
                            CheckUser.UserRole = "teacher";
                            var TeacherForm = new MenuTeacher();
                            Application.Current.MainWindow = TeacherForm;
                            this.Hide();
                            TeacherForm.Show();
                            break;

                        case 2:
                            checkform = "student";
                            CheckUser.UserRole = "student";
                            var StudentForm = new MenuStudent();
                            Application.Current.MainWindow = StudentForm;
                            this.Hide();
                            StudentForm.Show();
                            break;

                        case 3:
                            checkform = "administrator";
                            var AdministratorForm = new MenuAdministrator();
                            Application.Current.MainWindow = AdministratorForm;
                            this.Hide();
                            AdministratorForm.Show();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка! Введен неверный пароль.\n Повторите попытку.", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb_password.Clear();
                    count_auth++;
                }
            }
        }
    }
}
