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
using System.Windows.Shapes;
using TCSchelkovskiyAPI.Enums;
using TradeCenterAdmin.Enums;

namespace TradeCenterAdmin.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShowFullList.xaml
    /// </summary>
    public partial class ShowFullList : Window
    {
        MapTerminalPointType _type;
        PointState _state;
        public ShowFullList(MapTerminalPointType type,PointState state)
        {
            InitializeComponent();
           
            _type = type;
            _state = state;

        }

        private void LoadWindowItems(MapTerminalPointType type, PointState state,Floor floor = null)
        {
            switch (type)
            {
                case MapTerminalPointType.ATMCash:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Банкоматы в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeATMs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeATMs;
                            }                        
                            break;
                        case PointState.Used:
                            title.Text = "Банкоматы, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedATMs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedATMs;
                            }
                            break;
                    }
                    break;
                case MapTerminalPointType.Escolator:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Эскалаторы в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeEscolators.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeEscolators;
                            }
                            break;
                        case PointState.Used:
                            title.Text = "Эскалаторы, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedEscolators.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedEscolators;
                            }
                            break;
                    }
                    break;
                case MapTerminalPointType.Lift:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Лифты в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeLifts.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeLifts;
                            }
                            break;
                        case PointState.Used:
                            title.Text = "Лифты, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedLifts.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedLifts;
                            }
                            break;
                    }
                    break;
                case MapTerminalPointType.Stairs:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Лестницы в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeStairs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeStairs;
                            }
                            break;
                        case PointState.Used:
                            title.Text = "Лестницы, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedStairs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedStairs;
                            }
                            break;
                    }
                    break;
                case MapTerminalPointType.Termanals:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Киоски в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeTerminals.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeTerminals;
                            }
                            break;
                        case PointState.Used:
                            title.Text = "Киоски, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedTerminals.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedTerminals;
                            }
                            break;
                    }
                    break;
                case MapTerminalPointType.WC:
                    switch (state)
                    {
                        case PointState.Free:
                            title.Text = "Туалеты в резерве";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeWCs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).FreeWCs;
                            }

                            break;
                        case PointState.Used:
                            title.Text = "Туалеты, установленные на этажах";
                            listbox.ItemsSource = null;
                            if (floor != null)
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedWCs.Where(o => o.Floor.Name == floor.Name);
                            }
                            else
                            {
                                listbox.ItemsSource = ((ViewModels.MapEditorViewModel)this.DataContext).UsedWCs;
                            }
                            break;
                    }
                    break;
            }
        }

        private void filterFloors(object sender, SelectionChangedEventArgs e)
        {
          
            Floor floor = floors.SelectedItem as Floor;
            if (useFloorFilter.IsChecked == true)
            {
                if (floor != null)
                {
                    if (useFloorFilter.IsChecked == true)
                    {
                        LoadWindowItems(_type, _state, floor);
                    }
                }
            }
        }

        private void useFilter(object sender, RoutedEventArgs e)
        {
            Floor floor = floors.SelectedItem as Floor;
            if (floor != null)
            {
                if (useFloorFilter.IsChecked == true)
                {
                    LoadWindowItems(_type, _state, floor);
                }
            }
            
        }

        private void unuseFilter(object sender, RoutedEventArgs e)
        {
            LoadWindowItems(_type, _state);
        }

        private void loaded(object sender, RoutedEventArgs e)
        {       
            LoadWindowItems(_type, _state);
        }
    }
}
