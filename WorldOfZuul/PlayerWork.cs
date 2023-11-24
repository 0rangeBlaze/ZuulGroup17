using System.Linq;
using System;
using System.Collections.Generic;

namespace WorldOfZuul
{
    public partial class Player {
        public void Work(Game game) {
            if(game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("work")) {
                if(!tasks["work"].done) {
                    if (WorkReputation < 5)
                    {
                        SupplyReview(game);
                    }
                    else if (WorkReputation < 10) 
                    {
                        Hire(game);
                    }
                    else if (WorkReputation < 15)
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
                    Utilities.GamePrint("You are tired from the work you have already done today.");
                }
            }
            else{
                Utilities.GamePrint("You need your office environment to be able to work.");
            }
        }

        private void SupplyReview(Game game)
        {
            Utilities.GamePrint("=========");
            Utilities.GamePrint("You are tasked with overlooking the quality of the egg supplements.");
            Utilities.GamePrint("A batch of 25 eggs is only acceptable if there are 5 or less small eggs.");
            Utilities.GamePrint("The good eggs are marked with an 'X' and the small ones with an 'O'.");
            Utilities.GamePrint("After looking at a batch checking its quality: \nType 'y' if its acceptable \nType 'n' if not");

            Utilities.GamePrint("\n<Press any button to continue>");
            Console.ReadKey(true);

            int goodChoices = 0;
            for(int k = 0; k < 3; k++)
            {
                int badEggs = 0;
                bool GoodBatch;
                string question = "";
                for(int i = 0; i < 5; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {     
                        int badEgg = Game.RandomGenerator.Next(1, 6);
                        if(badEgg == 1)
                        {
                            question += "O ";
                            badEggs += 1;
                        }
                        else
                        {
                            question += "X ";
                        }
                    }
                    question += "\n";
                } 

                if(badEggs > 5)
                {
                    GoodBatch = false;
                }
                else
                {
                    GoodBatch = true;
                }
                question += "Is this a good or a bad batch?";
                int chosen = Utilities.SelectOption(question, new List<string> {"yes", "no"});
                if((chosen == 0 && GoodBatch) || (chosen == 1 && !GoodBatch)) {
                    goodChoices++;
                    Utilities.GamePrint("You are correct, your supervisors are going to be satisfied!");
                }
                else {
                    Utilities.GamePrint("Unfortunately you are incorrect, this was a good batch, but do not despair! You can still prove yourself.");
                }
                Utilities.GamePrint("\n<Press any button to continue>");
                Console.ReadKey(true);
            }
            if(goodChoices > 2)
            {
                WorkReputation++;
            }
            Console.Clear();
            Utilities.GamePrint("You are finished for the day, get some rest.");
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
                string question = $"Who would you like to buy {name.FoodName} from?";
                decisionValue += SupplyChoiceProvider(game, providers, question);
            }
            if(decisionValue > 0) {
                WorkReputation++;
            }
        }
        private int SupplyChoiceProvider(Game game, List<Provider> providers, string question)
        {
            List<string> options = new(){};
            for(int i =0; i < providers.Count(); i++)
            {
                options.Add($"{providers[i].ProviderName}, {providers[i].providerDescription}");
            }
            int input = Utilities.SelectOption(question, options);
            
            Utilities.GamePrint($"You have chosen {providers[input].ProviderName}");
            Console.ReadKey(true);
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

                string question = "";
                question += $"Candidate: {hireName}\n";
                question += $"Hobbies: {hireHobby}\n";
                question += $"Last Job: {hireLastJob}\n";

                question += "Do you want to hire this candidate?\n";
                int decision = Utilities.SelectOption("Do you want to hire this candidate?", new() {"Yes", "No"});

                if (decision == 0)
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