using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Windows.Threading;

namespace SMTP_Email_OTP_Registration
{
    /// <summary>
    /// Interaction logic for OTPWindow.xaml
    /// </summary>
    public partial class OTPWindow : Window
    {
        public int? OTP = 0;
        public int Attempts = 0;
        private string SenderEmail = "ADD YOUR OWN";
        private string Password = "SOME RANDOM TEXT";
        public string UserName = string.Empty;
        public string ReceiverEmail = string.Empty;
        private string UserPassword = string.Empty;
        public int DefaultTimer = 60;
        private DispatcherTimer TimeLimit;

        public OTPWindow(string user, string pass, string email)
        {
            InitializeComponent();
            UserName = user;
            UserPassword = pass;
            ReceiverEmail = email;
            InitializeOTP();
        }
        public void SetDefault()
        {
            textbox_inputOTP.IsEnabled = true;
            DefaultTimer = 60;
            textbox_inputOTP.Clear();
            Attempts = 0;
        }
        public void InitializeOTP()
        {

            SetDefault();
            OTP = randomOTP();
            sendOTP();
            TimeLimit = new DispatcherTimer();
            TimeLimit.Tick += DispatcherTimer_Tick;
            TimeLimit.Interval = TimeSpan.FromMilliseconds(1000); // Set the interval (1 second in this case)
            TimeLimit.Start(); // Start the timer
        }

        public int randomOTP()
        {
            int OTP = 0;
            Random rnd = new Random();
            OTP = rnd.Next(100001, 999999);
            return OTP;
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            DefaultTimer--;
            label_counter.Content = DefaultTimer.ToString();
            if (DefaultTimer <= 0)
            {
                textbox_inputOTP.IsEnabled = false;
                OTP = null;
                TimeLimit.Stop();
            }
        }

        public void sendOTP()
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(SenderEmail, Password),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("kurtfrancisgame5@gmail.com"),
                Subject = "Email Verification",
                Body = "Here is your OTP:" + OTP,
                IsBodyHtml = true,

            };
            mailMessage.To.Add(ReceiverEmail);
            smtpClient.Send(mailMessage);
        }

        private void button_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (Attempts >= 3)
            {
                MessageBox.Show("You Have reached the maximum attempts\nPlease Send a new OTP");
                SetDefault();
                return;
            }
            string UserInput = textbox_inputOTP.Text;
            if (UserInput == OTP.ToString())
            {
                VerifyUser();
                MessageBox.Show("Your Account has been verified, \nMake sure to memorize your Password",
                   "Account Verify", MessageBoxButton.OK, MessageBoxImage.Information);
                TimeLimit.Stop();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong OTP Input");
                Attempts++;
            }
        }
        private void VerifyUser()
        {
            string filePath = "UserAccounts.txt";
            string searchUsername = UserName;
            List<string> linesToKeep = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 1 && parts[0] == searchUsername)
                    {
                        //continue;
                    }
                    else
                    {
                        linesToKeep.Add(line);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter("UserAccounts.txt"))
            {

                foreach (string line in linesToKeep)
                {
                    writer.WriteLine(line);
                }
            }
            string VerifyLine = UserName + "," + ReceiverEmail + "," + UserPassword + "," + "Verified";
            using (StreamWriter sw = new StreamWriter("UserAccounts.txt", true))
            {
                sw.WriteLine(VerifyLine);
            }
        }

        private void button_resendOTP_Click(object sender, RoutedEventArgs e)
        {
            InitializeOTP();
            MessageBox.Show("OTP Send");
        }
    }
}
