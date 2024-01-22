using System.Text.Json;

namespace WorldOfZuul
{
    public class Game
    {
        public Player Player {get; set;}
        public World World {get; set;}
        public bool Running {get; set;}
        public static Random RandomGenerator = new Random();
        public int Turn {get; set;}
        public bool New {get; set;}

        public static Game Load(){
            string savePath = "./saves/save.json";
            Game? game;
            if(File.Exists(savePath))
            {
                int input = Utilities.SelectOption("Do you want to load your previous save?", new List<string> {"yes", "no"});
                if(input == 0) {
                    game = JsonSerializer.Deserialize<Game>(File.ReadAllText(savePath));
                    if(game == null) {
                        game = new();
                        game.Running = false;
                        Utilities.GamePrint("Failed to load save file!");
                    }
                    else {
                        game.Running = true;
                    }
                    return game;
                }
            }
            game = new();
            return game;
        }

        public Game()
        {
            New = true;
            Turn = 0;
            World = new World("assets/world.json");
            if(!World.Loaded) {
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
            CheckSize();

            if(New) {
                New = false;
                PrintWelcome();
                CommandProcessor.Process("help", this);
            }
            Utilities.GamePrint(World.GetRoom(Player.CurrentArea, Player.CurrentRoom).ShortDescription);
            while (Running)
            {
                Console.WriteLine();
                Console.Write("> ");

                string? input = Console.ReadLine();
                CommandProcessor.HandleInput(input, this);
            }

            Utilities.GamePrint("Thank you for playing Lasting Impact!");
        }

        
        public void NextTurn()
        {
            if (World.GetRoom(Player.CurrentArea, Player.CurrentRoom).Actions.Contains("nextTurn"))
            {
                if(Player.TasksDone()) 
                {
                    if(Turn == 3)
                    {
                        EndGame();
                    }
                    else
                    {
                        Utilities.GamePrint("You wake up the next day fully refreshed!");
                        PersonalWelfareChange();
                        LookingOutTheWindow();
                        NewsInTheMorning();
                        Turn++;
                        Player.ResetTasks(this);
                        SettingRoomEvents();
                    }
                }
            }
            else
            {
                Utilities.GamePrint("You would much rather sleep in your comfy bed in your bedroom.");
            }

        }

        private void SettingRoomEvents() {
            if(Turn % 3 == 0) {
                World.GetRoom("Mall", "Hall").Events.Add("ad");
            }
        }

        private void PersonalWelfareChange()
        {
            if(Player.PersonalWelfare > Player.PreviousPersonalWelfare)
            {
                Utilities.GamePrint("Your health has got better.");
            }
            else if(Player.PersonalWelfare < Player.PreviousPersonalWelfare)
            {
                Utilities.GamePrint("Your health has gotten worse.");
            }

            Player.PreviousPersonalWelfare = Player.PersonalWelfare;
        }

        private void LookingOutTheWindow()
        {
            if(World.Environment < 20)
                Utilities.GamePrint("Yeah... Looking out the window you see the remains of the tree in front of your house. Nothing's left but a sad trunk.");
            else if(World.Environment < 40)
                Utilities.GamePrint("Wow, looking out the window you really can't see that far, because of the constant smog.");
            else if(World.Environment < 60)
                Utilities.GamePrint("When you look out the window you see the view you always saw.");
            else if(World.Environment < 80)
                Utilities.GamePrint("Looking out the window is refreshing and encouraging. You see birds resting on the branches, and the air is just crystal clear.");
            else
                Utilities.GamePrint("It really is a pleasure to look out the window. There isn't even a sign of poor or left behind neighbourhood, and nature is just thriving.");
        }

        private void NewsInTheMorning()
        {
            int headlineNumber = Turn % 10;
            Utilities.GamePrint("");
            Utilities.GamePrint("In the  news you read the following headlines:");
            Utilities.GamePrint("");
            if(World.Environment > World.PreviousEnvironment)
            {
                BetterEnvironment(headlineNumber);
            }
            else if(World.Environment < World.PreviousEnvironment)
            {
                WorseEnvironment(headlineNumber);
            }

            if(World.PopulationWelfare > World.PreviousPopulationWelfare)
            {
                BetterPopulationWelfare(headlineNumber);
            }
            else
            {
                WorsePopulationWelfare(headlineNumber);
            }


            World.PreviousEnvironment = World.Environment;
            World.PreviousPopulationWelfare = World.PopulationWelfare;
        }

        private void BetterEnvironment(int headlineNumber)
        {

            string[] News = new string[]
            {
                "Farmers were organized to collect eggs of endangered Wildfowl, saving the Australian species.",
                "This California highway has just become the first state road made from recycled plastic in the US.",
                "World's largest green hydrogen plant will soon be turning California's trash into ultra-cheap fuel.",
                "U.S. approves plan to build the Nation's largest solar project in the desert.",
                "A new poll has just been released, saying that 66% of Americans willing to try anything that can help save the environment.",
                "Divers recovered 12 tons of trash from lake Tahoe.",
                "Patagonia, the American retailer of outdoor recreation clothing store, gives away its entire $3 billion worth to fight climate change.",
                "Ocean clean-up non-profit gets 2,5 million dollars from Airbnb co-founder to launch massive plastic pollution clean-up.",
                "Germany prioritizes climate crisis by supporting sustainability in developing countries with $4 billion plan.",
                "Finnish scuba-diver finds a way to turn sea algae into a replacement for plastic in common products.",
                "Tesco, the Europe's biggest supermarket chain, removed 1 billion pieces of plastic from across its stores.",
                "There has been a surprising reduction in the number of hurricanes in the past years.",
                "The World achieves its target to protect more land, adding 42%—the size of Russia—in last decade.",
                "Volunteers in India are planting 250 million saplings in single day and seeing 80% survival rate.",
                "World's 3rd largest grocery chain eliminates 20 million single-use plastic wrappings from Christmas goodies.",
                "France debuts geothermal plant using heat from the earth to power 10,000 homes.",
                "The UK has enforced a new Waste Reduction Policy.",
                "Bezos created $10 billion Earth Fund to meet climate crisis, first grants of $800M go to iconic environmental groups.",
                "Australian scientists create seaweed supplement for cows that reduces methane emissions by 80%",
                "The Search Engine that plants trees with every search has just planted its 100-millionth tree."
            };

            Utilities.GamePrint("\t" + News[headlineNumber]);

        }

        private void WorseEnvironment(int headlineNumber)
        {

            string[] News = new string[]
            {
                "Disaster in Lisbon, a sandstorm hit, reasons are still unclear.",
                "Scientists say Earth on track for disastrous sea level rise.",
                "Global heat deaths could quadruple if action is not taken on climate change, study finds.",
                "Worsening warming is hurting people in all regions, U.S. climate assessment shows.",
                "4000 evacuating in Iceland under threat of volcanic eruption.",
                "More than 100 dolphins found dead in Brazilian Amazon rainforest drought.",
                "Underground heat from climate change could cause cities to sink.",
                "Flooding from cyclone in southern Columbia kills dozens.",
                "Earth just had its hottest 12 months ever recorded, analysis finds.",
                "Drought that has hammered Syria, Iraq and Iran was exacerbated by climate change.",
                "Melting of West Antarctic Ice Sheet may be completely unavoidable already, new comprehensive study finds.",
                "Extreme ocean temperatures threaten to wipe out Caribbean coral.",
                "Amazon rainforest port records lowest water level in 121 years amid drought.",
                "U.K.'s filthy waterways foul up government image as companies dump sewage in rivers.",
                "Flash floods foreshadow a threatening reality in New York City.",
                "Air pollution causes an estimated 10 to 12 million premature deaths annually.",
                "18 million tons of plastic enters the ocean every year as a result of mismanagement of domestic waste in coastal areas.",
                "Pathogen-polluted drinking water and inadequate sanitation cause approximately 1.4 million human deaths annually, with many millions more becoming ill.",
                "Brazil gripped by 'unbearable' heatwave.",
                "Airport submits permit for more private jet flights."
            };

            Utilities.GamePrint("\t" + News[headlineNumber]);

        }

        private void BetterPopulationWelfare(int headlineNumber)
        {

            string[] News = new string[]
            {
                "Hungarian Mental Health Awareness Campaigns Contribute to National Well-being",
                "EU's Innovative Policies Bring a Remarkable Rise in Happiness Index",
                "In Venezuela Community Support Programs Reap Benefits, Enhancing Public Welfare",
                "Zambian Government's Initiatives Lead to Surge in Employment and Well-being",
                "Economic Explosion Results in Significant Boost to Public Welfare",
                "Healthcare Overhaul Results in Improved Well-being Across the U.S.",
                "Croatian Education Reforms Bridge Gaps, Elevating Overall Welfare",
                "Holland's Housing Problems Solved, Contributing to Elevated Quality of Life Across All Social Classes",
                "Social Safety Nets Strengthened, Ensuring a Better Quality of Life"
            };

            Utilities.GamePrint("\t" + News[headlineNumber]);
        
        }

        private void WorsePopulationWelfare(int headlineNumber)
        {

            string[] News = new string[]
            {
                "The British are risingly unhappy, due to the French",
                "Outbreak Breaks Out in Guatemala",
                "Rising Inequality in Bulgaria Sparks Concerns About the People's Well-being",
                "Economic Downturn Takes Toll on Public Happiness",
                "U.S.'s Social Safety Nets Strained as Welfare Declines Nationwide",
                "Austrian Budget Cuts Lead to Challenges in Access to Vital Services",
                "Well-Being Among Young Adults is Declining, Harvard-led Study Finds",
                "Tension Caused by Unemployment Alarms People about the French Welfare",
                "Survey Reveals People's Mental and Physical Health Declining",
                "Swiss Government Faces Criticism Over Worsening Social Welfare Indicators"
            };

            Utilities.GamePrint("\t" + News[headlineNumber]);

        }
        
        private void EndGame()
        {
            EnvironmentConclusion();
            HumanWellfareConclusion();
            Utilities.GamePrint("You can see now, how much effect small, seemingly worthless choices have, if we all do the right thing. Even at times, where these things seem agonizing.");
            Utilities.GamePrint("Yes, in this game you also had more-and-more control over a large food company, but even if that's not the case in real life, you shouldn't despair, if you just do your seemingly small part, positive change will take effect in time.");
            Utilities.GamePrint("Make sure to do the right thing!\n");
            Utilities.GamePrint("Hope you enjoyed our game :)");
            Running = false;
            return;
        }

        private void EnvironmentConclusion()
        {
            if(World.Environment >= 80)
                Utilities.GamePrint("You have managed to look out for the environment through-out your choices, and now you can see its results. Nature is thriving, finally humanity isn't fighting against it, but embracing its strengths.");
                
            else if(World.Environment >= 60)
                Utilities.GamePrint("You have gotten the environment to a better state, compared to when you started, this is all thanks to your choices. If we were to continue down this road, with time, we could achieve a situation where humanity isn't fighting against nature but embracing its strengths.");

            else if(World.Environment >= 40)
                Utilities.GamePrint("The state of the environments health more or less stayed the same through-out your progress. This isn't a bad thing, but with a bit more effort we could make sure that this planet survives with us.");

            else if(World.Environment >= 20)
                Utilities.GamePrint("Following your choices humanity has made the state of the environment worse, with careless consumption and production, wasting resources. Maybe only looking out for your own interest isn't the best for the future.");

            else
                Utilities.GamePrint("Humanity has almost completely wiped-out nature. We are as far from symbiosis as we can be. Maybe if, just a few times, we put aside our selfish interests we can make sure that everyone and everything is living happily and healthy on this planet.");
        }

        private void HumanWellfareConclusion()
        {
            if(World.PopulationWelfare >= 80)
                Utilities.GamePrint("The population's wellfare has improved greatly. Almost everyone is living well, poverty and homelessness has been has been clamped back, by figures never seen before.");
            
            else if(World.PopulationWelfare >= 60)            
                Utilities.GamePrint("The population's welfare has improved. Most people are living well, both poverty and homelessness has been clamped back. World hunger is closer to being solved than ever before.");

            else if(World.PopulationWelfare >= 40)
                Utilities.GamePrint("The population's welfare has more-or-less stayed the same. This isn't a bad thing, but with a bit more effort we could make sure that more people are living well.");

            else if(World.PopulationWelfare >= 20)
                Utilities.GamePrint("The population's welfare has gotten worse because of your decisions. Homelessness has grown, the less developed countries are falling behind even more, and even the developed countries are struggling.");

            else
                Utilities.GamePrint("Due to some of your decisions the population's welfare has gotten a lot worse. The gap between the social stratums is the biggest it has ever been. ");

        }



        private static void PrintWelcome()
        {
            Console.Clear();
            Utilities.GamePrint("Welcome to Lasting Impact!");
            Utilities.GamePrint("In this game you will experience humanity's impact on the environment.\n");
            Utilities.GamePrint("You will be playing as an average person whose impact on the world is projected to an entire generation of people.");
            Utilities.GamePrint("Throughout your choices you will have to look out for the environment's health, the population's welfare, and your own health.");
            Utilities.GamePrint("");
            Utilities.GamePrint("The game is built on a turn basis, each representing a large amount of time, like 1 or 2 years.");
            Utilities.GamePrint("In one turn you have to work in your office, eat in one of the restaurants and choose to sort trash in your kitchen. On top of this, feel free to wonder around, and see your impact in real time.");
            Utilities.GamePrint();
        }

        public static void CheckSize(){
            Console.Clear();
            Console.CursorVisible=false;
            bool accepted = false;
            int minW=105;
            int minH=45;
            while(!accepted || (Console.WindowWidth < minW) || (Console.WindowHeight < minH)){
                accepted=false;
                Console.WriteLine("Set the window to a size, where both lines are green!");
                Console.WriteLine("Please don't change the size of the window during the game or you might come across some issue.");
                Console.WriteLine();
                if(Console.WindowWidth < minW) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your window isn't wide enough!");
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your window is wide enough!");
                }
                Console.WriteLine();
                if(Console.WindowHeight < minH) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Your window isn't tall enough!");
                }
                else{
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your window is tall enough!");
                }
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("<Press any keys to continue>");

                while(Console.KeyAvailable){
                    Console.ReadKey(true);
                    accepted=true;
                }
                Thread.Sleep(33);
                Console.Clear();
            }
            Console.Clear();
            Console.CursorVisible=true;
        }
    }
}
