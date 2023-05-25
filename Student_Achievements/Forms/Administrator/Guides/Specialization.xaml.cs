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
    /// Interaction logic for Specialization.xaml
    /// </summary>
    public partial class Specialization : Window
    {
        private int id_specialization;
        int count = 0;

        public Specialization()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Specialization();
            UpdateSpecializationTable();
        }

        public void Fill_Specialization()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT id_specialization AS 'ID', specialization_name AS 'Код специальности',
                    specialization_abbreviation AS 'Обозначение' FROM specialization;";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        using (var adapter = new MySqlDataAdapter(com))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgv_specialization.ItemsSource = dt.DefaultView;
                            dgv_specialization.Columns[0].Visibility = Visibility.Collapsed;
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

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputValidator validator = new InputValidator();
                if (!validator.Validate(tb_specialization_name.Text) || !validator.Validate(tb_specialization_abbreviation.Text))
                {
                    return;
                }

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format("INSERT INTO specialization (specialization_name, specialization_abbreviation) VALUES ('{0}', '{1}')", tb_specialization_name.Text, tb_specialization_abbreviation.Text);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateSpecializationTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление категории", MessageBoxButton.OK, MessageBoxImage.Information);
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
                InputValidator validator = new InputValidator();
                if (!validator.Validate(tb_specialization_name.Text) || !validator.Validate(tb_specialization_abbreviation.Text))
                {
                    return;
                }

                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "UPDATE specialization SET specialization_name = @specialization_name, specialization_abbreviation = @specialization_abbreviation WHERE id_specialization = @id_specialization";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    com.Parameters.Add("@specialization_name", MySqlDbType.VarChar).Value = tb_specialization_name.Text;
                    com.Parameters.Add("@specialization_abbreviation", MySqlDbType.VarChar).Value = tb_specialization_abbreviation.Text;
                    com.Parameters.Add("@id_specialization", MySqlDbType.Int32).Value = id_specialization;

                    com.ExecuteNonQuery();

                    connection.CloseConnect();

                    UpdateSpecializationTable();

                    MessageBox.Show("Запись успешно отредактирована.", "Редактирование записи", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = "DELETE FROM specialization WHERE id_specialization = @id_specialization";
                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                        com.Parameters.Add("@id_specialization", MySqlDbType.Int32).Value = id_specialization;

                        com.ExecuteNonQuery();

                        connection.CloseConnect();

                        UpdateSpecializationTable();

                        MessageBox.Show("Запись успешно удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var GuideForm = new Guide();
            Application.Current.MainWindow = GuideForm;
            this.Hide();
            GuideForm.Show();
        }

        private void CountRowsDgv()
        {
            count_specialization.Content = +dgv_specialization.Items.Count;
        }

        private void UpdateSpecializationTable()
        {
            tb_specialization_name.Clear();
            tb_specialization_abbreviation.Clear();
            Fill_Specialization();
            CountRowsDgv();
        }

        private void dgv_specialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tb_specialization_name.Text = row_selected["Код специальности"].ToString() + " ";
                tb_specialization_abbreviation.Text = row_selected["Обозначение"].ToString() + " ";
                id_specialization = Convert.ToInt32(row_selected["ID"]);
            }
        }
        
        private void tb_specialization_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = tb_specialization_name.SelectionStart; // сохраняем положение курсора

            if (tb_specialization_name.Text.Length == 1)
            {
                tb_specialization_name.Text = Convert.ToString(char.ToUpper(tb_specialization_name.Text[0]));
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки
            tb_specialization_name.Text = Regex.Replace(tb_specialization_name.Text, "[^А-Яа-яЁё]", "");

            tb_specialization_name.SelectionStart = cursorPosition; // восстанавливаем положение курсора
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void tb_specialization_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void tb_abbreviation_name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Блокируем ввод английских букв и специальных символов
            Regex regex = new Regex("[^А-Яа-яЁё]");
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

        private void tb_specialization_abbreviation_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = tb_specialization_abbreviation.SelectionStart; // сохраняем положение курсора

            if (tb_specialization_abbreviation.Text.Length == 1)
            {
                tb_specialization_abbreviation.Text = Convert.ToString(char.ToUpper(tb_specialization_abbreviation.Text[0]));
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки, разрешая добавление пробелов
            tb_specialization_abbreviation.Text = Regex.Replace(tb_specialization_abbreviation.Text, "[^А-Яа-яЁё\\s]+", " ");

            tb_specialization_abbreviation.SelectionStart = Math.Min(cursorPosition, tb_specialization_abbreviation.Text.Length); // восстанавливаем положение курсора, учитывая количество удаленных символов
        }

        private void tb_specialization_abbreviation_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void tb_specialization_abbreviation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();

            // Блокируем ввод английских букв и специальных символов
            Regex regex = new Regex("[^А-Яа-яЁё]");
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
    }
}
