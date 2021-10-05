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

namespace TCSChelkovskiy.Controls
{
    /// <summary>
    /// Логика взаимодействия для SaleItemTemplate.xaml
    /// </summary>
    public partial class NewsItemTemplate : UserControl
    {
        public NewsItemTemplate()
        {
            InitializeComponent();
            Unloaded+= OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _disposableImage?.Dispose();
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(NewsItemTemplate));
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty IconURIProperty = DependencyProperty.Register("IconURI", typeof(string), typeof(NewsItemTemplate));
        public string IconURI
        {
            get { return (string)GetValue(IconURIProperty); }
            set { SetValue(IconURIProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(string), typeof(NewsItemTemplate));
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private DisposableImage _disposableImage;
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _disposableImage =await Services.ImageDownloader.DownloadImage(IconURI, Icon);
            image.Source = _disposableImage.Source;
            description.Text = Description;
        }
    }
}
