using System.Linq;
using System;
using System.Collections.Generic;

namespace WorldOfZuul
{
    public class Player {
        public string CurrentRoom {get; set;}
        public string PreviousRoom {get; set;}
        public string CurrentArea {get; set;}
        public string? PreviousArea {get; set;}
        public int WorkReputation {get; set;}
        private int personalWelfare;
        private Dictionary<string, (bool done, string incompleteMessage)> tasks = new() {
            {"work", (false, "You still haven't done any work today. Your boss will be mad.")}
        };

        public Player(string currentArea, string previousArea, string currentRoom) {
            CurrentArea = currentArea;
            PreviousArea = previousArea;
            CurrentRoom = currentRoom;
            PreviousRoom = currentRoom;
            personalWelfare = 0;
            WorkReputation = 0;
        }
        
        public void Move(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0])) {
                Console.WriteLine("Please choose a direction.");
                return;
            }
            string direction = args[0].ToLower();
            if (direction == "back") {
                if (PreviousArea != CurrentArea) {
                    Console.WriteLine("You came from a different area you need to use travel to go back.");
                }
                else {
                    (CurrentRoom, PreviousRoom) = (PreviousRoom, CurrentRoom);
                }
                return;
            }
            else if (game.World.GetRoom(CurrentArea, CurrentRoom).Exits.ContainsKey(direction))
            {
                PreviousRoom = CurrentRoom;
                CurrentRoom = game.World.GetRoom(CurrentArea, CurrentRoom).Exits[direction];
                PreviousArea = CurrentArea;
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }

        public void Travel(Game game, string[] args)
        {
            if (args.Length < 1 || string.IsNullOrEmpty(args[0])) {
                Console.WriteLine("Please choose a destination.");
                return;
            }
            string destination = args[0].ToLower();
            if (!game.World.Areas.ContainsKey(destination)){
                Console.WriteLine("This destination doesn't exist!");
                return;
            } 
            else if (destination == CurrentArea) {
                Console.WriteLine($"You are already at (the) {CurrentArea}.");
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

            PreviousArea = CurrentArea;
            CurrentArea = destination;
            PreviousRoom = CurrentRoom;
            CurrentRoom = game.World.GetRoom(destination).ShortDescription;
        }

        public void Work(Game game) {
            if(game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("work")) {
                if(!tasks["work"].done) {
                    if (WorkReputation < 5)
                    {
                        SupplyReview(game);
                    }
                    else if (WorkReputation < 10) 
                    {

                    }
                    else if (WorkReputation < 15)
                    {

                    }
                    else //if (WorkReputation < 20)
                    {

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
                        int badEgg = game.RandomGenerator.Next(1, 6);
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
                                Console.WriteLine("Unfortunately you are incorrect, this is a bad batch, but do not despair! You can still prove yourself.");

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

            providers.Add(new Provider() {ProviderName = "Zabka",  personalWelfareChange = -1, environmentChange = -1, populationWelfareChange = -1, providerDescription = "Lorem ipsum"});
            providers.Add(new Provider() {ProviderName = "Lidl",  personalWelfareChange = 1, environmentChange = 1, populationWelfareChange = 1, providerDescription = "Lorem ipsum"});
            providers.Add(new Provider() {ProviderName = "Bilka", personalWelfareChange = 1, environmentChange = 1, populationWelfareChange = 1, providerDescription = "Lorem ipsum"});

            foreach(Food name in foods)
            {
                Console.WriteLine($"Who would you like to buy {name.FoodName} from.?");
                SupplyChoiceProvider(game, providers);
            }
        }
        private void SupplyChoiceProvider(Game game, List<Provider> providers)
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
        }

        public void NextTurn(Game game) {
            if(game.World.GetRoom(CurrentArea, CurrentRoom).Actions.Contains("nextTurn")) {
                foreach(var task in tasks.Values) {
                    if (!task.done) {
                        Console.WriteLine(task.incompleteMessage);
                        return;
                    }
                }
                ResetTasks();
                Console.WriteLine("You wake up the next day fully refreshed!");
            }
            else {
                Console.WriteLine("You would much rather sleep in your comfy bed in your bedroom.");
            }

        }

        private void ResetTasks() {
            tasks["work"] = (false, tasks["work"].incompleteMessage);
        }
    }
}