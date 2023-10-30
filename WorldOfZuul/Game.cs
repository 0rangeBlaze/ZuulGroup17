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
        public Random RandomGenerator {get; set;}

        public Game()
        {
            World = new World("assets/world.json");
            if(!World.loaded) {
                Console.WriteLine("Couldn't load world.");
                Running = false;
                return;
            }
            Player = new("home", "home", World.GetRoom("home").ShortDescription);
            Running = true;
            RandomGenerator = new();
        }

        public void Play()
        {
            if(!Running){
                return;
            }

            PrintWelcome();

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
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
            Console.WriteLine();
        }




        private void Hire()
        {
            List<string> hireNames = new List<string>
            {
                "Zuhao", "Oskar", "Daniel", "Sebestyén", "Szymon"
            };


            List<string> hireHobbies = new List<string>
            {
                "Filler1", "Filler2", "Filler3", "Filler4", "Filler5"
            };

            List<string> hireLastJobs = new List<string>
            {
                "Filler1", "Filler2", "Filler3", "Filler4", "Filler5"
            };

            Dictionary<string, int> traitValues = new Dictionary<string, int>
            {
                { "Filler1", 1 },   // Stat changes for different traits (can be negative?)
                { "Filler2", 2 },
                { "Filler3", 3 },
                { "Filler4", 4 },
                { "Filler5", 5 },
            };

            int hires = 0;

            while (hires < 5) // Loop until the player has hired five times
            {
                string[] hireTraits = GetRandomCandidate(hireNames, hireHobbies, hireLastJobs);
                string hireName = hireTraits[0];
                string hireHobby = hireTraits[1];
                string hireLastJob = hireTraits[2];

                Console.WriteLine($"Candidate: {hireName}");
                Console.WriteLine($"Hobbies: {hireHobby}");
                Console.WriteLine($"Last Job: {hireLastJob}");

                string decision = string.Empty;
                bool validDecision = false;

                while (!validDecision)
                {
                    Console.Write("Do you want to hire this candidate? (Yes/No): ");
                    decision = Console.ReadLine();

                    if (decision.Equals("Yes"))
                    {
                        // Stat changes based on randomized traits
                        int hobbyValue = traitValues[hireHobby];
                        int lastJobValue = traitValues[hireLastJob];

                        // Update game stats based on trait values
                        //Examples:
                        //personalWelfare += hobbyValue;
                        //populationWelfare += lastJobValue;
                        //something for environment +=

                        hires++;
                        validDecision = true;
                    }
                    else if (decision.Equals("No"))
                    {
                        validDecision = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'Yes' or 'No'.");
                    }
                }
            }
        }

        private static void SupplyChoice()
        {

        }

        
        private static void Promoted()
        {

        }
        private string[] GetRandomCandidate(List<string> names, List<string> hobbies, List<string> lastJobs)
        {
            string hireName = GetRandomCandidateTrait(names);
            string hireHobby = GetRandomCandidateTrait(hobbies);
            string hireLastJob = GetRandomCandidateTrait(lastJobs);

            return new string[] { hireName, hireHobby, hireLastJob };
        }

        private string GetRandomCandidateTrait(List<string> traitList)
        {
            int index = RandomGenerator.Next(traitList.Count);
            return traitList[index];
        }
    }
}
