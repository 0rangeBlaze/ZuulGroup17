using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace WorldOfZuul
{
    public class Game
    {
        public Player Player {get; set;}
        public World World {get; set;}
        public bool Running {get; set;}
        public static Random RandomGenerator = new Random();

        public Game()
        {
            World = new World("assets/world.json");
            Player = new("home", "home", World.GetRoom("home").ShortDescription);
            if(!World.loaded) {
                Console.WriteLine("Couldn't load world.");
                Running = false;
                return;
            }
            Running = true;
        }

        public void Play()
        {
            if(!Running){
                return;
            }

            PrintWelcome();
            CommandProcessor.Process("help", this);

            while (Running)
            {
                Console.WriteLine(World.GetRoom(Player.CurrentArea, Player.CurrentRoom).ShortDescription);
                Console.Write("> ");

                string? input = Console.ReadLine();
                CommandProcessor.Process(input, this);
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        



        private static void PrintWelcome()
        {
            Utilities.WriteLineWordWrap("Welcome to Lasting Impact!");
            Utilities.WriteLineWordWrap("In this game you will experience humanity's impact on the environment.");
            Utilities.WriteLineWordWrap("You will be playing as an average person whose impact on the world is projected to an entire generation of people.");
            Utilities.WriteLineWordWrap();
        }


        

        
    }
}
