using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NavigationMap.Controls
{
    public class CurvePolyLine : Shape
    {
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            "Points", typeof(PointCollection), typeof(CurvePolyLine), new PropertyMetadata(default(PointCollection)));

        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public static readonly DependencyProperty TensionProperty = DependencyProperty.Register(
            "Tension", typeof(double), typeof(CurvePolyLine), new PropertyMetadata(0.5));

        public double Tension
        {
            get => (double)GetValue(TensionProperty);
            set => SetValue(TensionProperty, value);
        }

        public static readonly DependencyProperty IterationCountProperty = DependencyProperty.Register(
            "IterationCount", typeof(int), typeof(CurvePolyLine), new PropertyMetadata(1));

        public int IterationCount
        {
            get => (int)GetValue(IterationCountProperty);
            set => SetValue(IterationCountProperty, value);
        }

        protected override Geometry DefiningGeometry => DefineGeometry();

        private Geometry DefineGeometry()
        {
            if (Points.Count < 3)
            {
                Points.Add(Points[0]);
            }

            Point[] points = Points
                .Select(p => new PointD(p.X, p.Y))
                .ToList()
                .GetSmoothCurve(Tension, IterationCount)
                .Select(p => new Point(p.X, p.Y))
                .ToArray();

            PathFigure pathFigure = new PathFigure
            {
                StartPoint = points[0]
            };

            pathFigure.Segments.Add(new PolyBezierSegment(points, true));

            PathFigureCollection pthFigureCollection = new PathFigureCollection
            {
                pathFigure
            };

            PathGeometry pthGeometry = new PathGeometry
            {
                Figures = pthFigureCollection
            };

            return pthGeometry;
        }
    }

    public class PointD
    {
        public PointD()
        {
        }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD(PointD p)
        {
            X = p.X;
            Y = p.Y;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public static PointD operator +(PointD p1, PointD p2)
        {
            return new PointD(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static PointD operator *(PointD p, double d)
        {
            return new PointD(p.X * d, p.Y * d);
        }

        public static PointD operator *(double d, PointD p)
        {
            return p * d;
        }
    }

    public static class InterpolateUtils
    {
        public static List<PointD> GetSmoothCurve(this List<PointD> points, double tension, int iterationCount)
        {
            if (points == null || points.Count < 3)
            {
                return null;
            }

            iterationCount = iterationCount switch
            {
                < 1 => 1,
                > 10 => 10,
                _ => iterationCount
            };

            tension = tension switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => tension
            };

            double cutDistance = 0.05 + (tension * 0.4);

            List<PointD> result = new List<PointD>();

            for (int i = 0; i <= points.Count - 1; i++)
            {
                result.Add(new PointD(points[i]));
            }

            for (int i = 1; i <= iterationCount; i++)
            {
                result = GetSmoothCurve(result, cutDistance);
            }

            return result;
        }
        public static List<PointD> GetSmoothCurve(this List<PointD> points, double cutDistance)
        {
            List<PointD> result = new List<PointD>
            {
                new PointD(points[0])
            };

            for (int i = 0; i <= points.Count - 2; i++)
            {
                PointD p1 = (1 - cutDistance) * points[i] + cutDistance * points[i + 1];
                PointD p2 = cutDistance * points[i] + (1 - cutDistance) * points[i + 1];

                result.Add(p1);
                result.Add(p2);
            }

            result.Add(new PointD(points.LastOrDefault()));

            return result;
        }
    }
}
