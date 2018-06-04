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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using wpfTask1;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public EmailUser LoggedUser { get; set; }

        private bool isUserLogged;
        public bool IsUserLogged
        {
            get { return isUserLogged; }
            set
            {
                isUserLogged = value;
                OnPropertyChanged(nameof(IsUserLogged));
            }
        }

        private EmailMessage selectedMessage;
        public EmailMessage SelectedMessage
        {
            get { return selectedMessage; }
            set
            {
                selectedMessage = value;
                OnPropertyChanged(nameof(SelectedMessage));
            }
        }

        private EmailMessage selectedMessageSent;
        public EmailMessage SelectedMessageSent
        {
            get { return selectedMessageSent; }
            set
            {
                selectedMessageSent = value;
                OnPropertyChanged(nameof(SelectedMessageSent));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            IsUserLogged = false;
            this.DataContext = this;
        }

        private EmailMessage lastSelectedMessage;
        private EmailMessage lastSelectedMessageSent;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool UserFilter(object item)
        {
            string text = SearchTextBox.Text;
            if (String.IsNullOrEmpty(text))
                return true;
            else
            {
                string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                EmailMessage msg = (EmailMessage)item;
                foreach (string word in words)
                {
                    string w = word.ToLowerInvariant();
                    if (msg.Date.ToLowerInvariant().Contains(w) || (msg.To?.ToLowerInvariant().Contains(w) ?? false)
                        || (msg.From?.ToLowerInvariant().Contains(w) ?? false) || msg.Title.ToLowerInvariant().Contains(w))
                        return true;
                }
                return false;
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUserLogged)
            {
                Window1 wnd = new Window1();
                wnd.Owner = this;
                this.Opacity = 0.5;

                wnd.ShowDialog();
                if (wnd.DialogResult.HasValue && wnd.DialogResult.Value == true)
                {
                    LoggedUser = wnd.LoggedUser;
                    emailList.ItemsSource = LoggedUser.MessagesReceived;
                    sentEmailList.ItemsSource = LoggedUser.MessagesSent;
                    IsUserLogged = true;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(emailList.ItemsSource);
                    view.Filter = UserFilter;
                    view = (CollectionView)CollectionViewSource.GetDefaultView(sentEmailList.ItemsSource);
                    view.Filter = UserFilter;
                    OnPropertyChanged("LoggedUser");
                }

                this.Opacity = 1;
            }
            else
                LogoutUser();
        }

        private void LogoutUser()
        {
            IsUserLogged = false;
            SelectedMessage = null;
            SelectedMessageSent = null;
            lastSelectedMessage = null;
            lastSelectedMessageSent = null;
            SearchTextBox.Clear();
        }

        public double NewMailButtonHeight { get; set; }
        public double NewMailButtonWidth { get; set; }

        private void newMailButton_Click(object sender, RoutedEventArgs e)
        {
            NewEmailWindow wnd = new NewEmailWindow();
            wnd.Owner = this;
            this.Opacity = 0.5;
            NewMailButtonHeight = 0.8 * ActualHeight;
            NewMailButtonWidth = 0.8 * ActualWidth;

            var width = new Binding(nameof(NewMailButtonWidth)) { Source = this };
            var height = new Binding(nameof(NewMailButtonHeight)) { Source = this };
            wnd.SetBinding(FrameworkElement.WidthProperty, width);
            wnd.SetBinding(FrameworkElement.HeightProperty, height);

            wnd.ShowDialog();
            if (wnd.DialogResult.HasValue && wnd.DialogResult.Value == true)
            {
                var newMessage = new EmailMessage()
                {
                    To = wnd.ToTextBoxString,
                    Title = wnd.TitleTextBoxString,
                    Body = wnd.MessageTextBoxString,
                    Date = DateTime.Now.Date.ToShortDateString(),
                    From = ""
                };
                LoggedUser.MessagesSent.Add(newMessage);
            }

            this.Opacity = 1;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            app.ChangeTheme(new Uri("Assets/en-US.xaml", UriKind.Relative));
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            app.ChangeTheme(new Uri("Assets/pl-PL.xaml", UriKind.Relative));
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedMessage != null)
                lastSelectedMessage = SelectedMessage;
            if (SelectedMessageSent != null)
                lastSelectedMessageSent = SelectedMessageSent;

            CollectionViewSource.GetDefaultView(emailList.ItemsSource)?.Refresh();
            CollectionViewSource.GetDefaultView(sentEmailList.ItemsSource)?.Refresh();

            foreach (var it in CollectionViewSource.GetDefaultView(emailList.ItemsSource))
                if ((EmailMessage)it == lastSelectedMessage)
                {
                    emailList.SelectedItem = lastSelectedMessage; break;
                }

            foreach (var it in CollectionViewSource.GetDefaultView(sentEmailList.ItemsSource))
                if ((EmailMessage)it == lastSelectedMessageSent)
                {
                    sentEmailList.SelectedItem = lastSelectedMessageSent; break;
                }
        }

        //private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    CollectionViewSource.GetDefaultView(emailList.ItemsSource)?.Refresh();
        //    CollectionViewSource.GetDefaultView(sentEmailList.ItemsSource)?.Refresh();
        //}

        private void emailList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                ListView lv = (ListView)sender;
                ((ObservableCollection<EmailMessage>)lv?.ItemsSource)?.Remove((EmailMessage)lv?.SelectedItem);
            }
        }
    }
}
