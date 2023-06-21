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
    public partial class Courсe : Window
    {
        static public int role_user = 0;
        static public string id_user = "";
        private int id_cource;

        public Courсe()
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
            UpdateCourceTable();
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

        private void UpdateCourceTable()
        {
            CbCourceGroup.SelectedIndex = -1;
            CbCource.SelectedIndex = -1;
            TbYearsF.Clear();
            TbYearsS.Clear();
            Fill_Cource();
            Fill_ComboBoxGroup();
            Fill_Cource();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            CountCource.Content = +DgvCource.Items.Count;
        }

        public void Fill_Cource()
        {
            try
            {
                using (var connection = new DB_Connect())
                using (var dataAdapter = new MySqlDataAdapter(@"SELECT cource.id_cource AS 'ID', `group`.group_code AS 'Код группы', 
                                                              cource.courсe_score AS 'Курс', cource.cource_years_of_study AS 'Годы обучения' 
                                                              FROM cource 
                                                              INNER JOIN `group` ON cource.courсe_group_name = `group`.id_group
                                                              ORDER BY cource.courсe_score ASC;", connection.GetConnect()))
                {
                    var dataTable = new DataTable("cource");
                    dataAdapter.Fill(dataTable);

                    DgvCource.ItemsSource = dataTable.DefaultView;
                    DgvCource.Columns[0].Visibility = Visibility.Collapsed;

                    // Сортировка по убыванию Id курса, чтобы новая запись оказалась первой
                    var dv = dataTable.DefaultView;
                    dv.Sort = "ID DESC";
                    DgvCource.ItemsSource = dv.ToTable().DefaultView;

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
                CbCourceGroup.Items.Clear();

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
                                CbCourceGroup.Items.Add(reader[0].ToString());
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
                if (int.TryParse(TbYearsS.Text, out years))
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
                string yearOfStudy = TbYearsS.Text + "-" + TbYearsF.Text;

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"INSERT INTO `cource` (courсe_group_name, courсe_score, cource_years_of_study) 
                                               SELECT `group`.id_group, {0}, '{1}' FROM `group` WHERE `group`.group_code = '{2}'",
                                                 CbCource.SelectedIndex + 1, yearOfStudy, CbCourceGroup.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateCourceTable();
                Fill_Cource();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление курса", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgv_cource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                CbCourceGroup.Text = row_selected["Код группы"].ToString();
                CbCource.Text = row_selected["Курс"].ToString();

                string years = row_selected["Годы обучения"].ToString();
                string[] yearArray = years.Split('-');
                if (yearArray.Length == 2) // проверяем, что строка была успешно разделена на две части
                {
                    TbYearsS.Text = yearArray[0].Trim(); // устанавливаем первое число в первый TextBox, удаляя пробелы в начале и конце строки
                    TbYearsF.Text = yearArray[1].Trim(); // устанавливаем второе число во второй TextBox, удаляя пробелы в начале и конце строки
                }
                else
                {
                    // обработка ошибки, если строка не может быть разделена на две части
                }

                id_cource = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            int years;
            if (int.TryParse(TbYearsS.Text, out years))
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
            string yearOfStudy = TbYearsS.Text + "-" + TbYearsF.Text;

            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    // запрос на обновление данных выбранной строки в таблице course
                    string sql = string.Format(@"UPDATE `cource` SET courсe_group_name = (SELECT id_group FROM `group` WHERE group_code = '{0}' LIMIT 1),
                                               courсe_score = '{1}', cource_years_of_study = '{2}' WHERE id_cource = {3}",
                                                CbCourceGroup.Text, CbCource.SelectedIndex + 1, yearOfStudy, id_cource);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateCourceTable();

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

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Удаление курса", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    // если пользователь ответил "нет", то просто выходим из метода
                    return;
                }

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "DELETE FROM `cource` WHERE `id_cource` = @id_cource";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.Parameters.AddWithValue("@id_cource", id_cource);
                        com.ExecuteNonQuery();
                    }
                }

                UpdateCourceTable();
                Fill_Cource();

                MessageBox.Show("Успешно! Запись удалена.", "Удаление курса", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            UpdateCourceTable();
        }
    }
}
