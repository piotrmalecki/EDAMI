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
        //    - eps - eps value
        //    - k - k value for searching nearest nb
        //    - all / eps / k - what we want to calculate
        //    - 0 - id of target point

        static void Main(string[] args) {
            bool calcAll = false;
            if( args.Length < 6 ) {
                Console.WriteLine(String.Format("Wrong arguments!"));
                Console.ReadKey();

                return;
            }

            if( args.Length == 6 )
                calcAll = true;

            // get arguments
            Tree.ifRandom = args[1] == "r" ? true : false;
            Tree.distanceMeteric = args[2] == "m" ? "manhattan" : "euclidean";

            double eps = double.Parse(args[3]);
            int k = Convert.ToInt32(args[4]);
            string mode = args[5];

            // create tree
            Tree tree = new Tree(Utilities.GetData(args[0]));
            tree.Build();

            if( calcAll ) {
                foreach( Point p in tree.allPoints ) {
                    if( mode == "all" ) {
                        tree.SearchKNearest(p, k, false);
                        tree.SearchEpsNB(p, eps, false);
                    }
                    else if( mode == "eps" ) {
                        tree.SearchEpsNB(p, eps, false);
                    }
                    else if( mode == "k" ) {
                        tree.SearchKNearest(p, k, false);
                    }
                }

                if( mode == "all" ) {
                    Output.AllStatsEpsNB(eps);
                    Output.AllStatsKNearest(k);
                }
                else if( mode == "eps" ) {
                    Output.AllStatsEpsNB(eps);
                }
                else if( mode == "k" ) {
                    Output.AllStatsKNearest(k);
                }
            }
            else {
                int id = Convert.ToInt32(args[6]);

                // search k nearest
                tree.SearchKNearest(tree.allPoints.First(p => p.Id == id), k, true);

                // search eps nb
                tree.SearchEpsNB(tree.allPoints.First(p => p.Id == id), eps, true);
            }

            Console.Write("Program commands:\n  kNearest k pointId\n  epsNB eps pointId\n  exit\n\n");
            string command;
            while( ( command = Console.ReadLine() ) != "exit" ) {
                string[] arguments = command.Split(' ');

                if( arguments[0] == "kNearest" ) {
                    Point point = tree.allPoints.First(p => p.Id == Convert.ToInt32(arguments[2]));

                    if( point != null )
                        tree.SearchKNearest(point, Convert.ToInt32(arguments[1]), true);
                }
                else if( arguments[0] == "epsNB" ) {
                    Point point = tree.allPoints.First(p => p.Id == Convert.ToInt32(arguments[2]));

                    if( point != null )
                        tree.SearchEpsNB(point, double.Parse(arguments[1]), true);
                }
            }
        }
    }
}