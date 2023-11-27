using System.Linq;
using System;
using System.Collections.Generic;

namespace WorldOfZuul
{
    public partial class Player {
        public void Work(Game game) {
            if(game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("work")) {
                if(!tasks["work"].done) {
                    if (WorkReputation < 2)
                    {
                        SupplyReview(game);
                    }
                    else if (WorkReputation == 10000) 
                    {
                        Hire(game);
                    }
                    else if (WorkReputation < 4)
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
            Utilities.GamePrint("After looking at a batch checking its quality: \nChoose 'yes' if its acceptable \nChoose 'no' if not");

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
            List<Provider> providers = new() {
                new Provider() {
                    Food = "fish", ProviderName = "EcoHarbor Fisheries",  
                    personalWelfareChange = -5, environmentChange = 5, populationWelfareChange = -5, 
                    providerDescription = "They invest in advanced technologies to reduce bycatch, use selective fishing methods, and support marine conservation initiatives. EcoHarbor works closely with local communities to ensure responsible fishing practices. Customers appreciate the premium quality and traceability of their products. Products from EcoHarbor Fisheries are positioned at a higher price point.", 
                    Desirablity = 0
                },
                new Provider() {
                    Food = "fish", ProviderName = "MarineMingle Seafoods",
                    personalWelfareChange = -5, environmentChange = 5, populationWelfareChange = 5, 
                    providerDescription = " MarineMingle Seafoods source from a mix of wild-caught and aquaculture suppliers, with an emphasis on responsible practices. MarineMingle offers seafood at a mid-range price.",
                    Desirablity = 1
                },
                new Provider() {
                    Food = "fish", ProviderName = "BlueCrest Seafoods",
                    personalWelfareChange = 5, environmentChange = -10, populationWelfareChange = 5, 
                    providerDescription = "They may source from fisheries with questionable environmental practices, including overfishing and habitat destruction. BlueCrest Seafoods priced at the lower end of the market.",
                    Desirablity = 1
                },
                new Provider() {
                    Food = "fruits", ProviderName = "Sunrise Orchards Cooperative",
                    personalWelfareChange = 5, environmentChange = 10, populationWelfareChange = 7, 
                    providerDescription = "Sunrise Orchards Cooperative is a collective of small-scale farmers emphasizing community-based agriculture. They follow a mix of organic and conventional farming practices, with a focus on fair trade principles. They produce relatively affordable goods with good quality.",
                    Desirablity = 1
                },
                new Provider() {
                    Food = "fruits", ProviderName = "BudgetHarvest Fruits",
                    personalWelfareChange = 7, environmentChange = -7, populationWelfareChange =7, 
                    providerDescription = "BudgetHarvest Fruits aims to make fresh produce accessible to all, but their practices may not prioritize sustainability. They source fruits from various farms, including those with less environmentally friendly practices. However, customers are drawn to BudgetHarvest for their incredibly low prices, however their quiality is often undesirable.",
                    Desirablity = 1
                },
                new Provider() {
                    Food = "fruits", ProviderName = "EverGreen Harvests",
                    personalWelfareChange = -7, environmentChange = 10, populationWelfareChange = 10, 
                    providerDescription = "EverGreen Harvests is dedicated to sustainable fruit production through regenerative agriculture. They implement cutting-edge techniques such as agroforestry and use organic practices to ensure soil health. EverGreen actively engages with local communities to promote responsible farming and biodiversity. Customers appreciate the exceptional quality and unique varieties of fruits. EverGreen Harvests products are extremely expensive.",
                    Desirablity = 0
                },
                new Provider() {
                    Food = "meat", ProviderName = "EconoMeat Processors", 
                    personalWelfareChange = 0, environmentChange = -12, populationWelfareChange = 2,
                    providerDescription = "They source meat from various suppliers, including those with less environmentally friendly practices. EconoMeat may use conventional farming methods and be less transparent about sourcing. Generally cheap with an exception for a few quality products.", 
                    Desirablity = 0
                },
                new Provider() {
                    Food = "meat", ProviderName = "CommunityCraze Ranch", 
                    personalWelfareChange = 5, environmentChange = 6, populationWelfareChange = 9,
                    providerDescription = "They source from local farmers who follow a mix of conventional and sustainable practices, emphasizing humane treatment of animals. CommunityCraze Ranch is committed to supporting local economies and responsible farming. They offer meat at a mid-range price with great quality.", 
                    Desirablity = 1
                },
                new Provider() {
                    Food = "meat", ProviderName = "SustainableSavanna Meats", 
                    personalWelfareChange = -8, environmentChange = 15, populationWelfareChange = 1,
                    providerDescription = "SustainableSavanna Meats is a pioneer in sustainable and ethical meat production. They prioritize pasture-raised, grass-fed livestock, avoiding the use of hormones and antibiotics. SustainableSavanna engages in regenerative farming practices to enhance soil health and biodiversity. Unfortunately the quality of their products is often bad and they are expensive.", 
                    Desirablity = 0
                },
                new Provider() {
                    Food = "spices", ProviderName = "Needs_To_Be_Written", 
                    personalWelfareChange = 0, environmentChange = 0, populationWelfareChange = 0,
                    providerDescription = "Ignore this, and just skip", 
                    Desirablity = 1
                },
                new Provider() {
                    Food = "spices", ProviderName = "Needs_To_Be_Written", 
                    personalWelfareChange = 0, environmentChange = 0, populationWelfareChange = 0,
                    providerDescription = "Ignore this, and just skip", 
                    Desirablity = 1
                },
                new Provider() {
                    Food = "spices", ProviderName = "Needs_To_Be_Written", 
                    personalWelfareChange = 0, environmentChange = 0, populationWelfareChange = 0,
                    providerDescription = "Ignore this, and just skip", 
                    Desirablity = 1
                },
                
            };
            foods.Add(new Food() {FoodName = "fish"});
            foods.Add(new Food() {FoodName = "fruits"});
            foods.Add(new Food() {FoodName = "meat"});
            foods.Add(new Food() {FoodName = "spices"});
            //foods.Add(new Food() {FoodName = "vegetables"});


            
            int decisionValue = 0;

            for(int i = game.Player.CurrentProviderIndex; i < game.Player.CurrentProviderIndex + 2; i++)
            {
                string question = $"Who would you like to buy {foods[i].FoodName} from?";
                decisionValue += SupplyChoiceProvider(game, providers, question, foods[i]);

            }    
            /*
            foreach(Food name in foods)
            {
                string question = $"Who would you like to buy {name.FoodName} from?";
                decisionValue += SupplyChoiceProvider(game, providers, question, name);
            }
            */

            Console.WriteLine("You have worked enough for today, get some rest");
            game.Player.CurrentProviderIndex += 2;
            if(decisionValue >= 0) {
                WorkReputation++;
            }

        }
        private int SupplyChoiceProvider(Game game, List<Provider> providers, string question, Food food)
        {
            List<Provider> filteredProviders = new(){};
            List<string> options = new(){};
            string descriptions = "";
            for(int i = 0; i < providers.Count; i++)
            {
                if(food.FoodName == providers[i].Food){
                    filteredProviders.Add(providers[i]);
                    options.Add($"{providers[i].ProviderName}");
                    descriptions += $"{providers[i].ProviderName}:\n\n {providers[i].providerDescription}\n\n\n";
                }
            }
            descriptions += "\n" + question;
            int input = Utilities.SelectOption(descriptions, options);

            Utilities.GamePrint($"You have chosen {filteredProviders[input].ProviderName}");
            Console.ReadKey(true);
            personalWelfare += filteredProviders[input].personalWelfareChange;
            game.World.PopulationWelfare += filteredProviders[input].populationWelfareChange;
            game.World.Environment += filteredProviders[input].environmentChange;
            return filteredProviders[input].Desirablity;
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
            Dictionary<string, (int PER, int ENV, int desirablity)> traitValues = new Dictionary<string, (int, int, int)>
            {
                { "Filler1", (1, 3, 1) },
                { "Filler2", (1, 3, 0) },
                { "Filler3", (1, 3, -1) },
                { "Filler4", (1, 3, 1) },
                { "Filler5", (1, 3, 1) },
                { "Filler6", (1, 3, -1) },
                { "Filler7", (1, 3, 1) },
                { "Filler8", (1, 3, 0) },
                { "Filler9", (1, 3, 1) },
                { "Filler0", (1, 3, -1) },
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
                    int hobbyEnvironment = traitValues[hireHobby].ENV;
                    int lastJobPersonal = traitValues[hireLastJob].PER;
                    int lastJobEnvironment = traitValues[hireLastJob].ENV;
                    decisionValue += traitValues[hireHobby].desirablity + traitValues[hireLastJob].desirablity;

                    // Update game stats based on trait values
                    //Examples:

                    personalWelfare += hobbyPersonal + lastJobPersonal;
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