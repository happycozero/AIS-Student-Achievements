using Student_Achievements.Classes;
using Student_Achievements.Forms;
using Student_Achievements.Forms.Main;
using System;
using System.Collections.Generic;
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

namespace Student_Achievements
{
    /// <summary>
    /// Interaction logic for MenuTeacher.xaml
    /// </summary>
    public partial class MenuTeacher : Window
    {
        public MenuTeacher()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        public void MenuStudent(string userRole)
        {
            InitializeComponent();
            CheckUser.UserRole = userRole;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DB_Connect connection = new DB_Connect();
            tbFIO.Text = connection.Fill_FIO();
            tbAccess.Text = connection.Fill_Access();
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButChangeUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из учетной записи?", "Выход из учетной записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SwitchWindow(new Authorization());
            }
        }

        private void ButReport_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindow(new Report());
        }

        private void ButGroup_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindow(new Group());
        }

        private void ButStudent_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindow(new Student());
        }

        private void ButCource_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindow(new Courсe());
        }

        private void ButEvent_Click(object sender, RoutedEventArgs e)
        {
            SwitchWindow(new Event());
        }

        // Метод для переключения окон
        private void SwitchWindow(Window newWindow)
        {
            Application.Current.MainWindow = newWindow;
            this.Hide();
            newWindow.Show();
        }
    }
}
