namespace WorldOfZuul
{
    public partial class Player{
    public class Contract
    {
        private string? CompanyName;
        private string? CompanyShortDescription;
        private int YearOfFoundation;
        private double TotalNetWorth;
        private string? FieldOfWork;
        private string? ContractDescription;
        private string ifAccepted = "";
        private string ifDenied = "";
        private int GoodForEnvironment;
        public static int ContractNamesIndex = 0;
        private int Desirability;


        public static List<Contract> ContractNames = new List<Contract>()
            {
                new()
                {
                    CompanyName = "Lidl",
                    CompanyShortDescription = "Huge german discount retailer",
                    YearOfFoundation = 1932,
                    TotalNetWorth = 1400000000,
                    FieldOfWork = "Food and Clothing",
                    ContractDescription = "Lidl wants to support smaller, local farms, but doesn't have the manpower to allocate to this project. They are asking your company to find the best local farmers close to their big distribution centers. \nThey propose a 6 month trial period with their biggest warehouse, based in Münich. If the sales nor the quality of the food goes down, they will ask to prolong this relationship. \nThey are willing to spend 500'000 euros ",
                    GoodForEnvironment = 8,
                    Desirability = 1,
                    ifAccepted = "You've made the right choice, this is a great deal both for your company, and for the environment.",
                    ifDenied = "Too bad, this would have been a great deal both for your company, and for the environment."
                },

                new()
                {
                    CompanyName = "Bean Harbor Coffee Co.",
                    CompanyShortDescription = "Largest Coffee farming chain in the world",
                    YearOfFoundation = 1914,
                    TotalNetWorth = 20000000000,
                    FieldOfWork = "Coffee bean farming and distribution",
                    ContractDescription = "Bean Harbor Coffee is offering your company a discount for all its products, in exchange for helping their distribution in Europe. \nThis company's methods regarding child labour and enviromentally consciousness are an extremely contreversial topic.",
                    GoodForEnvironment = -5,
                    Desirability = 1,
                    ifAccepted = "While this decision may help you move up the ladder in the company, it may not have been the best for the future of the environment.",
                    ifDenied = "It can seem like you set your company back, by not accepting this contract, ultimately this choice may be for the better."
                },

                new()
                {
                    CompanyName = "Data Innovate Analytics",
                    CompanyShortDescription = "Pioneer in advanced data analytics solutions",
                    YearOfFoundation = 2010,
                    TotalNetWorth = 800000000,
                    FieldOfWork = "Data Science",
                    ContractDescription = "Data Innovate Analytics is proposing a collaboration to develop an AI-driven predictive analytics platform. Your company's part is to provide continoues data regarding people's habits, to optimize the supply chain. The contract is 12 months long . The contract includes free implementation for your company, monthly payments and a substantial bonus if the development is successful.",
                    GoodForEnvironment = 5,
                    Desirability = 1,
                    ifAccepted = "This was a great deal.",
                    ifDenied = "It's a shame you rejected this offer, it would have been good both for the environment and the firm."
                },

                new()
                {
                    CompanyName = "TasteCraft Culinary Solutions",
                    CompanyShortDescription = "Innovators in gourmet food experiences",
                    YearOfFoundation = 2014,
                    TotalNetWorth = 500000000,
                    FieldOfWork = "Food Technology",
                    ContractDescription = "TasteCraft Culinary Solutions wants to collaborate with your company on developing a state-of-the-art meal delivery service. The contract spans 18 months and involves creating a user-friendly app, optimizing delivery logistics, and ensuring food quality standards. Compensation includes milestone payments and a share of the subscription revenue.",
                    GoodForEnvironment = 2,
                    Desirability = 1,
                    ifAccepted = "This is a great deal for you company, it would have been foolish to reject it.",
                    ifDenied = "Too bad you rejected this offer, it would have made your superiors really happy, if they saw the projected revenue figures."
                },

                new()
                {
                    CompanyName = "Organic Harvest Farms",
                    CompanyShortDescription = "Leaders in organic and sustainable farming",
                    YearOfFoundation = 1976,
                    TotalNetWorth = 700000000,
                    FieldOfWork = "Agriculture",
                    ContractDescription = "Organic Harvest Farms is interested in a partnership to develop a farm-to-table mobile app. The contract duration is 12 months, focusing on app design and supply chain integration. Pay-out is a percentage of sales through the app.",
                    GoodForEnvironment = 7,
                    Desirability = 0,
                    ifAccepted = "Accepting this contract aligns with your commitment to sustainability. The financial rewards may be highly dependant on the final up, but that should never stop you.",
                    ifDenied = "By rejecting this contract, you avoid potential challenges associated with the dynamic pay-out. However, consider the missed opportunity to contribute to sustainable farming practices ."
                },

                new()
                {
                    CompanyName = "Apple",
                    CompanyShortDescription = "Highest valued company in the world",
                    YearOfFoundation = 1989,
                    TotalNetWorth = 3000000000000,
                    FieldOfWork = "Smart Devices",
                    ContractDescription = "Apple proposed to work together with you on it's new meal preping application. They want to learn the habits of people when it comes to making food. \nThey propose a 2 year long contract, where you work together to develop an add, that's helping people get the best nutrition. \nThe contract proposes that you get 1,5 million dollars at the end of the first year, and than 25% of the app's revenue.",
                    GoodForEnvironment = 6,
                    Desirability = 1,
                    ifAccepted = "While accepting this contract may seem a bit risky, because of the late pay-date, this is a great deal both for the company and the people's health.",
                    ifDenied = "You avoided the risk of having to work for more than a year out of the company's own pockets, but was it really worth it?"
                }

            };

        private void ContractReview(Game game)
        {
            
            string question = this.GetContractString();
            int goodChoices = 0;
            question += "Would you like to accept this contract?";
            int chosen = Utilities.SelectOption(question, new() {"Yes", "No"});
            if (chosen == 0)
            {
                Utilities.GamePrint(this.ifAccepted);
                game.World.Environment += this.GoodForEnvironment;

                if(this.Desirability == 1)
                    goodChoices++;
            }
            else if (chosen == 1)
            {
                Utilities.GamePrint(this.ifDenied);
                game.World.Environment -= this.GoodForEnvironment;
                if(this.Desirability == 0)
                    goodChoices++;
            }
            Utilities.GamePrint("\n<Press any button to continue>");
            Console.ReadKey(true);
            if(goodChoices > 0)
                game.Player.WorkReputation++;
        }

        public static void ContractReviewWork(Game game) 
        {
            const int NUMBEROFCONTRACTS = 2;


            for(int i = ContractNamesIndex; i < ContractNamesIndex + NUMBEROFCONTRACTS; i++) 
            {
                ContractNames[i%ContractNames.Count].ContractReview(game);
            }
            ContractNamesIndex += NUMBEROFCONTRACTS;
            ContractNamesIndex %= ContractNames.Count;
        }

        public string GetContractString()
        {
            return
            $"\n" +
            $"Company Name: {this.CompanyName} {Environment.NewLine}" +
            $"Short Description: {this.CompanyShortDescription}\n" +
            $"\n" +
            $"Year of Fundation: {this.YearOfFoundation}\n" +
            $"Total Net Worth: {this.TotalNetWorth} dollars\n" +
            $"Field of Work: {this.FieldOfWork}\n" +
            $"\n" +
            $"Contract: {this.ContractDescription}\n";
        }
    }
    }

}