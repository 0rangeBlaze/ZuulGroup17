using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Utilities;

namespace WorldOfZuul
{
    public class Game
    {
        private string currentRoom;
        private string? previousRoom;
        private string currentArea;
        private string? previousArea;
        private World world;
        private int personalWelfare = 0, populationWelfare = 0, environment = 0;
        private bool running;

        public Game()
        {
            world = new World("assets/world.json");
            if(!world.loaded) {
                Console.WriteLine("Couldn't load world.");
                running = false;
                return;
            }

            currentArea = "home";
            previousArea = "home";
            currentRoom = world.GetRoom(currentArea).ShortDescription;
            running = true;
        }

        public void Play()
        {
            if(!running){
                return;
            }

            Parser parser = new();

            PrintWelcome();

            while (running)
            {
                Console.WriteLine(world.GetRoom(currentArea, currentRoom).ShortDescription);
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);

                if (command == null)
                {
                    Console.WriteLine("I don't know that command.");
                    continue;
                }
                switch(command.Name)
                {
                    case "look":
                        world.GetRoom(currentArea, currentRoom).Describe();
                        break;

                    case "move":
                        Move(command.SecondWord);
                        break;

                    case "quit":
                        running = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    case "travel":
                        Travel(command.SecondWord);
                        break;

                    default:
                        Console.WriteLine("I don't know what command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private void Move(string? direction)
        {
            if (string.IsNullOrEmpty(direction)) {
                Console.WriteLine("Please choose a direction.");
                return;
            }
            if (direction == "back") {
                if (previousArea != currentArea) {
                    Console.WriteLine("You came from a different area you need to use travel to go back.");
                }
                else {
                    (currentRoom, previousRoom) = (previousRoom, currentRoom);
                }
                return;
            }
            else if (world.GetRoom(currentArea, currentRoom).Exits.ContainsKey(direction))
            {
                previousRoom = currentRoom;
                currentRoom = world.GetRoom(currentArea, currentRoom).Exits[direction];
                previousArea = currentArea;
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }

        public void Travel(string? destination)
        {
            if (string.IsNullOrEmpty(destination)) {
                Console.WriteLine("Please choose a destination.");
                return;
            }
            else{
                destination = destination.ToLower();
            }
            if (!world.Areas.ContainsKey(destination)){
                Console.WriteLine("This destination doesn't exist!");
                return;
            } 
            else if (destination == currentArea) {
                Console.WriteLine($"You are already at (the) {currentArea}");
                return;
            }
            string[] transportMethods = {"car", "public transport", "walk"};
            string? travelCommand;
            Console.WriteLine($"Choose method of transport({string.Join(" / ", transportMethods)}):");
            travelCommand = Console.ReadLine();
            while(!transportMethods.Contains(travelCommand)){
                Console.WriteLine($"Invalid transport method. Choose from these options: {string.Join(" / ", transportMethods)}. Try again:");
                travelCommand = Console.ReadLine();
            }

            if(travelCommand == "car")
            {
                Console.WriteLine($"\nYou took the car to {destination} \n");
            }
            else if(travelCommand == "walk")
            {
                Console.WriteLine($"\nYou decided to walk to {destination}. That means you have to walk on for another 600 meters and then take a right.");
                Console.ReadKey(true);
                Console.WriteLine($"Now you are on 5th avenue. That means you can take a shortcut by walking up the stair to Margrethe II street.");
                Console.ReadKey(true);
                Console.WriteLine($"Another 400 meters at you're there.");
                Console.ReadKey(true);
                Console.WriteLine();
            }
            else if(travelCommand == "public transport")
            {
                Console.WriteLine("\nYou get on the next bus. Press any key to continue");
                Console.ReadKey(true);
                Console.WriteLine($"You travelled for 30 minutes, and you now have arrived at {destination}\n");
            }

            previousArea = currentArea;
            currentArea = destination;
            previousRoom = currentRoom;
            currentRoom = world.GetRoom(destination).ShortDescription;
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
            PrintHelp();
            Console.WriteLine();
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

        
        private static void Hire()
        {

        }
        
        private static void SupplyChoice()
        {

        }

        private static void SupplyReview()
        {
            Console.WriteLine("=========");
            Console.WriteLine("You are tasked with overlooking the quality of the egg supplements.");
            Console.WriteLine("A batch of 25 eggs is only acceptable if there are 5 or less small eggs.");
            Console.WriteLine("The good eggs are marked with an 'X' and the small ones with an 'O'.");
            Console.WriteLine("After looking at a batch checking its quality: \nType 'Good' if its acceptable \nType 'Bad' if not");

            int  upForPromotion = 0;
            int goodChoices = 0;
            for(int k = 0; k < 3; k++)
            {
                int badEggs = 0;
                bool GoodBatch;
                Console.WriteLine("");
                for(int i = 0; i < 5; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        Random random = new Random();
                        int badEgg = random.Next(1, 6);
                        if(badEgg == 1)
                        {
                            Console.Write("O ");
                            badEggs += 1;
                        }
                        else
                        {
                            Console.Write("X ");
                        }
                    }
                    Console.WriteLine("");
                } 

                if(badEggs >= 5)
                {
                    GoodBatch = false;
                }
                else
                {
                    GoodBatch = true;
                }
                Console.WriteLine("Is this a good or a bad batch?");
                bool batchChecked = false;
                while(!batchChecked)
                {
                    Console.Write(">");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if(key.KeyChar == '1' || key.KeyChar == '2')
                    {
                        Console.WriteLine("");
                        if(key.KeyChar == '1')
                        {
                            if(GoodBatch)
                            {
                                goodChoices += 1;
                                Console.WriteLine("You are correct, your supervisors are going to be satisfied!");
                            }
                            else
                                Console.WriteLine("Unfortunately you are uncorrect, this is a bad batch, but do not despair! You can still prove yourself.");
                        }
                        else if(key.KeyChar == '2')
                        {
                            if(!GoodBatch)
                            {
                                goodChoices += 1;
                                Console.WriteLine("You are correct, your supervisors are going to be satisfied!");
                            }
                            else
                                Console.WriteLine("Unfortunately you are uncorrect, this is a bad batch, but do not despair! You can still prove yourself.");

                        }
                        else
                        {
                            Console.WriteLine("Something is WRONG, i need to fix it");
                        }
                        batchChecked = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid inpput. Please try again!");
                    }
                }
            }
            if(goodChoices > 2)
            {
                upForPromotion += 1;
            }

            if(upForPromotion > 2)
            {
                Promoted();
            }

            Console.WriteLine("You are finished for the day, get some rest.");
        }
        private static void Promoted()
        {

        }
    }
}
