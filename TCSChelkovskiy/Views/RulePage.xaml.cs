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

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для RulePage.xaml
    /// </summary>
    public partial class RulePage : Page
    {
        public RulePage(RuleModel rule)
        {
            InitializeComponent();
            Model = rule;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            foreach (var disposableImage in Images)
            {
                disposableImage?.Dispose();
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Model.Images != null)
            {
                foreach (var imgUri in Model.Images)
                {
                    var disposableImage = await ImageDownloader.DownloadImage(Model.ImagesPrefix+ imgUri, imgUri);
                    Images.Add(disposableImage);
                }
            }
               

        }
        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
       "Model", typeof(RuleModel), typeof(RulePage), new PropertyMetadata(default(RuleModel)));

        public RuleModel Model
        {
            get => (RuleModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }
        public static readonly DependencyProperty ImagesProperty = DependencyProperty.Register(
        "Images", typeof(ObservableCollection<DisposableImage>), typeof(RulePage), new PropertyMetadata(new ObservableCollection<DisposableImage>()));

        public ObservableCollection<DisposableImage> Images
        {
            get => (ObservableCollection<DisposableImage>)GetValue(ImagesProperty);
            set => SetValue(ImagesProperty, value);
        }

    }
}
