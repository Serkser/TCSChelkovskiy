using NavigationMap.Models;
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
    /// Логика взаимодействия для SelectNextFloorForWay.xaml
    /// </summary>
    public partial class SelectNextFloorForWay : Window
    {
        public SelectNextFloorForWay()
        {
            InitializeComponent();
        }

        private void selectFloor(object sender, RoutedEventArgs e)
        {
            var currentFloor = ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor;
            Floor selected = (Floor)listbox.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Выберите этаж");
            }
            else if (currentFloor != null && selected.Id == currentFloor.Id)
            {
                MessageBox.Show("Выберите этаж, отличный от предыдущего");
            }
            else
            {
                ((ViewModels.MapEditorViewModel)this.DataContext).SelectedFloor = selected;
                this.DialogResult = true;
            }
        }
    }
}
