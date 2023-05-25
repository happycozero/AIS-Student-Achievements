using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Student_Achievements.Forms.Main
{
    /// <summary>
    /// Interaction logic for Group.xaml
    /// </summary>
    public partial class Group : Window
    {
        private int id_group;

        public Group()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Group();
            Fill_ComboBoxSpecialization();
        }
        
        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUser.UserRole == "student")
            {
                var MenuStudentForm = new MenuStudent();
                Application.Current.MainWindow = MenuStudentForm;
                this.Hide();
                MenuStudentForm.Show();
            }
            else
            {
                var MenuTeacherForm = new MenuTeacher();
                Application.Current.MainWindow = MenuTeacherForm;
                this.Hide();
                MenuTeacherForm.Show();
            }
        }

        public void Fill_Group()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `group`.id_group AS 'ID', `group`.group_code AS 'Код группы', 
                    specialization.specialization_abbreviation AS 'Аббревиатура' 
                    FROM `group`
                    JOIN specialization ON specialization.id_specialization = `group`.group_specialization";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    DataTable dataTable = new DataTable("`group`");
                    dataAdapter.Fill(dataTable);

                    dgv_group.ItemsSource = dataTable.DefaultView;
                    dgv_group.Columns[0].Visibility = Visibility.Collapsed;

                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_ComboBoxSpecialization()
        {
            try
            {
                cb_specialization.Items.Clear();

                string sql = "SELECT specialization_abbreviation FROM specialization;";

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cb_specialization.Items.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке специализаций произошла ошибка: " + ex.Message, "Ошибка загрузки специализаций", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateGroupTable()
        {
            tb_group_code.Clear();
            cb_specialization.SelectedIndex = -1;
            Fill_Group();
            Fill_ComboBoxSpecialization();
            CountRowsDgv();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format("INSERT INTO `group` VALUES (null, '{0}', {1})", tb_group_code.Text, group_id_u());

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateGroupTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление учебной группы", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int group_id_u()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_specialization FROM specialization WHERE specialization_abbreviation = '" + cb_specialization.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int group_int_id_u = Convert.ToInt32(com.ExecuteScalar());

                    return group_int_id_u;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id специализации: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        private void CountRowsDgv()
        {
            count_group.Content = +dgv_group.Items.Count;
        }

        private void tb_group_code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();

            // Блокируем ввод английских букв и специальных символов, кроме дефиса
            Regex regex = new Regex(@"[^-\p{IsCyrillic}\d]");
            bool hasNonRussianLetters = regex.IsMatch(e.Text);
            e.Handled = hasNonRussianLetters;
        }

        private void tb_group_code_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = tb_group_code.SelectionStart; // сохраняем положение курсора

            if (!string.IsNullOrEmpty(tb_group_code.Text))
            {
                tb_group_code.Text = tb_group_code.Text.ToUpper(); // приводим все символы в текстбоксе к верхнему регистру
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки
            tb_group_code.Text = Regex.Replace(tb_group_code.Text, @"[^\w-]", "");

            tb_group_code.SelectionStart = cursorPosition; // восстанавливаем положение курсора
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_group.SelectedItems.Count > 0)
            {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)dgv_group.SelectedItems[0];
                        int id = Convert.ToInt32(selectedRow["ID"]);

                        using (var connection = new DB_Connect())
                        {
                            connection.OpenConnect();

                            string sql = string.Format("DELETE FROM `group` WHERE id_group={0}", id);

                            using (var com = new MySqlCommand(sql, connection.GetConnect()))
                            {
                                com.ExecuteNonQuery();
                            }
                        }

                        UpdateGroupTable();

                        MessageBox.Show("Запись успешно удалена.", "Удаление учебной группы", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при удалении записи: " + ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
            }
        }

        private void dgv_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tb_group_code.Text = row_selected["Код группы"].ToString();
                cb_specialization.Text = row_selected["Аббревиатура"].ToString();
                id_group = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_group.SelectedItems.Count > 0)
            {
                var confirmResult = MessageBox.Show("Вы действительно хотите удалить выбранную запись?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)dgv_group.SelectedItems[0];
                        int id = Convert.ToInt32(selectedRow["ID"]);

                        using (var connection = new DB_Connect())
                        {
                            connection.OpenConnect();

                            string sql = string.Format("DELETE FROM `group` WHERE id_group={0}", id);

                            using (var com = new MySqlCommand(sql, connection.GetConnect()))
                            {
                                com.ExecuteNonQuery();
                            }
                        }

                        UpdateGroupTable();

                        MessageBox.Show("Запись успешно удалена.", "Удаление учебной группы", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка при удалении записи: " + ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись, которую нужно удалить.", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
