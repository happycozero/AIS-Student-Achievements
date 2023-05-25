using Student_Achievements.Classes;
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
    /// Interaction logic for MenuStudent.xaml
    /// </summary>
    public partial class MenuStudent : Window
    {
        public MenuStudent()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButChangeUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из учетной записи?", "Выход из учетной записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var AuthForm = new Authorization();
                Application.Current.MainWindow = AuthForm;
                this.Hide();
                AuthForm.Show();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var EventForm = new Event();
            Application.Current.MainWindow = EventForm;
            this.Hide();
            EventForm.Show();
        }

        private void ButGroup_Click(object sender, RoutedEventArgs e)
        {
            var GroupForm = new Group();
            Application.Current.MainWindow = GroupForm;
            this.Hide();
            GroupForm.Show();
        }

        private void ButStudent_Click(object sender, RoutedEventArgs e)
        {
            var StudentForm = new Student();
            Application.Current.MainWindow = StudentForm;
            this.Hide();
            StudentForm.Show();
        }

        private void ButCource_Click(object sender, RoutedEventArgs e)
        {
            var CourceForm = new Course();
            Application.Current.MainWindow = CourceForm;
            this.Hide();
            CourceForm.Show();
        }
    }
}
