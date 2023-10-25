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

        public Game()
        {
            world = new World("assets/world.json");
            currentArea = "home";
            previousArea = "home";
            currentRoom = world.GetRoom(currentArea).ShortDescription;
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
                        world.GetRoom(currentArea, currentRoom).Describe();
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

        }
    }
}
