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

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string text,string caption)
        {
            InitializeComponent();
            Text = text;
            Caption = caption;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text", typeof(string), typeof(CustomMessageBox), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
          "Caption", typeof(string), typeof(CustomMessageBox), new PropertyMetadata(default(string)));

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        private RelayCommand close;
        public RelayCommand CloseCommand => close ??= new RelayCommand(obj =>
        {
             this.DialogResult = true;
        });
    }
}
