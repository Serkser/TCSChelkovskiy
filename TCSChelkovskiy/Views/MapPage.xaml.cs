using NavigationMap.Models;
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
using TCEvropeyskiy;
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        public MapPage(MainWindow main)
        {
            InitializeComponent();
            DataContext = new ViewModels.MapViewModel(this,main);
         
           
        }
        public MapPage(MainWindow main,ShopModel shop)
        {
            InitializeComponent();
            DataContext = new ViewModels.MapViewModel(this, main,shop);
        }

        private void Map_OnOnAreaSelected(Area obj)
        {
            ((ViewModels.MapViewModel)this.DataContext).CurrentFloorShop = obj;
        }
    }
}
