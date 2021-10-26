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
using TradeCenterAdmin.Models;

namespace TradeCenterAdmin.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TemplateWays.xaml
    /// </summary>
    public partial class TemplateWays : Window
    {
        public TemplateWays()
        {
            InitializeComponent();
        }

        public TemplateWaysContainer WaysContainer { get; set; }
        private void selectFloor(object sender, RoutedEventArgs e)
        {
            var selected = listbox.SelectedItem;
            if (selected != null)
            {
                WaysContainer = selected as TemplateWaysContainer;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Выберите маршрут перехода для завершения пути");
            }
        }
    }
}
