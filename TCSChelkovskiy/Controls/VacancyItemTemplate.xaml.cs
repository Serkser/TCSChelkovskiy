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

namespace TCSChelkovskiy.Controls
{
    /// <summary>
    /// Логика взаимодействия для VacancyItemTemplate.xaml
    /// </summary>
    public partial class VacancyItemTemplate : UserControl
    {
        public VacancyItemTemplate()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(VacancyItemTemplate));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty ContactsProperty = DependencyProperty.Register("Contacts", typeof(string), typeof(VacancyItemTemplate));
        public string Contacts
        {
            get { return (string)GetValue(ContactsProperty); }
            set { SetValue(ContactsProperty, value); }
        }

        public static readonly DependencyProperty VacancyBlocksProperty = DependencyProperty.Register("VacancyBlocks", typeof(List<TCSchelkovskiyAPI.Models.VacancyBlock>), 
            typeof(VacancyItemTemplate));
        public List<TCSchelkovskiyAPI.Models.VacancyBlock> VacancyBlocks
        {
            get { return (List<TCSchelkovskiyAPI.Models.VacancyBlock>)GetValue(VacancyBlocksProperty); }
            set { SetValue(VacancyBlocksProperty, value); }
        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            vacancyBlocksList.ItemsSource = VacancyBlocks;
            expander.Header = Title;
            contacts.Text = Contacts;
        }
    }
}
