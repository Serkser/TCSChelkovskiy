using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace NavigationMap.Helpers
{
    public static class GeometryHelper
    {
        public static Point GetMiddlePoint(IEnumerable<Point> points)
        {
            if (points is null)
            {
                throw new NullReferenceException(nameof(points));
            }

            double totalX = 0;
            double totalY = 0;

            foreach (Point point in points)
            {
                totalX += point.X;
                totalY += point.Y;
            }

            double centerX = totalX / points.Count();
            double centerY = totalY / points.Count();

            Point position = new(centerX, centerY);

            return position;
        }
    }
}
