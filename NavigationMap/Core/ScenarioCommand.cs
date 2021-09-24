using System;
using System.Windows.Media.Animation;

namespace NavigationMap.Core
{
    public class ScenarioCommand : ObservableObject
    {
        private MatrixAnimationBase _animation;

        public MatrixAnimationBase Animation
        {
            get => _animation;
            set
            {
                _animation = value;
                OnPropertyChanged();
            }
        }

        private Action _scenarioBeforeAction;

        public Action ScenarioBeforeAction
        {
            get => _scenarioBeforeAction;
            set
            {
                _scenarioBeforeAction = value;
                OnPropertyChanged();
            }
        }

        private Action _scenarioAfterAction;

        public Action ScenarioAfterAction
        {
            get => _scenarioAfterAction;
            set
            {
                _scenarioAfterAction = value;
                OnPropertyChanged();
            }
        }
    }
}
