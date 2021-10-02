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
    /// Логика взаимодействия для CategoryItemTemplate.xaml
    /// </summary>
    public partial class CategoryItemTemplate : UserControl
    {
        
        public CategoryItemTemplate()
        {
            InitializeComponent();
            
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(CategoryItemTemplate));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty IconURIProperty = DependencyProperty.Register("IconURI", typeof(string), typeof(CategoryItemTemplate));
        public string IconURI
        {
            get { return (string)GetValue(IconURIProperty); }
            set { SetValue(IconURIProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(CategoryItemTemplate));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = Services.ImageDownloader.DownloadImage(IconURI, Icon);
            image.Source = Services.BitmapToImageSourceConverter.BitmapToImageSource(bitmap, System.IO.Path.Combine(Environment.CurrentDirectory, Icon));
            bitmap.Dispose();
            title.Text = Title;
        }
    }
}
