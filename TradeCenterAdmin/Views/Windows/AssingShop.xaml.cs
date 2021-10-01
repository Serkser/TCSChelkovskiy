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

namespace TradeCenterAdmin.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AssingShop.xaml
    /// </summary>
    public partial class AssingShop : Window
    {
        public AssingShop()
        {
            InitializeComponent();
        }

        private void select(object sender, RoutedEventArgs e)
        {
            if(lists.SelectedItem != null)
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Выберите магазин для назначения");
            }
        }
    }
}
