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
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Categories.xaml
    /// </summary>
    public partial class News : Page
    {
        public News()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty AllNewsProperty = DependencyProperty.Register(
         "AllNews", typeof(ObservableCollection<PromoModel>), typeof(News), new PropertyMetadata(default(ObservableCollection<PromoModel>)));

        public ObservableCollection<PromoModel> AllNews
        {
            get => (ObservableCollection<PromoModel>)GetValue(AllNewsProperty);
            set => SetValue(AllNewsProperty, value);
        }

        private ICommand _goNewsPage;

        public ICommand GoNewsPage => _goNewsPage ??= new RelayCommand(f =>
        {
            var item = f as PromoModel;
            NavigationService?.Navigate(new NewsItemsPage(item)) ;
        });

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
