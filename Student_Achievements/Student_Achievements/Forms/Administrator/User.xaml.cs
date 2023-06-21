using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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

namespace Student_Achievements.Forms.Administrator
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Window
    {
        private int _idUser;

        public User()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private string GetPassHash()
        {
            string passhash = "";

            using (var sh2 = SHA256.Create())
            {
                var sh2byte = sh2.ComputeHash(Encoding.UTF8.GetBytes(TbPasswordUser.Text));
                passhash = BitConverter.ToString(sh2byte).Replace("-", "").ToLower();
            }

            return passhash;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB_Connect connection = new DB_Connect();
            tbFIO.Text = connection.Fill_FIO();
            tbAccess.Text = connection.Fill_Access();
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            UpdateUserTable();
            FillUserTable();
        }

        private void ButGenPass_Click(object sender, RoutedEventArgs e)
        {
            var generatedPassword = PasswordGenerator.GeneratePassword(12); // Здесь можно указать желаемую длину пароля
            TbPasswordUser.Text = generatedPassword;
        }

        public void FillUserTable()
        {
            Fill_ComboBoxEmployer();
            Fill_ComboBoxStatus();
            Fill_User();
            CountRowsDgv();
        }

        public void UpdateUserTable()
        {
            CbChoiceEmployer.SelectedIndex = -1;
            CbChoiceStatus.SelectedIndex = -1;
            TbLoginUser.Clear();
            TbPasswordUser.Clear();
        }

        private void CountRowsDgv()
        {
            LabelUser.Content = +DgvUser.Items.Count;
        }

        public void Fill_User()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT u.id_user AS 'ID',
                                 e.employer_FIO AS 'ФИО пользователя',
                                 r.role_user_name AS 'Доступ',
                                 u.user_login AS 'Логин', u.user_password AS 'Пароль'
                                 FROM user u
                                 INNER JOIN employer e ON u.user_employer = e.id_employer
                                 INNER JOIN user_role r ON u.user_role = r.id_user_role
                                 ORDER BY e.employer_FIO;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DgvUser.ItemsSource = dt.DefaultView;
                            DgvUser.Columns[0].Visibility = Visibility.Collapsed;
                            DgvUser.Columns[4].Visibility = Visibility.Collapsed;
                        }
                    }

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_UserNew()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT u.id_user AS 'ID',
                                 e.employer_FIO AS 'ФИО пользователя',
                                 r.role_user_name AS 'Доступ',
                                 u.user_login AS 'Логин', 
                                 u.user_password AS 'Пароль'
                                 FROM user u
                                 INNER JOIN employer e ON u.user_employer = e.id_employer
                                 INNER JOIN user_role r ON u.user_role = r.id_user_role
                                 ORDER BY u.id_user DESC;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DgvUser.ItemsSource = dt.DefaultView;
                            DgvUser.Columns[0].Visibility = Visibility.Collapsed;
                            DgvUser.Columns[4].Visibility = Visibility.Collapsed;
                        }
                    }

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_ComboBoxEmployer()
        {
            CbChoiceEmployer.Items.Clear();

            string sql = "SELECT employer_FIO FROM employer;";

            try
            {
                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();

                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CbChoiceEmployer.Items.Add(reader["employer_FIO"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_ComboBoxStatus()
        {
            CbChoiceStatus.Items.Clear();

            string sql = "SELECT role_user_name FROM user_role;";

            try
            {
                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();

                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CbChoiceStatus.Items.Add(reader["role_user_name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetEmployerId(DB_Connect connection)
        {
            try
            {
                string sql = "SELECT id_employer FROM employer WHERE employer_FIO = '" + CbChoiceEmployer.Text + "';";
                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                int employerId = Convert.ToInt32(com.ExecuteScalar());

                return employerId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id сотрудника: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        private int GetRoleId(DB_Connect connection)
        {
            try
            {
                string sql = "SELECT id_user_role FROM user_role WHERE role_user_name = '" + CbChoiceStatus.Text + "';";
                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                int roleId = Convert.ToInt32(com.ExecuteScalar());

                return roleId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id роли: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        private int GetUserId(DB_Connect connection)
        {
            try
            {
                string sql = "SELECT id_user FROM user WHERE user_login = @login AND user_password = @password";
                using (var com = new MySqlCommand(sql, connection.GetConnect()))
                {
                    com.Parameters.AddWithValue("@login", TbLoginUser.Text);
                    com.Parameters.AddWithValue("@password", TbPasswordUser.Text);
                    object result = com.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
        }

        private void DgvUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row_selected = dg.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                CbChoiceEmployer.Text = row_selected["ФИО пользователя"].ToString();
                CbChoiceStatus.Text = row_selected["Доступ"].ToString();
                TbLoginUser.Text = row_selected["Логин"].ToString();
                TbPasswordUser.Text = row_selected["Пароль"].ToString();
                _idUser = Convert.ToInt32(row_selected["ID"]);
            }

            TbPasswordUser.Visibility = Visibility.Collapsed;
            Tb1.Visibility = Visibility.Collapsed;
            ButGenPass.Visibility = Visibility.Collapsed;
        }

        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            var menuAdmin = new MenuAdministrator();
            Application.Current.MainWindow = menuAdmin;
            this.Hide();
            menuAdmin.Show();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Get employer ID
                    int employerId = GetEmployerId(connection);

                    // Get role ID
                    int roleId = GetRoleId(connection);

                    string hashedPassword = PasswordHasher.HashPassword(TbPasswordUser.Text);

                    string sql = "INSERT INTO `user` (`user_employer`, `user_login`, `user_password`, `user_role`) VALUES (" + employerId + ", '" + TbLoginUser.Text + "', '" + GetPassHash() + "', " + roleId + ")";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                Fill_UserNew();
                MessageBox.Show("Успешно! Пользователь добавлен.", "Добавление пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Get user ID
                    int userId = GetUserId(connection);

                    // Get employer ID
                    int employerId = GetEmployerId(connection);

                    // Get role ID
                    int roleId = GetRoleId(connection);

                    string sql = "UPDATE `user` SET `user_employer`=@employerId, `user_login`=@login, `user_password`=@password, `user_role`=@roleId WHERE `id_user`=@userId;";
                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.Parameters.AddWithValue("@employerId", employerId);
                        com.Parameters.AddWithValue("@login", TbLoginUser.Text);
                        com.Parameters.AddWithValue("@password", TbPasswordUser.Text);
                        com.Parameters.AddWithValue("@roleId", roleId);
                        com.Parameters.AddWithValue("@userId", userId);
                        com.ExecuteNonQuery();
                    }
                }

                FillUserTable();

                MessageBox.Show("Успешно! Пользователь отредактирован.", "Редактирование пользователя", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        // Get user ID
                        int userId = GetUserId(connection);

                        string sql = "DELETE FROM `user` WHERE `id_user` =" + userId;

                        using (var com = new MySqlCommand(sql, connection.GetConnect()))
                        {
                            com.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Успешно! Пользователь удален.", "Удаление пользователя", MessageBoxButton.OK, MessageBoxImage.Information);

                    UpdateUserTable();
                    FillUserTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            CbChoiceEmployer.SelectedIndex = -1;
            CbChoiceStatus.SelectedIndex = -1;
            TbLoginUser.Clear();
            TbPasswordUser.Clear();

            TbPasswordUser.Visibility = Visibility.Visible;
            Tb1.Visibility = Visibility.Visible;
            ButGenPass.Visibility = Visibility.Visible;
        }
    }
}