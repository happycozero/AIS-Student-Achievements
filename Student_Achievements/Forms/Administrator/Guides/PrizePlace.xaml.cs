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
    /// Interaction logic for PricezPlace.xaml
    /// </summary>
    public partial class PrizePlace : Window
    {
        private int id_prize_place;
        int count = 0;

        public PrizePlace()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            UpdatePrizePlaceTable();
        }

        private void UpdatePrizePlaceTable()
        {
            tb_place_name.Clear();
            Fill_Prize_Place();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            count_prize_place.Content = + dgv_prize_place.Items.Count;
        }

        public void Fill_Prize_Place()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_prize_place AS 'ID', prize_place_name AS 'Призовое место' FROM prize_place;";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter data_adapter = new MySqlDataAdapter(com);
                    DataTable data_table = new DataTable("prize_place");
                    data_adapter.Fill(data_table);
                    dgv_prize_place.ItemsSource = data_table.DefaultView;
                    dgv_prize_place.Columns[0].Visibility = Visibility.Collapsed;
                    dgv_prize_place.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                    connection.CloseConnect();

                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // вызов метода Validate класса InputValidator для проверки введенных данных
                InputValidator validator = new InputValidator();
                if (!validator.Validate(tb_place_name.Text))
                {
                    return;
                }

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"INSERT INTO prize_place (prize_place_name) 
                             VALUES ('{0}');
                             SELECT * FROM prize_place 
                             WHERE id_prize_place = LAST_INSERT_ID();", tb_place_name.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdatePrizePlaceTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление призового места", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var GuideForm = new Guide();
            Application.Current.MainWindow = GuideForm;
            this.Hide();
            GuideForm.Show();
        }

        private void dgv_prize_place_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tb_place_name.Text = row_selected["Призовое место"].ToString();
                id_prize_place = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            // вызов метода Validate класса InputValidator для проверки введенных данных
            InputValidator validator = new InputValidator();
            if (!validator.Validate(tb_place_name.Text))
            {
                return;
            }

            try
            {
                DB_Connect connection = new DB_Connect();
                connection.OpenConnect();

                string sql = @"UPDATE prize_place SET prize_place_name = @prize_place_name, id_prize_place = 
                (SELECT new_id FROM (SELECT MAX(id_prize_place) + 1 AS new_id FROM prize_place) AS temp_table)
                WHERE id_prize_place = @id_prize_place;";

                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                com.Parameters.Add("@prize_place_name", MySqlDbType.VarChar).Value = tb_place_name.Text;
                com.Parameters.Add("@id_prize_place", MySqlDbType.Int32).Value = id_prize_place;

                com.ExecuteNonQuery();

                connection.CloseConnect();

                UpdatePrizePlaceTable();

                MessageBox.Show("Запись успешно отредактирована.", "Редактирование записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при редактировании записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    DB_Connect connection = new DB_Connect();
                    connection.OpenConnect();

                    string sql = "DELETE FROM prize_place WHERE id_prize_place = @id_prize_place";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    com.Parameters.Add("@id_prize_place", MySqlDbType.Int32).Value = id_prize_place;

                    com.ExecuteNonQuery();

                    connection.CloseConnect();

                    UpdatePrizePlaceTable();

                    MessageBox.Show("Запись успешно удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_place_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = tb_place_name.SelectionStart; // сохраняем положение курсора

            if (tb_place_name.Text.Length == 1)
            {
                tb_place_name.Text = Convert.ToString(char.ToUpper(tb_place_name.Text[0]));
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки
            tb_place_name.Text = Regex.Replace(tb_place_name.Text, "[^А-Яа-яЁё]", "");

            tb_place_name.SelectionStart = cursorPosition; // восстанавливаем положение курсора
        }

        private void tb_place_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();

            // Блокируем ввод английских букв и специальных символов
            Regex regex = new Regex("[^А-Яа-яЁё0-9]");
            bool hasNonRussianLetters = regex.IsMatch(e.Text);
            e.Handled = hasNonRussianLetters;

            foreach (char c in e.Text)
            {
                if (!IsRussianLetter(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void tb_place_name_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
