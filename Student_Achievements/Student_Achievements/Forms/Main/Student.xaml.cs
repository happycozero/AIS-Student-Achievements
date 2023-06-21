using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        private int id_student;

        public Student()
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
            UpdateStudentTable();
        }

        private void UpdateStudentTable()
        {
            TbFioStudent.Clear();
            CbGroupCode.SelectedIndex = -1;
            CbStudentStatus.SelectedIndex = -1;
            Fill_Student();
            Fill_Status();
            Fill_Group();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            CountStudent.Content = +DgvStudent.Items.Count;
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

        public void Fill_Student()
        {
            try
            {
                DB_Connect connection = new DB_Connect();
                connection.OpenConnect();

                string sql = @"SELECT student.id_student AS 'ID', student.student_fio AS 'ФИО студента',
                             `group`.group_code AS 'Группа',
                             student_status.student_status_name AS 'Статус'
                             FROM student
                             INNER JOIN `group` ON student.student_group_code = `group`.id_group
                             INNER JOIN student_status ON student.student_status = student_status.id_student_status
                             ORDER BY student.student_fio ASC;";

                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                MySqlDataAdapter data_adapter = new MySqlDataAdapter(com);
                DataTable data_table = new DataTable("student");
                data_adapter.Fill(data_table);
                DgvStudent.ItemsSource = data_table.DefaultView;
                DgvStudent.Columns[0].Visibility = Visibility.Collapsed;

                connection.CloseConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_StudentNew()
        {
            try
            {
                DB_Connect connection = new DB_Connect();
                connection.OpenConnect();

                string sql = @"SELECT student.id_student AS 'ID', student.student_fio AS 'ФИО студента',
                             `group`.group_code AS 'Группа',
                             student_status.student_status_name AS 'Статус'
                             FROM student
                             INNER JOIN `group` ON student.student_group_code = `group`.id_group
                             INNER JOIN student_status ON student.student_status = student_status.id_student_status
                             ORDER BY student.id_student DESC;";

                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                MySqlDataAdapter data_adapter = new MySqlDataAdapter(com);
                DataTable data_table = new DataTable("student");
                data_adapter.Fill(data_table);
                DgvStudent.ItemsSource = data_table.DefaultView;
                DgvStudent.Columns[0].Visibility = Visibility.Collapsed;

                connection.CloseConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_Group()
        {
            // Загрузка списка групп из базы данных
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Формируем SQL-запрос для получения списка групп
                    string sql = @"SELECT id_group, group_code FROM `group`;";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    // Создаем адаптер данных и заполняем таблицу данными из базы данных
                    MySqlDataAdapter data_adapter = new MySqlDataAdapter(com);
                    DataTable data_table = new DataTable("group");
                    data_adapter.Fill(data_table);

                    // Устанавливаем поле для отображения значений в выпадающем списке
                    CbGroupCode.DisplayMemberPath = "group_code";

                    // Устанавливаем источник данных для выпадающего списка
                    CbGroupCode.ItemsSource = data_table.DefaultView;

                    connection.CloseConnect();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_Status()
        {
            // Загрузка статусов студентов из базы данных
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Формируем SQL-запрос для получения списка статусов студентов
                    string sql = @"SELECT id_student_status, student_status_name FROM student_status;";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    // Создаем адаптер данных и заполняем таблицу данными из базы данных
                    MySqlDataAdapter data_adapter = new MySqlDataAdapter(com);
                    DataTable data_table = new DataTable("student_status");
                    data_adapter.Fill(data_table);

                    // Устанавливаем поле для отображения значений в выпадающем списке
                    CbStudentStatus.DisplayMemberPath = "student_status_name";

                    // Устанавливаем источник данных для выпадающего списка
                    CbStudentStatus.ItemsSource = data_table.DefaultView;

                    connection.CloseConnect();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputValidator validator = new InputValidator();
                if (!validator.Validate(TbFioStudent.Text))
                {
                    return;
                }

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Получаем id_group из таблицы group по введенному коду группы
                    string sqlGroupId = string.Format("SELECT id_group FROM `group` WHERE group_code = '{0}'", CbGroupCode.Text);
                    int groupId;
                    using (var comGroup = new MySqlCommand(sqlGroupId, connection.GetConnect()))
                    {
                        var result = comGroup.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Группа с указанным кодом не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        groupId = Convert.ToInt32(result);
                    }

                    // Получаем id_student_status из таблицы student_status по выбранному статусу студента
                    string sqlStudentStatusId = string.Format("SELECT id_student_status FROM student_status WHERE student_status_name = '{0}'", CbStudentStatus.Text);
                    int studentStatusId;
                    using (var comStudentStatus = new MySqlCommand(sqlStudentStatusId, connection.GetConnect()))
                    {
                        var result = comStudentStatus.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Указанный статус студента не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        studentStatusId = Convert.ToInt32(result);
                    }

                    // Добавляем запись в таблицу student
                    string sqlInsert = string.Format("INSERT INTO student (student_fio, student_status, student_group_code) VALUES ('{0}', {1}, {2})",
                                                     TbFioStudent.Text, studentStatusId, groupId);

                    using (var comInsert = new MySqlCommand(sqlInsert, connection.GetConnect()))
                    {
                        comInsert.ExecuteNonQuery();
                    }
                }

                UpdateStudentTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление студента", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int GetGroupId(string groupCode)
        {
            int groupId = 0;

            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Формируем SQL-запрос для получения ID группы по ее коду
                    string sql = "SELECT id_group FROM `group` WHERE group_code = '" + groupCode + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    // Получаем ID группы из базы данных
                    groupId = Convert.ToInt32(com.ExecuteScalar());

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении ID группы: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return groupId;
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // Получаем id_group из таблицы group по введенному коду группы
                    string sqlGroupId = string.Format("SELECT id_group FROM `group` WHERE group_code = '{0}'", CbGroupCode.Text);
                    int groupId;
                    using (var comGroup = new MySqlCommand(sqlGroupId, connection.GetConnect()))
                    {
                        var result = comGroup.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Группа с указанным кодом не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        groupId = Convert.ToInt32(result);
                    }

                    // Получаем id_student_status из таблицы student_status по выбранному статусу студента
                    string sqlStudentStatusId = string.Format("SELECT id_student_status FROM student_status WHERE student_status_name = '{0}'", CbStudentStatus.Text);
                    int studentStatusId;
                    using (var comStudentStatus = new MySqlCommand(sqlStudentStatusId, connection.GetConnect()))
                    {
                        var result = comStudentStatus.ExecuteScalar();
                        if (result == null)
                        {
                            MessageBox.Show("Указанный статус студента не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        studentStatusId = Convert.ToInt32(result);
                    }

                    // Обновляем запись в таблице student
                    string sqlUpdate = string.Format("UPDATE student SET student_fio='{0}', student_status={1}, student_group_code={2} WHERE id_student={3}",
                                                     TbFioStudent.Text, studentStatusId, groupId, id_student);

                    using (var comUpdate = new MySqlCommand(sqlUpdate, connection.GetConnect()))
                    {
                        comUpdate.ExecuteNonQuery();
                    }
                }

                UpdateStudentTable();

                MessageBox.Show("Успешно! Запись обновлена.", "Редактирование студента", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли студент
            try
            {
                if (DgvStudent.SelectedItem == null)
                {
                MessageBox.Show("Выберите студента для удаления.", "Удаление студента", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
                }

                // Получаем ID выбранного студента
                int student_id = (int)((DataRowView)DgvStudent.SelectedItem)["ID"];

                // Запрашиваем у пользователя подтверждение удаления
                MessageBoxResult confirmation = MessageBox.Show("Вы уверены, что хотите удалить этого студента?", "Удаление студента", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirmation == MessageBoxResult.Yes)
                {
                    // Устанавливаем соединение с базой данных и открываем его
                    using (var connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        // Удаляем запись о студенте
                        string sql_delete_student = "DELETE FROM `student` WHERE `id_student`=" + student_id;

                        using (var com_delete_student = new MySqlCommand(sql_delete_student, connection.GetConnect()))
                        {
                            com_delete_student.ExecuteNonQuery();
                        }
                    }

                    // Обновляем таблицу со списком студентов
                    UpdateStudentTable();

                    MessageBox.Show("Запись студента успешно удалена!", "Удаление студента", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
            MessageBox.Show("Произошла ошибка при удалении студента: " + ex.Message, "Ошибка удаления студента", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgv_student_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                TbFioStudent.Text = row_selected["ФИО студента"].ToString();
                CbGroupCode.Text = row_selected["Группа"].ToString();
                CbStudentStatus.Text = row_selected["Статус"].ToString();
                id_student = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void tb_fio_student_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
            
            // Блокируем ввод английских букв и специальных символов, кроме дефиса
            Regex regex = new Regex(@"[^-\p{IsCyrillic}]");
            bool hasNonRussianLetters = regex.IsMatch(e.Text);
            e.Handled = hasNonRussianLetters;
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            TbFioStudent.Clear();
            CbGroupCode.SelectedIndex = -1;
            CbStudentStatus.SelectedIndex = -1;
            Fill_Student();
            Fill_Status();
            Fill_Group();
            CountRowsDgv();
        }
    }
}
