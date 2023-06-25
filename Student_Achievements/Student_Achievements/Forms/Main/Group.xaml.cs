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
        static public int role_user = 0;
        static public string id_user = "";
        private int id_group;

        public Group()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB_Connect connection = new DB_Connect();
            tbFIO.Text = connection.Fill_FIO();
            tbAccess.Text = connection.Fill_Access();
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Group();
            Fill_ComboBoxSpecialization();
        }
        
        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            DB_Connect dbConnect = new DB_Connect();
            string userAccess = dbConnect.Fill_Access();

            if (userAccess == "Староста")
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
                                 JOIN specialization ON specialization.id_specialization = `group`.group_specialization
                                 ORDER BY `group`.group_code ASC;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    DataTable dataTable = new DataTable("`group`");
                    dataAdapter.Fill(dataTable);

                    DgvGroup.ItemsSource = dataTable.DefaultView;
                    DgvGroup.Columns[0].Visibility = Visibility.Collapsed;

                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_GroupNew()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `group`.id_group AS 'ID', `group`.group_code AS 'Код группы', 
                                 specialization.specialization_abbreviation AS 'Аббревиатура'
                                 FROM `group`
                                 JOIN specialization ON specialization.id_specialization = `group`.group_specialization
                                 ORDER BY `group`.id_group DESC;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    DataTable dataTable = new DataTable("`group`");
                    dataAdapter.Fill(dataTable);

                    DgvGroup.ItemsSource = dataTable.DefaultView;
                    DgvGroup.Columns[0].Visibility = Visibility.Collapsed;

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
                CbSpecialization.Items.Clear();

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
                                CbSpecialization.Items.Add(reader[0].ToString());
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
            TbGroupCode.Clear();
            CbSpecialization.SelectedIndex = -1;
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

                    string sql = string.Format("INSERT INTO `group` VALUES (null, '{0}', {1})", TbGroupCode.Text, group_id_u());

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateGroupTable();
                Fill_GroupNew();

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

                    string sql = "SELECT id_specialization FROM specialization WHERE specialization_abbreviation = '" + CbSpecialization.Text + "';";
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
            CountGroup.Content = +DgvGroup.Items.Count;
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
            int cursorPosition = TbGroupCode.SelectionStart; // сохраняем положение курсора

            if (!string.IsNullOrEmpty(TbGroupCode.Text))
            {
                TbGroupCode.Text = TbGroupCode.Text.ToUpper(); // приводим все символы в текстбоксе к верхнему регистру
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки
            TbGroupCode.Text = Regex.Replace(TbGroupCode.Text, @"[^\w-]", "");

            TbGroupCode.SelectionStart = cursorPosition; // восстанавливаем положение курсора
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)DgvGroup.SelectedItem;
                if (selectedRow != null)
                {
                    int id = Convert.ToInt32(selectedRow["ID"]);

                    using (var connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = string.Format("UPDATE `group` SET group_code='{0}', group_specialization={1} WHERE id_group={2}", TbGroupCode.Text, GetSpecializationId(CbSpecialization.SelectedItem.ToString()), id);

                        using (var com = new MySqlCommand(sql, connection.GetConnect()))
                        {
                            com.ExecuteNonQuery();
                        }
                    }

                    UpdateGroupTable();
                    Fill_GroupNew();

                    MessageBox.Show("Успешно! Запись отредактирована.", "Редактирование учебной группы", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите запись для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при редактировании записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int GetSpecializationId(string abbreviation)
        {
            try
            {
                string sql = string.Format("SELECT id_specialization FROM specialization WHERE specialization_abbreviation='{0}'", abbreviation);

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                return Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При получении id специализации произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return -1;
        }

        private void dgv_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                TbGroupCode.Text = row_selected["Код группы"].ToString();
                CbSpecialization.Text = row_selected["Аббревиатура"].ToString();
                id_group = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            if (DgvGroup.SelectedItems.Count > 0)
            {
                var confirmResult = MessageBox.Show("Вы действительно хотите удалить выбранную запись?", "Подтвердите удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        DataRowView selectedRow = (DataRowView)DgvGroup.SelectedItems[0];
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

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            TbGroupCode.Clear();
            CbSpecialization.SelectedIndex = -1;
            Fill_Group();
            Fill_ComboBoxSpecialization();
            CountRowsDgv();
        }
    }
}
