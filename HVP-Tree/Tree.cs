using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Tree {
        public static string distanceMeteric = "euclidean";
        public static bool ifRandom = false;

        public List<Point> allPoints;
        private Node root;

        public Tree(List<Point> p) {
            allPoints = p;
        }

        public void Build() {
            TreeCounters.Runtime = Utilities.GetTimestamp();
            TreeCounters.DistanceCalculations = Counters.DistanceCalculations;

            if( !ifRandom )
                CalculateStandardDeviation();

            int pathLength = 0;
            root = RecursiveVPBuilder(allPoints.Select(i => i).ToList(), pathLength);
            TreeCounters.Runtime = Utilities.GetTimestamp() - TreeCounters.Runtime;
            TreeCounters.DistanceCalculations = Counters.DistanceCalculations - TreeCounters.DistanceCalculations;

            Output.WriteToStats(Output.TreeResults());
        }

        private Node RecursiveVPBuilder(List<Point> points, int pathLength) {
            if( points.Count == 0 )
                return null;

            Node node = new Node();
            TreeCounters.Nodes++;
            pathLength++;
            
            if(!ifRandom)
                node.Point = ChooseVP(points);
            else
                node.Point = RandomChooseVP(points);
            
            points.Remove(node.Point);

            if( points.Count == 0 ) {
                TreeCounters.PathsLength.Add(pathLength);

                return node;
            }

            List<double> distances = node.Point.Distances(points);
            node.Median = distances.Median();

            Tuple<List<Point>, List<Point>> subtrees = GetSubtrees(points, distances, node.Point, node.Median);
            List<Point> leftPoints = subtrees.Item1;
            List<Point> rightPoints = subtrees.Item2;

            node.LS = RecursiveVPBuilder(leftPoints, pathLength);
            node.RS = RecursiveVPBuilder(rightPoints, pathLength);

            return node;
        }

        #region helpers
        // dumb version searching whole dataset
        private void CalculateStandardDeviation() {
            foreach( Point current in allPoints ) {
                List<double> distances = current.Distances(allPoints);
                current.StandardDeviation = distances.StandardDeviation();
            }
        }

        private Point ChooseVP(List<Point> points) {
            double max = points.Max(p => p.StandardDeviation);

            return points.Where(p => p.StandardDeviation == max).First();
        }

        private Point RandomChooseVP(List<Point> points) {
            Random rnd = new Random();
            int index = rnd.Next(points.Count);

            return points.ElementAt(index);
        }

        private Tuple<List<Point>, List<Point>> GetSubtrees(List<Point> points, List<double> distances, Point vp, double median) {
            List<Point> L = new List<Point>();
            List<Point> R = new List<Point>();

            for(int i = 0; i < points.Count; i++) {
                if( distances.ElementAt(i) > median )
                    R.Add(points.ElementAt(i));
                else
                    L.Add(points.ElementAt(i));
            }

            return new Tuple<List<Point>,List<Point>>(L, R);
        }
        #endregion

        #region search
        public void SearchKNearest(Point target, int k, bool print) {
            double tau = double.PositiveInfinity;
            List<NodeWithDistance> queue = new List<NodeWithDistance>();

            KNearestCounters.DistanceCalculations = Counters.DistanceCalculations;
            KNearestCounters.Runtime = Utilities.GetTimestamp();

            KNearest(root, target, k, ref queue, ref tau);

            KNearestCounters.Runtime = Utilities.GetTimestamp() - KNearestCounters.Runtime;
            KNearestCounters.DistanceCalculations = Counters.DistanceCalculations - KNearestCounters.DistanceCalculations;

            if( print ) {
                Output.WriteToStats(Output.KNearestStats(target, k));
                Output.WriteToResults(Output.KNearestResults(target, queue, k, tau));
            }

            AllKNearestCounters.DistanceCalculations.Add((int)KNearestCounters.DistanceCalculations);
            AllKNearestCounters.NodesVisited.Add(KNearestCounters.NodesVisited);
            AllKNearestCounters.Runtime.Add(KNearestCounters.Runtime);

            KNearestCounters.Reset();
        }

        private void KNearest(Node node, Point target, int k, ref List<NodeWithDistance> queue, ref double tau) {
            if( node == null )
                return;

            double distance = Utilities.Distance(node.Point, target);
            KNearestCounters.NodesVisited++;

            if( distance < tau && node.Point.Id != target.Id) {
                if( queue.Count == k ) {
                    double maxDistance = queue.Max(n => n.distance);
                    queue.Remove(queue.Where(n => n.distance == maxDistance).First());
                }

                queue.Add(new NodeWithDistance(node, distance));

                if( queue.Count == k )
                    tau = queue.Max(n => n.distance);
            }

            if( node.LS == null && node.RS == null ) {
                return;
            }

            if( distance < node.Median ) {
                if( distance - tau <= node.Median )
                    KNearest(node.LS, target, k, ref queue, ref tau);

                if( distance + tau > node.Median )
                    KNearest(node.RS, target, k, ref queue, ref tau);
            }
            else {
                if( distance + tau > node.Median )
                    KNearest(node.RS, target, k, ref queue, ref tau);

                if( distance - tau <= node.Median )
                    KNearest(node.LS, target, k, ref queue, ref tau);
            }
        }

        public void SearchEpsNB(Point target, double eps, bool print) {
            List<NodeWithDistance> queue = new List<NodeWithDistance>();

            EpsCounters.DistanceCalculations = Counters.DistanceCalculations;
            EpsCounters.Runtime = Utilities.GetTimestamp();

            EpsNB(root, target, ref queue, eps);

            EpsCounters.Runtime = Utilities.GetTimestamp() - EpsCounters.Runtime;
            EpsCounters.DistanceCalculations = Counters.DistanceCalculations - EpsCounters.DistanceCalculations;

            if( print ) {
                Output.WriteToStats(Output.EPSNbStats(target, queue.Count, eps));
                Output.WriteToResults(Output.EpsNBResults(target, queue, eps));
            }

            AllEpsCounters.DistanceCalculations.Add((int)EpsCounters.DistanceCalculations);
            AllEpsCounters.NodesVisited.Add(EpsCounters.NodesVisited);
            AllEpsCounters.Runtime.Add(EpsCounters.Runtime);

            EpsCounters.Reset();
        }

        public void EpsNB(Node node, Point target, ref List<NodeWithDistance> queue, double eps) {
            if( node == null )
                return;

            double distance = Utilities.Distance(node.Point, target);
            EpsCounters.NodesVisited++;

            if( distance <= eps && node.Point.Id != target.Id ) {
                queue.Add(new NodeWithDistance(new Node() { Point = node.Point }, distance));
            }

            if( node.LS == null && node.RS == null ) {
                return;
            }

            if( distance - node.Median < eps ) {
                EpsNB(node.LS, target, ref queue, eps);
            }

            if( node.Median - distance <= eps ) {
                EpsNB(node.RS, target, ref queue, eps);
            }
        }
        #endregion
    }
}
