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
    /// Логика взаимодействия для Keyboard.xaml
    /// </summary>
    public partial class Keyboard : UserControl
    {
        public Keyboard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CurrentKeyProperty = DependencyProperty.Register("CurrentKey", typeof(string), typeof(CategoryItemTemplate));
        public string CurrentKey
        {
            get { return (string)GetValue(CurrentKeyProperty); }
            set { SetValue(CurrentKeyProperty, value); }
        }
        public static readonly DependencyProperty CurrentTextProperty = DependencyProperty.Register("CurrentText", typeof(string), typeof(CategoryItemTemplate));
        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            set { SetValue(CurrentTextProperty, value); }
        }

        private bool isUppercase = false;
        private void Click(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            string btnText = button.Content.ToString();
            switch (btnText)
            {
                case "Lang":
                    CurrentKey = "Lang";
                    break;
                case "Backspace":
                    CurrentKey = "Backspace";
                    CurrentText = CurrentText.Remove(CurrentText.Length - 1);
                    break;
                case "Shift":
                    CurrentKey = "Shift";
                    isUppercase = !isUppercase;
                    break;
                default:
                    if (isUppercase)
                    {
                        CurrentKey = btnText.ToUpper();
                    }
                    else
                    {
                        CurrentKey = btnText.ToLower();
                    }
                    CurrentText += CurrentKey;
                    break;

            }
        }

        private void touch(object sender, TouchEventArgs e)
        {
            Click(sender, null);
        }
    }
}
