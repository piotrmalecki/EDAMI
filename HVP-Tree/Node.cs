using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Point {
        public int Id;
        public List<float> Coordinates;

        public double StandardDeviation;

        public Point() { }

        public Point(int i, float[] p) {
            Id = i;
            Coordinates = p.ToList();
        }

        public Point(int i, List<float> p) {
            Id = i;
            Coordinates = p;
        }

        public List<double> Distances(List<Point> points) {
            List<double> dist = new List<double>();

            foreach( Point p in points ) {
                if(Id != p.Id)
                    dist.Add(Utilities.Distance(this, p));
            }

            return dist;
        }
    }

    public class Node {
        // median
        private double _median;
        public double Median {
            get {
                return _median;
            }
            set {
                _median = value;
            }
        }

        // left and right subtree
        public Node LS;
        public Node RS;

        // point which node contains
        public Point Point;

        public Node() {

        }

        public Node(double m) {
            _median = m;
        }

        public Node(double m, Node L, Node R) {
            _median = m;
            LS = L;
            RS = R;
        }
    }

    public class NodeWithDistance : Node {
        public double distance;

        public NodeWithDistance() {

        }

        public NodeWithDistance(Node n, double d) {
            Median = n.Median;
            LS = n.LS;
            RS = n.RS;

            Point = n.Point;

            distance = d;
        }
    }
}
