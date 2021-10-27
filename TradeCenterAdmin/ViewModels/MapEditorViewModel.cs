using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCSchelkovskiyAPI.Enums;
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.Drawing;
using TradeCenterAdmin.Enums;
using TradeCenterAdmin.MapEditorGUIModules;
using TradeCenterAdmin.Models;
using TradeCenterAdmin.Services;
using TradeCenterAdmin.Services.MapObjectSavers;
using TradeCenterAdmin.Storage;
using TradeCenterAdmin.Utilities;
using Image = System.Windows.Controls.Image;

namespace TradeCenterAdmin.ViewModels
{
    public class MapEditorViewModel : INotifyPropertyChanged
    {
        Views.Pages.MapEditor This;
        public MapEditorViewModel(Views.Pages.MapEditor _this)
        {
           
        }

        public void InitViewModel(Views.Pages.MapEditor _this)
        {
            This = _this;
            ChangesPoolMethods.MapEditorDataContext = this;

            MapEditorTool = MapEditorTool.Cursor;

            Floors = Storage.KioskObjects.Floors;
            Shops = Storage.KioskObjects.Shops;

            Terminals = Storage.KioskObjects.Terminals;
            WCs = Storage.KioskObjects.WCs;
            ATMs = Storage.KioskObjects.ATMs;
            Stairs = Storage.KioskObjects.Stairs;
            Lifts = Storage.KioskObjects.Lifts;
            Escolators = Storage.KioskObjects.Escolators;

            if (Floors.Count > 0)
            {
                SelectedFloor = Floors.FirstOrDefault();
            }
            if (Terminals.Count > 0)
            {
                SelectedTerminal = Terminals.FirstOrDefault();
            }
            MakeStartZoom();
            FreeAndUsedObjectsSorter.SortAllPointObjects();

            Storage.KioskObjects.ChangesPool.OnRedoing += ChangesPool_OnRedoing;
            Storage.KioskObjects.ChangesPool.OnUndoing += ChangesPool_OnUndoing;
            Storage.KioskObjects.ChangesPool.OnEntryAdded += ChangesPool_OnEntryAdded;
        }
     


      


        #region Свойства редактора карт
        private MapEditorTool mapEditorTool;
        public MapEditorTool MapEditorTool
        {
            get { return mapEditorTool; }
            set
            {
                mapEditorTool = value;
                OnPropertyChanged("MapEditorTool");
            }
        }
        private ObservableCollection<TerminalModel> terminals;
        public ObservableCollection<TerminalModel> Terminals
        {
            get { return terminals; }
            set
            {
                terminals = value;
                OnPropertyChanged("Terminals");
            }
        }
        private ObservableCollection<TerminalModel> usedterminals;
        public ObservableCollection<TerminalModel> UsedTerminals
        {
            get { return usedterminals; }
            set
            {
                usedterminals = value;
                OnPropertyChanged("UsedTerminals");
            }
        }
        private ObservableCollection<TerminalModel> freeterminals;
        public ObservableCollection<TerminalModel> FreeTerminals
        {
            get { return freeterminals; }
            set
            {
                freeterminals = value;
                OnPropertyChanged("FreeTerminals");
            }
        }
        private TerminalModel selectedTerminal;
        public TerminalModel SelectedTerminal
        {
            get { return selectedTerminal; }
            set
            {
                selectedTerminal = value;
                OnPropertyChanged("SelectedTerminal");
            }
        }
        private TerminalModel currentExistingTerminal;
        public TerminalModel CurrentExistingTerminal
        {
            get { return currentExistingTerminal; }
            set
            {
                currentExistingTerminal = value;
                OnPropertyChanged("CurrentExistingTerminal");
            }
        }



        private ObservableCollection<TerminalModel> wcs;
        public ObservableCollection<TerminalModel> WCs
        {
            get { return wcs; }
            set
            {
                wcs = value;
                OnPropertyChanged("WCs");
            }
        }
        private ObservableCollection<TerminalModel> usedwcs;
        public ObservableCollection<TerminalModel> UsedWCs
        {
            get { return usedwcs; }
            set
            {
                usedwcs = value;
                OnPropertyChanged("UsedWCs");
            }
        }
        private ObservableCollection<TerminalModel> freewcs;
        public ObservableCollection<TerminalModel> FreeWCs
        {
            get { return freewcs; }
            set
            {
                freewcs = value;
                OnPropertyChanged("FreeWCs");
            }
        }
        private TerminalModel selectedWC;
        public TerminalModel SelectedWC
        {
            get { return selectedWC; }
            set
            {
                selectedWC = value;
                OnPropertyChanged("SelectedWC");
            }
        }
        private TerminalModel currentExistingWC;
        public TerminalModel CurrentExistingWC
        {
            get { return currentExistingWC; }
            set
            {
                currentExistingWC = value;
                OnPropertyChanged("CurrentExistingWC");
            }
        }




        private ObservableCollection<TerminalModel> atms;
        public ObservableCollection<TerminalModel> ATMs
        {
            get { return atms; }
            set
            {
                atms = value;
                OnPropertyChanged("ATMs");
            }
        }
        private ObservableCollection<TerminalModel> usedatms;
        public ObservableCollection<TerminalModel> UsedATMs
        {
            get { return usedatms; }
            set
            {
                usedatms = value;
                OnPropertyChanged("UsedATMs");
            }
        }
        private ObservableCollection<TerminalModel> freeatms;
        public ObservableCollection<TerminalModel> FreeATMs
        {
            get { return freeatms; }
            set
            {
                freeatms = value;
                OnPropertyChanged("FreeATMs");
            }
        }

        private TerminalModel selectedATM;
        public TerminalModel SelectedATM
        {
            get { return selectedATM; }
            set
            {
                selectedATM = value;
                OnPropertyChanged("SelectedATM");
            }
        }
        private TerminalModel currentExistingATM;
        public TerminalModel CurrentExistingATM
        {
            get { return currentExistingATM; }
            set
            {
                currentExistingATM = value;
                OnPropertyChanged("CurrentExistingATM");
            }
        }




        private ObservableCollection<TerminalModel> stairs;
        public ObservableCollection<TerminalModel> Stairs
        {
            get { return stairs; }
            set
            {
                stairs = value;
                OnPropertyChanged("Stairs");
            }
        }
        private ObservableCollection<TerminalModel> usedstairs;
        public ObservableCollection<TerminalModel> UsedStairs
        {
            get { return usedstairs; }
            set
            {
                usedstairs = value;
                OnPropertyChanged("UsedStairs");
            }
        }
        private ObservableCollection<TerminalModel> freestairs;
        public ObservableCollection<TerminalModel> FreeStairs
        {
            get { return freestairs; }
            set
            {
                freestairs = value;
                OnPropertyChanged("FreeStairs");
            }
        }

        private TerminalModel selectedStairs;
        public TerminalModel SelectedStairs
        {
            get { return selectedStairs; }
            set
            {
                selectedStairs = value;
                OnPropertyChanged("SelectedStairs");
            }
        }
        private TerminalModel currentExistingStairs;
        public TerminalModel CurrentExistingStairs
        {
            get { return currentExistingStairs; }
            set
            {
                currentExistingStairs = value;
                OnPropertyChanged("CurrentExistingStairs");
            }
        }



        private ObservableCollection<TerminalModel> lifts;
        public ObservableCollection<TerminalModel> Lifts
        {
            get { return lifts; }
            set
            {
                lifts = value;
                OnPropertyChanged("Lifts");
            }
        }
        private ObservableCollection<TerminalModel> usedlifts;
        public ObservableCollection<TerminalModel> UsedLifts
        {
            get { return usedlifts; }
            set
            {
                usedlifts = value;
                OnPropertyChanged("UsedLifts");
            }
        }
        private ObservableCollection<TerminalModel> freelifts;
        public ObservableCollection<TerminalModel> FreeLifts
        {
            get { return freelifts; }
            set
            {
                freelifts = value;
                OnPropertyChanged("FreeLifts");
            }
        }
        private TerminalModel selectedLift;
        public TerminalModel SelectedLift
        {
            get { return selectedLift; }
            set
            {
                selectedLift = value;
                OnPropertyChanged("SelectedLift");
            }
        }
        private TerminalModel currentExistingLift;
        public TerminalModel CurrentExistingLift
        {
            get { return currentExistingLift; }
            set
            {
                currentExistingLift = value;
                OnPropertyChanged("CurrentExistingLift");
            }
        }



        private ObservableCollection<TerminalModel> escolators;
        public ObservableCollection<TerminalModel> Escolators
        {
            get { return escolators; }
            set
            {
                escolators = value;
                OnPropertyChanged("Escolators");
            }
        }
        private ObservableCollection<TerminalModel> usedescolators;
        public ObservableCollection<TerminalModel> UsedEscolators
        {
            get { return usedescolators; }
            set
            {
                usedescolators = value;
                OnPropertyChanged("UsedEscolators");
            }
        }
        private ObservableCollection<TerminalModel> freescolators;
        public ObservableCollection<TerminalModel> FreeEscolators
        {
            get { return freescolators; }
            set
            {
                freescolators = value;
                OnPropertyChanged("FreeEscolators");
            }
        }
        private TerminalModel selectedEscolator;
        public TerminalModel SelectedEscolator
        {
            get { return selectedEscolator; }
            set
            {
                selectedEscolator = value;
                OnPropertyChanged("SelectedEscolator");
            }
        }
        private TerminalModel currentExistingEscolator;
        public TerminalModel CurrentExistingEscolator
        {
            get { return currentExistingEscolator; }
            set
            {
                currentExistingEscolator = value;
                OnPropertyChanged("CurrentExistingEscolator");
            }
        }


        private ObservableCollection<Floor> floors;
        public ObservableCollection<Floor> Floors
        {
            get { return floors; }
            set
            {
                floors = value;
                OnPropertyChanged("Floors");
            }
        }
        private ObservableCollection<ShopModel> shops;
        public ObservableCollection<ShopModel> Shops
        {
            get { return shops; }
            set
            {
                shops = value;
                OnPropertyChanged("Shops");
            }
        }
        private ShopModel selectedShop;
        public ShopModel SelectedShop
        {
            get { return selectedShop; }
            set
            {
                selectedShop = value;
                OnPropertyChanged("SelectedShop");
            }
        }
        private Floor selectedFloor;
        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
                if (value != null)
                {
                    MapObjectsDrawer.LoadFloorObjects();
                    CurrentFloorImage?.Dispose();
                    CurrentFloorImage = new DisposableImage(selectedFloor.Image);
                    AreasExpander.LoadFloorAreaWrappers();
                    TemplateWaysExpander.ShowTemplateWaysList();

                }
                OnPropertyChanged("SelectedFloor");
            }
        }

        private DisposableImage currentFloorImage;
        public DisposableImage CurrentFloorImage
        {
            get { return currentFloorImage; }
            set
            {
                currentFloorImage = value;
                OnPropertyChanged("CurrentFloorImage");
            }
        }

     
        private bool showOnlySelectedTerminalWays;
        public bool ShowOnlySelectedTerminalWays
        {
            get { return showOnlySelectedTerminalWays; }
            set
            {
                showOnlySelectedTerminalWays = value;
                if (showOnlySelectedTerminalWays == false)
                {
                    MapObjectsDrawer.LoadFloorObjects();
                }
                else
                {
                    MapObjectsDrawer.LoadFloorObjects(SelectedTerminal);
                }
                
                OnPropertyChanged("ShowOnlySelectedTerminalWays");
            }
        }
        #endregion

        #region Команды выбора инструментов
        private RelayCommand useCursor;
        public RelayCommand UseCursor
        {
            get
            {
                return useCursor ??
                    (useCursor = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Arrow;
                        MapEditorTool = MapEditorTool.Cursor;
                    }));
            }
        }

        private RelayCommand useKiosk;
        public RelayCommand UseKiosk
        {
            get
            {
                return useKiosk ??
                    (useKiosk = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.Kiosk;
                    }));
            }
        }
        private RelayCommand useHand;
        public RelayCommand UseHand
        {
            get
            {
                return useHand ??
                    (useHand = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Hand;
                        MapEditorTool = MapEditorTool.Hand;
                    }));
            }
        }

        private RelayCommand useArea;
        public RelayCommand UseArea
        {
            get
            {
                return useArea ??
                    (useArea = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Pen;
                        MapEditorTool = MapEditorTool.Area;
                    }));
            }
        }

        private RelayCommand useEntry;
        public RelayCommand UseEntry
        {
            get
            {
                return useEntry ??
                    (useEntry = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.Entry;
                    }));
            }
        }

        private RelayCommand useLift;
        public RelayCommand UseLift
        {
            get
            {
                return useLift ??
                    (useLift = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.Lift;
                    }));
            }
        }

        private RelayCommand useStairs;
        public RelayCommand UseStairs
        {
            get
            {
                return useStairs ??
                    (useStairs = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.Stairs;
                    }));
            }
        }

        private RelayCommand useWay;
        public RelayCommand UseWay
        {
            get
            {
                return useWay ??
                    (useWay = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Pen;
                        MapEditorTool = MapEditorTool.Way;
                    }));
            }
        }
        private RelayCommand useWC;
        public RelayCommand UseWC
        {
            get
            {
                return useWC ??
                    (useWC = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.WC;
                    }));
            }
        }
        private RelayCommand useATM;
        public RelayCommand UseATM
        {
            get
            {
                return useATM ??
                    (useATM = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.ATM;
                    }));
            }
        }
        private RelayCommand useEscalator;
        public RelayCommand UseEscalator
        {
            get
            {
                return useEscalator ??
                    (useEscalator = new RelayCommand(obj =>
                    {
                        This.Cursor = Cursors.Cross;
                        MapEditorTool = MapEditorTool.Escalator;
                    }));
            }
        }
        #endregion

    

        #region Работа с картой
        double scaleX = 1.0;
        double scaleY = 1.0;

        public void ZoomIN(double coeff = 1.4)
        {
            var st = new ScaleTransform();
            This.canvasMap.RenderTransform = st;
            st.ScaleX = scaleX * coeff;
            st.ScaleY = scaleY * coeff;
            scaleX = st.ScaleX;
            scaleY = st.ScaleY;
        }
        public void ZoomOUT(double coeff = 1.4)
        {
            var st = new ScaleTransform();
            This.canvasMap.RenderTransform = st;
            st.ScaleX = scaleX / coeff;
            st.ScaleY = scaleY / coeff;
            scaleX = st.ScaleX;
            scaleY = st.ScaleY;
        }
        private RelayCommand zoomIn;
        public RelayCommand ZoomIn
        {
            get
            {
                return zoomIn ??
                    (zoomIn = new RelayCommand(obj =>
                    {
                        ZoomIN();
                    }));
            }
        }
        private RelayCommand zoomOut;
        public RelayCommand ZoomOut
        {
            get
            {
                return zoomOut ??
                    (zoomOut = new RelayCommand(obj =>
                    {
                        ZoomOUT();
                    }));
            }
        }

        void MakeStartZoom()
        {
            for (int i = 0; i < 7; i++)
            {
                ZoomOut.Execute("анал");
            }
        }

        #endregion

        #region Открытие списка объектов в окне

        public MapTerminalPointType MapTerminalPointType = MapTerminalPointType.WC;

        private RelayCommand showFullFreeObjectsList;
        public RelayCommand ShowFullFreeObjectsList
        {
            get
            {
                return showFullFreeObjectsList ??
                    (showFullFreeObjectsList = new RelayCommand(obj =>
                    {
                        Views.Windows.ShowFullList f = new Views.Windows.ShowFullList(MapTerminalPointType,PointState.Free);
                        f.DataContext = this; f.Show();
                    }));
            }
        }
        private RelayCommand showFullUsedObjectsList;
        public RelayCommand ShowFullUsedObjectsList
        {
            get
            {
                return showFullUsedObjectsList ??
                    (showFullUsedObjectsList = new RelayCommand(obj =>
                    {
                        Views.Windows.ShowFullList f = new Views.Windows.ShowFullList(MapTerminalPointType, PointState.Used);
                        f.DataContext = this; f.Show();
                    }));
            }
        }
        #endregion
        #region Отмена и возврат, кнопки и попапы
        /// <summary>
        /// Команда отмены изменения (ctrl+z)
        /// </summary>
        private RelayCommand goBackInChangesPool;
        public RelayCommand GoBackInChangesPool
        {
            get
            {
                return goBackInChangesPool ??
                    (goBackInChangesPool = new RelayCommand(obj =>
                    {
                        KioskObjects.ChangesPool.Undo();
                    }));
            }
        }
        /// <summary>
        /// Команда возврата изменения (ctrl+u)
        /// </summary>
        private RelayCommand goForwardInChangesPool;
        public RelayCommand GoForwardInChangesPool
        {
            get
            {
                return goForwardInChangesPool ??
                    (goForwardInChangesPool = new RelayCommand(obj =>
                    {
                        KioskObjects.ChangesPool.Redo();
                    }));
            }
        }
        /// <summary>
        /// Отмена действий
        /// </summary>
        private RelayCommand undoChangesStepActions;
        public RelayCommand UndoChangesStepActions
        {
            get
            {
                return undoChangesStepActions ??
                    (undoChangesStepActions = new RelayCommand(obj =>
                    {
                        if(ChangesStep > 0)
                        {
                            KioskObjects.ChangesPool.UndoMany(ChangesStep);
                        }
                        else
                        {
                            MessageBox.Show("Введите число которое больше нуля");
                        }                       
                    }));
            }
        }
        private RelayCommand redoChangesStepActions;
        public RelayCommand RedoChangesStepActions
        {
            get
            {
                return redoChangesStepActions ??
                    (redoChangesStepActions = new RelayCommand(obj =>
                    {
                        if (ChangesStep > 0)
                        {
                            KioskObjects.ChangesPool.RedoMany(ChangesStep);
                        }
                        else
                        {
                            MessageBox.Show("Введите число которое больше нуля");
                        }
                    }));
            }
        }
        private ObservableCollection<ChangeEntry> undoActions;
        public ObservableCollection<ChangeEntry> UndoActions
        {
            get { return undoActions; }
            set
            {
                undoActions = value;
                OnPropertyChanged("UndoActions");
            }
        }
        private ObservableCollection<ChangeEntry> redoActions;
        public ObservableCollection<ChangeEntry> RedoActions
        {
            get { return redoActions; }
            set
            {
                redoActions = value;
                OnPropertyChanged("RedoActions");
                
            }          
        }

        private int changesStep;
        public int ChangesStep
        {
            get { return changesStep; }
            set
            {
                changesStep = value;
                OnPropertyChanged("ChangesStep");

            }
        }
        private void ChangesPool_OnUndoing(ChangesPool.Events.EventArgs.UndoEventArgs args)
        {
            GetChangesLists();
        }

        private void ChangesPool_OnRedoing(ChangesPool.Events.EventArgs.RedoEventArgs args)
        {
            GetChangesLists();
        }
        private void ChangesPool_OnEntryAdded(ChangesPool.Events.EventArgs.EntryAddedEventArgs args)
        {
            GetChangesLists();
        }

        void GetChangesLists()
        {
            List<ChangeEntry> reverseRedo = new List<ChangeEntry>();
            for (int i = Storage.KioskObjects.ChangesPool.GetPossibleRedoActions().Count - 1; i > -1; i--)
            {
                reverseRedo.Add(Storage.KioskObjects.ChangesPool.GetPossibleRedoActions()[i]);
            }

            RedoActions = new ObservableCollection<ChangeEntry>(reverseRedo);



            List<ChangeEntry> reverseUndo = new List<ChangeEntry>();
            for (int i = Storage.KioskObjects.ChangesPool.GetPossibleUndoActions().Count - 1; i > -1; i--)
            {
                reverseUndo.Add(Storage.KioskObjects.ChangesPool.GetPossibleUndoActions()[i]);
            }

            UndoActions = new ObservableCollection<ChangeEntry>(reverseUndo);
        }
        #endregion


        #region Сокрытие путей
        private bool hideAllWays;
        public bool HideAllWays
        {
            get { return hideAllWays; }
            set
            {
                hideAllWays = value;              
                OnPropertyChanged("HideAllWays");
                MapObjectsDrawer.LoadFloorObjects();
            }
        }
        private Way editingWay;
        public Way EditingWay
        {
            get { return editingWay; }
            set
            {
                editingWay = value;
                OnPropertyChanged("EditingWay");
            }
        }

        #endregion

        #region Экспандер областей на этаже


        private ObservableCollection<AreaWrapper> floorAreaWrappers;
        public ObservableCollection<AreaWrapper> FloorAreaWrappers
        {
            get { return floorAreaWrappers; }
            set
            {
                floorAreaWrappers = value;
                OnPropertyChanged("FloorAreaWrappers");
            }
        }
        private RelayCommand highlightSelectedArea;
        public RelayCommand HighlightSelectedArea
        {
            get
            {
                return highlightSelectedArea ??
                    (highlightSelectedArea = new RelayCommand(obj =>
                    {
                        AreaWrapper wrapper = obj as AreaWrapper;
                        if (wrapper != null)
                        {
                            MapObjectsDrawer.HighlightArea(wrapper.Area);
                            SelectedAreaExpander.ShowAreaInfo(wrapper.Area);
                        }                  
                    }));
            }
        }
  

        //Сортировка по областям

        private int affectedFloorsByAreaRoutes;
        public int AffectedFloorsByAreaRoutes
        {
            get { return affectedFloorsByAreaRoutes; }
            set
            {
                if (value > -1 && SortAreasWithFloorsWay)
                {
                    AreasExpander.LoadFloorAreaWrappers();
                }
                affectedFloorsByAreaRoutes = value; OnPropertyChanged("SortAllAreas");
            }
        }

        private bool sortAllAreas;
        public bool SortAllAreas
        {
            get { return sortAllAreas; }
            set {
                AreasExpander.LoadFloorAreaWrappers();
                sortAllAreas = value; OnPropertyChanged("SortAllAreas"); }
        }

        private bool sortAreasWithShop;
        public bool SortAreasWithShop
        {
            get { return sortAreasWithShop; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAreasWithShop = value; OnPropertyChanged("SortAreasWithShop"); }
        }
        private bool sortAreasWithNoShop;
        public bool SortAreasWithNoShop
        {
            get { return sortAreasWithNoShop; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAreasWithNoShop = value; OnPropertyChanged("SortAreasWithNoShop"); }
        }

        //Сортировка по путям

        private bool sortAllAreasWay;
        public bool SortAllAreasWay
        {
            get { return sortAllAreasWay; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAllAreasWay = value; OnPropertyChanged("SortAllAreasWay"); }
        }

        private bool sortAreasWithWay;
        public bool SortAreasWithWay
        {
            get { return sortAreasWithWay; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAreasWithWay = value; OnPropertyChanged("SortAreasWithWay"); }
        }
        private bool sortAreasWithNoWay;
        public bool SortAreasWithNoWay
        {
            get { return sortAreasWithNoWay; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAreasWithNoWay = value; OnPropertyChanged("SortAreasWithNoWay"); }
        }
        private bool sortAreasWithFloorsWay;
        public bool SortAreasWithFloorsWay
        {
            get { return sortAreasWithFloorsWay; }
            set
            {
                AreasExpander.LoadFloorAreaWrappers();
                sortAreasWithFloorsWay = value; OnPropertyChanged("SortAreasWithFloorsWay");
            }
        }


        #endregion

        #region Экспандер маршрутов-переходов

        private ObservableCollection<TemplateWaysContainer> floorTemplateWays;
        public ObservableCollection<TemplateWaysContainer> FloorTemplateWays
        {
            get { return floorTemplateWays; }
            set
            {
                floorTemplateWays = value;
                OnPropertyChanged("FloorTemplateWays");
            }
        }
        private ObservableCollection<TemplateWaysContainer> allFloorTemplateWays;
        public ObservableCollection<TemplateWaysContainer> AllFloorTemplateWays
        {
            get { return allFloorTemplateWays; }
            set
            {
                allFloorTemplateWays = value;
                OnPropertyChanged("AllFloorTemplateWays");
            }
        }

        private bool filterFloorTemplatesByFloor;
        public bool FilterFloorTemplatesByFloor
        {
            get { return filterFloorTemplatesByFloor; }
            set
            {
                filterFloorTemplatesByFloor = value;
                OnPropertyChanged("FilterFloorTemplatesByFloor");
                if (FilterFloorTemplatesByFloor)
                {
                    TemplateWaysExpander.ShowTemplateWaysList(FilterFloorTemplatesFloor);
                }
                else
                {
                    TemplateWaysExpander.ShowTemplateWaysList();
                }

            }
        }
        private string filterFloorTemplatesFloor;
        public string FilterFloorTemplatesFloor
        {
            get { return filterFloorTemplatesFloor; }
            set
            {
                filterFloorTemplatesFloor = value;
                OnPropertyChanged("FilterFloorTemplatesFloor");
                if (FilterFloorTemplatesByFloor)
                {
                    TemplateWaysExpander.ShowTemplateWaysList(FilterFloorTemplatesFloor);
                }
                else
                {
                    TemplateWaysExpander.ShowTemplateWaysList();
                }
               
            }
        }

        private RelayCommand highlightSelectedTemplateWay;
        public RelayCommand HighlightSelectedTemplateWay
        {
            get
            {
                return highlightSelectedTemplateWay ??
                    (highlightSelectedTemplateWay = new RelayCommand(obj =>
                    {
                        TemplateWaysContainer wrapper = obj as TemplateWaysContainer;
                        if (wrapper != null)
                        {
                            var wayToHighlight = wrapper.Ways.Where(o => o.FloorId == SelectedFloor.Id).FirstOrDefault();
                            if (wayToHighlight != null)
                            {
                                //MessageBox.Show(wrapper.Ways.Count.ToString());
                                MapObjectsDrawer.LoadFloorObjectsWithWayHighlighting(wayToHighlight);
                            }                       
                        }
                    }));
            }
        }


        #endregion




        #region Прочие команды
        /// <summary>
        /// Открытие окна для изменения этажей
        /// </summary>
        private RelayCommand openFloors;
        public RelayCommand OpenFloors
        {
            get
            {
                return openFloors ??
                    (openFloors = new RelayCommand(obj =>
                    {
                        Views.Windows.Floors f = new Views.Windows.Floors();
                        f.Show();
                    }));
            }
        }

        /// <summary>
        /// Открытие окна для назначения области магазина
        /// </summary>
        private RelayCommand assignShop;
        public RelayCommand AssignShop
        {
            get
            {
                return assignShop ??
                    (assignShop = new RelayCommand(obj =>
                    {
                        Views.Windows.Floors f = new Views.Windows.Floors();
                        f.Show();
                    }));
            }
        }
        /// <summary>
        /// Сохранение изменений
        /// </summary>
        private RelayCommand saveChanges;
        public RelayCommand SaveChanges
        {
            get
            {
                return saveChanges ??
                    (saveChanges = new RelayCommand(obj =>
                    {
                        //IMapObjectSaver saver = new ServerMapObjectsSaver();
                        //saver.Save(Floors);
                        IMapObjectSaver saver2 = new LocalJsonMapObjectsSaver();
                        saver2.Save(Floors);

                        MessageBox.Show("Изменения успешно сохранены");
                    }));
            }
        }
        /// <summary>
        /// Подгрузка объектов этажа при событии  SelectionChanged для Floors
        /// </summary>
        private RelayCommand redrawFloorItems;
        public RelayCommand RedrawFloorItems
        {
            get
            {
                return redrawFloorItems ??
                    (redrawFloorItems = new RelayCommand(obj =>
                    {
                        MapObjectsDrawer.LoadFloorObjects(); 
                    }));
            }
        }    
        #endregion

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
