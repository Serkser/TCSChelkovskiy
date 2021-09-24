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
            Bitmap bitmap = Services.ImageDownloader.DownloadImage(CategoryModel.IconURI, CategoryModel.Icon);
            image.Source = Services.BitmapToImageSourceConverter.BitmapToImageSource(bitmap);
            bitmap.Dispose();
            title.Text = CategoryModel.Name;
        }
        static CategoryItemTemplate()
        {
            CategoryModelProperty = DependencyProperty.Register("CateroryModel", typeof(CategoryModel), typeof(CategoryItemTemplate));
        }
        public static readonly DependencyProperty CategoryModelProperty;
        public CategoryModel CategoryModel
        {
            get { return (CategoryModel)GetValue(CategoryModelProperty); }
            set { SetValue(CategoryModelProperty, value); }
        }

    }
}
