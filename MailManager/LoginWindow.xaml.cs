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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Reflection;
using wpfTask1;
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        public EmailUser LoggedUser { get; set; }

        private int passwordLength;
        public int PasswordLength
        {
            get { return passwordLength; }
            set
            {
                passwordLength = value;
                OnPropertyChanged(nameof(PasswordLength));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Window1()
        {
            InitializeComponent();
            passwordTextbox.PasswordChanged += SetPasswordLength;
            DataContext = this;
        }

        void SetPasswordLength(object sen, RoutedEventArgs a)
        {
            PasswordLength = passwordTextbox.Password.Length;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (EmailData.GetUserData(usernameTextbox.Text, passwordTextbox.Password, out EmailUser loggedUser))
            {
                DialogResult = true;
                LoggedUser = loggedUser;
                var messages = loggedUser.MessagesReceived;
                Close();
            }
            else
            {
                var app = (App)Application.Current;
                MessageBox.Show((string)app.ThemeDictionary["incorrectLogin"]);
            }
        }
    }
}
