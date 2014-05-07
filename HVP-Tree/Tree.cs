using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Tree {
        public List<Point> allPoints;
        private Node root;

        public Tree(List<Point> p) {
            allPoints = p;
        }

        public void Build() {
            root = RecursiveVPBuilder(allPoints.Select(i => i).ToList());
        }

        private Node RecursiveVPBuilder(List<Point> points) {
            if( points.Count == 0 )
                return null;

            Node node = new Node();
            node.Point = ChooseVP(points);
            points.Remove(node.Point);

            if( points.Count == 0 )
                return node;

            node.Median = node.Point.Distances(points).Median();
            Tuple<List<Point>, List<Point>> subtrees = GetSubtrees(points, node.Point, node.Median);
            List<Point> leftPoints = subtrees.Item1;
            List<Point> rightPoints = subtrees.Item2;

            node.LS = RecursiveVPBuilder(leftPoints);
            node.RS = RecursiveVPBuilder(rightPoints);

            return node;
        }

        #region helpers
        // dumb version searching whole dataset
        private Point ChooseVP(List<Point> points) {
            Point vp = new Point();
            double maxStandardDeviation = 0;

            foreach( Point current in points ) {
                List<double> distances = current.Distances(points);

                if( points.Count <= 2 )
                    return current;

                double sd = distances.StandardDeviation();
                if( sd >= maxStandardDeviation ) {
                    maxStandardDeviation = sd;
                    vp = current;
                }
            }

            return vp;
        }

        private Tuple<List<Point>, List<Point>> GetSubtrees(List<Point> points, Point vp, double median) {
            List<Point> L = new List<Point>();
            List<Point> R = new List<Point>();

            foreach( Point p in points ) {
                if( Utilities.Distance(vp, p) > median )
                    R.Add(p);
                else
                    L.Add(p);
            }

            return new Tuple<List<Point>,List<Point>>(L, R);
        }
        #endregion

        #region search
        public List<Node> SearchKNearest(Point target, int k) {
            double tau = double.PositiveInfinity;
            List<NodeWithDistance> queue = new List<NodeWithDistance>();
            KNearest(root, target, k, ref queue, ref tau);

            Console.WriteLine(String.Format("Searching for point: {0}. {1}x{2}", target.Id, target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1)));
            foreach( NodeWithDistance n in queue ) {
                Console.WriteLine(String.Format("{0}. {1}", n.Point.Id, n.distance));
            }

            return null;
        }

        private void KNearest(Node node, Point target, int k, ref List<NodeWithDistance> queue, ref double tau) {
            if( node == null )
                return;

            double distance = Utilities.Distance(node.Point, target);

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

        public List<Node> SearchEpsNB(Point target, double eps) {
            List<Node> queue = new List<Node>();
            EpsNB(root, target, ref queue, eps);

            Console.WriteLine(String.Format("EPS Searching for point: {0}. {1}x{2}, eps={3}", target.Id, target.Coordinates.ElementAt(0), target.Coordinates.ElementAt(1), eps));
            foreach( Node n in queue ) {
                Console.WriteLine(String.Format("{0}. {1}x{2}", n.Point.Id, n.Point.Coordinates.ElementAt(0), n.Point.Coordinates.ElementAt(1)));
            }

            return null;
        }

        public void EpsNB(Node node, Point target, ref List<Node> queue, double eps) {
            if( node == null )
                return;

            double distance = Utilities.Distance(node.Point, target);
            if( distance <= eps && node.Point.Id != target.Id ) {
                queue.Add(node);
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
