using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using Student_Achievements.Forms.Main;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for Event.xaml
    /// </summary>
    public partial class Event : Window
    {
        static public int role_user = 0;
        static public string id_user = "";
        bool flag = false;
        private int id_event;
        int id_stud = 0;

        public Event()
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
            Fill_Event();
            Fill_CbGroup();
            Fill_CbLR();
            Fill_CbLevelEvent();
            Fill_CbPlaceEvent();


            DtOrder.SelectedDate = DateTime.Now;

            DtOrder.DisplayDateStart = new DateTime(2000, 1, 1);
            DtOrder.DisplayDateEnd = new DateTime(2100, 12, 31);
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

        private void UpdateEventTable()
        {
            CbGroup.SelectedIndex = -1;
            CbCource.SelectedIndex = -1;
            //CbLR.SelectedIndex = -1;
            CbLevelEvent.SelectedIndex = -1;
            CbPlaceEvent.SelectedIndex = -1;
            TbYears.Clear();
            TbNameEvent.Clear();

            Fill_Event();
            //Fill_CbGroup();
            //Fill_CbLevelEvent();
            //Fill_CbLR();
            //Fill_CbPlaceEvent();
            CountRowsDgv();
        }

        private void CountRowsDgv()
        {
            CountEvent.Content = +DgvEvent.Items.Count;
        }

        public void Fill_Event()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `event`.id_event AS 'ID',
                    `group`.group_code AS 'Группа',
                    `group`.group_code AS 'Код группы',
                    `event`.event_cource_score AS 'Курс',
                    `event`.event_years_study AS 'Учебный год',
                    `event`.event_name AS 'Название',
                    level_event.level_event_name AS 'Уровень',
                    list_result.list_result_code AS 'Код ЛР',
                    prize_place.id_prize_place AS 'Место',
                    student.student_fio AS 'Участники',
                    `event`.event_document AS 'Грамота',
                    `event`.event_certificate AS 'Сертификат',
                    `event`.event_order_number AS 'Номер приказа',
                    `event`.event_order_date AS 'Дата приказа'
                    FROM `event`
                    INNER JOIN student ON `event`.event_students = student.id_student
                    INNER JOIN `group` ON `event`.event_group = `group`.id_group
                    INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                    INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                    INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    DataTable dataTable = new DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvEvent.ItemsSource = dataTable.DefaultView;
                    DgvEvent.Columns[0].Visibility = Visibility.Collapsed;
                    DgvEvent.Columns[1].Visibility = Visibility.Collapsed;
                    DgvEvent.Columns[2].Visibility = Visibility.Collapsed;

                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_CbGroup()
        {
            try
            {
                CbGroup.Items.Clear();

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
                                CbGroup.Items.Add(reader[0].ToString());
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

        public void Fill_CbLR()
        {
            try
            {
                CbLR.Items.Clear();

                string sql = "SELECT list_result_code FROM list_result;";

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CbLR.Items.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке ЛР произошла ошибка: " + ex.Message, "Ошибка загрузки ЛР", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_CbLevelEvent()
        {
            try
            {
                CbLevelEvent.Items.Clear();

                string sql = "SELECT level_event_name FROM level_event;";

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CbLevelEvent.Items.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке уровня мероприятий произошла ошибка: " + ex.Message, "Ошибка загрузки уровней мероприятий", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Fill_CbPlaceEvent()
        {
            try
            {
                CbPlaceEvent.Items.Clear();

                string sql = "SELECT prize_place_name FROM prize_place;";

                using (DB_Connect connect = new DB_Connect())
                {
                    connect.OpenConnect();
                    using (MySqlCommand com = new MySqlCommand(sql, connect.GetConnect()))
                    {
                        using (MySqlDataReader reader = com.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CbPlaceEvent.Items.Add(reader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке призовых мест произошла ошибка: " + ex.Message, "Ошибка загрузки призовых мест", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CbGroup.Text == "" || CbCource.Text == "" || CbLR.Text == "" || CbLevelEvent.Text == "" || CbPlaceEvent.Text == "" || TbNameEvent.Text == "")
                {
                    MessageBox.Show("Ошибка! Заполните поля.", "Добавление события", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    using (var connection = new DB_Connect())
                    {

                        connection.OpenConnect();
                        foreach (string s in selectedStudents2)
                        {

                            foreach (string s2 in selectedLR2)
                            {
                                if (s2 != "" && s != "")
                                {
                                    string sql = string.Format("INSERT INTO `event` VALUES (null, {0}, {1}, '{2}', '{3}', {4}, {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11})",
                                    group_id_u(), CbCource.Text, TbYears.Text, TbNameEvent.Text,
                                    s2, event_level_event_id_u(), event_prize_place_id_u(),
                                    TbNumberOrder.Text, DtOrder.Text, TbDocumentPhoto.Text, TbCertificatePhoto.Text, s);

                                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                                    {
                                        com.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        UpdateEventTable();

                        MessageBox.Show("Успешно! Запись добавлена.", "Добавление события", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (MySqlException ex)
            {
                UpdateEventTable();
                MessageBox.Show("Ошибка при загрузке БД: " + ex.Message, "Ошибка при загрузке БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                UpdateEventTable();
                MessageBox.Show("Успешно! Запись добавлена.", "Добавление события", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                UpdateEventTable();
            }
        }

        private void ButEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int eventId = GetEventId();

                if (eventId == -1)
                {
                    MessageBox.Show("Выберите событие для редактирования.", "Редактирование записи", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "UPDATE `event` SET ";
                    sql += string.Format("event_group={0}, event_cource_score='{1}', event_years_study='{2}', event_name='{3}', event_code_lr={4}, event_level_event={5}, event_prize_place={6}, event_order_number='{7}', event_order_date='{8}', event_document='{9}', event_certificate='{10}', event_students={11} ",
                        group_id_u(), CbCource.Text, TbYears.Text, TbNameEvent.Text,
                        get_code_id_lr(), event_level_event_id_u(), event_prize_place_id_u(),
                        TbNumberOrder.Text, DtOrder.Text, TbDocumentPhoto.Text, TbCertificatePhoto.Text, get_id_stud());

                    sql += string.Format("WHERE id_event={0}", id_event);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateEventTable();

                MessageBox.Show("Успешно! Запись обновлена.", "Обновление записи", MessageBoxButton.OK, MessageBoxImage.Information);
                UpdateEventTable();
            }
            catch (MySqlException ex)
            {
                UpdateEventTable();
                MessageBox.Show("Ошибка при загрузке БД: " + ex.Message, "Ошибка при загрузке БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                UpdateEventTable();
                MessageBox.Show("Ошибка при редактировании записи: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButDel_Click(object sender, RoutedEventArgs e)
        {
            int eventId = GetSelectedEventId();

            if (eventId != 0)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteEvent(eventId); // удалить запись из таблицы "event"
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteEvent(int eventId)
        {
            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = string.Format("DELETE FROM `event` WHERE id_event={0}", eventId);

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        com.ExecuteNonQuery();
                    }
                }

                UpdateEventTable();

                MessageBox.Show("Успешно! Запись удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (MySqlException ex)
            {
                // Обработка ошибок MySQL
                MessageBox.Show("Ошибка при загрузке БД: " + ex.Message, "Ошибка при загрузке БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                UpdateEventTable();
                // Общая обработка ошибок
                MessageBox.Show("Успешно! Запись удалена.", "Удаление записи", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                UpdateEventTable();
            }
        }

        private int GetSelectedEventId()
        {
            if (DgvEvent.SelectedItem != null)
            {
                DataRowView row = (DataRowView)DgvEvent.SelectedItem;
                return Convert.ToInt32(row["ID"]);
            }
            else
            {
                return 0;
            }
        }

        private int GetEventId()
        {
            int eventId = 0;

            try
            {
                using (var connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_event FROM event ORDER BY id_event DESC LIMIT 1";

                    using (var com = new MySqlCommand(sql, connection.GetConnect()))
                    {
                        eventId = (int)com.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return eventId + 1;
        }

        public int group_id_u()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_group FROM `group` WHERE group_code='" + CbGroup.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int group_int_id_u = Convert.ToInt32(com.ExecuteScalar());

                    return group_int_id_u;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id группы: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        //public int event_code_lr_id_u()
        //{
        //    try
        //    {
        //        using (DB_Connect connection = new DB_Connect())
        //        {
        //            connection.OpenConnect();

        //            string sql = "SELECT id_list_result FROM list_result WHERE list_result_code='" + CbLR.Text + "';";
        //            MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

        //            int event_code_lr_int_id_u = Convert.ToInt32(com.ExecuteScalar());

        //            return event_code_lr_int_id_u;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Произошла ошибка при получении id кода результата списка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
        //    }
        //}

        public int event_level_event_id_u()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_level_event FROM level_event WHERE level_event_name='" + CbLevelEvent.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int event_level_event_int_id_u = Convert.ToInt32(com.ExecuteScalar());

                    return event_level_event_int_id_u;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id уровня события: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        public int get_code_id_lr()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_list_result FROM list_result WHERE list_result_code='" + CbLR.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int id = Convert.ToInt32(com.ExecuteScalar());

                    return id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id уровня события: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        public int get_id_stud()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_student FROM student WHERE student_fio='" + TbSelectedStudents.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int id = Convert.ToInt32(com.ExecuteScalar());

                    return id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id уровня события: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        public int event_prize_place_id_u()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = "SELECT id_prize_place FROM prize_place WHERE prize_place_name='" + CbPlaceEvent.Text + "';";
                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    int event_prize_place_int_id_u = Convert.ToInt32(com.ExecuteScalar());

                    return event_prize_place_int_id_u;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при получении id места призера: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1; // возвращаем значение по умолчанию или некий флаг ошибки, чтобы дальше можно было обработать эту ситуацию
            }
        }

        private List<string> GetLRForGroup(string listLR)
        {
            List<string> lrs = new List<string>();
            using (DB_Connect connection = new DB_Connect())
            {
                connection.OpenConnect();
                string sql = string.Format("SELECT list_result_code FROM list_result WHERE list_result_specialty", listLR);
                MySqlCommand cmd = new MySqlCommand(sql, connection.GetConnect());
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lrs.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return lrs;
        }

        private List<string> GetStudentsForGroup(string groupCode)
        {
            List<string> students = new List<string>();
            using (DB_Connect connection = new DB_Connect())
            {
                connection.OpenConnect();
                string sql = string.Format("SELECT student_fio AS 'ФИО' FROM student WHERE student_group_code = (SELECT id_group FROM `group` WHERE group_code = '{0}')", groupCode);
                MySqlCommand cmd = new MySqlCommand(sql, connection.GetConnect());
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    students.Add(reader.GetString(0));
                }
                reader.Close();
            }
            return students;
        }

        private void ButUploadDocument_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.jpg;*.png)|*.jpg;*.png";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName.Replace("\\", "/");
                TbDocumentPhoto.Text = filename;
            }
        }

        private void DgvEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ButAdd.IsEnabled = false;
                DataGrid dg = (DataGrid)sender;
                DataRowView row_selected = dg.SelectedItem as DataRowView;

                if (row_selected != null)
                {
                    flag = true;
                    id_event = Convert.ToInt32(row_selected["ID"]);
                    CbGroup.Text = row_selected["Группа"].ToString();
                    CbCource.Text = row_selected["Курс"].ToString();
                    TbYears.Text = row_selected["Учебный год"].ToString();
                    TbNameEvent.Text = row_selected["Название"].ToString();
                    CbLevelEvent.Text = row_selected["Уровень"].ToString();
                    CbLR.Text = row_selected["Код ЛР"].ToString();
                    flag = false;
                    CbPlaceEvent.Text = row_selected["Место"].ToString();
                    TbSelectedStudents.Text = row_selected["Участники"].ToString();

                    // получение адреса файла из поля TbDocumentPhoto.Text
                    string documentphoto = row_selected["Грамота"].ToString();

                    // проверка наличия файла по указанному пути
                    if (documentphoto == "")
                    {
                        BitmapImage placeholder = new BitmapImage();
                        placeholder.BeginInit();
                        placeholder.UriSource = new Uri("/Student_Achievements;component/Documents/Template/document-holder.png",
                            UriKind.RelativeOrAbsolute);
                        placeholder.EndInit();
                        DocumentEvent.Source = placeholder;
                    }
                    else if (File.Exists(documentphoto))
                    {
                        // загрузка файла изображения в Image control
                        BitmapImage bitmap_document = new BitmapImage();
                        bitmap_document.BeginInit();
                        bitmap_document.UriSource = new Uri(documentphoto);
                        bitmap_document.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap_document.EndInit();
                        DocumentEvent.Source = bitmap_document;
                    }

                    // получение адреса файла из поля TbSertificatePhoto.Text
                    string certificatephoto = row_selected["Сертификат"].ToString();

                    // проверка наличия файла по указанному пути
                    if (certificatephoto == "")
                    {
                        BitmapImage placeholder = new BitmapImage();
                        placeholder.BeginInit();
                        placeholder.UriSource =
                            new Uri("/Student_Achievements;component/Documents/Template/certificate-holder.png",
                                UriKind.RelativeOrAbsolute);
                        placeholder.EndInit();
                        CertificateEvent.Source = placeholder;
                    }
                    else if (File.Exists(certificatephoto))
                    {
                        // загрузка файла изображения в Image control
                        BitmapImage bitmap_certificate = new BitmapImage();
                        bitmap_certificate.BeginInit();
                        bitmap_certificate.UriSource = new Uri(certificatephoto);
                        bitmap_certificate.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap_certificate.EndInit();
                        CertificateEvent.Source = bitmap_certificate;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            ButAdd.IsEnabled = true;
            UpdateEventTable();
        }

        string[] selectedStudents = new string[30];
        string[] selectedStudents2 = new string[30];

        string[] selectedLR = new string[30];
        string[] selectedLR2 = new string[30];

        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                try
                {
                    string groupCode = CbGroup.SelectedItem.ToString();
                    List<string> students = GetStudentsForGroup(groupCode);
                    ChoiceStudent choiceStudent = new ChoiceStudent(students);
                    choiceStudent.ShowDialog();
                    // Получаем выбранных студентов из окна ChoiceStudent
                    selectedStudents = ChoiceStudent.FIOStudent;

                    for (int i = 0; i < selectedStudents.Length; i++)
                    {
                        if (selectedStudents[i] != null && selectedStudents[i] != "")
                        {
                            using (DB_Connect connection = new DB_Connect())
                            {
                                connection.OpenConnect();

                                string sql = "SELECT id_student FROM student WHERE student_fio = '" + selectedStudents[i] + "';";
                                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                                selectedStudents2[i] = com.ExecuteScalar().ToString();
                            }
                        }
                        else
                        {
                            selectedStudents[i] = "";
                            selectedStudents2[i] = "";
                        }
                    }
                }
                catch (Exception)
                {
                    ;
                }
            }
        }
        private void CbLR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flag == false)
            {
                try
                {
                    string listLR = CbLR.SelectedItem.ToString();
                    List<string> lr = GetLRForGroup(listLR);
                    ChoiceListResult choiceListResult = new ChoiceListResult(lr);
                    choiceListResult.ShowDialog();
                    // Получаем выбранные ЛР из окна ChoiceStudent
                    selectedLR = ChoiceListResult.LRS;

                    for (int i = 0; i < selectedLR.Length; i++)
                    {
                        if (selectedLR[i] != null && selectedLR[i] != "")
                        {
                            using (DB_Connect connection = new DB_Connect())
                            {
                                connection.OpenConnect();

                                string sql = "SELECT id_list_result, list_result_description FROM list_result WHERE list_result_code = '" + selectedLR[i] + "';";
                                MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                                MySqlDataReader reader = com.ExecuteReader();

                                if (reader.Read())
                                {
                                    selectedLR2[i] = reader["id_list_result"].ToString();
                                    string description = reader["list_result_description"].ToString();
                                    // Используйте значение description по вашему усмотрению
                                }
                                else
                                {
                                    selectedLR2[i] = "";
                                }

                                reader.Close();
                            }
                        }
                        else
                        {
                            selectedLR[i] = "";
                            selectedLR2[i] = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("При загрузке модального окна выбора личностных результатов произошла ошибка: " + ex.Message, "Ошибка загрузки модального окна", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void DgvEvent_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void TbNameEvent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            KeyboardLayout.SetToRussian();
        }
    }
}
