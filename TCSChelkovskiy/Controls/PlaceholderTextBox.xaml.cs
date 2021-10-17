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
                if (Text.Length == 0)
                {
                    PlaceholderVisibility = false;
                }
                else
                {
                  
                    PlaceholderVisibility = true;
                }
            }
        }
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(PlaceholderTextBox));
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVisibilityProperty = DependencyProperty.Register("PlaceholderVisibility", typeof(bool), typeof(PlaceholderTextBox));
        public bool PlaceholderVisibility
        {
            get { return (bool)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVerticalAlligmentProperty = DependencyProperty.Register("PlaceholderVerticalAlligment", typeof(VerticalAlignment), typeof(PlaceholderTextBox),
            new PropertyMetadata(new Thickness(0,0,0,0)));
        public VerticalAlignment PlaceholderVerticalAlligment
        {
            get { return (VerticalAlignment)GetValue(PlaceholderVerticalAlligmentProperty); }
            set { SetValue(PlaceholderVerticalAlligmentProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderMarginProperty = DependencyProperty.Register("PlaceholderMargin", typeof(VerticalAlignment), typeof(Thickness));
        public Thickness PlaceholderMargin
        {
            get { return (Thickness)GetValue(PlaceholderMarginProperty); }
            set { SetValue(PlaceholderMarginProperty, value); }
        }
    }
}
