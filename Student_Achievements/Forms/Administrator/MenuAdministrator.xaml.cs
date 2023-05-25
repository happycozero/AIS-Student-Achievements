using Student_Achievements.Classes;
using Student_Achievements.Forms;
using Student_Achievements.Forms.Administrator.Employers;
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
    /// Interaction logic for MenuAdministrator.xaml
    /// </summary>
    public partial class MenuAdministrator : Window
    {
        public MenuAdministrator()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButGuide_Click(object sender, RoutedEventArgs e)
        {
            var GuideForm = new Guide();
            Application.Current.MainWindow = GuideForm;
            this.Hide();
            GuideForm.Show();
        }

        private void ButEmp_Click(object sender, RoutedEventArgs e)
        {
            var UserForm = new User();
            Application.Current.MainWindow = UserForm;
            this.Hide();
            UserForm.Show();
        }

        private void ButSpecFeat_Click(object sender, RoutedEventArgs e)
        {
            var SpecialFeatureForm = new SpecialFeature();
            Application.Current.MainWindow = SpecialFeatureForm;
            this.Hide();
            SpecialFeatureForm.Show();
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
    }
}
