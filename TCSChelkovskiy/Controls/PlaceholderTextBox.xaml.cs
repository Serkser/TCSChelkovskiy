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

namespace TCSChelkovskiy.Controls
{
    /// <summary>
    /// Логика взаимодействия для PlaceholderTextBox.xaml
    /// </summary>
    public partial class PlaceholderTextBox : UserControl
    {
        public PlaceholderTextBox()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PlaceholderTextBox));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                if (string.IsNullOrEmpty(Text))
                {
                    PlaceholderVisibility = Visibility.Visible;
                }
                else
                {
                    PlaceholderVisibility = Visibility.Hidden;
                }
            }
        }
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox));
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVisibilityProperty = DependencyProperty.Register("PlaceholderVisibility", typeof(Visibility), typeof(PlaceholderTextBox));
        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVerticalAlligmentProperty = DependencyProperty.Register("PlaceholderVerticalAlligment", typeof(VerticalAlignment), typeof(PlaceholderTextBox));
        public VerticalAlignment PlaceholderVerticalAlligment
        {
            get { return (VerticalAlignment)GetValue(PlaceholderVerticalAlligmentProperty); }
            set { SetValue(PlaceholderVerticalAlligmentProperty, value); }
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            if (PlaceholderVerticalAlligment == VerticalAlignment.Top)
            {

                placeholder.VerticalAlignment = VerticalAlignment.Stretch;
            }
            else
            {
                placeholder.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void textchanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            Text = tb.Text;
            if (string.IsNullOrEmpty(Text))
            {
                PlaceholderVisibility = Visibility.Visible;
            }
            else
            {
                PlaceholderVisibility = Visibility.Hidden;
            }
        }

        private void gotfocus(object sender, RoutedEventArgs e)
        {
            this.OnGotFocus(e);
        }
    }
}

