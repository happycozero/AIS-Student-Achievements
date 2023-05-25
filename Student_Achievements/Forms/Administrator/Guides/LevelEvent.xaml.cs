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
    /// Interaction logic for LevelEvent.xaml
    /// </summary>
    public partial class LevelEvent : Window
    {
        private int _idLevelEvent;
        int count = 0;
        
        public LevelEvent()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_Level_Event();
            UpdateLevelEventTable();
        }

        private void Fill_Level_Event()
        {
            try
            {
                using (var dbConnect = new DB_Connect())
                {
                    var sql = "SELECT id_level_event AS 'ID', level_event_name AS 'Уровень мероприятия' FROM level_event ORDER BY id_level_event DESC";
                    using (var command = new MySqlCommand(sql, dbConnect.GetConnect()))
                    {
                        using (var dataAdapter = new MySqlDataAdapter(command))
                        {
                            var dataTable = new DataTable("level_event");
                            dataAdapter.Fill(dataTable);
                            dgv_level_event.ItemsSource = dataTable.DefaultView;
                            dgv_level_event.Columns[0].Visibility = Visibility.Collapsed;
                        }
                    }
                }

                CountRowsDgv();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка при загрузке данных: {0}", ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgv_level_event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                tb_level_event.Text = row_selected["Уровень мероприятия"].ToString();
                _idLevelEvent = Convert.ToInt32(row_selected["ID"]);
            }
        }

        private void UpdateLevelEventTable()
        {
            tb_level_event.Clear();
            Fill_Level_Event();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            label_level_event.Content = + dgv_level_event.Items.Count;
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var guideForm = new Guide();
            Application.Current.MainWindow = guideForm;
            this.Hide();
            guideForm.Show();
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // вызов метода Validate класса InputValidator для проверки введенных данных
                InputValidator validator = new InputValidator();
                if (!validator.Validate(tb_level_event.Text))
                {
                    return;
                }

                using (var connection = new DB_Connect())
                using (var command = connection.GetConnect().CreateCommand())
                {
                    connection.OpenConnect();

                    string sql = "INSERT INTO level_event (level_event_name) VALUES (@level_event_name); SELECT LAST_INSERT_ID();";
                    command.CommandText = sql;

                    var levelEventNameParameter = command.CreateParameter();
                    levelEventNameParameter.ParameterName = "@level_event_name";
                    levelEventNameParameter.Value = tb_level_event.Text;
                    command.Parameters.Add(levelEventNameParameter);

                    int lastInsertedId = Convert.ToInt32(command.ExecuteScalar());
                }

                UpdateLevelEventTable();

                MessageBox.Show("Успешно! Запись добавлена.", "Добавление записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка при добавлении записи: {0}", ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var inputValidator = new InputValidator();

                if (!inputValidator.Validate(tb_level_event.Text))
                {
                    return;
                }

                using (var connection = new DB_Connect())
                using (var command = connection.GetConnect().CreateCommand())
                {
                    connection.OpenConnect();

                    string sql = "UPDATE level_event SET level_event_name = @level_event_name WHERE id_level_event = @id_level_event;";
                    command.CommandText = sql;

                    var levelEventNameParameter = command.CreateParameter();
                    levelEventNameParameter.ParameterName = "@level_event_name";
                    levelEventNameParameter.Value = tb_level_event.Text;
                    command.Parameters.Add(levelEventNameParameter);

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@id_level_event";
                    idParameter.Value = _idLevelEvent;
                    command.Parameters.Add(idParameter);

                    int rowsAffected = command.ExecuteNonQuery();
                }

                UpdateLevelEventTable();
                Fill_LevelEventNew();

                MessageBox.Show("Успешно! Запись отредактирована.", "Редактирование записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Ошибка при редактировании записи: {0}", ex.Message), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    var deleteQuery = "DELETE FROM level_event WHERE id_level_event = @id_level_event";
                    using (var command = new MySqlCommand(deleteQuery, connection.GetConnect()))
                    {
                        command.Parameters.Add("@id_level_event", MySqlDbType.Int32).Value = _idLevelEvent;

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            UpdateLevelEventTable();
                            MessageBox.Show("Запись успешно удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }

                    connection.CloseConnect();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fill_LevelEventNew()
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = String.Format(@"SELECT id_level_event AS 'ID', level_event_name AS 'Уровень мероприятия' FROM level_event
                             ORDER BY CASE WHEN id_level_event = {0} THEN 0 ELSE 1 END, id_level_event", _idLevelEvent);

                    var command = new MySqlCommand(sql, connection.GetConnect());
                    command.ExecuteNonQuery();

                    var adapter = new MySqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgv_level_event.ItemsSource = dt.DefaultView;
                    dgv_level_event.Columns[0].Visibility = Visibility.Hidden;
                    dgv_level_event.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

                    connection.CloseConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при заполнении данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tb_level_event_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void tb_level_event_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = tb_level_event.SelectionStart; // сохраняем положение курсора
         
            if (tb_level_event.Text.Length == 1)
            {
                tb_level_event.Text = Convert.ToString(char.ToUpper(tb_level_event.Text[0]));
                cursorPosition++; // увеличиваем положение курсора на 1, если была добавлена заглавная буква
            }

            // Удаляем все недопустимые символы из строки, разрешая добавление пробелов
            tb_level_event.Text = Regex.Replace(tb_level_event.Text, "[^А-Яа-яЁё\\s]+", " ");

            tb_level_event.SelectionStart = Math.Min(cursorPosition, tb_level_event.Text.Length); // восстанавливаем положение курсора, учитывая количество удаленных символов
        }

        private bool IsRussianLetter(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

        private void tb_level_event_OnPreviewKeyDown(object sender, KeyEventArgs e)
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