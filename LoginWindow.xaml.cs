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
using System.Windows.Shapes;

namespace SMTP_Email_OTP_Registration
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public string UserName = String.Empty;
        public string Password = String.Empty;
        public string FilePath = "UserAccounts.txt";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            UserName = textbox_username.Text;
            Password = textbox_password.Password;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1 && parts[0] == UserName && parts[1] == Password)
                    {
                        MessageBox.Show("Login Successful: " + UserName);
                    }
                    else
                    {
                        MessageBox.Show("Wrong Username/Password");
                    }

                }
            }


        }
        public void addLogin()
        {
        }
    }
}
