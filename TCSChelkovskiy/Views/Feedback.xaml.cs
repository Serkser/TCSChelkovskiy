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

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Feedback.xaml
    /// </summary>
    public partial class Feedback : Page
    {
        public Feedback()
        {
            InitializeComponent();
        }

        TextBox selectedTextBox;
        private void buttonPressed(object sender, EventArgs e)
        {
            if (selectedTextBox != null)
            {
                TCSChelkovskiy.Controls.KeyboardEventArgs args = e as TCSChelkovskiy.Controls.KeyboardEventArgs;
                switch (args.CurrentKey)
                {
                    case "Lang":
                    
                        break;
                    case "Backspace":                    
                        if (selectedTextBox.Text.Length > 0)
                        {
                            selectedTextBox.Text = selectedTextBox.Text.Remove(selectedTextBox.Text.Length - 1);
                        }
                        break;
                    case "Shift":
                     
                        break;
                    default:
                        selectedTextBox.Text += args.CurrentKey;
                        break;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            selectedTextBox = sender as TextBox;
        }
    }
}
