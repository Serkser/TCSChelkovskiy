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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TradeCenterAdmin.Enums;

namespace TradeCenterAdmin.ViewModels
{
    public class MapEditorViewModel : INotifyPropertyChanged
    {
        Views.Pages.MapEditor This;
        public MapEditorViewModel(Views.Pages.MapEditor _this)
        {
            This = _this;
            Floors = Storage.KioskObjects.Floors;
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
        private Floor selectedFloor;
        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
                if (value != null)
                {
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
        private RelayCommand leftMouseButtonClick;
        public RelayCommand LeftMouseButtonClick
        {
            get
            {
                return leftMouseButtonClick ??
                    (leftMouseButtonClick = new RelayCommand(obj =>
                    {
                        switch (MapEditorTool)
                        {
                            case MapEditorTool.Cursor:

                                break;
                            case MapEditorTool.Kiosk:
                                
                                break;
                        }
                    }));
            }
        }
        private RelayCommand rightMouseButtonClick;
        public RelayCommand RightMouseButtonClick
        {
            get
            {
                return rightMouseButtonClick ??
                    (rightMouseButtonClick = new RelayCommand(obj =>
                    {

                    }));
            }
        }


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

        #endregion


        #region Прочие команды
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
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
