using MySql.Data.MySqlClient;
using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using Student_Achievements.Forms;

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for TrackingGroup.xaml
    /// </summary>
    public partial class TrackingGroup : System.Windows.Window
    {
        public TrackingGroup()
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

                CountRowsDgv();
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке групп произошла ошибка: " + ex.Message, "Ошибка загрузки групп", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CountRowsDgv()
        {
            CountTrackingGroup.Content = +DgvTrackingGroup.Items.Count;
        }

        private void CbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (DB_Connect connection = new DB_Connect())
                {
                    connection.OpenConnect();

                    string sql = @"SELECT
                                student.id_student AS 'ID',
                                student.student_fio AS 'ФИО',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР01', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР01',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР02', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР02',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР03', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР03',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР04', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР04',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР05', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР05',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР06', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР06',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР07', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР07',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР08', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР08',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР09', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР09',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР10', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР10',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР11', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР11',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР12', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР12',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР13', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР13',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР14', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР14',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР15', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР15',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР16', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР16',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР17', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР17',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР18', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР18',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР19', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР19',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР20', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР20',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР21', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР21',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР22', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР22',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР23', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР23',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР24', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР24',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР25', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР25',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР26', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР26',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР27', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР27',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР28', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР28',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР29', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР29',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР30', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР30'
                                FROM
                                student
                                LEFT JOIN event ON event.event_students = student.id_student
                                LEFT JOIN list_result ON event.event_code_lr = list_result.id_list_result
                                GROUP BY
                                student.id_student
                                ORDER BY
                                student.id_student";

                    MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                    System.Data.DataTable dataTable = new System.Data.DataTable("student");
                    dataAdapter.Fill(dataTable);

                    DgvTrackingGroup.Columns.Clear();
                    var idColumn = new DataGridTextColumn() { Header = "ID", Binding = new Binding("ID"), Visibility = Visibility.Hidden };
                    var fioColumn = new DataGridTextColumn() { Header = "ФИО", Binding = new Binding("ФИО") };
                    DgvTrackingGroup.Columns.Add(idColumn);
                    DgvTrackingGroup.Columns.Add(fioColumn);
                    for (int i = 1; i <= 30; i++)
                    {
                        string lrColumnName = "ЛР" + i.ToString("D2");
                        var lrColumn = new DataGridTextColumn() { Header = lrColumnName, Binding = new Binding(lrColumnName) };
                        DgvTrackingGroup.Columns.Add(lrColumn);
                    }

                    DgvTrackingGroup.ItemsSource = dataTable.DefaultView;

                    Fill_CbCource();
                    CountRowsDgv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static DataGridCell GetCell(DataGrid dg, int row, int column)
        {
            DataGridRow rowContainer = GetRow(dg, row);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    dg.ScrollIntoView(rowContainer, dg.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        private static DataGridRow GetRow(DataGrid dg, int index)
        {
            DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                dg.UpdateLayout();
                dg.ScrollIntoView(dg.Items[index]);
                row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        private static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void GenerateToExcel()
        {
            try
            {
                // Prompt the user to select a file path
                var saveDialog = new Microsoft.Win32.SaveFileDialog();
                saveDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveDialog.FileName = "Мониторинг группы.xlsx";
                bool? result = saveDialog.ShowDialog();
                if (result != true) return;
                string filePath = saveDialog.FileName;

                // Create a new Excel workbook and worksheet
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                var excelWorkbook = excelApp.Workbooks.Add();
                var excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.ActiveSheet;

                // Write the column headers to the worksheet
                int columnCount = 0;
                for (int i = 0; i < DgvTrackingGroup.Columns.Count; i++)
                {
                    // Check if the column is visible and not the ID column
                    if (DgvTrackingGroup.Columns[i].Visibility == Visibility.Visible && DgvTrackingGroup.Columns[i].Header.ToString() != "ID")
                    {
                        excelWorksheet.Cells[2, columnCount + 1] = DgvTrackingGroup.Columns[i].Header.ToString();
                        excelWorksheet.Cells[2, columnCount + 1].Font.Bold = true; // Set the font to bold
                        columnCount++;
                    }
                }

                // Set the report title
                string reportTitle = "Отчет по группе (" + CbGroup.Text + ")";
                excelWorksheet.Range[excelWorksheet.Cells[1, 1], excelWorksheet.Cells[1, columnCount]].Merge();
                excelWorksheet.Range[excelWorksheet.Cells[1, 1], excelWorksheet.Cells[1, columnCount]].Value = reportTitle;
                excelWorksheet.Range[excelWorksheet.Cells[1, 1], excelWorksheet.Cells[1, columnCount]].HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
                excelWorksheet.Range[excelWorksheet.Cells[1, 1], excelWorksheet.Cells[1, columnCount]].Font.Bold = true;

                // Write the data rows to the worksheet
                for (int i = 0; i < DgvTrackingGroup.Items.Count; i++)
                {
                    DataRowView rowView = DgvTrackingGroup.Items[i] as DataRowView;
                    if (rowView != null)
                    {
                        DataRow row = rowView.Row;
                        int columnNumber = 0;
                        for (int j = 0; j < DgvTrackingGroup.Columns.Count; j++)
                        {
                            // Check if the column is visible and not the ID column
                            if (DgvTrackingGroup.Columns[j].Visibility == Visibility.Visible && DgvTrackingGroup.Columns[j].Header.ToString() != "ID")
                            {
                                string value = row[j].ToString();

                                // Check for '-' symbol or empty cell
                                if (value == "-" || string.IsNullOrEmpty(value))
                                {
                                    // Leave the cell empty
                                    excelWorksheet.Cells[i + 3, columnNumber + 1] = "";
                                }
                                else
                                {
                                    // Check for '+' symbol
                                    if (value == "+")
                                    {
                                        // Replace the '+' with an empty string
                                        value = "";

                                        // Set the cell background color to green
                                        Microsoft.Office.Interop.Excel.Range cell = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Cells[i + 3, columnNumber + 1];
                                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(182, 227, 136));
                                    }
                                    else if (value.Contains("+-") || value.Contains("-+"))
                                    {
                                        // Remove the '+' symbol and replace with '-'
                                        value = value.Replace("+", "-");

                                        // Set the cell background color to green
                                        Microsoft.Office.Interop.Excel.Range cell = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Cells[i + 3, columnNumber + 1];
                                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(182, 227, 136));
                                    }

                                    // Write the cell value to the worksheet
                                    excelWorksheet.Cells[i + 3, columnNumber + 1] = value;
                                }

                                columnNumber++;
                            }
                        }
                    }
                }

                // Autofit all columns
                excelWorksheet.Cells.EntireColumn.AutoFit();

                // Set word wrap for the "ФИО" column
                int fioColumnIndex = -1;
                for (int i = 0; i < DgvTrackingGroup.Columns.Count; i++)
                {
                    if (DgvTrackingGroup.Columns[i].Header.ToString() == "ФИО")
                    {
                        fioColumnIndex = i;
                        break;
                    }
                }
                if (fioColumnIndex >= 0)
                {
                    Microsoft.Office.Interop.Excel.Range fioColumn = (Microsoft.Office.Interop.Excel.Range)excelWorksheet.Columns[fioColumnIndex + 1];
                    fioColumn.WrapText = true;
                }

                // Set the page orientation to landscape
                excelWorksheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;


                // Get the range of the cells containing data in the worksheet
                int lastRow = DgvTrackingGroup.Items.Count + 2;
                int lastColumn = columnCount;
                Microsoft.Office.Interop.Excel.Range dataRange = excelWorksheet.Range[excelWorksheet.Cells[2, 1], excelWorksheet.Cells[lastRow, lastColumn]];

                // Add a border to the data range
                dataRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                dataRange.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;

                // Save the workbook and open it in Excel
                excelWorkbook.SaveAs(filePath);
                excelWorkbook.Close();
                excelApp.Quit();
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте данных в Excel: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButExportExcel_Click(object sender, RoutedEventArgs e)
        {
            GenerateToExcel();
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
                CountRowsDgv();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButBack_Click(object sender, RoutedEventArgs e)
        {
            var ReportTeacher = new Report();
            System.Windows.Application.Current.MainWindow = ReportTeacher;
            this.Hide();
            ReportTeacher.Show();
        }

        private void CbCource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CbCource.SelectedItem.ToString() == "")
                {
                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = @"SELECT
                                student.id_student AS 'ID',
                                student.student_fio AS 'ФИО',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР01', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР01',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР02', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР02',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР03', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР03',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР04', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР04',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР05', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР05',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР06', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР06',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР07', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР07',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР08', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР08',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР09', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР09',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР10', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР10',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР11', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР11',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР12', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР12',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР13', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР13',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР14', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР14',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР15', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР15',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР16', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР16',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР17', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР17',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР18', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР18',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР19', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР19',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР20', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР20',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР21', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР21',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР22', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР22',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР23', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР23',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР24', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР24',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР25', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР25',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР26', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР26',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР27', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР27',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР28', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР28',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР29', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР29',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР30', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР30'
                                FROM
                                student
                                LEFT JOIN event ON event.event_students = student.id_student
                                LEFT JOIN list_result ON event.event_code_lr = list_result.id_list_result
                                GROUP BY
                                student.id_student
                                ORDER BY
                                student.id_student";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("student");
                        dataAdapter.Fill(dataTable);

                        DgvTrackingGroup.Columns.Clear();
                        var idColumn = new DataGridTextColumn() { Header = "ID", Binding = new Binding("ID"), Visibility = Visibility.Hidden };
                        var fioColumn = new DataGridTextColumn() { Header = "ФИО", Binding = new Binding("ФИО") };
                        DgvTrackingGroup.Columns.Add(idColumn);
                        DgvTrackingGroup.Columns.Add(fioColumn);
                        for (int i = 1; i <= 30; i++)
                        {
                            string lrColumnName = "ЛР" + i.ToString("D2");
                            var lrColumn = new DataGridTextColumn() { Header = lrColumnName, Binding = new Binding(lrColumnName) };
                            DgvTrackingGroup.Columns.Add(lrColumn);
                        }

                        DgvTrackingGroup.ItemsSource = dataTable.DefaultView;
                        CountRowsDgv();
                    }
                }

                else
                {
                    using (DB_Connect connection = new DB_Connect())
                    {
                        connection.OpenConnect();

                        string sql = @"SELECT
                                student.id_student AS 'ID',
                                student.student_fio AS 'ФИО',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР01', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР01',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР02', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР02',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР03', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР03',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР04', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР04',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР05', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР05',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР06', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР06',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР07', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР07',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР08', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР08',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР09', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР09',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР10', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР10',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР11', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР11',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР12', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР12',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР13', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР13',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР14', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР14',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР15', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР15',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР16', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР16',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР17', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР17',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР18', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР18',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР19', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР19',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР20', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР20',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР21', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР21',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР22', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР22',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР23', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР23',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР24', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР24',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР25', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР25',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР26', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР26',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР27', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР27',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР28', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР28',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР29', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР29',
                                GROUP_CONCAT(DISTINCT IFNULL(IF(list_result.list_result_code = 'ЛР30', '+', NULL), '') ORDER BY event.event_code_lr SEPARATOR '') AS 'ЛР30'
                                FROM
                                student
                                LEFT JOIN event ON event.event_students = student.id_student
                                LEFT JOIN list_result ON event.event_code_lr = list_result.id_list_result
                                WHERE `event`.event_cource_score = " + CbCource.SelectedItem.ToString() + " GROUP BY student.id_student";

                        MySqlCommand com = new MySqlCommand(sql, connection.GetConnect());
                        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(com);
                        System.Data.DataTable dataTable = new System.Data.DataTable("student");
                        dataAdapter.Fill(dataTable);

                        DgvTrackingGroup.Columns.Clear();
                        var idColumn = new DataGridTextColumn() { Header = "ID", Binding = new Binding("ID"), Visibility = Visibility.Hidden };
                        var fioColumn = new DataGridTextColumn() { Header = "ФИО", Binding = new Binding("ФИО") };
                        DgvTrackingGroup.Columns.Add(idColumn);
                        DgvTrackingGroup.Columns.Add(fioColumn);
                        for (int i = 1; i <= 30; i++)
                        {
                            string lrColumnName = "ЛР" + i.ToString("D2");
                            var lrColumn = new DataGridTextColumn() { Header = lrColumnName, Binding = new Binding(lrColumnName) };
                            DgvTrackingGroup.Columns.Add(lrColumn);
                        }

                        DgvTrackingGroup.ItemsSource = dataTable.DefaultView;

                        CountRowsDgv();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
