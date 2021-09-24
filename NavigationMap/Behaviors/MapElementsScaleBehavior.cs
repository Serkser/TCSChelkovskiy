using Microsoft.Xaml.Behaviors;

using System.Windows;
using System.Windows.Media;

namespace NavigationMap.Behaviors
{
    public class MapElementsScaleBehavior : Behavior<FrameworkElement>
    {
        private State _state;

        protected override void OnAttached()
        {
            _state = State.Instance;

            _state.OnMapScaleChanged += _mainState_OnMapScaleChanged;

            base.OnAttached();

            ScaleObject(_state.MapScale);
        }

        protected override void OnDetaching()
        {
            _state.OnMapScaleChanged -= _mainState_OnMapScaleChanged;

            _state = null;

            base.OnDetaching();
        }

        private void _mainState_OnMapScaleChanged(double mapScale)
        {
            ScaleObject(mapScale);
        }

        private void ScaleObject(double mapScale)
        {
            double scale = 1 / mapScale;

            AssociatedObject.RenderTransform = new ScaleTransform(scale, scale);
        }
    }
}
