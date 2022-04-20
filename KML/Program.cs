using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "KMLang by Kaab";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;



            Log.Success("KMLang By Kaab | Github : SchwarzSchlange ");
            while(!Global.isError)
            {
                
                Console.Write(">>>");
                string line = Console.ReadLine();

                List<Token> tokens = new List<Token>();
                tokens = Parser.ParseLine(line);

                Engine.Run(tokens);

            }





        }
    }
}
