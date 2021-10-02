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
using TCEvropeyskiy.ViewModels;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Page
    {
        public Search()
        {
            InitializeComponent();
        }

        private void buttonPressed(object sender, EventArgs e)
        {
            TCSChelkovskiy.Controls.KeyboardEventArgs args = e as TCSChelkovskiy.Controls.KeyboardEventArgs;
            ((MainWindowViewModel)this.DataContext).SearchText = args.CurrentText;
            ((MainWindowViewModel)this.DataContext).ShopsByCategory = new ObservableCollection<TCSchelkovskiyAPI.Models.ShopModel>(
                  ((MainWindowViewModel)this.DataContext).Shops.Where(o => o.Name.Contains(args.CurrentText, StringComparison.OrdinalIgnoreCase)).ToList());
        }
    }
}
