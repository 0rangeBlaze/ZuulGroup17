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
        private int personalWelfare;
        private Dictionary<string, (bool done, string incompleteMessage)> tasks = new() {
            {"work", (false, "You still haven't done any work today. Your boss will be mad.")}
        };

        public Player(string currentArea = "home", string previousArea = "home", string currentRoom = "livingroom") {
            CurrentArea = currentArea;
            PreviousArea = previousArea;
            CurrentRoom = currentRoom;
            PreviousRoom = currentRoom;
            personalWelfare = 0;
            WorkReputation = 0;
        }

        public void Move(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Please choose a direction.");
                return;
            }
            string direction = args[0].ToLower();
            if (direction == "back")
            {
                if (PreviousArea != CurrentArea)
                {
                    Console.WriteLine("You came from a different area you need to use travel to go back.");
                }
                else
                {
                    (CurrentRoom, PreviousRoom) = (PreviousRoom, CurrentRoom);
                }
                return;
            }
            else if (game.World.GetRoom(CurrentArea, CurrentRoom).Exits.ContainsKey(direction))
            {
                PreviousRoom = CurrentRoom;
                CurrentRoom = game.World.GetRoom(CurrentArea, CurrentRoom).Exits[direction];
                PreviousArea = CurrentArea;
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }

        public void Travel(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Please choose a destination.");
                return;
            }
            string destination = args[0].ToLower();
            if (!game.World.Areas.ContainsKey(destination))
            {
                Console.WriteLine("This destination doesn't exist!");
                return;
            }
            else if (destination == CurrentArea)
            {
                Console.WriteLine($"You are already at (the) {CurrentArea}.");
                return;
            }
            string[] transportMethods = { "car", "public transport", "walk" };
            string? travelCommand;
            Console.WriteLine($"Choose method of transport({string.Join(" / ", transportMethods)}):");
            travelCommand = Console.ReadLine();
            while (!transportMethods.Contains(travelCommand))
            {
                Console.WriteLine($"Invalid transport method. Choose from these options: {string.Join(" / ", transportMethods)}. Try again:");
                travelCommand = Console.ReadLine();
            }

            if (travelCommand == "car")
            {
                Console.WriteLine($"\nYou took the car to {destination} \n");
            }
            else if (travelCommand == "walk")
            {
                Console.WriteLine($"\nYou decided to walk to {destination}. That means you have to walk on for another 600 meters and then take a right.");
                Console.ReadKey(true);
                Console.WriteLine($"Now you are on 5th avenue. That means you can take a shortcut by walking up the stair to Margrethe II street.");
                Console.ReadKey(true);
                Console.WriteLine($"Another 400 meters at you're there.");
                Console.ReadKey(true);
                Console.WriteLine();
            }
            else if (travelCommand == "public transport")
            {
                Console.WriteLine("\nYou get on the next bus. Press any key to continue");
                Console.ReadKey(true);
                Console.WriteLine($"You travelled for 30 minutes, and you now have arrived at {destination}\n");
            }

            PreviousArea = CurrentArea;
            CurrentArea = destination;
            PreviousRoom = CurrentRoom;
            CurrentRoom = game.World.GetRoom(destination).ShortDescription;
        }

        public void NextTurn(Game game)
        {
            if (game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("nextTurn"))
            {
                foreach (var task in tasks.Values)
                {
                    if (!task.done)
                    {
                        Console.WriteLine(task.incompleteMessage);
                        return;
                    }
                }
                ResetTasks();
                Console.WriteLine("You wake up the next day fully refreshed!");
            }
            else
            {
                Console.WriteLine("You would much rather sleep in your comfy bed in your bedroom.");
            }

        }

        public void ResetTasks()
        {

        }

        public void SortTrash()
        {
            Dictionary<string, string> trashAlignment = new Dictionary<string, string>()
        {
            { "Plastic Bottle", "plastic" },
            { "Metal Can", "metal" },
            { "Newspaper", "paper" },
            { "Food Scraps", "organic" },
            { "Glass Jar", "glass" },
            { "Cardboard Box", "paper" },
            { "Aluminum Foil", "metal" },
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
            "Cardboard Box", "Aluminum Foil", "Banana Peel", "Plastic Wrap", "Tin Can", "Coffee Cup", "Egg Carton", "Soda Can", "Milk Jug", "Pizza Box" };

            List<string> trashBins = new List<string>() { "plastic", "metal", "paper", "organic", "glass" };

            List<string> start = new List<string>() { "Yes", "No" };
            int points = 0;
            string option = SelectOption("Do you want to sort a trash", start);

            if (option == "Yes")
            {
                Random random = new Random();
                for (int i = 0; i < 4; i++)
                {
                    int randomNumber = random.Next(temporary.Count);
                    string randomTrash = temporary[randomNumber];
                    string trash = SelectOption($"Where does this trash belong {temporary[randomNumber]}", trashBins);
                    if (trashAlignment[randomTrash] == trash)
                    {
                        points++;
                        Console.WriteLine("Good choice");
                    }
                    else
                    {
                        Console.WriteLine("Wrong choice");
                    }
                }

            }
            else
            {
                Console.WriteLine("You are not environmentally friendly");
            }
        }


        public string SelectOption(string question, List<string> temp)
        {
            int option = 1;
            int startingPosition = temp.Count;
            int endingPosition = (temp.Count + 1) - startingPosition;

            bool selected = false;

            while (!selected)
            {
                Console.WriteLine(question);

                for (int i = 0; i < temp.Count; i++)
                {
                    Console.WriteLine(option == i + 1 ? $" >{temp[i]}" : $" {temp[i]}");
                }

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.DownArrow:
                        if (option < startingPosition)
                        {
                            option++;
                        }
                        Console.Clear();
                        break;
                    case ConsoleKey.UpArrow:
                        if (option > endingPosition)
                        {
                            option--;
                        }
                        Console.Clear();
                        break;
                    case ConsoleKey.Enter:
                        selected = true;
                        break;
                }
            }
            return temp[option - 1];
        }
    }
}      