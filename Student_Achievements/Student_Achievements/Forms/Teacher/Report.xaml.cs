using Student_Achievements.Classes;
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
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Report : Window
    {
        public Report()
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
            var MenuTeacherForm = new MenuTeacher();
            Application.Current.MainWindow = MenuTeacherForm;
            this.Hide();
            MenuTeacherForm.Show();
        }

        private void ButTrackingGroup_Click(object sender, RoutedEventArgs e)
        {
            var TrackingGroupForm = new TrackingGroup();
            Application.Current.MainWindow = TrackingGroupForm;
            this.Hide();
            TrackingGroupForm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var RecordEventForm = new RecordEvent();
            Application.Current.MainWindow = RecordEventForm;
            this.Hide();
            RecordEventForm.Show();
        }
    }
}
