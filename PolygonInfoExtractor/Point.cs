using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonInfoExtractor
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"X = {X:f3}, Y = {Y:f3}";
        }
    }
}
