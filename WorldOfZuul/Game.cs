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
            RandomGenerator = new();
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
                "Filler6", "Filler7", "Filler8", "Filler9", "Filler0"
            };
            //I really need a better way to address these stats
            Dictionary<string, (int PER, int POPU, int ENV)> traitValues = new Dictionary<string, (int, int, int)>
            {
                { "Filler1", (1, 2, 3) },
                { "Filler2", (1, 2, 3) },
                { "Filler3", (1, 2, 3) },
                { "Filler4", (1, 2, 3) },
                { "Filler5", (1, 2, 3) },
                { "Filler6", (1, 2, 3) },
                { "Filler7", (1, 2, 3) },
                { "Filler8", (1, 2, 3) },
                { "Filler9", (1, 2, 3) },
                { "Filler0", (1, 2, 3) },
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
                    decision = Console.ReadLine() ?? "";

                    if (decision.Equals("Yes"))
                    {
                        // Stat changes
                        int hobbyPersonal = traitValues[hireHobby].PER;
                        int hobbyPopulation = traitValues[hireHobby].POPU;
                        int hobbyEnvironment = traitValues[hireHobby].ENV;
                        int lastJobPersonal = traitValues[hireLastJob].PER;
                        int lastJobPopulation = traitValues[hireLastJob].POPU;
                        int lastJobEnvironment = traitValues[hireLastJob].ENV;

                        // Update game stats based on trait values
                        //Examples:
                        //personalWelfare += hobbyPersonal;
                        //populationWelfare += lastJobPopulation;
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
