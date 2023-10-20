using System.Text.Json;

namespace WorldOfZuul
{
    public class Game
    {
        private string currentRoom;
        private string? previousRoom;
        private string currentArea;
        private string? previousArea;
        private World world;

        public Game()
        {
            world = new World("assets/world.json");
            //temporary:
            currentRoom = "Restaurant";
            currentArea = "Mall";
        }

        public void Play()
        {
            Parser parser = new();

            PrintWelcome();

            bool continuePlaying = true;
            while (continuePlaying)
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
                        Console.WriteLine(world.GetRoom(currentArea, currentRoom).LongDescription);
                        break;

                    case "back":
                        if (previousRoom == null)
                            Console.WriteLine("You can't go back from here!");
                        else
                            currentRoom = previousRoom;
                        break;

                    case "move":
                        Move(command.SecondWord);
                        break;

                    case "quit":
                        continuePlaying = false;
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
            if(string.IsNullOrEmpty(direction)) {
                Console.WriteLine("Please choose a direction.");
                return;
            }
            if (world.GetRoom(currentArea, currentRoom).Exits.ContainsKey(direction))
            {
                previousRoom = currentRoom;
                currentRoom = world.GetRoom(currentArea, currentRoom).Exits[direction];
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }

        public void Travel(string? destination)
        {
            if(string.IsNullOrEmpty(destination)) {
                Console.WriteLine("Please choose a destination.");
                return;
            }
            if(!world.Areas.ContainsKey(destination)){
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

            }
            else if(travelCommand == "walk")
            {

            }
            else if(travelCommand == "public transport")
            {

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
            Console.WriteLine("Navigate between rooms by typing: move [direction]");
            Console.WriteLine("Navigate between areas by typing: travel [destination]");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }

        
        //Generic error message, can later be moved somewhere else
        public static void ErrorMessage()
        {
            Console.WriteLine("Invalid Input, Please try again!");
        }
    }
}
