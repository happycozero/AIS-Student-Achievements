using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using Student_Achievements.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for RecordEvent.xaml
    /// </summary>
    public partial class RecordEvent : System.Windows.Window
    {
        public RecordEvent()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB_Connect connection = new DB_Connect();
            tbFIO.Text = connection.Fill_FIO();
            tbAccess.Text = connection.Fill_Access();

            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            Fill_CbGroup();
            Fill_LevelEvent();

            #region cbFill

            cbSearch.Items.Clear();
            cbSort.Items.Clear();
            cbSearch.Items.Add("");
            cbSearch.Items.Add("По наименованию");
            cbSearch.Items.Add("По ФИО студента");
            cbSort.Items.Add("");
            cbSort.Items.Add("По ФИО студента от А до Я");
            cbSort.Items.Add("По ФИО студента от Я до А");

            #endregion

            UpdateRecordEvent();
        }

        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUser.UserRole == "student")
            {
                var MenuStudentForm = new MenuStudent();
                System.Windows.Application.Current.MainWindow = MenuStudentForm;
                this.Hide();
                MenuStudentForm.Show();
            }
            else
            {
                var MenuTeacherForm = new MenuTeacher();
                System.Windows.Application.Current.MainWindow = MenuTeacherForm;
                this.Hide();
                MenuTeacherForm.Show();
            }
        }

        private void CountRowsDgv()
        {
            CountRecordEvent.Content = +DgvRecordEvent.Items.Count;
        }

        private void UpdateRecordEvent()
        {
            CountRowsDgv();
        }

        private void Fill_DgvRecordEvent()
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
                    `student`.student_fio AS 'Участники',
                    `event`.event_document AS 'Грамота',
                    `event`.event_certificate AS 'Сертификат',
                    `event`.event_order_number AS 'Номер приказа',
                    `event`.event_order_date AS 'Дата приказа'
                    FROM `event`
                    INNER JOIN `group` ON `event`.event_group = `group`.id_group
                    INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                    INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                    INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                    INNER JOIN `student` ON `event`.event_students = `student`.id_student;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    DgvRecordEvent.Columns[1].Visibility = Visibility.Collapsed;
                    DgvRecordEvent.Columns[2].Visibility = Visibility.Collapsed;

                    UpdateRecordEvent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fill_CbGroup()
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
                UpdateRecordEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке групп произошла ошибка: " + ex.Message, "Ошибка загрузки групп", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fill_CbCource()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = String.Format(@"SELECT `cource`.courсe_score FROM `cource` INNER JOIN `group` ON `cource`.courсe_group_name = `group`.id_group 
                                                WHERE `group`.group_code = '{0}'", CbGroup.SelectedItem.ToString());

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        CbCource.Items.Clear();
                        CbCource.Items.Add("");
                        while (reader.Read())
                        {
                            CbCource.Items.Add(reader[0].ToString());
                        }
                    }
                }
                UpdateRecordEvent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();

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
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;

                    Fill_CbCource();
                    UpdateRecordEvent();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GenerateExcel()
        {
            try
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();
                string reportTitle = "Отчет по группе " + selectedGroupCode; // за {selectedCource}

                // Создание объекта диалогового окна сохранения файла Excel
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";

                if (saveFileDialog.ShowDialog() == true)
                {
                    var fileName = saveFileDialog.FileName;

                    // Создание приложения Excel
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;

                    // Создание новой книги Excel и листа
                    var excelWorkbook = excelApp.Workbooks.Add();
                    var excelWorksheet = (Worksheet)excelWorkbook.ActiveSheet;

                    // Заполнение заголовка отчета
                    Range titleRange = excelWorksheet.get_Range("A1", "E2");
                    titleRange.Merge();
                    titleRange.Font.Bold = true;
                    titleRange.Font.Size = 16;
                    titleRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    titleRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    titleRange.Value2 = reportTitle;

                    // Заполнение заголовков таблицы и установка стилей
                    Range headerRange = excelWorksheet.get_Range("A3", "E3");
                    headerRange.Font.Bold = true;
                    headerRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                    headerRange.Borders.Weight = XlBorderWeight.xlThin;
                    headerRange.WrapText = true;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    headerRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    excelWorksheet.Cells[3, 1] = "Код ЛР";
                    excelWorksheet.Cells[3, 2] = "Уровень";
                    excelWorksheet.Cells[3, 3] = "Название";
                    excelWorksheet.Cells[3, 4] = "Место";
                    excelWorksheet.Cells[3, 5] = "Участники";

                    // Установка размеров столбцов
                    Range columnWidthRange = excelWorksheet.get_Range("A3", "E3");
                    columnWidthRange.Columns[1].ColumnWidth = 6.8;
                    columnWidthRange.Columns[2].ColumnWidth = 15.75;
                    columnWidthRange.Columns[3].ColumnWidth = 24.20;
                    columnWidthRange.Columns[4].ColumnWidth = 6.15;
                    columnWidthRange.Columns[5].ColumnWidth = 25.00;

                    // Создание списка объектов Record и заполнение его значениями из DataGrid
                    var records = new List<Record>();
                    foreach (var item in DgvRecordEvent.Items)
                    {
                        var record = new Record(
                            ((DataRowView)item)[7].ToString(),
                            ((DataRowView)item)[6].ToString(),
                            ((DataRowView)item)[5].ToString(),
                            ((DataRowView)item)[8].ToString(),
                            ((DataRowView)item)[9].ToString()
                        );
                        records.Add(record);
                    }

                    // Заполнение данных из списка объектов Record и обработка переноса текста
                    for (int i = 0; i < records.Count; i++)
                    {
                        excelWorksheet.Cells[i + 4, 1] = records[i].LabCode;
                        excelWorksheet.Cells[i + 4, 2] = records[i].Level;
                        excelWorksheet.Cells[i + 4, 3] = records[i].Name;
                        excelWorksheet.Cells[i + 4, 4] = records[i].Place;
                        excelWorksheet.Cells[i + 4, 5] = records[i].Participants;

                        // Обработка переноса текста
                        Range cellRange = excelWorksheet.Cells[i + 3, 1];
                        cellRange.WrapText = true;
                        if (cellRange.Rows.Height < 30)
                        {
                            cellRange.Rows.AutoFit();
                        }

                        cellRange = excelWorksheet.Cells[i + 3, 2];
                        cellRange.WrapText = true;
                        if (cellRange.Rows.Height < 30)
                        {
                            cellRange.Rows.AutoFit();
                        }

                        cellRange = excelWorksheet.Cells[i + 3, 3];
                        cellRange.WrapText = true;
                        if (cellRange.Rows.Height < 30)
                        {
                            cellRange.Rows.AutoFit();
                        }

                        cellRange = excelWorksheet.Cells[i + 3, 5];
                        cellRange.WrapText = true;
                        if (cellRange.Rows.Height < 30)
                        {
                            cellRange.Rows.AutoFit();
                        }
                    }

                    // Установка выравнивания для всех ячеек
                    Range dataRange = excelWorksheet.get_Range("A2:E" + (records.Count + 2).ToString());
                    dataRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    dataRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                    // Настройка форматированияdataRange.EntireColumn.AutoFit();
                    dataRange.Borders.LineStyle = XlLineStyle.xlContinuous;
                    dataRange.Borders.Weight = XlBorderWeight.xlThin;
                    dataRange.Font.Size = 11;
                    dataRange.Font.Name = "Calibri";

                    // Сохранение файла
                    excelWorkbook.SaveAs(fileName, XlFileFormat.xlOpenXMLWorkbook, Missing.Value,
                    Missing.Value, false, false, XlSaveAsAccessMode.xlNoChange,
                    XlSaveConflictResolution.xlUserResolution, true, Missing.Value, Missing.Value, Missing.Value);

                    // Закрытие приложения Excel
                    excelWorkbook.Close(false, Missing.Value, Missing.Value);
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                    // Открываем сохраненный файл
                    Process.Start(fileName);

                    MessageBox.Show("Файл успешно сохранен", "Сохранение файла", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при генерации Excel файла: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButExpExcel_Click(object sender, RoutedEventArgs e)
        {
            GenerateExcel();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text.Length > 2)
            {
                if (cbSearch.SelectedItem.ToString() == "По наименованию")
                {
                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = String.Format(@"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `event`.event_name LIKE '%{0}%';", tbSearch.Text);

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }

                else if (cbSearch.SelectedItem.ToString() == "По ФИО студента")
                {
                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = String.Format(@"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `student`.student_fio LIKE '%{0}%';", tbSearch.Text);

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }

                else
                {
                    string selectedGroupCode = CbGroup.SelectedItem.ToString();

                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }
            }

            else
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();

                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void CbCource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbCource.SelectedItem.ToString() == "")
                {
                    string selectedGroupCode = CbGroup.SelectedItem.ToString();

                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }

                else
                {
                    using (DB_Connect connection = new DB_Connect())
                    {
                        string selectedGroupCode = CbGroup.SelectedItem.ToString();
                        string selectedCource = CbCource.SelectedItem.ToString();

                        connection.OpenConnect();

                        string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode AND `event`.event_cource_score = @groupCource;";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                        com.Parameters.AddWithValue("@groupCource", selectedCource);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Fill_LevelEvent()
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `level_event`.level_event_name FROM `level_event`";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());

                    using (MySqlDataReader reader = com.ExecuteReader())
                    {
                        cbLevelEvent.Items.Clear();
                        cbLevelEvent.Items.Add("");
                        while (reader.Read())
                        {
                            cbLevelEvent.Items.Add(reader[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbLevelEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbLevelEvent.SelectedItem.ToString() == "")
                {
                    string selectedGroupCode = CbGroup.SelectedItem.ToString();

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
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;

                    }
                }

                else
                {
                    string selectedLevelEvent = cbLevelEvent.SelectedItem.ToString();

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
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE level_event.level_event_name = @levelevent;";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        com.Parameters.AddWithValue("@levelevent", selectedLevelEvent);
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                        dataAdapter.Fill(dataTable);

                        DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                        DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbSearch_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cbSearch.SelectedItem.ToString() == "По наименованию" || cbSearch.SelectedItem.ToString() == "По ФИО студента")
            {
                tbSearch.IsEnabled = true;
            }

            else
            {
                tbSearch.IsEnabled = false;
            }
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSort.SelectedItem.ToString() == "По ФИО студента от А до Я")
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();

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
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode ORDER BY `student`.student_fio DESC;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                }
            }

            else if (cbSort.SelectedItem.ToString() == "По ФИО студента от Я до А")
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();

                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode ORDER BY `student`.student_fio ASC;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                string selectedGroupCode = CbGroup.SelectedItem.ToString();

                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT `event`.id_event AS 'ID',
                                `group`.group_code AS 'Код группы',
                                `event`.event_cource_score AS 'Курс',
                                `event`.event_years_study AS 'Учебный год',
                                `event`.event_name AS 'Название',
                                level_event.level_event_name AS 'Уровень',
                                list_result.list_result_code AS 'Код ЛР',
                                prize_place.id_prize_place AS 'Место',
                                `student`.student_fio AS 'Участники',
                                `event`.event_document AS 'Грамота',
                                `event`.event_certificate AS 'Сертификат',
                                `event`.event_order_number AS 'Номер приказа',
                                `event`.event_order_date AS 'Дата приказа'
                                FROM `event`
                                INNER JOIN `group` ON `event`.event_group = `group`.id_group
                                INNER JOIN level_event ON `event`.event_level_event = level_event.id_level_event
                                INNER JOIN list_result ON `event`.event_code_lr = list_result.id_list_result
                                INNER JOIN prize_place ON `event`.event_prize_place = prize_place.id_prize_place
                                INNER JOIN `student` ON `event`.event_students = `student`.id_student
                                WHERE `group`.group_code = @groupCode;";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    com.Parameters.AddWithValue("@groupCode", selectedGroupCode);
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("`event`");
                    dataAdapter.Fill(dataTable);

                    DgvRecordEvent.ItemsSource = dataTable.DefaultView;
                    DgvRecordEvent.Columns[0].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var ReportTeacher = new Report();
            System.Windows.Application.Current.MainWindow = ReportTeacher;
            this.Hide();
            ReportTeacher.Show();
        }

        private void ButClear_Click(object sender, RoutedEventArgs e)
        {
            if (CbGroup != null)
            {
                CbGroup.SelectedIndex = -1;
            }

            if (CbCource != null)
            {
                CbCource.SelectedIndex = -1;
            }

            if (cbLevelEvent != null)
            {
                cbLevelEvent.SelectedIndex = -1;
            }

            if (cbSearch != null)
            {
                cbSearch.SelectedIndex = -1;
            }

            if (cbSort != null)
            {
                cbSort.SelectedIndex = -1;
            }

            if (DgvRecordEvent != null)
            {
                DgvRecordEvent.ItemsSource = null;
                DgvRecordEvent.Items.Clear();
            }
        }
    }
}
