using NavigationMap.Core;
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using TCSchelkovskiyAPI.Enums;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.ActionPlaceInfo;
using TradeCenterAdmin.ChangesPool.Entries;
using TradeCenterAdmin.ChangesPool.Enums;
using TradeCenterAdmin.ChangesPool.RedoActionsInfo;
using TradeCenterAdmin.ChangesPool.UndoActionsInfo;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.Enums;
using TradeCenterAdmin.MapEditorGUIModules;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.Services;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;

namespace TradeCenterAdmin.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapEditor.xaml
    /// </summary>
    public partial class MapEditor : Page
    {
        public MapEditor()
        {
            InitializeComponent();

            this.DataContext = new MapEditorViewModel(this);
            CollapseAllExpanders();

        }
        private void MapEditorLoaded(object sender, RoutedEventArgs e)
        {
         
            MapObjectsDrawer.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            MapObjectsDrawer.MapEditorPage = this;

            MapObjectsCreator.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            MapObjectsCreator.MapEditorPage = this;

            SelectedAreaExpander.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            SelectedAreaExpander.MapEditorPage = this;

            AvailableObjectsExpander.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            AvailableObjectsExpander.MapEditorPage = this;

            TemplateWaysExpander.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            TemplateWaysExpander.MapEditorPage = this;

            AreasExpander.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            AreasExpander.MapEditorPage = this;

            FreeAndUsedObjectsSorter.MapEditorDataContext = (MapEditorViewModel)this.DataContext;
            FreeAndUsedObjectsSorter.MapEditorPage = this;

            ((MapEditorViewModel)this.DataContext).InitViewModel(this);
        }

       public Area currentArea = null; //область, над которой работаем
       public Station currentWayStation = null; //лестница, лифт или эскалатор для создания маршрута-перехода
       public WC currentWC = null; //туалет, к которому чертим путь
       public ATM currentATM = null; //туалет, к которому чертим путь


       public WayType wayType; //для чего чертить маршрут. для области или маршрут-переход

       public Way currentWay = null;  //редактируемый маршрут
       public Floor firstFloor = null; // 1-й этаж маршрута

       public Floor currentFloor; //выбранный этаж

       
        private void click(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Kiosk)
                {
                    MapObjectsCreator.CreateKiosk(sender, e);                  
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Entry)
                {
                    MapObjectsCreator.CreateEntry(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Stairs)
                {
                    MapObjectsCreator.CreateStairs(sender, e);
                }  
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Escalator)
                {
                    MapObjectsCreator.CreateEscalator(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Lift)
                {
                    MapObjectsCreator.CreateLift(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.WC)
                {
                    MapObjectsCreator.CreateWC(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.ATM)
                {
                    MapObjectsCreator.CreateATM(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    MapObjectsCreator.CreateArea(sender, e);
                }
                else if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Way)
                {
                    MapObjectsCreator.CreateWay(sender, e);
                }
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Area)
                {
                    if (MessageBox.Show("Вы действительно хотите завершить расчерчивание области?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        currentArea = null;
                    }
                }
                if (((ViewModels.MapEditorViewModel)this.DataContext).MapEditorTool == Enums.MapEditorTool.Way)
                {
                    if (currentWay!= null && currentArea != null)
                    {
                        if (MessageBox.Show("Вы действительно хотите отменить создание маршрута?","Предупреждение",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (((ViewModels.MapEditorViewModel)this.DataContext)
                           .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault() != null)
                            {
                                var way = ((ViewModels.MapEditorViewModel)this.DataContext)
                                  .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                                  .Ways.Where(o => o.Id == currentWay.Id).FirstOrDefault();

                                ((ViewModels.MapEditorViewModel)this.DataContext)
                               .SelectedFloor.Areas.Where(o => o.Id == o.Id).FirstOrDefault()
                               .Ways.Remove(way);

                                currentWay = null; currentArea = null;
                                MapObjectsDrawer.LoadFloorObjects();
                            }
                        }                  
                    }                  
                }
            }
        }


        #region Работа с левым меню, выделенная область
        
        private void areaDeleteWayHandler(object sender, RoutedEventArgs e)
        {
            SelectedAreaExpander.AreaWaysDeleteHandler();
        }
        private void areaDeleteAllWaysHandler(object sender, RoutedEventArgs e)
        {
            SelectedAreaExpander.AreaDeleteAllWaysHandler();
        }

        /// Подсвечивание выбранного маршута области
        private void areaWaysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAreaExpander.HighlightSelectedAreaWay();
        }
        #region Показ маршрутов только выделенной области
        private void areaCheckedOwnWays(object sender, RoutedEventArgs e)
        {
            MapObjectsDrawer.LoadFloorObjects(currentArea);
        }

        private void areaUncheckedOwnWays(object sender, RoutedEventArgs e)
        {
            MapObjectsDrawer.LoadFloorObjects();
        }
        #endregion
        #endregion


        #region Экспандер со списками объектов
        private void objectsTabControlSwitched(object sender, SelectionChangedEventArgs e)
        {
            AvailableObjectsExpander.ObjectsTabControlSwitched(sender, e);
        }

        #region События изменения текстбоксов этажеи объектов для фильтрации
        private void wcFreeListTextChanded(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.WcFreeListTextChanded(sender, e);
        }

        private void atmListTextChanged(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.AtmListTextChanged(sender, e);
        }

        private void escalatorListTextChanged(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.EscalatorListTextChanged(sender, e);
        }

        private void liftListTextChanged(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.LiftListTextChanged(sender, e);
        }

        private void stairsListTextChanged(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.StairsListTextChanged(sender, e);
        }

        private void kioskListTextChanged(object sender, TextChangedEventArgs e)
        {
            AvailableObjectsExpander.KioskListTextChanged(sender, e);
        }
        #endregion

        #region Удаление объектов посредством экспандера объектов
        private void kioskTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.KioskTabDelete(sender, e);
        }
        private void kioskTabDeleteExceptIt(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.KioskTabDeleteExceptIt(sender, e);
        }

        private void wcTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.WcTabDelete(sender, e);
        }

        private void atmTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.AtmTabDelete(sender, e);
        }

        private void escalatorTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.EscalatorTabDelete(sender, e);
        }

        private void liftTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.LiftTabDelete(sender, e);
        }

        private void stairsTabDelete(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.StairsTabDelete(sender, e);
        }
        #endregion

        #region Подсветка объектов посредством экспандера объектов
        private void kioskTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.KioskTabShow(sender, e);
        }

        private void wcTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.WcTabShow(sender, e);
        }

        private void atmTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.AtmTabShow(sender, e);
        }

        private void escalatorTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.EscalatorTabShow(sender, e);
        }

        private void liftTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.LiftTabShow(sender, e);
        }

        private void stairsTabShow(object sender, RoutedEventArgs e)
        {
            AvailableObjectsExpander.StairsTabShow(sender, e);
        }


        #endregion

        #endregion

        #region Изменение "фиксированного" размера экспандеров, чтобы свернутый экспандер не мешал
        // работать с картой


        private void objectsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
            expander.Width = 185;
        }
        private void objectsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 409;
            expander.Width = 328;
        }

        private void areaExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
            expander.Width = 142;
        }

        private void areaExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 137;
            expander.Width = 328;
        }

        private void floorExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void floorExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 100;
        }

        private void toolsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
        }

        private void toolsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 471;
        }
        private void shopsExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
            expander.Width = 134;
        }

        private void shopsExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 347;
            expander.Width = 328;
        }

        private void templateWaysExpanderCollapsed(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 26;
            expander.Width = 150;
        }

        private void templateWaysExpanderExpanded(object sender, RoutedEventArgs e)
        {
            Expander expander = sender as Expander;
            expander.Height = 326;
            expander.Width = 328;
        }
        private void CollapseAllExpanders()
        {
            objectsExpander.IsExpanded = false;
            areaExpander.IsExpanded = false;
            floorsExpander.IsExpanded = false;
            toolsExpander.IsExpanded = false;
            shopsExpander.IsExpanded = false;
            templateWaysExpander.IsExpanded = false;
        }
        #endregion

       


        public string GetTerminalMapObjectUIDPostfix(TerminalModel model)
        {
            string postfix = "";
            switch (model.Type)
            {
                case MapTerminalPointType.Termanals:
                    postfix = "kioskuid";
                    break;
                case MapTerminalPointType.ATMCash:
                    postfix = "atmuid";
                    break;
                case MapTerminalPointType.Escolator:
                    postfix = "escalatoruid";
                    break;
                case MapTerminalPointType.Stairs:
                    postfix = "stairsuid";
                    break;
                case MapTerminalPointType.WC:
                    postfix = "wcuid";
                    break;
                case MapTerminalPointType.Lift:
                    postfix = "liftuid";
                    break;
            }
            return postfix;
        }
        private void keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                KioskObjects.ChangesPool.Undo();
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.U)
            {
               KioskObjects.ChangesPool.Redo();
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                ((MapEditorViewModel)this.DataContext).SaveChanges.Execute(null);
            }
        }

        private void scrolling(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                ((MapEditorViewModel)this.DataContext).ZoomIN(1.1);
            }           
            else if (e.Delta < 0)
            {
                ((MapEditorViewModel)this.DataContext).ZoomOUT(1.1);
            }
             
        }

       
    }
}
