using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Output {
        private static string statsFile = "stats.txt";
        private static string resultsFile = "results.txt";

        #region tree text
        public static string TreeResults() {
            StringBuilder sb = new StringBuilder("", 500);
            sb.AppendLine("Tree creation data:");
            sb.AppendFormat("- distance metric = {0}{1}", Tree.distanceMeteric, Environment.NewLine);
            sb.AppendFormat("- choosing root point = {0}{1}\n", Tree.ifRandom ? "random" : "max std. deviation", Environment.NewLine);

            sb.AppendFormat("- no. nodes = {0}{1}", TreeCounters.Nodes, Environment.NewLine);
            sb.AppendFormat("- runtime = {0}{1}", TreeCounters.Runtime, Environment.NewLine);
            sb.AppendFormat("- distance calculations = {0}{1}\n", TreeCounters.DistanceCalculations, Environment.NewLine);

            sb.AppendFormat("- max path length = {0}{1}", TreeCounters.PathsLength.Max(), Environment.NewLine);
            sb.AppendFormat("- min path length = {0}{1}", TreeCounters.PathsLength.Min(), Environment.NewLine);
            sb.AppendFormat("- avg path length = {0}{1}\n", TreeCounters.PathsLength.Average(), Environment.NewLine);
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        #region k nearest text
        public static string KNearestResults(Point target, List<NodeWithDistance> found, int k, double tau) {
            StringBuilder sb = new StringBuilder("", 500);
            sb.AppendLine("k-Nearest search:");
            sb.AppendFormat("- k = {0}", k).AppendLine();
            sb.AppendFormat("- target point id = {0}\n", target.Id).AppendLine();
            sb.AppendFormat("- target point coords = {0}x{1}\n\n", target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1)).AppendLine();
            sb.AppendFormat("- tau = {0}\n", tau).AppendLine();

            sb.AppendLine("- found points (id and distance to target):");
            foreach( NodeWithDistance n in found ) {
                sb.AppendFormat("  - {0}. {1}\n", n.Point.Id, Math.Round(n.distance)).AppendLine();
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public static string KNearestStats(Point target, int k) {
            StringBuilder sb = new StringBuilder("", 500);
            sb.AppendLine("k-Nearest search stats:");
            sb.AppendFormat("- k = {0}\n", k).AppendLine();
            sb.AppendFormat("- target point id = {0}\n", target.Id).AppendLine();
            sb.AppendFormat("- target point coords = {0}x{1}\n\n", target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1)).AppendLine();

            sb.AppendFormat("- nodes visited = {0}\n", KNearestCounters.NodesVisited).AppendLine();
            sb.AppendFormat("- runtime = {0}\n", KNearestCounters.Runtime).AppendLine();
            sb.AppendFormat("- distance calculations = {0}\n\n", KNearestCounters.DistanceCalculations).AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        #region eps text
        public static string EpsNBResults(Point target, List<NodeWithDistance> found, double eps) {
            StringBuilder sb = new StringBuilder("Eps-neighborhood search:", 500);
            sb.AppendFormat("- eps = {0}{1}", eps, Environment.NewLine);
            sb.AppendFormat("- target point id = {0}{1}", target.Id, Environment.NewLine);
            sb.AppendFormat("- target point coords = {0}x{1}{2}", target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1), Environment.NewLine);

            sb.AppendLine("- found points (id and distance to target):");
            foreach( NodeWithDistance n in found ) {
                sb.AppendFormat("  - {0}. {1}{2}", n.Point.Id, Math.Round(n.distance), Environment.NewLine);
            }
            sb.AppendLine();

            return sb.ToString();
        }

        public static string EPSNbStats(Point target, int foundCount, double eps) {
            StringBuilder sb = new StringBuilder("", 500);
            sb.AppendLine("Eps-neighborhood search stats:");
            sb.AppendFormat("- eps = {0}\n", eps).AppendLine();
            sb.AppendFormat("- target point id = {0}\n", target.Id).AppendLine();
            sb.AppendFormat("- target point coords = {0}x{1}\n", target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1)).AppendLine();
            sb.AppendFormat("- found points count = {0}\n\n", foundCount).AppendLine();

            sb.AppendFormat("- nodes visited = {0}\n", KNearestCounters.NodesVisited).AppendLine();
            sb.AppendFormat("- runtime = {0}\n", KNearestCounters.Runtime).AppendLine();
            sb.AppendFormat("- distance calculations = {0}\n\n", KNearestCounters.DistanceCalculations).AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
        #endregion

        #region helpers
        public static void WriteToResults(string text) {
            WriteToFile(text, resultsFile);
        }

        public static void WriteToStats(string text) {
            WriteToFile(text, statsFile);
        }

        private static void WriteToFile(string text, string path) {
            File.AppendAllText(path, text);
        }
        #endregion
    }
}
