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
        private int turn;

        public Game()
        {
            turn = 0;
            World = new World("assets/world.json");
            if(!World.loaded) {
                Console.WriteLine("Couldn't load world.");
                Player = new();
                Running = false;
                return;
            }
            Player = new("home", "home", World.GetRoom("home").Name);
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
                CommandProcessor.HandleInput(input, this);
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        
        public void NextTurn()
        {
            if (World.GetRoom(Player.CurrentArea, Player.CurrentRoom).Actions.Contains("nextTurn"))
            {
                if(Player.ResetTasks()) 
                {
                    if(turn == 20)
                    {
                        EndGame();
                    }
                    else
                    {
                        Console.WriteLine("You wake up the next day fully refreshed!");
                        turn++;
                    }
                }
            }
            else
            {
                Console.WriteLine("You would much rather sleep in your comfy bed in your bedroom.");
            }

        }
        private void EndGame()
        {
            EnvironmentConclusion();
            Console.WriteLine("Hope you enjoyed our game :)");
            Running = false;
            return;
        }

        private void EnvironmentConclusion()
        {
            if(World.Environment >= 80)
            {
                Console.WriteLine("You have managed to look out for the envrionment through-out your choices, and now you can see its results. Nature is thriving, finally humanity isn't figthing against it, but embracing its strengths.");
                HumanWellfareConclusion();
                Console.WriteLine("You can see now, how much effect small, seemingly wortless choices have, if we all do the right thing. Even at times, where these things seem agonizing.");
                Console.WriteLine("Yes, in this game you also had more-and-more control over a large food company, but even if that's not the case in real life, you shouldn't despair, if you just do your seemingly small part, positive change will take effect in time.");

            }
            else if(World.Environment >= 60)
            {
                Console.WriteLine("");
                HumanWellfareConclusion();
            }
            else if(World.Environment >= 40)
            {
                Console.WriteLine("");
                HumanWellfareConclusion();

            }
            else if(World.Environment >= 20)
            {
                Console.WriteLine("");
                HumanWellfareConclusion();

            }
            else
            {
                Console.WriteLine("");
                HumanWellfareConclusion();

            }
        }

        private void HumanWellfareConclusion()
        {
            if(World.PopulationWelfare >= 80)
            {
                Console.WriteLine("The population's wellfare has improved greatly. Almost everyone is living well, poverty and homelessness has been has been clamped back, by figures never seen before.");
            }
            else if(World.PopulationWelfare >= 60)
            {
                Console.WriteLine("");

            }
            else if(World.PopulationWelfare >= 40)
            {
                Console.WriteLine("");

            }
            else if(World.PopulationWelfare >= 20)
            {
                Console.WriteLine("");

            }
            else
            {
                Console.WriteLine("");

            }
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
