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
    /// Логика взаимодействия для Vacancies.xaml
    /// </summary>
    public partial class Vacancies : Page
    {
        public Vacancies(ObservableCollection<VacancyModel> vacancies)
        {
            InitializeComponent();
            AllVacancies = vacancies;
        }

        public static readonly DependencyProperty AllVacanciesProperty = DependencyProperty.Register(
      "AllVacancies", typeof(ObservableCollection<VacancyModel>), typeof(Vacancies), new PropertyMetadata(default(ObservableCollection<VacancyModel>)));

        public ObservableCollection<VacancyModel> AllVacancies
        {
            get => (ObservableCollection<VacancyModel>)GetValue(AllVacanciesProperty);
            set => SetValue(AllVacanciesProperty, value);
        }
    }
}
