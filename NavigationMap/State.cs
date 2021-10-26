using System;
using NavigationMap.Models;

namespace NavigationMap
{
    public class State
    {
        private State() { }

        private static readonly object LockerObject = new();

        private static State _instance;

        public static State Instance
        {
            get
            {
                if (_instance is not null)
                {
                    return _instance;
                }

                lock (LockerObject)
                {
                    _instance = new State();
                }

                return _instance;
            }
        }

        public event Action<Area> OnAreaSelected;
        public event Action<WC> OnWCSelected;
        public event Action<ATM> OnATMSelected;

        public event Action<double> OnMapScaleChanged;

        public void SelectArea(Area area)
        {
            OnAreaSelected?.Invoke(area);
        }
        public void SelectWC(WC wc)
        {
            OnWCSelected?.Invoke(wc);
        }
        public void SelectATM(ATM atm)
        {
            OnATMSelected?.Invoke(atm);
        }
        public void ChangeMapScale(double mapScale)
        {
            MapScale = mapScale;
        }

        private double _mapScale = 1;

        public double MapScale
        {
            get => _mapScale;
            set
            {
                _mapScale = value;

                OnMapScaleChanged?.Invoke(value);
            }
        }
    }
}
