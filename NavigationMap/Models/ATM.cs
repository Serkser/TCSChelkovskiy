using NavigationMap.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationMap.Models
{
    public class ATM : Station, ICloneable
    {
        private State _state;

        public ATM()
        {
            _state = State.Instance;
            AreaPoint = new AreaPoint();
        }
        private RelayCommand _selectATMCommand;
        public RelayCommand SelectATMCommand =>
            _selectATMCommand ??= new RelayCommand(obj =>
            {
                _state.SelectATM(this);
            });

        public new object Clone()
        {
            ATM station = (ATM)this.MemberwiseClone();
            station.AreaPoint = new AreaPoint
            {
                FloorId = this.AreaPoint.FloorId,
                AreaId = this.AreaPoint.AreaId,
                StationId = this.AreaPoint.StationId,
                Id = this.AreaPoint.Id,
                PointType = this.AreaPoint.PointType,
                Position = this.AreaPoint.Position,
                X = this.AreaPoint.X,
                Y = this.AreaPoint.Y
            };
            return station;
        }
    }
}
