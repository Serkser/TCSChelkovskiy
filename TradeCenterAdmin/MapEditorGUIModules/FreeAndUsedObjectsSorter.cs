using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.ViewModels;
using TradeCenterAdmin.Views.Pages;

namespace TradeCenterAdmin.MapEditorGUIModules
{
    public static class FreeAndUsedObjectsSorter
    {
        public static MapEditor MapEditorPage { get; set; }
        public static MapEditorViewModel MapEditorDataContext { get; set; }
        #region Сортировка списков объектов по типу доступен/установлен
        public static void SortAllPointObjects()
        {
            SortWCs();
            SortATMs();
            SortStairs();
            SortLifts();
            SortKiosks();
            SortEscalators();
        }
        public static void SortWCs(Floor floor = null)
        {
            MapEditorDataContext.FreeWCs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedWCs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.WCs = new ObservableCollection<TerminalModel>(Storage.KioskObjects.WCs.ToList());
            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.WCs; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.WCs.Where(o => o.Floor.Name == floor.Name).ToList());
            }

            if (MapEditorDataContext.Floors != null)
            {
                foreach (var wc in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.WCs).ToList())
                    {
                        wc.StatusOnMap = "";
                        if (st.Id == wc.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedWCs.Add(wc); wc.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeWCs.Add(wc); wc.StatusOnMap = ""; }
                }
            }
        }
        public static void SortATMs(Floor floor = null)
        {
            MapEditorDataContext.FreeATMs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedATMs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.ATMs = new ObservableCollection<TerminalModel>(Storage.KioskObjects.ATMs.ToList());
            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.ATMs; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.ATMs.Where(o => o.Floor.Name == floor.Name).ToList());
            }

            if (MapEditorDataContext.Floors != null)
            {
                foreach (var atm in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.ATMs).ToList())
                    {
                        atm.StatusOnMap = "";
                        if (st.Id == atm.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedATMs.Add(atm); atm.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeATMs.Add(atm); atm.StatusOnMap = ""; }
                }
            }
        }
        public static void SortStairs(Floor floor = null)
        {
            MapEditorDataContext.FreeStairs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedStairs = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.Stairs = new ObservableCollection<TerminalModel>(Storage.KioskObjects.Stairs.ToList());
            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.Stairs; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.Stairs.Where(o => o.Floor.Name == floor.Name).ToList());
            }

            if (MapEditorDataContext.Floors != null)
            {
                foreach (var stairs in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.Stations).ToList())
                    {
                        stairs.StatusOnMap = "";
                        if (st.Id == stairs.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedStairs.Add(stairs); stairs.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeStairs.Add(stairs); stairs.StatusOnMap = ""; }
                }
            }
        }
        public static void SortLifts(Floor floor = null)
        {
            MapEditorDataContext.FreeLifts = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedLifts = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.Lifts = new ObservableCollection<TerminalModel>(Storage.KioskObjects.Lifts.ToList());

            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.Lifts; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.Lifts.Where(o => o.Floor.Name == floor.Name).ToList());
            }


            if (MapEditorDataContext.Floors != null)
            {
                foreach (var lift in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.Stations).ToList())
                    {
                        lift.StatusOnMap = "";
                        if (st.Id == lift.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedLifts.Add(lift); lift.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeLifts.Add(lift); lift.StatusOnMap = ""; }
                }
            }
        }
        public static void SortKiosks(Floor floor = null)
        {
            MapEditorDataContext.FreeTerminals = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedTerminals = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.Terminals = new ObservableCollection<TerminalModel>(Storage.KioskObjects.Terminals.ToList());
            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.Terminals; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.Terminals.Where(o => o.Floor.Name == floor.Name).ToList());
            }

            if (MapEditorDataContext.Floors != null)
            {
                foreach (var kiosk in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.Stations).ToList())
                    {
                        kiosk.StatusOnMap = "";
                        if (st.Id == kiosk.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedTerminals.Add(kiosk); kiosk.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeTerminals.Add(kiosk); kiosk.StatusOnMap = ""; }
                }
            }
        }
        public static void SortEscalators(Floor floor = null)
        {
            MapEditorDataContext.FreeEscolators = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.UsedEscolators = new ObservableCollection<TerminalModel>();
            MapEditorDataContext.Escolators = new ObservableCollection<TerminalModel>(Storage.KioskObjects.Escolators.ToList());
            ObservableCollection<TerminalModel> sort = null;
            if (floor == null) { sort = Storage.KioskObjects.Escolators; }
            else
            {
                sort = new ObservableCollection<TerminalModel>
               (Storage.KioskObjects.Escolators.Where(o => o.Floor.Name == floor.Name).ToList());
            }

            if (MapEditorDataContext.Floors != null)
            {
                foreach (var escalator in sort)
                {
                    bool isUsed = false;
                    foreach (var st in MapEditorDataContext.Floors.SelectMany(o => o.Stations).ToList())
                    {
                        escalator.StatusOnMap = "";
                        if (st.Id == escalator.ID)
                        {
                            isUsed = true;
                        }
                    }
                    if (isUsed) { MapEditorDataContext.UsedEscolators.Add(escalator); escalator.StatusOnMap = "Установлен"; }
                    else { MapEditorDataContext.FreeEscolators.Add(escalator); escalator.StatusOnMap = ""; }
                }
            }
        }
        #endregion
    }
}
