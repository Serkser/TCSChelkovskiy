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
using TCSchelkovskiyAPI.Models;
using TradeCenterAdmin.Enums;
using Image = System.Windows.Controls.Image;

namespace TradeCenterAdmin.ViewModels
{
    public class MapEditorViewModel : INotifyPropertyChanged
    {
        Views.Pages.MapEditor This;
        public MapEditorViewModel(Views.Pages.MapEditor _this)
        {
         
            This = _this;
            Floors = Storage.KioskObjects.Floors;
            Shops = Storage.KioskObjects.Shops;
            if (Floors.Count > 0)
            {
                SelectedFloor = Floors.FirstOrDefault();

               
            }
            MakeStartZoom();
        }

        public void LoadFloorObjects()
        {
            //очистка старых элементов
            for (int a = 0; a < 5; a++)
            {
                for (int i = 0; i < This.canvasMap.Children.Count; i++)
                {
                    UIElement obj = This.canvasMap.Children[i];
                    if (obj is Image)
                    {
                        if (((Image)obj).Name == "img")
                        {
                            continue;
                        }
                        else
                        {
                            This.canvasMap.Children.Remove(obj);
                        }
                    }
                    else
                    {
                        This.canvasMap.Children.Remove(obj);
                    }
                }
            }
        

            foreach (var obj in SelectedFloor.Stations)
            {
                switch (obj.AreaPoint.PointType)
                {
                    case NavigationMap.Enums.PointTypeEnum.Entry:
                        if (obj.Name.Contains("Вход"))
                        {
                            This.DrawEntry(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                        }
                        else if (obj.Name.Contains("Лестница"))
                        {
                            This.DrawStairs(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                        }
                        break;
                    case NavigationMap.Enums.PointTypeEnum.Station:
                        This.DrawKiosk(obj, new System.Windows.Point(obj.AreaPoint.X, obj.AreaPoint.Y), false);
                        break;
                }
            }
            //Рисуем области магазинов
            foreach (var obj in SelectedFloor.Areas)
            {
                This.DrawAreaPerimeter(obj);
             
            }
            //Рисуем пути магазинов
            foreach (var floor in Floors)
            {
                foreach (var area in floor.Areas)
                {
                    //foreach (var currentFloorFrea in SelectedFloor.Areas)
                    //{
                        foreach (var way in area.Ways)
                        {
                            if (way.WayPoints.Where(o => o.FloorId == SelectedFloor.Id).FirstOrDefault() != null)
                            {
                                This.DrawWays(area.Ways[0], SelectedFloor.Id);
                            }
                    }
                       
                    //}
                }
            }
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
                    LoadFloorObjects();
                    Bitmap img = (Bitmap)System.Drawing.Image.FromFile(selectedFloor.Image);
                    CurrentFloorImage = Services.BitmapToImageSourceConverter.BitmapToImageSource(img);
                 
                }
                OnPropertyChanged("SelectedFloor");
            }
        }

        private BitmapImage currentFloorImage;
        public BitmapImage CurrentFloorImage
        {
            get { return currentFloorImage; }
            set
            {
                currentFloorImage = value;
                OnPropertyChanged("CurrentFloorImage");
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
        #endregion


        #region Работа с картой
        double scaleX = 1.0;
        double scaleY = 1.0;
        private RelayCommand zoomIn;
        public RelayCommand ZoomIn
        {
            get
            {
                return zoomIn ??
                    (zoomIn = new RelayCommand(obj =>
                    {
                        //This.ZoomIn();
                        var st = new ScaleTransform();
                        //var textBox = new TextBox { Text = "Test" };
                        This.canvasMap.RenderTransform = st;
                        //This.canvasMap.Children.Add(textBox);
                        st.ScaleX = scaleX * 1.4;
                        st.ScaleY = scaleY * 1.4;
                        scaleX = st.ScaleX;
                        scaleY = st.ScaleY;

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
                        //This.ZoomOut();
                        var st = new ScaleTransform();
                        //var textBox = new TextBox { Text = "Test" };
                        This.canvasMap.RenderTransform = st;
                        //This.canvasMap.Children.Add(textBox);
                        st.ScaleX = scaleX / 1.4;
                        st.ScaleY = scaleY / 1.4;
                        scaleX = st.ScaleX;
                        scaleY = st.ScaleY;
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
                        Storage.KioskObjects.SaveSettings();
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
                        LoadFloorObjects(); 
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
