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
using TCEvropeyskiy.ViewModels;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutTradeCenter.xaml
    /// </summary>
    public partial class AboutTradeCenter : Page
    {

        public AboutTradeCenter()
        {
            InitializeComponent();         
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            string url = "https://navigator.useful.su";
            string prefix = ((MainWindowViewModel)this.DataContext).AboutMall.ImagesPrefix;
            var images = ((MainWindowViewModel)this.DataContext).AboutMall.Images;
            List<BitmapImage> bitmaps = new List<BitmapImage>(20);
            foreach (var i in images)
            {
                var bitmap = Services.ImageDownloader.DownloadImage($"{prefix}{i}", i);
                var bitmapImage = Services.BitmapToImageSourceConverter.BitmapToImageSource(bitmap, System.IO.Path.Combine(Environment.CurrentDirectory, i));
                bitmaps.Add(bitmapImage);
            }
            try
            {
                image1.Source = bitmaps[0];
                image2.Source = bitmaps[1];
            }
            catch { }
          
        }
    }
}
