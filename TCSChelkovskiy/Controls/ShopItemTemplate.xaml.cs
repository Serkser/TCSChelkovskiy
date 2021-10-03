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
using TCSChelkovskiy.Utilities;
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
            Unloaded+= OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ImageBind.Dispose();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ShopItemTemplate));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register("Category", typeof(string), typeof(ShopItemTemplate));
        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }
        public static readonly DependencyProperty IconURIProperty = DependencyProperty.Register("IconURI", typeof(string), typeof(ShopItemTemplate));
        public string IconURI
        {
            get { return (string)GetValue(IconURIProperty); }
            set { SetValue(IconURIProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(ShopItemTemplate));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty ImageBindProperty = DependencyProperty.Register(
            "ImageBind", typeof(DisposableImage), typeof(ShopItemTemplate), new PropertyMetadata(default(DisposableImage)));

        public DisposableImage ImageBind
        {
            get => (DisposableImage) GetValue(ImageBindProperty);
            set => SetValue(ImageBindProperty, value);
        }

        private async void ShopItemTemplate_OnLoaded(object sender, RoutedEventArgs e)
        {
            ImageBind = await Services.ImageDownloader.DownloadImage(IconURI, Icon);
            category.Text = Category;
            title.Text = Title;
        }
    }
}
