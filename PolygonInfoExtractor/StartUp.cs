using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace PolygonInfoExtractor
{
    class StartUp
    {
        static void Main(string[] args)
        {
            List<string> resultText = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader("../../../input.txt"))
                {
                    string pattern = @"(?<polygon>{""type"":""Polygon"".*?})";
                    string line = string.Empty;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (Regex.IsMatch(line, pattern))
                        {
                            string currentPolygon = Regex.Match(line, pattern).Groups["polygon"].Value;
                            string coordPattern = @"\[(?<x>\d+(.\d+)*),(?<y>\d+(.\d+)*)\]";
                            Regex rgx = new Regex(coordPattern);
                            MatchCollection points = rgx.Matches(currentPolygon);
                            List<Point> allPoints = new List<Point>();

                            foreach (Match match in points)
                            {
                                double x = double.Parse(match.Groups["x"].Value);
                                double y = double.Parse(match.Groups["y"].Value);
                                Point point = new Point(x, y);
                                allPoints.Add(point);
                            }

                            double area = Area(allPoints);
                            Point centerPoint = new Point(FindCentroid(allPoints, area).Item1,
                                FindCentroid(allPoints, area).Item2);

                            StringBuilder result = new StringBuilder();
                            result.AppendLine("Polygon Coordinates:");
                            result.AppendLine(currentPolygon);
                            result.AppendLine("Center Point:");
                            result.AppendLine(centerPoint.ToString());
                            result.AppendLine("----------------------");
                            resultText.Add(result.ToString());
                        }
                    }

                    using (StreamWriter writer = new StreamWriter("../../../output.txt"))
                    {
                        foreach (string polygonInfo in resultText)
                        {
                            writer.Write(polygonInfo);
                        }
                    }

                    Console.WriteLine("Your output file is ready!");
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static double Area(List<Point> vertices)
        {
            vertices.Add(vertices[0]);
            return Math.Abs(vertices.Take(vertices.Count - 1)
                .Select((p, i) => (p.X * vertices[i + 1].Y) - (p.Y * vertices[i + 1].X)).Sum() / 2);
        }

        // Find the polygon's centroid.
        public static ValueTuple<double, double> FindCentroid(List<Point> points, double area)
        {
            points.Add(points[0]);

            double x = 0;
            double y = 0;
            double secondFactor = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                secondFactor = points[i].X * points[i + 1].Y - points[i + 1].X * points[i].Y;

                x += (points[i].X + points[i + 1].X) * secondFactor;
                y += (points[i].Y + points[i + 1].Y) * secondFactor;
            }

            x /= (6 * area);
            y /= (6 * area);

            if (x < 0)
            {
                x = -x;
                y = -y;
            }

            return ValueTuple.Create(x, y);
        }
    }
}
