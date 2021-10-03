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
using TCEvropeyskiy.ViewModels;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductsView.xaml
    /// </summary>
    public partial class ShopsView : Page
    {
        public ShopsView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AllShopsProperty = DependencyProperty.Register(
            "AllShops", typeof(ObservableCollection<ShopModel>), typeof(ShopsView), new PropertyMetadata(default(ObservableCollection<ShopModel>)));

        public ObservableCollection<ShopModel> AllShops
        {
            get => (ObservableCollection<ShopModel>) GetValue(AllShopsProperty);
            set => SetValue(AllShopsProperty, value);
        }

        private ICommand _goShopPage;

        public ICommand GoShopPage => _goShopPage ??= new RelayCommand(f =>
        {
            var shop = f as ShopModel;
            NavigationService?.Navigate(new ShopPage(shop));
        });

    }
}
