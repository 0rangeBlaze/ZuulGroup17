using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public static class CommandProcessor
    {
        private static readonly Dictionary<string, Action<Game, string[]>> commandDict = new Dictionary<string, Action<Game, string[]>> (StringComparer.OrdinalIgnoreCase){
            {"", (_,_) => {}},
            {"help", (_,_) => PrintHelp()},
            {"look", (Game game, string[] _) => 
                game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Describe()},
            {"move", (Game game, string[] arguments) => game.Player.Move(game, arguments)},
            {"travel", (Game game, string[] arguments) => game.Player.Travel(game, arguments)},
            {"work", (Game game, string[] _) => game.Player.Work(game)},
            {"quit", (Game game, string[] arguments) => game.Running = false}
        };
        public static int Process(string command, Game game){
            string[] commandWords = command.Split();
            if(!commandDict.ContainsKey(commandWords[0])) {
                Console.WriteLine($"Unknown command '{commandWords[0]}'. Please try again!");
                return 1;
            }
            commandDict[commandWords[0]](game, (commandWords.Length > 1 ? commandWords[1..] : new string[0]{})); //I don't know why this is red, when it doesn't give any errors
            return 0; //no error might change to void
        }

        

        private static void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the university.");
            Console.WriteLine();
            Console.WriteLine("Type 'move [direction]' to navigate between rooms. \nType 'move back', that takes you to the previous room.");
            Console.WriteLine("Type 'travel [destination]' to navigate between areas.");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
    }
}