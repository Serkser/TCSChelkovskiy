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
using TCSChelkovskiy.Services;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для SalePage.xaml
    /// </summary>
    public partial class NewsItemsPage : Page
    {
        public NewsItemsPage(PromoModel promoModel)
        {
            InitializeComponent();
            Model = promoModel;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Logo?.Dispose();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Logo = await ImageDownloader.DownloadImage(Model.Shop.IconURI, Model.Shop.Icon);
        }

        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(
            "Logo", typeof(DisposableImage), typeof(ShopPage), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage Logo
        {
            get => (DisposableImage)GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
       "Model", typeof(PromoModel), typeof(NewsItemsPage), new PropertyMetadata(default(PromoModel)));

        public PromoModel Model
        {
            get => (PromoModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }
    }
}
