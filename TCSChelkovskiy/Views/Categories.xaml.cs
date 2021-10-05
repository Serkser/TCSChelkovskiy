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
           var ShopsByCategory = new ObservableCollection<ShopModel>(KioskObjects.Shops.Where(o => o.Category.ID == category?.ID).ToList());
           NavigationService?.Navigate(new ShopsView(){AllShops = ShopsByCategory});
        });
    }
}
