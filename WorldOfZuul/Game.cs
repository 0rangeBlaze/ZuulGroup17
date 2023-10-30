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
        private Random randomGenerator = new();

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
                "Zuhao", "Oskar", "Daniel", "SebestyÃ©n", "Szymon"
            };

            List<string> hireHobbies = new List<string>
            {
                "Filler1", "Filler2", "Filler3", "Filler4", "Filler5"
            };

            List<string> hireLastJobs = new List<string>
            {
                "Filler1", "Filler2", "Filler3", "Filler4", "Filler5"
            };
            int hires = 0;

            while (hires < 5)
            {
                string[] hireTraits = GetRandomCandidate(hireNames, hireHobbies, hireLastJobs);
                string hireName = hireTraits[0];
                string hireHobby = hireTraits[1];
                string hireLastJob = hireTraits[2];

                Console.WriteLine($"Candidate: {hireName}");
                Console.WriteLine($"Hobbies: {hireHobby}");
                Console.WriteLine($"Last Job: {hireLastJob}");

                Console.Write("Do you want to hire this candidate? (Yes/No): ");
                string decision = Console.ReadLine();

                if (decision == "Yes")
                {
                    // stat changes based on randomized traits
                    hires++;
                }
            }

        }

        private static void SupplyChoice()
        {

        }

        private void SupplyReview()
        {
            Console.WriteLine("=========");
            Console.WriteLine("You are tasked with overlooking the quality of the egg supplements.");
            Console.WriteLine("A batch of 25 eggs is only acceptable if there are 5 or less small eggs.");
            Console.WriteLine("The good eggs are marked with an 'X' and the small ones with an 'O'.");
            Console.WriteLine("After looking at a batch checking its quality: \nType 'y' if its acceptable \nType 'n' if not");

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
                        int badEgg = randomGenerator.Next(1, 6);
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

                if(badEggs > 5)
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
                    if(key.KeyChar == 'y' || key.KeyChar == 'n')
                    {
                        Console.WriteLine("");
                        if(key.KeyChar == 'y')
                        {
                            if(GoodBatch)
                            {
                                goodChoices += 1;
                                Console.WriteLine("You are correct, your supervisors are going to be satisfied!");
                            }
                            else
                                Console.WriteLine("Unfortunately you are uncorrect, this is a bad batch, but do not despair! You can still prove yourself.");
                        }
                        else if(key.KeyChar == 'n')
                        {
                            if(!GoodBatch)
                            {
                                goodChoices += 1;
                                Console.WriteLine("You are correct, your supervisors are going to be satisfied!");
                            }
                            else
                                Console.WriteLine("Unfortunately you are uncorrect, this is a bad batch, but do not despair! You can still prove yourself.");

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
        private string[] GetRandomCandidate(List<string> names, List<string> hobbies, List<string> lastJobs)
        {
            string hireName = GetRandomCandidateTrait(names);
            string hireHobby = GetRandomCandidateTrait(hobbies);
            string hireLastJob = GetRandomCandidateTrait(lastJobs);

            return new string[] { hireName, hireHobby, hireLastJob };
        }

        private string GetRandomCandidateTrait(List<string> traitList)
        {
            int index = randomGenerator.Next(traitList.Count);
            return traitList[index];
        }
    }
}
