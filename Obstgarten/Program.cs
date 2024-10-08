using Obstgarten;
using Obstgarten.Dices;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Obstgarten.");

            const int N = 100000;
            var results = new List<int>();

            foreach(var i in Enumerable.Range(0, N)) 
            { 
                // play a game of Obstgarten
                var game = new Game<GameParameters.Colors>
                {
                    Dice = new DefaultDice<GameParameters.Colors>(Guid.NewGuid()),
                    RavenColors = new []{GameParameters.Colors.Raven},
                    JokerColors = new []{GameParameters.Colors.Basket}
                };

                game.InitFruitTree();

                while(game.HasGameEnded() == false)
                {
                    game.TakeTurn();

                    //Console.WriteLine(game.ToString());
                }

                //Console.WriteLine(game.ToString());

                if (game.IsGameWon())
                {
                    //Console.WriteLine("The players win.");
                    results.Add(1);
                }
                else
                {
                    //Console.WriteLine("The raven wins.");
                    results.Add(0);
                }

                //Console.WriteLine();

                //if(results.Count % 1000 == 0)
                //{
                //    Console.WriteLine($"Players won {(double)results.Where(r=>r == 1).Count() / i *100}% of {results.Count} games.");
                //}
            }

            Console.WriteLine($"Players won {(double)results.Where(r=>r == 1).Count() / N *100}% of {results.Count} games.");
        }
    }
}