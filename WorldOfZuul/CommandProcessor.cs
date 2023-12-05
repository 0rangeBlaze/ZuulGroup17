using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WorldOfZuul
{
    public static class CommandProcessor
    {
        private static readonly Dictionary<string, Action<Game, string[]>> commandDict = new Dictionary<string, Action<Game, string[]>> (StringComparer.OrdinalIgnoreCase){
            {"", (_,_) => {}},
            {"help", (Game game,string[] arguments) => Help(arguments)},
            {"look", (Game game, string[] _) => 
                game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Describe(game)},
            {"move", (Game game, string[] arguments) => game.Player.Move(game, arguments)},
            {"travel", (Game game, string[] arguments) => game.Player.Travel(game, arguments)},
            {"work", (Game game, string[] _) => game.Player.Work(game)},
            {"interact", (Game game, string[] arguments) => TalkToNpc(game, arguments)},
            {"map", (Game game, string[] arguments ) => Map(game, arguments)},
            {"sort", (Game game, string[] _) => game.Player.SortTrash(game)},
            {"sleep", (Game game, string[] arguments) => game.NextTurn()},
            {"quit", (Game game, string[] arguments) => Quit(game)},

            {"eat", (Game game, string[] _) => game.Player.Eat()},
            {"ad", (Game game, string[] _) => Advertisement(game)},
            {"promoted", (Game game, string[] _ ) => game.Player.Promoted = false},
            {"scizzors", (Game game, string[] _) => MiniGames.RockPaperScizors()},
            {"whack", (Game game, string[] _) => MiniGames.WhackAMole()},
            {"betterPersonalWelfare", (Game game, string[] _) => game.Player.BetterPersonalWelfare()},
            {"worseEnvironment", (Game game, string[] _) => game.World.WorseEnvironment()},
            {"betterEnvironment", (Game game, string[] _) => game.World.BetterEnvironment()},
            {"connect", (Game game, string[] _) => MiniGames.ConnectFour()},
            {"runningGame", (Game game, string[] _) => MiniGames.RunGame()}
            


        };
        private static readonly Dictionary<string, string> possibleCommands = new Dictionary<string, string> (StringComparer.OrdinalIgnoreCase) {
            {"", ""}, 
            {"help", "\nhelp: help [command]\n\tPrint usage of command.\n\tIf command is not specified list all commands.\n"},
            {"look", "\nlook:\n\tFind out more about your surroundings and which ways you can move.\n"},
            {"move", "\nmove: move [direction]\n\tMove in direction.\n\tIf direction is set to back move back to previous room.\n"},
            {"travel", "\ntravel: travel [destination]\n\tNavigate to area specified in destination.\n"},
            {"work", "\nwork: \n\tWhen in your office start working.\n"},
            {"interact", "\ninteract: interact [name]\n\tInteract with the person/object specified by name in the current room.\n"},
            {"map", "\nmap: map [area]\n\tLists room in area. \n\tIf area is not specified lists areas.\n"},
            {"sort", "\ntrash:\n\tWhen in kitchen sort or just throw out trash.\n"},
            {"sleep", "\nsleep:\n\tWhen in your bedroom start a new turn.\n"},
            {"quit", "\nquit:\n\tExit the game.\n"}
        };

        public static void Process(string? command, Game game){
            if(command == null) {
                Utilities.GamePrint("Specify a valid command!");
            }
            else {
                string[] commandWords = command.Split();
                if(commandDict.ContainsKey(commandWords[0])) {
                    commandDict[commandWords[0]](game, (commandWords.Length > 1 ? commandWords[1..] : new string[0]{}));
                }
                else{
                    Utilities.GamePrint($"Unknown action '{commandWords[0]}'.");
                }
            }
        }

        public static void HandleInput(string? command, Game game) {
            if(command == null) {
                Utilities.GamePrint("Specify a valid command!");
            }
            else {
                if(possibleCommands.ContainsKey(command.Split()[0])) {
                    Process(command, game);
                }
                else {
                    Utilities.GamePrint($"Unknown command '{command.Split()[0]}'. Please try again!");
                }
            }
        }       

        private static void Help(string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                /*Utilities.WriteLineWordWrap("""
The world is divided into four main areas: home, mall, town and work.
Each of these areas contain rooms for you to explore.
""");*/ //put this somehow in welcome text

                Utilities.GamePrint("Possible commands are:");
                string commandList = "\t";
                foreach(string command in possibleCommands.Keys) {
                    if(command != "")
                        commandList += $"{command}, ";
                }
                Utilities.GamePrint(commandList[..^2]);
                Utilities.GamePrint("\tTo find out more about a command use 'help [command]'.");
                Console.WriteLine();
            }
            else {
                if(possibleCommands.ContainsKey(args[0])){
                    Utilities.GamePrint(possibleCommands[args[0]]);
                }
                else{
                    Utilities.GamePrint($"Command {args[0]} does not exist.");
                }
            }
        }
    
        private static void TalkToNpc(Game game, string[] arguments) {
            if(arguments.Length == 0 || string.IsNullOrEmpty(arguments[0])) {
                Utilities.GamePrint("You need to specify who you are talking to!");
            }
            else if(!game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Npcs.ContainsKey(arguments[0])) {
                Utilities.GamePrint($"{arguments[0]} is not here.");
            }
            else{
                game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).Npcs[arguments[0]].NpcTalk(game);
            }

        }

        private static void Map(Game game, string[] arguments) 
        {
            if(arguments.Length == 0 || arguments[0] == "")
            {
                Utilities.GamePrint("Areas:");
                foreach(var area in game.World.Areas.Keys)
                {
                    if(area.ToLower() == game.Player.CurrentArea)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Utilities.GamePrint($"{area} <- You Are Here");
                        Console.ResetColor();
                    }
                    else
                    {
                        Utilities.GamePrint(area);
                    }
                }
            }
            else
            {
                if(!game.World.Areas.ContainsKey(arguments[0]))
                {
                    Utilities.GamePrint("There is no such location!");
                }
                else
                {
                    Utilities.GamePrint("Rooms:");
                    foreach(var room in game.World.Areas[arguments[0]].Rooms.Values)
                    {
                        if(room.Name == game.Player.CurrentRoom)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Utilities.GamePrint($"{room.Name} <- You Are Here");
                            Console.ResetColor();
                        } 
                        else
                        {
                            Utilities.GamePrint(room.Name);
                        }
                    }
                }
            }
        }

        public static void Advertisement(Game game) {
            List<string> ads = new List<string>() {"first", "second"};
            Utilities.SelectOption(ads[game.Turn/3], new List<string>() {"yes", "no"});
        }

        public static void Quit(Game game)
        {
            int input = Utilities.SelectOption("Do you want to save your progress?", new List<string>(){"yes", "no"});
            if(input == 0) {
                string jsonString = JsonSerializer.Serialize(game);
                string savePath = "./saves/save.json";
                Directory.CreateDirectory("./saves"); 
                using (StreamWriter sw = File.CreateText(savePath))
                {
                    sw.WriteLine(jsonString);
                }
            }
            game.Running = false;
        }
    }
}
