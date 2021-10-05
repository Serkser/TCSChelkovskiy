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
using TCSchelkovskiyAPI.Models;

namespace TCSChelkovskiy.Views
{
    /// <summary>
    /// Логика взаимодействия для Contacts.xaml
    /// </summary>
    public partial class Contacts : Page
    {
        public Contacts(ContactsModel contactsModel)
        {
            InitializeComponent();
            ContactsModel = contactsModel;
        }
        public static readonly DependencyProperty ContactsModelProperty = DependencyProperty.Register(
     "ContactsModel", typeof(ContactsModel), typeof(Contacts), new PropertyMetadata(default(ContactsModel)));

        public ContactsModel ContactsModel
        {
            get => (ContactsModel)GetValue(ContactsModelProperty);
            set => SetValue(ContactsModelProperty, value);
        }
    }
}