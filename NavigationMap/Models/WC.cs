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
    public class WC : Station, ICloneable
    {
        private State _state;
        public WC()
        {
            _state = State.Instance;
            AreaPoint = new AreaPoint();
        }
        private RelayCommand _selectWCCommand;
        public RelayCommand SelectWCCommand =>
            _selectWCCommand ??= new RelayCommand(obj =>
            {
              
                _state.SelectWC(this);
            });
        public new object Clone()
        {
            WC station = (WC)this.MemberwiseClone();
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
