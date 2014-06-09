using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Utilities {
        public static double Distance(Point from, Point to) {
            if( Tree.distanceMeteric == "manhattan" )
                return ManhattanDistance(from, to);
            else
                return EuclideanDistance(from, to);
        }

        private static double EuclideanDistance(Point from, Point to) {
            if( from.Coordinates.Count != to.Coordinates.Count )
                throw new Exception();

            double result = 0;
            for( int i = 0; i < from.Coordinates.Count; i++ ) {
                result += Math.Pow(to.Coordinates.ElementAt(i) - from.Coordinates.ElementAt(i), 2);
            }

            Counters.DistanceCalculations++;
            return Math.Sqrt(result);
        }

        private static double ManhattanDistance(Point from, Point to) {
            if( from.Coordinates.Count != to.Coordinates.Count )
                throw new Exception();

            double result = 0;
            for( int i = 0; i < from.Coordinates.Count; i++ ) {
                result += Math.Abs(to.Coordinates.ElementAt(i) - from.Coordinates.ElementAt(i));
            }

            Counters.DistanceCalculations++;
            return result;
        }

        public static List<Point> GetData(string path) {
            List<Point> result = new List<Point>();
            string[] lines = System.IO.File.ReadAllLines(@path);

            foreach( string line in lines ) {
                List<float> coords = new List<float>();
                string[] coordinates = line.Split(' ');

                foreach( string coord in coordinates ) {
                    coords.Add(float.Parse(coord));
                }

                result.Add(new Point(result.Count, coords));
            }

            return result;
        }

        public static long GetTimestamp() {
            return (long)( DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)) ).TotalMilliseconds;
        }
    }

    #region counters
    public class Counters {
        public static int DistanceCalculations = 0;
    }

    public class TreeCounters {
        public static List<int> PathsLength = new List<int>();
        public static int Nodes = 0;
        public static long Runtime = 0;
        public static int DistanceCalculations = 0;
    }

    public class LeafCounters {
        public static List<int> Points = new List<int>();
    }

    public class KNearestCounters {
        public static int DistanceCalculations = 0;
        public static int NodesVisited = 0;
        public static long Runtime = 0;
    }

    public class EpsCounters {
        public static int DistanceCalculations = 0;
        public static int NodesVisited = 0;
        public static long Runtime = 0;
    }
    #endregion

    public static class ListMathExtensions {
        public static double Mean(this List<double> values) {
            return values.Sum() / values.Count;
        }

        public static double Variance(this List<double> values) {
            double variance = 0;
            double mean = values.Mean();

            for( int i = 0; i < values.Count; i++ ) {
                variance += Math.Pow(( values[i] - mean ), 2);
            }

            return variance / ( values.Count - 1 );
        }

        public static double StandardDeviation(this List<double> values) {
            double variance = values.Variance();

            return Math.Sqrt(variance);
        }

        public static double Median(this List<double> values) {
            List<double> copy = values.Select(s => s).ToList();
            
            copy.Sort();
            double[] sorted = copy.ToArray();

            int mid = values.Count / 2;
            double median = ( values.Count % 2 != 0 ) ? sorted[mid] : ( sorted[mid] + sorted[mid - 1] ) / 2;

            return median;
        }
    }
}
