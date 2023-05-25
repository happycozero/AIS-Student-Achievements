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
    /// Interaction logic for Captch.xaml
    /// </summary>
    public partial class Captch : Window
    {
        bool flag = false; 

        public Captch()
        {
            InitializeComponent();
            Loaded += Captch_Loaded;
            WindowHelper.InitializeSource(this);
        }

        private void Captch_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RemoveSysMenu(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            GenCaptch();
        }

        public void GenCaptch()
        {
            string text = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789";
            string captch = "";
            int len = text.Length;

            Random rnd = new Random();

            for (int i = 0; i < 6; i++)
            {
                captch += text[rnd.Next(1, len)];
            }

            label_captch.Content = captch;
        }

        private void CaptchForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (flag == true)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void ButRefresh_Click(object sender, RoutedEventArgs e)
        {
            tb_captch.Clear();
            GenCaptch();
        }

        private async void ButInputCaptch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tb_captch.Text))
            {
                MessageBox.Show("Ошибка. Введите капчу!", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ButInputCaptch.IsEnabled = false;
                ButRefresh.IsEnabled = false;

                await Task.Delay(10000);

                ButInputCaptch.IsEnabled = true;
                ButRefresh.IsEnabled = true;
            }
            else
            {
                if (label_captch.Content.ToString() == tb_captch.Text)
                {
                    tb_captch.Clear();
                    flag = true;
                    this.Close();
                }

                else
                {
                    MessageBox.Show("Ошибка. Капча неверная! Повторите попытку через 10 секунд.", "Возникла ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    ButInputCaptch.IsEnabled = false;
                    ButRefresh.IsEnabled = false;

                    await Task.Delay(10000);

                    ButInputCaptch.IsEnabled = true;
                    ButRefresh.IsEnabled = true;

                    GenCaptch();
                    tb_captch.Clear();
                }
            }
        }
    }
}
