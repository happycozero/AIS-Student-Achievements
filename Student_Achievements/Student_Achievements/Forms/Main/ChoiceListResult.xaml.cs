﻿using Student_Achievements.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Student_Achievements.Forms.Main
{
    /// <summary>
    /// Interaction logic for ChoiceListResult.xaml
    /// </summary>
    public partial class ChoiceListResult : Window
    {
        public List<string> SelectedLRS { get; set; }

        public static string[] LRS = new string[30];
        public static int count = 0;

        public string description_lr;

        public ChoiceListResult(List<string> lrs)
        {
            InitializeComponent();

            // создаем таблицу
            DataTable dt = new DataTable();

            // добавляем столбец Код ЛР в таблицу (если он не был создан ранее в XAML)
            if (!dt.Columns.Contains("Код ЛР"))
            {
                dt.Columns.Add("Код ЛР");
            }
            // добавляем столбец Описание в таблицу (если он не был создан ранее в XAML)
            if (!dt.Columns.Contains("Описание"))
            {
                dt.Columns.Add("Описание");
            }

            // заполняем таблицу данными из списка
            foreach (string lr in lrs)
            {
                DataRow dr = dt.NewRow();
                dr["Код ЛР"] = lr;
                dr["Описание"] = description_lr; // Добавьте это
                dt.Rows.Add(dr);
            }

            // устанавливаем источник данных для DataGrid
            DgvChoiceStudent.ItemsSource = dt.DefaultView;
        }

        private void ButClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем окно
            DialogResult = false;
            count = 0;
        }

        public void AddStudent()
        {
            DataRowView row_selected = DgvChoiceStudent.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                string lr = row_selected["Код ЛР"].ToString();
                LRS[count++] = lr; // Добавляем ЛР в массив и увеличиваем счетчик на 1
                labelCountStudent.Content = count;
            }
        }

        private void DgvChoiceStudent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)DgvChoiceStudent.ItemContainerGenerator.ContainerFromItem(DgvChoiceStudent.SelectedItem);

            if (row.Background.ToString() == "#B6E388")
            {
                row.Background = Brushes.White;
            }
            else
            {
                row.Background = new SolidColorBrush(Color.FromRgb(182, 227, 136));
                AddStudent();
            }
        }

        private void DgvChoiceStudent_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right && e.ClickCount == 1)
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    // Изменяем цвет выбранной строки на зеленый
                    row_selected.Row.ItemArray[0] = new SolidColorBrush(Color.FromRgb(182, 227, 136));

                    DgvChoiceStudent.Items.Refresh();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }
    }
}
