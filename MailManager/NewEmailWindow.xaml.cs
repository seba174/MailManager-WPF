using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for NewEmailWindow.xaml
    /// </summary>
    public partial class NewEmailWindow : Window, INotifyPropertyChanged
    {
        private string toString;
        public string ToTextBoxString
        {
            get { return toString; }
            set { toString = value; OnPropertyChanged(nameof(ToTextBoxString)); }
        }
        private string titleString;
        public string TitleTextBoxString
        {
            get { return titleString; }
            set { titleString = value; OnPropertyChanged(nameof(TitleTextBoxString)); }
        }
        private string messageString;
        public string MessageTextBoxString
        {
            get { return messageString; }
            set { messageString = value; OnPropertyChanged(nameof(MessageTextBoxString)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public NewEmailWindow()
        {
            InitializeComponent();
            DataContext = this;
            ToTextBoxString = "";
            TitleTextBoxString = "";
            MessageTextBoxString = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
