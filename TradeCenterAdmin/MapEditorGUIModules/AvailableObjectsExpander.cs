using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TCSchelkovskiyAPI.Enums;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.MapEditorGUIModules
{
    public static class AvailableObjectsExpander
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }

        public static void ObjectsTabControlSwitched(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = MapEditorPage.objectsTabControl.SelectedItem as TabItem;
            try
            {
                if (tab != null)
                {
                    switch (tab.Uid)
                    {
                        case "tab_wc":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.WC;
                            break;
                        case "tab_atm":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.ATMCash;
                            break;
                        case "tab_stairs":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.Stairs;
                            break;
                        case "tab_lifts":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.Lift;
                            break;
                        case "tab_escalator":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.Escolator;
                            break;
                        case "tab_kiosk":
                            MapEditorDataContext.MapTerminalPointType = MapTerminalPointType.Termanals;
                            break;
                    }
                }
            }
            catch { }

        }

        #region События изменения текстбоксов этажеи объектов для фильтрации
        public static void WcFreeListTextChanded(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortWCs(floor);
            }
            catch (Exception ex) { return; }
        }

        public static void AtmListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortATMs(floor);
            }
            catch (Exception ex) { return; }
        }

        public static void EscalatorListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortEscalators(floor);
            }
            catch (Exception ex) { return; }
        }

        public static void LiftListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortLifts(floor);
            }
            catch (Exception ex) { return; }
        }

        public static void StairsListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortStairs(floor);
            }
            catch (Exception ex) { return; }
        }

        public static void KioskListTextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            Floor floor = null;
            try
            {
                floor = Storage.KioskObjects.Floors.Where(o => o.FloorNumber == textbox.Text).FirstOrDefault();
                FreeAndUsedObjectsSorter.SortKiosks(floor);
            }
            catch (Exception ex) { return; }
        }
        #endregion

        #region Удаление объектов посредством экспандера объектов
        public static void KioskTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingTerminal;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);

        }
        public static void KioskTabDeleteExceptIt(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingTerminal;
            var others = KioskObjects.Terminals.Where(o => o.ID != selected.ID).ToList();
            foreach (var kiosk in others)
            {
                MapObjectsCreator.RemoveTerminalModelPoint(kiosk);
            }
        }

        public static void WcTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingWC;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);
        }

        public static void AtmTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingATM;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);
        }

        public static void EscalatorTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingEscolator;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);
        }

        public static void LiftTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingLift;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);
        }

        public static void StairsTabDelete(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingStairs;
            MapObjectsCreator.RemoveTerminalModelPoint(selected);
        }
        #endregion

        #region Подсветка объектов посредством экспандера объектов
        public static void KioskTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingTerminal;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }

        public static void WcTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingWC;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }

        public static void AtmTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingATM;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }

        public static void EscalatorTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingEscolator;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }

        public static void LiftTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingLift;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }

        public static void StairsTabShow(object sender, RoutedEventArgs e)
        {
            var selected = MapEditorDataContext.CurrentExistingStairs;
            MapObjectsDrawer.ShowAndHighligthTerminalModelPoint(selected);
        }


        #endregion
    }
}
