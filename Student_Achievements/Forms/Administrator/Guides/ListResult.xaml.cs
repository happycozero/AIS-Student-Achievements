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
    /// Interaction logic for ListResult.xaml
    /// </summary>
    public partial class ListResult : Window
    {
        private int id_list_result;
        int count = 0;

        public ListResult()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_ComboBoxSpecialization();
            Fill_List_Result();
            CountRowsDgv();
        }

        private void UpdateListResultTable()
        {
            tb_result_code.Clear();
            tb_description.Clear();
            cb_specialization.SelectedIndex = -1;
            CountRowsDgv();
            Fill_List_Result();
            Fill_ComboBoxSpecialization();
        }

        private void CountRowsDgv()
        {
            count_list_result.Content = +dgv_list_result.Items.Count;
        }

        public void Fill_List_Result()
        {
            using (var connection = new DB_Connect())
            {
                try
                {
                    connection.OpenConnect();

                    string sql = @"SELECT id_list_result AS ID, list_result_code AS 'Код ЛР', 
                    list_result_specialty AS 'Специальность', list_result_description AS 'Описание' FROM list_result;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgv_list_result.ItemsSource = dt.DefaultView;
                            dgv_list_result.Columns[0].Visibility = Visibility.Collapsed;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception here. For example, you can show an error message.
                    MessageBox.Show("An error occurred while filling the List Result table: " + ex.Message);
                }
                finally
                {
                    connection.CloseConnect();
                }
            }
        }

        public void Fill_ComboBoxSpecialization()
        {
            cb_specialization.Items.Clear();

            string sql = "SELECT specialization_abbreviation FROM specialization;";

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
                                cb_specialization.Items.Add(reader["specialization_abbreviation"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var GuideForm = new Guide();
            Application.Current.MainWindow = GuideForm;
            this.Hide();
            GuideForm.Show();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "INSERT INTO list_result (list_result_code, list_result_specialty, list_result_description) VALUES (@list_result_code, @list_result_specialty, @list_result_description); SELECT LAST_INSERT_ID();";

                    var command = new MySqlCommand(sql, connection.GetConnect());

                    var listResultCodeParameter = command.CreateParameter();
                    listResultCodeParameter.ParameterName = "@list_result_code";
                    listResultCodeParameter.Value = tb_result_code.Text;
                    command.Parameters.Add(listResultCodeParameter);

                    var listResultSpecialtyParameter = command.CreateParameter();
                    listResultSpecialtyParameter.ParameterName = "@list_result_specialty";
                    listResultSpecialtyParameter.Value = cb_specialization.Text;
                    command.Parameters.Add(listResultSpecialtyParameter);

                    var listResultDescriptionParameter = command.CreateParameter();
                    listResultDescriptionParameter.ParameterName = "@list_result_description";
                    listResultDescriptionParameter.Value = tb_description.Text;
                    command.Parameters.Add(listResultDescriptionParameter);

                    int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                }

                UpdateListResultTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление личностного результата", MessageBoxButton.OK, MessageBoxImage.Information);
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

                    string sql = @"UPDATE list_result 
                    SET list_result_code = @result_code, 
                        list_result_specialty = @specialty, 
                        list_result_description = @result_description, 
                        created_at = (SELECT MIN(created_at) FROM list_result)
                    WHERE id_list_result = @id_list_result;";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    com.Parameters.Add("@result_code", MySqlDbType.VarChar).Value = tb_result_code.Text;
                    com.Parameters.Add("@specialty", MySqlDbType.Int32).Value = cb_specialization.Text;
                    com.Parameters.Add("@result_description", MySqlDbType.VarChar).Value = tb_description.Text;

                    com.ExecuteNonQuery();
                }

                UpdateListResultTable();
                Fill_ListResultNew();

                MessageBox.Show("Запись успешно отредактирована.", "Редактирование записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fill_ListResultNew()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT id_list_result AS ID, list_result_code AS 'Код ЛР', 
                           list_result_specialty AS 'Специальность', list_result_description AS 'Описание' FROM list_result
                           ORDER BY CASE WHEN id_list_result = @selectedId THEN 0 ELSE 1 END, id_list_result;";

                    var command = new MySqlCommand(sql, connection.GetConnect());
                    command.Parameters.AddWithValue("@selectedId", id_list_result);
                    command.ExecuteNonQuery();

                    var adapter = new MySqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgv_list_result.ItemsSource = dt.DefaultView;
                    dgv_list_result.Columns[0].Visibility = Visibility.Hidden;

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при заполнении данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgv_list_result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tb_result_code.Text = row_selected["Код ЛР"].ToString();
                cb_specialization.Text = row_selected["Специальность"].ToString();
                tb_description.Text = row_selected["Описание"].ToString();
                id_list_result = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // вызов метода Validate класса InputValidator для проверки введенных данных
                InputValidator validator = new InputValidator();
                if (!validator.Validate(tb_result_code.Text) || !validator.Validate(tb_description.Text))
                {
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = @"DELETE FROM list_result WHERE id_list_result = @id_list_result;";
                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                        com.Parameters.Add("@id_list_result", MySqlDbType.Int32).Value = id_list_result;

                        com.ExecuteNonQuery();
                    }

                    UpdateListResultTable();

                    MessageBox.Show("Запись успешно удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_description_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_description_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void tb_result_code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
        }

        private void tb_description_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
        }
    }
}
