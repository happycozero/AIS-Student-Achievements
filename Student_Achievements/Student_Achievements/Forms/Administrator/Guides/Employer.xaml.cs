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

namespace Student_Achievements.Forms.Administrator.Guides
{
    /// <summary>
    /// Interaction logic for Employer.xaml
    /// </summary>
    public partial class Employer : Window
    {
        private int _idEmployer;
        int count = 0;

        public Employer()
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
            Fill_Employer();
        }

        private void UpdateEmployerTable()
        {
            TbFioEmployer.Clear();
            TbPositionEmployer.Clear();
            MtbPhone.Clear();
            Fill_Employer();
            CountRowsDgv();
        }

        public void Fill_Employer()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT id_employer AS 'ID', employer_FIO AS 'ФИО сотрудника', employer_position AS 'Должность',
                    employer_phone AS 'Телефон' FROM employer ORDER BY employer_FIO ASC;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            for (Int32 i = 0; i < dt.Rows.Count; i++)
                            {

                                String MtbPhone = dt.Rows[i].ItemArray[3].ToString();

                                if (MtbPhone != "")
                                {
                                    dt.Rows[i].SetField(3, MtbPhone.Substring(0, 2) + "*********" + MtbPhone.Substring(4, 2));
                                }
                            }

                            DgvEmployer.AutoGenerateColumns = false;
                            DgvEmployer.ItemsSource = dt.DefaultView;
                            DgvEmployer.Columns[0].Visibility = Visibility.Collapsed;
                        }
                    }
                    CountRowsDgv();

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_EmployerNew()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT id_employer AS 'ID', employer_FIO AS 'ФИО сотрудника', employer_position AS 'Должность',
                    employer_phone AS 'Телефон' FROM employer ORDER BY id_employer DESC;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DgvEmployer.AutoGenerateColumns = false;
                            DgvEmployer.ItemsSource = dt.DefaultView;
                            DgvEmployer.Columns[0].Visibility = Visibility.Collapsed;
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

        private void CountRowsDgv()
        {
            LabelEmployer.Content = +DgvEmployer.Items.Count;
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"INSERT INTO employer (employer_FIO, employer_position, employer_phone) 
                     VALUES ('{0}', '{1}', '{2}');
                     SELECT * FROM employer 
                     WHERE id_employer = LAST_INSERT_ID();", TbFioEmployer.Text, TbPositionEmployer.Text, MtbPhone.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }

                }

                UpdateEmployerTable();
                Fill_EmployerNew();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление сотрудника", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"UPDATE employer 
                                          SET employer_FIO = '{0}', 
                                              employer_position = '{1}', 
                                              employer_phone = '{2}'
                                        WHERE id_employer = {3}; 
                                        SELECT * FROM employer 
                                        WHERE id_employer = {3};", TbFioEmployer.Text, TbPositionEmployer.Text, MtbPhone.Text, _idEmployer);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateEmployerTable();

                MessageBox.Show("Успешно! Запись обновлена.", "Обновление информации о сотруднике", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            var GuideForm = new Guide();
            Application.Current.MainWindow = GuideForm;
            this.Hide();
            GuideForm.Show();
        }

        private void tb_fio_employer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && count == 0)
            {
                count++;
            }
            else
            {
                if (e.Key == Key.Space && count == 1)
                {
                    e.Handled = true;
                }

                if (e.Key != Key.Space && count == 1)
                {
                    count--;
                }
            }
        }

        private void tb_position_employer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && count == 0)
            {
                count++;
            }
            else
            {
                if (e.Key == Key.Space && count == 1)
                {
                    e.Handled = true;
                }

                if (e.Key != Key.Space && count == 1)
                {
                    count--;
                }
            }
        }

        private void tb_fio_employer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();

            // Проверяем, что вводится только русская буква
            if (!Regex.IsMatch(e.Text, @"^[А-Яа-я]+$"))
            {
                e.Handled = true;
            }
        }

        private void dgv_employer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                TbFioEmployer.Text = row_selected["ФИО сотрудника"].ToString();
                TbPositionEmployer.Text = row_selected["Должность"].ToString();
                MtbPhone.Text = row_selected["Телефон"].ToString();

                _idEmployer = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var confirmation = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirmation != MessageBoxResult.Yes)
                    return;

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"DELETE FROM employer 
                                        WHERE id_employer = {0};", _idEmployer);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateEmployerTable();

                MessageBox.Show("Успешно! Запись удалена.", "Удаление работника", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            TbFioEmployer.Clear();
            TbPositionEmployer.Clear();
            MtbPhone.Clear();
            Fill_Employer();
            CountRowsDgv();
        }
        private void TbPositionEmployer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();

            // Проверяем, что вводится только русская буква
            if (!Regex.IsMatch(e.Text, @"^[А-Яа-я]+$"))
            {
                e.Handled = true;
            }
        }

        private void TbPhoneEmployer_GotFocus(object sender, RoutedEventArgs e)
        {
            // Перемещаем курсор в начало, если все символы заполнены по маске
            if (MtbPhone.MaskCompleted)
            {
                MtbPhone.SelectionStart = 0;
                MtbPhone.SelectionLength = 0;
            }
            // Перемещаем курсор на первый незаполненный символ, если есть
            else
            {
                int index = MtbPhone.Text.IndexOf('_');
                if (index >= 0)
                {
                    MtbPhone.SelectionStart = index;
                    MtbPhone.SelectionLength = 0;
                }
            }
        }

        private void TbPositionEmployer_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
        }
    }
}
