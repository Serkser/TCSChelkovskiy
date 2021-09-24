using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NavigationMap.Core
{
    public class MatrixAnimation : MatrixAnimationBase
    {
        public static DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(Matrix?), typeof(MatrixAnimation),
                new PropertyMetadata(null));

        public static DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(Matrix?), typeof(MatrixAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(MatrixAnimation),
                new UIPropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return new MatrixAnimation();
        }

        public IEasingFunction EasingFunction
        {
            get => (IEasingFunction)GetValue(EasingFunctionProperty);
            set => SetValue(EasingFunctionProperty, value);
        }

        protected override Matrix GetCurrentValueCore(Matrix defaultOriginValue, Matrix defaultDestinationValue,
            AnimationClock animationClock)
        {
            if (animationClock.CurrentProgress == null)
            {
                return Matrix.Identity;
            }

            double normalizedTime = animationClock.CurrentProgress.Value;

            if (EasingFunction != null)
            {
                normalizedTime = EasingFunction.Ease(normalizedTime);
            }

            Matrix from = From ?? defaultOriginValue;
            Matrix to = To ?? defaultDestinationValue;

            Matrix newMatrix = new Matrix(
                (to.M11 - from.M11) * normalizedTime + from.M11,
                (to.M12 - from.M12) * normalizedTime + from.M12,
                (to.M21 - from.M21) * normalizedTime + from.M21,
                (to.M22 - from.M22) * normalizedTime + from.M22,
                (to.OffsetX - from.OffsetX) * normalizedTime + from.OffsetX,
                (to.OffsetY - from.OffsetY) * normalizedTime + from.OffsetY);

            return newMatrix;
        }

        public Matrix? From
        {
            set => SetValue(FromProperty, value);
            get => (Matrix?)GetValue(FromProperty);
        }

        public Matrix? To
        {
            set => SetValue(ToProperty, value);
            get => (Matrix)GetValue(ToProperty);
        }

        public MatrixAnimation()
        {
        }

        public MatrixAnimation(Matrix toValue, Duration duration)
        {
            To = toValue;
            Duration = duration;
        }

        public MatrixAnimation(Matrix toValue, Duration duration, FillBehavior fillBehavior)
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        public MatrixAnimation(Matrix fromValue, Matrix toValue, Duration duration)
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }

        public MatrixAnimation(Matrix fromValue, Matrix toValue, Duration duration, FillBehavior fillBehavior)
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }
    }
}