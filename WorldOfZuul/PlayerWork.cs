using System.Linq;
using System;
using System.Collections.Generic;

namespace WorldOfZuul
{
    public partial class Player {
        public void Work(Game game) {
            if(game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("work")) {
                if(!tasks["work"].done) {
                    if (WorkReputation < 1)
                    {
                        SupplyReview(game);
                    }
                    else if (WorkReputation < 2) 
                    {
                        Hire(game);
                    }
                    else if (WorkReputation < 3)
                    {
                        SupplyChoice(game);
                    }
                    else
                    {
                        Contract.ContractReviewWork(game);
                    }
                    tasks["work"] = (true, tasks["work"].incompleteMessage);
                }
                else {
                    Console.WriteLine("You are tired from the work you have already done today.");
                }
            }
            else{
                Console.WriteLine("You need your office environment to be able to work.");
            }
        }

        private void SupplyReview(Game game)
        {
            Console.WriteLine("=========");
            Console.WriteLine("You are tasked with overlooking the quality of the egg supplements.");
            Console.WriteLine("A batch of 25 eggs is only acceptable if there are 5 or less small eggs.");
            Console.WriteLine("The good eggs are marked with an 'X' and the small ones with an 'O'.");
            Console.WriteLine("After looking at a batch checking its quality: \nType 'y' if its acceptable \nType 'n' if not");

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
                        int badEgg = Game.RandomGenerator.Next(1, 6);
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
                                Console.WriteLine("Unfortunately you are incorrect, this is a bad batch, but do not despair! You can still prove yourself.");
                        }
                        else if(key.KeyChar == 'n')
                        {
                            if(!GoodBatch)
                            {
                                goodChoices += 1;
                                Console.WriteLine("You are correct, your supervisors are going to be satisfied!");
                            }
                            else
                                Console.WriteLine("Unfortunately you are incorrect, this was a good batch, but do not despair! You can still prove yourself.");

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
                WorkReputation++;
            }

            Console.WriteLine("You are finished for the day, get some rest.");
        }

        private void SupplyChoice(Game game)
        {
            List<Food> foods = new();
            List<Provider> providers = new();
            foods.Add(new Food() {FoodName = "fish"});
            foods.Add(new Food() {FoodName = "meat"});
            foods.Add(new Food() {FoodName = "spices"});
            foods.Add(new Food() {FoodName = "fruits"});
            foods.Add(new Food() {FoodName = "vegetables"});

            providers.Add(new Provider() {ProviderName = "Zabka",  personalWelfareChange = -1, environmentChange = -1, populationWelfareChange = -1, providerDescription = "Lorem ipsum", Desirablity = 0});
            providers.Add(new Provider() {ProviderName = "Lidl",  personalWelfareChange = 1, environmentChange = 1, populationWelfareChange = 1, providerDescription = "Lorem ipsum", Desirablity = 0});
            providers.Add(new Provider() {ProviderName = "Bilka", personalWelfareChange = 1, environmentChange = 1, populationWelfareChange = 1, providerDescription = "Lorem ipsum", Desirablity = 1});
            
            int decisionValue = 0;
            foreach(Food name in foods)
            {
                Console.WriteLine($"Who would you like to buy {name.FoodName} from?");
                decisionValue += SupplyChoiceProvider(game, providers);
            }
            if(decisionValue > 0) {
                WorkReputation++;
            }
        }
        private int SupplyChoiceProvider(Game game, List<Provider> providers)
        {
            for(int i =0; i < providers.Count(); i++)
            {
                Console.WriteLine($"{i+1}. {providers[i].ProviderName}, \n{providers[i].providerDescription} ");
            }
            int input;
            bool inputBool = true;

            do
            {
                inputBool = int.TryParse(Console.ReadLine(), out input);
                input = input -1;
                if(input < 0 || input >= providers.Count())
                {
                    inputBool = false;
                }
            }while(!inputBool);
            
            Console.WriteLine($"{providers[input].ProviderName}");
            Console.WriteLine($"You have chosen {providers[input].ProviderName}");
            personalWelfare += providers[input].personalWelfareChange;
            game.World.PopulationWelfare += providers[input].populationWelfareChange;
            game.World.Environment += providers[input].environmentChange;
            return providers[input].Desirablity;
        }

        private void Hire(Game game)
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
            Dictionary<string, (int PER, int POPU, int ENV, int desirablity)> traitValues = new Dictionary<string, (int, int, int, int)>
            {
                { "Filler1", (1, 2, 3, 1) },
                { "Filler2", (1, 2, 3, 0) },
                { "Filler3", (1, 2, 3, -1) },
                { "Filler4", (1, 2, 3, 1) },
                { "Filler5", (1, 2, 3, 1) },
                { "Filler6", (1, 2, 3, -1) },
                { "Filler7", (1, 2, 3, 1) },
                { "Filler8", (1, 2, 3, 0) },
                { "Filler9", (1, 2, 3, 1) },
                { "Filler0", (1, 2, 3, -1) },
            };

            int hires = 0;
            int decisionValue = 0;
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
                        decisionValue += traitValues[hireHobby].desirablity + traitValues[hireLastJob].desirablity;

                        // Update game stats based on trait values
                        //Examples:
                        personalWelfare += hobbyPersonal + lastJobPersonal;
                        game.World.PopulationWelfare += hobbyPopulation + lastJobPopulation;
                        game.World.Environment += hobbyEnvironment + lastJobEnvironment;
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
            if(decisionValue > 0) {
                WorkReputation++;
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
            int index = Game.RandomGenerator.Next(traitList.Count);
            return traitList[index];
        }
    }
}