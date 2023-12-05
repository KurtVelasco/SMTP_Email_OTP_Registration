using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMTP_Email_OTP_Registration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Username = String.Empty;
        public string Password = String.Empty;
        public string Email = String.Empty;
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool CheckValidity()
        {
            if (textbox_username.Text.Length < 8)
            {
                MessageBox.Show("Username must be at least 8 characters long");
                return false;
            }

            if (textbox_password.Password.Length < 5)
            {
                MessageBox.Show("Password must be at least 5 characters long");
                return false;
            }

            if (string.IsNullOrEmpty(textbox_email.Text))
            {
                MessageBox.Show("Please enter a valid Email Address");
                return false;
            }

            return true;
        }
        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidity())
            {
                Username = textbox_username.Text;
                Password = textbox_password.Password;
                Email = textbox_email.Text;
                WriteFile();
                OTPWindow OTP = new OTPWindow(Username, Password, Email);
                OTP.Show();
            }
        }
        private void WriteFile()
        {
            List<string> linesToKeep = new List<string>();
            string line = Username + "," + Password + "," + Email + "," + "NonVerified";
            using (StreamWriter sw = new StreamWriter("UserAccounts.txt", true))
            {
                sw.WriteLine(line);
            }

        }
    }
}
