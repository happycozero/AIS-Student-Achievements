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
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Employer();
        }

        private void UpdateEmployerTable()
        {
            tb_fio_employer.Clear();
            tb_position_employer.Clear();
            mtbPhone.Clear();
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
                    employer_phone AS 'Телефон' FROM employer;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgv_employer.AutoGenerateColumns = false;
                            dgv_employer.ItemsSource = dt.DefaultView;
                            dgv_employer.Columns[0].Visibility = Visibility.Collapsed;
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
            label_employer.Content = +dgv_employer.Items.Count;
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
                     WHERE id_employer = LAST_INSERT_ID();", tb_fio_employer.Text, tb_position_employer.Text, mtbPhone.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }

                }

                UpdateEmployerTable();

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
                                        WHERE id_employer = {3};",
                                                tb_fio_employer.Text,
                                                tb_position_employer.Text,
                                                mtbPhone.Text,
                                                _idEmployer);

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
                tb_fio_employer.Text = row_selected["ФИО сотрудника"].ToString();
                tb_position_employer.Text = row_selected["Должность"].ToString();
                mtbPhone.Text = row_selected["Телефон"].ToString();
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
    }
}
