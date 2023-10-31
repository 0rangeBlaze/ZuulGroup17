using System;
using System.Collections.Generic;

namespace WorldOfZuul
{
    public class Player {
        public string CurrentRoom {get; set;}
        public string? PreviousRoom {get; set;}
        public string CurrentArea {get; set;}
        public string? PreviousArea {get; set;}
        public int WorkReputation {get; set;}
        public int personalWelfare, populationWelfare, environment;

        public Player(string currentArea, string previousArea, string currentRoom) {
            CurrentArea = currentArea;
            PreviousArea = previousArea;
            CurrentRoom = currentRoom;
            personalWelfare = populationWelfare = environment = 0;
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
        private void SupplyChoice()
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
                SupplyChoiceProvider(providers);
            }
        }
        private void SupplyChoiceProvider(List<Provider> providers)
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
            populationWelfare += providers[input].populationWelfareChange;
            environment += providers[input].environmentChange;
        }
    }
}