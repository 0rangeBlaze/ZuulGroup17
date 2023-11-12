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
                game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Describe(game)},
            {"move", (Game game, string[] arguments) => game.Player.Move(game, arguments)},
            {"travel", (Game game, string[] arguments) => game.Player.Travel(game, arguments)},
            {"work", (Game game, string[] _) => game.Player.Work(game)},
            {"talk", (Game game, string[] arguments) => TalkToNpc(game, arguments)},
            {"map", (Game game, string[] _ ) => Map(game)},
            {"sort", (Game game, string[] _) => game.Player.SortTrash()},
            {"sleep", (Game game, string[] arguments) => game.Player.NextTurn(game)},
            {"quit", (Game game, string[] arguments) => game.Running = false}
        };
        public static void Process(string? command, Game game){
            if(command == null) {
                Console.WriteLine("Specify a valid command!");
            }
            else {
                string[] commandWords = command.Split();
                if(!commandDict.ContainsKey(commandWords[0])) {
                    Console.WriteLine($"Unknown command '{commandWords[0]}'. Please try again!");
                    return;
                }
                commandDict[commandWords[0]](game, (commandWords.Length > 1 ? commandWords[1..] : new string[0]{})); //I don't know why this is red, when it doesn't give any errors
            }
        }

        

        private static void PrintHelp()
        {
           Utilities.WriteLineWordWrap("""
The world is divided into four main areas: home, mall, town and work.
Each of these areas contain rooms for you to explore.
""");
            Utilities.WriteLineWordWrap("""
Type 'move [direction]' to navigate between rooms. 
Type 'move back', that takes you to the previous room.
Type 'travel [destination]' to navigate between areas.
Type 'look' to find out more about your surroundings and which ways you can move.
Type 'work' in your office to start working.
Tpye 'sleep' in you bedroom to start a new turn.
Type 'help' to print this message again.
Type 'quit' to exit the game.

""");
        }
    
        private static void TalkToNpc(Game game, string[] arguments) {
            if(arguments.Length == 0 || string.IsNullOrEmpty(arguments[0])) {
                Console.WriteLine("You need to specify who you are talking to!");
            }
            else if(!game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Npcs.ContainsKey(arguments[0])) {
                Console.WriteLine($"{arguments[0]} is not here.");
            }
            else{
                game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Npcs[arguments[0]].NpcTalk();
            }

        }

        private static void Map(Game game) {

        }
    }
}