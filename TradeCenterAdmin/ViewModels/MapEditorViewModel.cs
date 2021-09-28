using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.Enums;

namespace TradeCenterAdmin.ViewModels
{
    public class MapEditorViewModel : INotifyPropertyChanged
    {

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
        private List<Floor> floors;
        public List<Floor> Floors
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
                OnPropertyChanged("SelectedFloor");
            }
        }
        #endregion

        #region Команды выбора инструментов
        private RelayCommand useHand;
        public RelayCommand UseHand
        {
            get
            {
                return useHand ??
                    (useHand = new RelayCommand(obj =>
                    {
                        MapEditorTool = MapEditorTool.Hand;
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
        #endregion


        #region Прочие комманды
        private RelayCommand openFloors;
        public RelayCommand OpenFloors
        {
            get
            {
                return openFloors ??
                    (openFloors = new RelayCommand(obj =>
                    {
                        Views.Windows.Floors f = new Views.Windows.Floors();
                        //f.DataContext = this;
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
