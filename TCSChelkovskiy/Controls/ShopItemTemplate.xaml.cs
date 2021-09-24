using System;
using System.Collections.Generic;
using System.Drawing;
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
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Controls
{
    /// <summary>
    /// Логика взаимодействия для ShopItemTemplate.xaml
    /// </summary>
    public partial class ShopItemTemplate : UserControl
    {
        public ShopItemTemplate()
        {
            InitializeComponent();
            Bitmap bitmap = Services.ImageDownloader.DownloadImage(ShopModel.IconURI, ShopModel.Icon);
            img.Source = Services.BitmapToImageSourceConverter.BitmapToImageSource(bitmap);
            bitmap.Dispose();
            title.Text = ShopModel.Name;
            category.Text = ShopModel.Category.Name;
        }
        static ShopItemTemplate()
        {
            ShopModelProperty = DependencyProperty.Register("ShopModel", typeof(ShopModel), typeof(ShopItemTemplate));
        }
        public static readonly DependencyProperty ShopModelProperty;
        public ShopModel ShopModel
        {
            get { return (ShopModel)GetValue(ShopModelProperty); }
            set { SetValue(ShopModelProperty, value); }
        }
    }
}
