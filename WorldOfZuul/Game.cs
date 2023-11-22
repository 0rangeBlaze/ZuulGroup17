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
        public int Turn {get; set;}

        public Game()
        {
            Turn = 0;
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
                if(Player.TasksDone()) 
                {
                    if(Turn == 20)
                    {
                        EndGame();
                    }
                    else
                    {
                        Console.WriteLine("You wake up the next day fully refreshed!");
                        NewsInTheMorning();
                        Turn++;
                        Player.ResetTasks(this);
                    }
                }
            }
            else
            {
                Console.WriteLine("You would much rather sleep in your comfy bed in your bedroom.");
            }

        }
        private void NewsInTheMorning()
        {
            int headlineNumber = Turn % 10;

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
                "Ocean cleanup nonprofit gets 2,5 million dollars from Airbnb co-founder to launch massive plastic pollution cleanup.",
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

            Console.WriteLine(News[headlineNumber]);

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

            Console.WriteLine(News[headlineNumber]);

        }

        private void BetterPopulationWelfare(int headlineNumber)
        {

            string[] News = new string[]
            {
                "This week's headline in the newspaper is that multiple villages in Africa finally got clean drinking water, thanks to donations."
            };

            Console.WriteLine(News[headlineNumber]);
        
        }

        private void WorsePopulationWelfare(int headlineNumber)
        {

            string[] News = new string[]
            {
                "Your local municipality has issued a letter to all residents, to beware of the rising numbers in homelessness."
            };

            Console.WriteLine(News[headlineNumber]);

        }
        
        private void EndGame()
        {
            EnvironmentConclusion();
            HumanWellfareConclusion();
            Console.WriteLine("You can see now, how much effect small, seemingly worthless choices have, if we all do the right thing. Even at times, where these things seem agonizing.");
            Console.WriteLine("Yes, in this game you also had more-and-more control over a large food company, but even if that's not the case in real life, you shouldn't despair, if you just do your seemingly small part, positive change will take effect in time.");
            Console.WriteLine("Hope you enjoyed our game :)");
            Running = false;
            return;
        }

        private void EnvironmentConclusion()
        {
            if(World.Environment >= 80)
                Console.WriteLine("You have managed to look out for the environment through-out your choices, and now you can see its results. Nature is thriving, finally humanity isn't fighting against it, but embracing its strengths.");
                
            else if(World.Environment >= 60)
                Console.WriteLine("");

            else if(World.Environment >= 40)
                Console.WriteLine("");

            else if(World.Environment >= 20)
                Console.WriteLine("");

            else
                Console.WriteLine("");
        }

        private void HumanWellfareConclusion()
        {
            if(World.PopulationWelfare >= 80)
                Console.WriteLine("The population's wellfare has improved greatly. Almost everyone is living well, poverty and homelessness has been has been clamped back, by figures never seen before.");
            
            else if(World.PopulationWelfare >= 60)            
                Console.WriteLine("");

            else if(World.PopulationWelfare >= 40)
                Console.WriteLine("");

            else if(World.PopulationWelfare >= 20)
                Console.WriteLine("");

            else
                Console.WriteLine("");

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
