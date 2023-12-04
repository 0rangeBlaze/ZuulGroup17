using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace WorldOfZuul
{
    public partial class Player
    {
        public string CurrentRoom { get; set; }
        public string PreviousRoom { get; set; }
        public string CurrentArea { get; set; }
        public string? PreviousArea { get; set; }
        public int WorkReputation { get; set; }
        public string Job { get; set; }
        public bool Promoted { get; set; }
        public int PersonalWelfare { get; set; }
        public int PreviousPersonalWelfare { get; set; }
        public int CurrentProviderIndex { get; set; }
        public int TravelIndex { get; set; }
        private int botPoints = 0;
        private int playerPoints = 0;
        private Dictionary<string, (bool done, string incompleteMessage)> tasks = new() {
            {"work", (false, "You still haven't done any work today. Your boss will be mad.")},
            {"eat", (false, "You still haven't eaten anything today. You are very hungry.")},
            {"sort", (false, "You need to throw out your trash, it's in the kitchen.")}
        };
        Random random = new Random();

        public Player(string currentArea = "home", string previousArea = "home", string currentRoom = "livingroom")
        {
            Job = "SupplyReview";
            CurrentArea = currentArea;
            PreviousArea = previousArea;
            CurrentRoom = currentRoom;
            PreviousRoom = currentRoom;
            PersonalWelfare = PreviousPersonalWelfare = 50;
            WorkReputation = 0;
            CurrentProviderIndex = TravelIndex = 0;
            Promoted = false;
        }

        /*
        private Dictionary<string, StatChanges> actionStatChanges = new Dictionary<string, StatChanges>
        {
            {"work", new StatChanges(5, -5)}, //Stats are just some examples, will change later probably
            {"eat", new StatChanges(3, -2)},
            {"sort", new StatChanges(2, -1)}
        };

        public void ApplyStatChanges(string action)
        {
            if (actionStatChanges.TryGetValue(action, out var changes))
            {
                personalWelfare += changes.PersonalWelfareChange;
                World.Environment += changes.EnvironmentChange;
            }
        }
        */


        public void Move(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Utilities.GamePrint("Please choose a direction.");
                return;
            }
            string direction = args[0].ToLower();
            if (direction == "back")
            {
                if (PreviousArea != CurrentArea)
                {
                    Utilities.GamePrint("You came from a different area you need to use travel to go back.");
                    return;
                }
                else
                {
                    (CurrentRoom, PreviousRoom) = (PreviousRoom, CurrentRoom);
                }
            }
            else if (game.World.GetRoom(CurrentArea, CurrentRoom).Exits.ContainsKey(direction))
            {
                PreviousRoom = CurrentRoom;
                CurrentRoom = game.World.GetRoom(CurrentArea, CurrentRoom).Exits[direction];
                PreviousArea = CurrentArea;
            }
            else
            {
                Utilities.GamePrint($"You can't go {direction}!");
                return;
            }
            Console.Clear();
            Utilities.GamePrint(game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).ShortDescription);
        }

        public void Travel(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Utilities.GamePrint("Please choose a destination.");
                return;
            }
            string destination = args[0].ToLower();
            if (!game.World.Areas.ContainsKey(destination))
            {
                Utilities.GamePrint("This destination doesn't exist!");
                return;
            }
            else if (destination == CurrentArea)
            {
                Utilities.GamePrint($"You are already at (the) {CurrentArea}.");
                return;
            }
            List<string> transportMethods = new() { "car", "public transport", "walk" };
            int chosen = Utilities.SelectOption("Choose method of transport:", transportMethods);

            switch (transportMethods[chosen])
            {
                case "car":
                    Utilities.GamePrint($"\nYou took the car to {destination} \n");
                    Console.ReadKey(true);
                    break;
                case "walk":

                    int currentWalkText = TravelIndex % 4;

                    string[,] Walk = new string[,]
                    {
                        {
                            $"\nYou decided to walk to {destination}. That means you have to walk on for another 600 meters and then take a right.",
                            $"Now you are on 5th avenue. That means you can take a shortcut by walking up the stair to Margrethe II street.",
                            $"Another 400 meters at you're there."
                        },
                        {
                            $"\nYou decided to walk to {destination}. You have to walk on for 700 meters, followed by a left turn.",
                            $"Now, you find yourself at the busy intersection of Sesame Street. You consider crossing the pedestrian bridge for a quicker route, but remember that that's under construction.",
                            $"A mere 300 meters later, you'll reach your destination."
                        },
                        {
                            $"\nYou decided to walk to {destination}. A charming 500 meter walk awaits through this beautiful neighbourhood, with a left turn just around the corner.",
                            $"Navigating through these lovely streets, you emerge onto Privet Drive. For a brief detour, ascend the staircase leading to Skovvej street.",
                            $"A mere 250 meters from the intersection, you'll find yourself at {destination}."
                        },
                        {
                            $"\nYou decided to walk to {destination}. After reaching Diagon Alley, turn left and continue for 300 meters until you reach the charming Central Perk.",
                            $"Now, on Main Street, you encounter a lively market. You don't have the time to explore the stalls, you have continue for 700 meters.",
                            $"Your next landmark is the old clock tower. Walk past it and proceed straight for another 500 meters to reach {destination}.",
                        },

                    };

                    Utilities.GamePrint(Walk[currentWalkText, 0]);
                    Console.ReadKey(true);
                    Utilities.GamePrint(Walk[currentWalkText, 1]);
                    Console.ReadKey(true);
                    Utilities.GamePrint(Walk[currentWalkText, 2]);
                    Console.ReadKey(true);

                    TravelIndex++;

                    break;

                case "public transport":

                    Utilities.GamePrint("\nYou get on the next bus. Press any key to continue");
                    Console.ReadKey(true);
                    Utilities.GamePrint($"You travelled for 30 minutes, and you now have arrived at {destination}\n");
                    Console.ReadKey(true);
                    break;

            }

            PreviousArea = CurrentArea;
            CurrentArea = destination;
            PreviousRoom = CurrentRoom;
            CurrentRoom = game.World.GetRoom(destination).Name;
            Console.Clear();
            Utilities.GamePrint(game.World.GetRoom(game.Player.CurrentArea, game.Player.CurrentRoom).ShortDescription);
            game.World.GetRoom(CurrentArea, CurrentRoom).HandleEvents(game);
        }

        public bool TasksDone()
        {
            foreach (var task in tasks.Values)
            {
                if (!task.done)
                {
                    Utilities.GamePrint(task.incompleteMessage);
                    return false;
                }
            }
            return true;
        }

        public void ResetTasks(Game game)
        {
            tasks["work"] = (false, tasks["work"].incompleteMessage);
            tasks["eat"] = (false, tasks["eat"].incompleteMessage);
            if (game.Turn % 2 == 0)
            {
                tasks["sort"] = (false, tasks["sort"].incompleteMessage);
            }
        }

        public void Eat()
        {
            if (tasks["eat"].done)
            {
                Utilities.GamePrint("You have eaten so much that you are still full. Maybe you should get some rest before you eat again.");
            }
            else
            {
                Utilities.GamePrint("You finish your delicious meal which makes you so full that you can't imagine eating anything for the rest of the day!");
                tasks["eat"] = (true, tasks["eat"].incompleteMessage);

                //ApplyStatChanges("eat");
            }
        }
        public void SortTrash(Game game)
        {
            if (!game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("sort"))
            {
                Console.WriteLine("Your trash bins are in your kitchen.");
                return;
            }
            if (tasks["sort"].done)
            {
                Console.WriteLine("There isn't enough trash yet.");
                return;
            }
            Dictionary<string, string> trashAlignment = new Dictionary<string, string>()
            {
                { "Plastic Bottle", "plastic" },
                { "Metal Can", "metal" },
                { "Newspaper", "paper" },
                { "Food Scraps", "organic" },
                { "Glass Jar", "glass" },
                { "Cardboard Box", "paper" },
                { "Aluminium Foil", "metal" },
                { "Banana Peel", "organic" },
                { "Plastic Wrap", "plastic" },
                { "Tin Can", "metal" },
                { "Coffee Cup", "paper" },
                { "Egg Carton", "paper" },
                { "Soda Can", "metal" },
                { "Milk Jug", "plastic" },
                { "Pizza Box", "paper" }
            };

            List<string> temporary = new List<string>() { "Plastic Bottle", "Metal Can", "Newspaper", "Food Scraps", "Glass Jar",
            "Cardboard Box", "Aluminium Foil", "Banana Peel", "Plastic Wrap", "Tin Can", "Coffee Cup", "Egg Carton", "Soda Can", "Milk Jug", "Pizza Box" };

            List<string> trashBins = new List<string>() { "plastic", "metal", "paper", "organic", "glass" };

            List<string> start = new List<string>() { "Yes", "No" };
            int points = 0;
            int option = Utilities.SelectOption("Do you want to sort a trash", start);

            if (option == 0)
            {
                Console.Clear();
                Random random = new Random();
                for (int i = 0; i < 4; i++)
                {
                    int randomNumber = random.Next(temporary.Count);
                    string randomTrash = temporary[randomNumber];
                    int trash = Utilities.SelectOption($"Where does this trash belong {temporary[randomNumber]}", trashBins);
                    if (trashAlignment[randomTrash] == trashBins[trash])
                    {
                        points++;
                        Utilities.CenterColor("Good choice", "green");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Utilities.CenterColor("Wrong choice", "red");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }


            }
            else
            {
                Utilities.CenterColor("You are not environmentally friendly", "red");
            }
            tasks["sort"] = (true, tasks["sort"].incompleteMessage);
        }

        public void RockPaperScizors()
        {  
            botPoints = 0;
            playerPoints = 0;
            
            Dictionary<int, string> dict = new Dictionary<int, string>()
            {
                {0,"Rock" },
                {1,"Paper" },
                {2,"Scizors" }
            };

            Utilities.PrintSlowlyCenter("How many rounds are we gonna play\n");

            Console.CursorLeft = Console.WindowWidth / 2;

            string srounds = Console.ReadLine();

            int rounds;
            while (srounds == null || int.TryParse(srounds, out _) == false)
            {
                Utilities.CenterColor("You typed something wrong try again", "red");
                srounds = Console.ReadLine();
            }
            int.TryParse(srounds, out rounds);
            Console.Clear();
            var list = dict.Values.ToList();
            string question = "What are gonna show";
            for (int i = 0; i < rounds; i++)
            {
                int bot = random.Next(3);
                string player = Utilities.SelectOption(question, list).ToString();
                int chosed = dict.FirstOrDefault(x => x.Value == player).Key;
                WhoWins(bot, chosed);
                Console.CursorVisible = false;
            }
            Utilities.CenterText("So who won ?\n");
            if (botPoints > playerPoints)
            {
                Utilities.CenterColor("You lost ;(", "red");
            }
            else if (botPoints < playerPoints)
            {
                Utilities.CenterColor("You won :)", "green");
            }
            else
            {
                Utilities.CenterColor("It was a draw", "yellow");
            }
            Thread.Sleep(4000);
            Console.Clear() ;
            Console.CursorVisible = true;
        }

        private void ShowPictures(int bot, int player, string decision)
        {

            string rock = @"
                    _______  
                ---'   ____) 
                      (_____)
                      (_____)
                      (____) 
                ---.__(___)  
            ";

            string paper = @"
                _______       
            ---'   ____)____  
                      ______) 
                     _______) 
                     _______) 
            ---.__________)   
            ";

            string scissors = @"
                _______       
            ---'   ____)____  
                      ______) 
                   __________)
                  (____)      
            ---.__(___)       
            ";

            Dictionary<int, string> intString = new Dictionary<int, string>()
            {
                {0,rock },
                {1, paper },
                {2, scissors }
            };

            List<string> listOfAcsi = new List<string>()
            {
                {scissors},
                {paper},
                {rock}
            };

            CountDown(3, listOfAcsi);

            var linesBot = intString[bot].Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var linesPlayer = intString[player].Split(new[] { Environment.NewLine }, StringSplitOptions.None);


            int maxLines = Math.Max(linesBot.Length, linesPlayer.Length);

            int totalWidth = linesBot.Max(line => line.Length) + decision.Length + linesPlayer.Max(line => line.Length) + 8;

            for (int i = 0; i < maxLines; i++)
            {
                string lineBot = i < linesBot.Length ? linesBot[i].PadRight(linesBot.Max(line => line.Length)) : string.Empty;
                string linePlayer = i < linesPlayer.Length ? linesPlayer[i].PadLeft(linesPlayer.Max(line => line.Length)) : string.Empty;

                if (i == 0)
                {
                    Utilities.CenterText(decision);
                }
                else
                {
                    Console.Write(lineBot);
                    Console.CursorLeft = Console.WindowWidth - linePlayer.Length;
                    Console.WriteLine(new string(linePlayer.ToCharArray().Select(c => FlipSign(c)).Reverse().ToArray()));
                }
            }

            Console.Write($"Bot Points: {botPoints}");
            Console.CursorLeft = Console.WindowWidth - $"Player Points: {playerPoints}".Length;
            Console.WriteLine($"Player Points: {playerPoints}");
            Thread.Sleep(3000);
            Console.Clear();
            char FlipSign(char c) => c == ')' ? '(' : c;

        }


        private void CountDown(int count, List<string> list)
        {
            for (int i = count; i >= 0; i--)
            {

                if (i != 0)
                {
                    Utilities.CenterText(list[i - 1]);
                    Utilities.CenterText($"Countdown {i}");
                }
                else
                {
                    Utilities.CenterText("Show Time");
                }

                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        private void WhoWins(int bot, int chosed)
        {
            Dictionary<string, bool> botwins = new Dictionary<string, bool>()
            {
                {"10", true},
                {"20", false},
                {"01", false},
                {"02", true},
                {"21", true},
                {"12", false}
            };

            string decision = "";
            string dicKey = bot.ToString() + chosed.ToString();



            if (bot == chosed)
            {
                decision = "It's a draw";
            }
            else if (botwins[dicKey] == true)
            {
                decision = "Bot wins";
                botPoints++;
            }
            else
            {
                decision = "You won";
                playerPoints++;
            }
            ShowPictures(bot, chosed, decision);
        }

    }
}