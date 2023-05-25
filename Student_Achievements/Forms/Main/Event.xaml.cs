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
    /// Interaction logic for Event.xaml
    /// </summary>
    public partial class Event : Window
    {
        public Event()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var ChoiceStudentWindow = new ChoiceStudent();
            ChoiceStudentWindow.Owner = this;
            ChoiceStudentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? result = ChoiceStudentWindow.ShowDialog();
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
    }
}
