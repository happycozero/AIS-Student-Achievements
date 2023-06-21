using Student_Achievements.Classes;
using Student_Achievements.Forms.Administrator.Guides;
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

namespace Student_Achievements.Forms
{
    /// <summary>
    /// Interaction logic for Guides.xaml
    /// </summary>
    public partial class Guide : Window
    {
        public Guide()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
        }

        private void ButMenu_Click(object sender, RoutedEventArgs e)
        {
            var MenuForm = new MenuAdministrator();
            Application.Current.MainWindow = MenuForm;
            this.Hide();
            MenuForm.Show();
        }

        private void ButLevelEvent_Click(object sender, RoutedEventArgs e)
        {
            var LevelEventForm = new LevelEvent();
            Application.Current.MainWindow = LevelEventForm;
            this.Hide();
            LevelEventForm.Show();
        }

        private void ButEmp_Click(object sender, RoutedEventArgs e)
        {
            var EmployerForm = new Employer();
            Application.Current.MainWindow = EmployerForm;
            this.Hide();
            EmployerForm.Show();
        }

        private void ButListLR_Click(object sender, RoutedEventArgs e)
        {
            var ListResultForm = new ListResult();
            Application.Current.MainWindow = ListResultForm;
            this.Hide();
            ListResultForm.Show();
        }

        private void ButSpecialty_Click(object sender, RoutedEventArgs e)
        {
            var SpecializationForm = new Specialization();
            Application.Current.MainWindow = SpecializationForm;
            this.Hide();
            SpecializationForm.Show();
        }

        private void ButPrizePlace_Click(object sender, RoutedEventArgs e)
        {
            var PrizePlaceForm = new PrizePlace();
            Application.Current.MainWindow = PrizePlaceForm;
            this.Hide();
            PrizePlaceForm.Show();
        }
    }
}
