using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Program {
        //Arguments:
        //    - path to dataset
        //    - r / d - choosing root point by random or max std deviation
        //    - e / m - euclidean or manhattan distance metric
        //    - 0 - id of target point
        //    - eps - eps value
        //    - k - k value for searching nearest nb

        static void Main(string[] args) {
            if( args.Length < 6 ) {
                Console.WriteLine(String.Format("Wrong arguments!"));
                Console.ReadKey();

                return;
            }

            // get arguments
            Tree.ifRandom = args[1] == "r" ? true : false;
            Tree.distanceMeteric = args[2] == "m" ? "manhattan" : "euclidean";

            int id = Convert.ToInt32(args[3]);
            double eps = double.Parse(args[4]);
            int k = Convert.ToInt32(args[5]);

            // create tree
            Tree tree = new Tree(Utilities.GetData(args[0]));
            tree.Build();
            Point target = tree.allPoints.First(p => p.Id == id);

            if( target != null ) {
                // search k nearest
                tree.SearchKNearest(tree.allPoints.First(p => p.Id == id), k);

                // search eps nb
                tree.SearchEpsNB(tree.allPoints.First(p => p.Id == id), eps);
            }

            Console.Write("Program commands:\n  kNearest k pointId\n  epsNB eps pointId\n  exit\n\n");
            string command;
            while( ( command = Console.ReadLine() ) != "exit" ) {
                string[] arguments = command.Split(' ');

                if( arguments[0] == "kNearest" ) {
                    Point point = tree.allPoints.First(p => p.Id == Convert.ToInt32(arguments[2]));

                    if( point != null )
                        tree.SearchKNearest(point, Convert.ToInt32(arguments[1]));
                }
                else if( arguments[0] == "epsNB" ) {
                    Point point = tree.allPoints.First(p => p.Id == Convert.ToInt32(arguments[2]));

                    if( point != null )
                        tree.SearchEpsNB(point, double.Parse(arguments[1]));
                }
            }

            Console.ReadKey();
        }
    }
}

//Tree.ifRandom = false;
//            Tree tree = new Tree(Utilities.GetData("../../sequoia-1200.txt"));
//            tree.Build();

//            Console.WriteLine(String.Format("While building VP-tree program calculated distance: {0} times", Counters.DistanceCalculations));

//            Console.Write("Program commands:\n  kNearest k pointId\n  epsNB eps pointId\n\n");

//            int count = Counters.DistanceCalculations;
//            tree.SearchKNearest(tree.allPoints.Where(p => p.Id == 0).First(), 2);
//            Console.WriteLine(String.Format("Program calculated distance: {0} times", Counters.DistanceCalculations - count));

//            Console.WriteLine();

//            count = Counters.DistanceCalculations;
//            tree.SearchEpsNB(tree.allPoints.Where(p => p.Id == 0).First(), 1.5);
//            Console.WriteLine(String.Format("Program calculated distance: {0} times\n\nType command:", Counters.DistanceCalculations - count));
            
//            string command;
//            while( ( command = Console.ReadLine() ) != "exit" ) {
//                count = Counters.DistanceCalculations;
//                string[] arguments = command.Split(' ');

//                if( arguments[0] == "kNearest" ) {
//                    int k = Convert.ToInt32(arguments[1]);
//                    int id = Convert.ToInt32(arguments[2]);

//                    tree.SearchKNearest(tree.allPoints.Where(p => p.Id == id).First(), k);
//                }
//                else if( arguments[0] == "epsNB" ) {
//                    double eps = double.Parse(arguments[1]);
//                    int id = Convert.ToInt32(arguments[2]);

//                    tree.SearchEpsNB(tree.allPoints.Where(p => p.Id == id).First(), eps);
//                }

//                Console.WriteLine(String.Format("Program calculated distance: {0} times\n\nxType command:", Counters.DistanceCalculations - count));
//            }

//            Console.ReadKey();
