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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCSChelkovskiy.Memory;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Categories.xaml
    /// </summary>
    public partial class Categories : Page
    {
        public Categories()
        {
            InitializeComponent();
        }

        private ICommand _goToShopsCommand;
        public ICommand GoToShopsCommand => _goToShopsCommand ??= new RelayCommand(f =>
        {
            var category = f as CategoryModel;
            List<ShopModel> shops = new List<ShopModel>();
            foreach (var shop in KioskObjects.Shops)
            {
                foreach(var cat in shop.Categories)
                {
                    if (cat.ID == category.ID)
                    {
                        shops.Add(shop);
                    }
                }
            }
            var ShopsByCategory = new ObservableCollection<ShopModel>(shops);
           NavigationService?.Navigate(new ShopsView(){AllShops = ShopsByCategory});
        });
    }
}
