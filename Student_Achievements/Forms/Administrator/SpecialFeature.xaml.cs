using Student_Achievements.Classes;
using Student_Achievements.Forms.Administrator.SpecialFeatures;
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
    /// Interaction logic for SpecialFeature.xaml
    /// </summary>
    public partial class SpecialFeature
    {
        public SpecialFeature()
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
            var menuAdmin = new MenuAdministrator();
            Application.Current.MainWindow = menuAdmin;
            this.Hide();
            menuAdmin.Show();
        }

        private void ButImport_Click(object sender, RoutedEventArgs e)
        {
            var importForm = new Import();
            Application.Current.MainWindow = importForm;
            this.Hide();
            importForm.Show();
        }

        private void ButExport_Click(object sender, RoutedEventArgs e)
        {
            var exportForm = new Import();
            Application.Current.MainWindow = exportForm;
            this.Hide();
            exportForm.Show();
        }
    }
}
