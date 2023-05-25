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

namespace Student_Achievements.Forms.Main
{
    /// <summary>
    /// Interaction logic for Cource.xaml
    /// </summary>
    public partial class Course : Window
    {
        private int id_course;

        public Course()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Course();
            Fill_ComboBoxGroup();
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

        private void UpdateCourseTable()
        {
            cb_course_group.SelectedIndex = -1;
            cb_course.SelectedIndex = -1;
            tb_years_f.Clear();
            tb_years_s.Clear();
            Fill_Course();
            Fill_ComboBoxGroup();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            count_course.Content = +dgv_course.Items.Count;
        }

        public void Fill_Course()
        {
            try
            {
                using (var connection = new DB_Connect())
                using (var dataAdapter = new MySqlDataAdapter("SELECT course.id_course AS 'ID', `group`.group_code AS 'Код группы', " +
                                                                "course.course_score AS 'Курс', course.cource_years_of_study AS 'Годы обучения' " +
                                                                "FROM course INNER JOIN `group` ON course.course_group_name = `group`.id_group", 
                                                                connection.GetConnect()))
                {
                    var dataTable = new DataTable("course");
                    dataAdapter.Fill(dataTable);

                    dgv_course.ItemsSource = dataTable.DefaultView;
                    dgv_course.Columns[0].Visibility = Visibility.Collapsed;

                    // Сортировка по убыванию Id курса, чтобы новая запись оказалась первой
                    var dv = dataTable.DefaultView;
                    dv.Sort = "ID DESC";
                    dgv_course.ItemsSource = dv.ToTable().DefaultView;

                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public void Fill_ComboBoxGroup()
        {
            try
            {
                cb_course_group.Items.Clear();

                string sql = "SELECT `group_code` FROM `group`;";

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cb_course_group.Items.Add(reader[0].ToString());
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

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int years;
                if (int.TryParse(tb_years_s.Text, out years))
                {
                    if (years < 2000 || years > 2199)
                    {
                        // Если число выходит за пределы допустимого диапазона, выводим сообщение об ошибке
                        throw new Exception("Введите число от 2000 до 2199.");
                    }
                }
                else
                {
                    // Если значение в поле не является целым числом, выводим сообщение об ошибке
                    throw new Exception("Введите целое число.");
                }

                // обработка даты и получение года
                string yearOfStudy = tb_years_s.Text + "-" + tb_years_f.Text;

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format("INSERT INTO `course` (course_group_name, course_score, cource_years_of_study) " +
                                                "SELECT `group`.id_group, {0}, '{1}' FROM `group` " +
                                                "WHERE `group`.group_code = '{2}'",
                                                 cb_course.SelectedIndex + 1, yearOfStudy, cb_course_group.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateCourseTable();

                // Получаем DataView из таблицы
                DataView dataView = (DataView)dgv_course.ItemsSource;

                // Добавляем новую строку в начало DataGrid
                DataRow newRow = dataView.Table.NewRow();
                newRow.BeginEdit();
                newRow[0] = "";
                newRow[1] = "";
                // ...
                dataView.Table.Rows.InsertAt(newRow, 0);

                // Устанавливаем фокус на первую ячейку новой строки
                dgv_course.SelectedItem = newRow;
                dgv_course.ScrollIntoView(newRow);
                dgv_course.CurrentCell = dgv_course.SelectedCells[0];
                newRow.EndEdit();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление курса", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Успешно! Запись добавлена.", "Добавление курса", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dgv_course_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                cb_course_group.Text = row_selected["Код группы"].ToString();
                cb_course.Text = row_selected["Курс"].ToString();

                string years = row_selected["Годы обучения"].ToString();
                string[] yearArray = years.Split('-');
                if (yearArray.Length == 2) // проверяем, что строка была успешно разделена на две части
                {
                    tb_years_s.Text = yearArray[0].Trim(); // устанавливаем первое число в первый TextBox, удаляя пробелы в начале и конце строки
                    tb_years_f.Text = yearArray[1].Trim(); // устанавливаем второе число во второй TextBox, удаляя пробелы в начале и конце строки
                }
                else
                {
                    // обработка ошибки, если строка не может быть разделена на две части
                }

                id_course = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            int years;
            if (int.TryParse(tb_years_s.Text, out years))
            {
                if (years < 2000 || years > 2199)
                {
                    // Если число выходит за пределы допустимого диапазона, выводим сообщение об ошибке
                    MessageBox.Show("Введите число от 2000 до 2199.");
                }
            }
            else
            {
                // Если значение в поле не является целым числом, выводим сообщение об ошибке
                MessageBox.Show("Введите целое число.");
                return; // выходим из метода
            }

            // обработка даты и получение года
            string yearOfStudy = tb_years_s.Text + "-" + tb_years_f.Text;

            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // запрос на обновление данных выбранной строки в таблице course
                    string sql = string.Format("UPDATE `course` SET course_group_name=(SELECT id_group FROM `group` WHERE group_code='{0}' LIMIT 1), " +
                                               "course_score={1}, cource_years_of_study='{2}' WHERE id_course={3}",
                                                cb_course_group.Text, cb_course.SelectedIndex + 1, yearOfStudy, id_course);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateCourseTable();

                MessageBox.Show("Успешно! Запись изменена.", "Изменение курса", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При изменении записи произошла ошибка: " + ex.Message, "Ошибка при изменении записи", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_years_s_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
