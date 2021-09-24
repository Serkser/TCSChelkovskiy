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
            Map.Floors = new List<Floor>
            {
                new Floor{Image = System.IO.Path.Combine(Environment.CurrentDirectory,"Map","Floors","1.png"),
                Width =9000,Height=9000,Name="1",Id=1}
            };
            Map.SelectedFloor = Map.Floors.FirstOrDefault();
           
        }
    }
}
