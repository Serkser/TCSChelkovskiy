using Microsoft.Win32;
using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeCenterAdmin.ViewModels
{
    public class FloorsWindowViewModel : INotifyPropertyChanged
    {
        public FloorsWindowViewModel()
        {
            Floors = Storage.KioskObjects.Floors;
            NewFloor = new Floor();
            if (Floors.Count > 0)
            {
                SelectedFloor = Floors.FirstOrDefault();
            }
            else
            {
                SelectedFloor = new Floor();
            }
        }

        #region Свойства окон действий с этажами
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
                if (value == null)
                {
                    selectedFloor = new Floor();
                }
                OnPropertyChanged("SelectedFloor");
            }
        }

        private Floor newFloor;
        public Floor NewFloor
        {
            get { return newFloor; }
            set
            {
                newFloor = value;
                if (value == null)
                {
                    newFloor = new Floor();
                }
                OnPropertyChanged("NewFloor");
            }
        }
        private string newFloorPhotoFilepath;
        public string NewFloorPhotoFilepath
        {
            get { return newFloorPhotoFilepath; }
            set
            {
                newFloorPhotoFilepath = value;
                OnPropertyChanged("NewFloorPhotoFilepath");
            }
        }

        #endregion

        #region Команды работы с картой
        private RelayCommand selectMap;
        public RelayCommand SelectMap
        {
            get
            {
                return selectMap ??
                    (selectMap = new RelayCommand(obj =>
                    {
                        OpenFileDialog ofd = new OpenFileDialog();
                        if (ofd.ShowDialog() == true)
                        {
                            NewFloorPhotoFilepath = ofd.SafeFileName;
                            NewFloor = new Floor();
                            NewFloor.Image = ofd.FileName;
                        }
                    }));
            }
        }
        private RelayCommand removeMap;
        public RelayCommand RemoveMap
        {
            get
            {
                return removeMap ??
                    (removeMap = new RelayCommand(obj =>
                    {
                        NewFloorPhotoFilepath = string.Empty;
                        NewFloor.Image = string.Empty;
                    }));
            }
        }
        #endregion


        #region Комманды CRUD
        private RelayCommand addFloor;
        public RelayCommand AddFloor
        {
            get
            {
                return addFloor ??
                    (addFloor = new RelayCommand(obj =>
                    {
                        Random rnd = new Random();
                        if (string.IsNullOrEmpty(NewFloor.Name))
                        {
                            MessageBox.Show("Укажите название этажа");
                            return;
                        }
                        
                        NewFloor.Id = rnd.Next(1, Int32.MaxValue);
                        Storage.KioskObjects.Floors.Add(NewFloor);
                        NewFloor = new Floor();
                    }));
            }
        }

        private RelayCommand editFloor;
        public RelayCommand EditFloor
        {
            get
            {
                return editFloor ??
                    (editFloor = new RelayCommand(obj =>
                    {
                        if (string.IsNullOrEmpty(SelectedFloor.Name))
                        {
                            MessageBox.Show("Укажите название этажа");
                            return;
                        }
                        var old=  Storage.KioskObjects.Floors.Where(o => o.Id == SelectedFloor.Id).FirstOrDefault();
                        Storage.KioskObjects.Floors.Remove(old);
                        Storage.KioskObjects.Floors.Add(SelectedFloor);
                    }));
            }
        }
        private RelayCommand removeFloor;
        public RelayCommand RemoveFloor
        {
            get
            {
                return removeFloor ??
                    (removeFloor = new RelayCommand(obj =>
                    {
                        Storage.KioskObjects.Floors.Remove(SelectedFloor);
                    }));
            }
        }
        #endregion


        #region Команды открытия окон
        private RelayCommand openEditFloor;
        public RelayCommand OpenEditFloor
        {
            get
            {
                return openEditFloor ??
                    (openEditFloor = new RelayCommand(obj =>
                    {
                        Views.Windows.EditFloor f = new Views.Windows.EditFloor();
                        f.DataContext = this; f.Show();
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
