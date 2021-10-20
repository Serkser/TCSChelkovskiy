using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TCSchelkovskiyAPI.Models;

namespace TradeCenterAdmin.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AssingShop.xaml
    /// </summary>
    public partial class AssingShop : Window
    {
        ObservableCollection<ShopModel> Shops;
        public AssingShop(ObservableCollection<ShopModel> shops,ObservableCollection<Floor> floors)
        {
            InitializeComponent();

            var sortedShops = shops.ToList();
            foreach (var shop in sortedShops)
            {
                foreach (var floor in floors)
                {
                    foreach (var area in floor.Areas)
                    {
                        if (area.Id == shop.ID)
                        {
                            shop.IsUsedOnMap = true;
                        }
                    }
                }
            }    

            sortedShops = sortedShops.OrderBy(o => o.IsUsedOnMap).ThenBy(o => o.Floor).ToList();

            Shops = new ObservableCollection<ShopModel>(sortedShops);

            lists.ItemsSource = Shops;

        }

        private void select(object sender, RoutedEventArgs e)
        {
            if(lists.SelectedItem != null)
            {
                if ((lists.SelectedItem as ShopModel).IsUsedOnMap)
                {
                    MessageBox.Show("Этот магазин уже установлен на карте");
                    return;
                }
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Выберите магазин для назначения");
            }
        }
    }
}
