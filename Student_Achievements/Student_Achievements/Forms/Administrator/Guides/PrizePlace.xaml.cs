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
            DB_Connect connection = new DB_Connect();
            tbFIO.Text = connection.Fill_FIO();
            tbAccess.Text = connection.Fill_Access();
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            UpdatePrizePlaceTable();
        }

        private void UpdatePrizePlaceTable()
        {
            TbPlaceName.Clear();
            Fill_Prize_Place();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            CountPrizePlace.Content = + DgvPrizePlace.Items.Count;
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
                    DgvPrizePlace.ItemsSource = data_table.DefaultView;
                    DgvPrizePlace.Columns[0].Visibility = Visibility.Collapsed;

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
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format(@"INSERT INTO prize_place (prize_place_name) 
                             VALUES ('{0}');
                             SELECT * FROM prize_place 
                             WHERE id_prize_place = LAST_INSERT_ID();", TbPlaceName.Text);

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
                TbPlaceName.Text = row_selected["Призовое место"].ToString();
                id_prize_place = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            // вызов метода Validate класса InputValidator для проверки введенных данных
            InputValidator validator = new InputValidator();
            if (!validator.Validate(TbPlaceName.Text))
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

                com.Parameters.Add("@prize_place_name", MySqlDbType.VarChar).Value = TbPlaceName.Text;
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
            int cursorPosition = TbPlaceName.SelectionStart; // сохраняем положение курсора

            if (TbPlaceName.Text.Length == 1)
            {
                TbPlaceName.Text = Convert.ToString(char.ToUpper(TbPlaceName.Text[0]));
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            TbPlaceName.SelectionStart = cursorPosition; // восстанавливаем положение курсора
        }

        private void tb_place_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
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

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            TbPlaceName.Clear();
            Fill_Prize_Place();
            CountRowsDgv();
        }
    }
}
