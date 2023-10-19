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

        private void Move(string direction)
        {
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

        public void TravelCheck()
        {
            string? travelCommand;

            while(true)
            {
                Console.WriteLine("Choosing type of transport");
                travelCommand = Console.ReadLine()?.ToLower();
                //this try catch block does nothing because Travel doesn't throw any exceptions
                try
                {
                    Travel(travelCommand);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"You can't travel by {ex.Message}");
                }
            } 
        }
        public void Travel(string destination, string? travelCommand = null)
        {
            if(travelCommand == "car")
            {

            }
            else if(travelCommand == "walk")
            {

            }
            else if(travelCommand == "public transport")
            {

            }
            else
            {
                ErrorMessage();
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
