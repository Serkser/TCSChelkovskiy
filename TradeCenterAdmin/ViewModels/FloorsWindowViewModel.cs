using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ViewModels
{
    public class FloorsWindowViewModel : INotifyPropertyChanged
    {


        #region Свойства окон действий с этажами
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

        #region Комманды работы с картой
        private RelayCommand selectMap;
        public RelayCommand SelectMap
        {
            get
            {
                return selectMap ??
                    (selectMap = new RelayCommand(obj =>
                    {

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
