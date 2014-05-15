using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVP_Tree {
    public class Program {
        static void Main(string[] args) {
            bool ifRandom = false;
            if( args.Length >= 1 && args[0] == "random") {
                ifRandom = true;
            }
            Tree tree = new Tree(Utilities.GetData("../../sequoia-1200.txt"));
            tree.Build(ifRandom);

            Console.WriteLine(String.Format("While building VP-tree program calculated distance: {0} times", Counters.DistanceCalculations));

            Console.Write("Program commands:\n  kNearest k pointId\n  epsNB eps pointId\n\n");

            int count = Counters.DistanceCalculations;
            tree.SearchKNearest(tree.allPoints.Where(p => p.Id == 0).First(), 2);
            Console.WriteLine(String.Format("Program calculated distance: {0} times", Counters.DistanceCalculations - count));

            Console.WriteLine();

            count = Counters.DistanceCalculations;
            tree.SearchEpsNB(tree.allPoints.Where(p => p.Id == 0).First(), 1.5);
            Console.WriteLine(String.Format("Program calculated distance: {0} times\n\nType command:", Counters.DistanceCalculations - count));
            
            string command;
            while( ( command = Console.ReadLine() ) != "exit" ) {
                count = Counters.DistanceCalculations;
                string[] arguments = command.Split(' ');

                if( arguments[0] == "kNearest" ) {
                    int k = Convert.ToInt32(arguments[1]);
                    int id = Convert.ToInt32(arguments[2]);

                    tree.SearchKNearest(tree.allPoints.Where(p => p.Id == id).First(), k);
                }
                else if( arguments[0] == "epsNB" ) {
                    double eps = double.Parse(arguments[1]);
                    int id = Convert.ToInt32(arguments[2]);

                    tree.SearchEpsNB(tree.allPoints.Where(p => p.Id == id).First(), eps);
                }

                Console.WriteLine(String.Format("Program calculated distance: {0} times\n\nxType command:", Counters.DistanceCalculations - count));
            }

            Console.ReadKey();
        }
    }
}
