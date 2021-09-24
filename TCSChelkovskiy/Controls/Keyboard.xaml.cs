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

        private bool isUppercase = false;
        private void Click(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            string btnText = button.Content.ToString();
            string symbol = "";
            switch (btnText)
            {
                case "Lang":

                    break;
                case "Backspace":

                    break;
                case "Shift":
                    isUppercase = !isUppercase;
                    break;
                default:
                    if (isUppercase)
                    {
                        symbol = btnText.ToUpper();
                    }
                    else
                    {
                        symbol = btnText.ToLower();
                    }
                    break;

            }
        }
    }
}
