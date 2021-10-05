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
    /// Логика взаимодействия для Rules.xaml
    /// </summary>
    public partial class Rules : Page
    {
        public Rules()
        {
            InitializeComponent();
            AllRules = Memory.KioskObjects.Rules;
        }


        public static readonly DependencyProperty AllRulesProperty = DependencyProperty.Register(
     "AllRules", typeof(ObservableCollection<RuleModel>), typeof(Rules), new PropertyMetadata(default(ObservableCollection<RuleModel>)));

        public ObservableCollection<RuleModel> AllRules
        {
            get => (ObservableCollection<RuleModel>)GetValue(AllRulesProperty);
            set => SetValue(AllRulesProperty, value);
        }

        private ICommand _goToRule;
        public ICommand GoToRule => _goToRule ??= new RelayCommand(f =>
        {
            var rule = f as RuleModel;
            NavigationService?.Navigate(new RulePage(rule));
        });

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
