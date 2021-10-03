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
using TCSChelkovskiy.Services;
using TCSChelkovskiy.Utilities;
using TCSchelkovskiyAPI.Models;
using Path = System.IO.Path;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        public ShopPage(ShopModel modelShop)
        {
            InitializeComponent();
            Model = modelShop;
            Loaded+= OnLoaded;
            Unloaded+= OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            foreach (var disposableImage in ImagesShop)
            {
                disposableImage.Dispose();
            }
            Logo.Dispose();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if(Model.Photos!=null)
            foreach (PhotoModel modelPhoto in Model.Photos)
            {
                var disposableImage = await ImageDownloader.DownloadImage(modelPhoto.ImageURI, modelPhoto.Image);
                ImagesShop.Add(disposableImage);
            }

            Logo = await ImageDownloader.DownloadImage(Model.IconURI, Path.GetFileName(Model.IconURI));
        }

        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(
            "Logo", typeof(DisposableImage), typeof(ShopPage), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage Logo
        {
            get => (DisposableImage) GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }

        public static readonly DependencyProperty ImagesShopProperty = DependencyProperty.Register(
            "ImagesShop", typeof(ObservableCollection<DisposableImage>), typeof(ShopPage), new PropertyMetadata(new ObservableCollection<DisposableImage>()));

        public ObservableCollection<DisposableImage> ImagesShop
        {
            get => (ObservableCollection<DisposableImage>) GetValue(ImagesShopProperty);
            set => SetValue(ImagesShopProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
            "Model", typeof(ShopModel), typeof(ShopPage), new PropertyMetadata(default(ShopModel)));

        public ShopModel Model
        {
            get => (ShopModel) GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }
    }
}
