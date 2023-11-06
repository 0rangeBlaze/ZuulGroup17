namespace WorldOfZuul
{
    public class Contract
    {
        private string? CompanyName;
        private string? CompanyShortDescription;
        private int YearOfFundation;
        private double TotalNetWorth;
        private string? FieldOfWork;
        private string? ContractDescription;
        private string? ifAccepted;
        private int GoodForEnvironment;
        public static int ContractNamesIndex = 0;


        public static List<Contract> ContractNames = new List<Contract>()
            {
                new()
                {
                    CompanyName = "Lidl",
                    CompanyShortDescription = "Huge german discount retailer",
                    YearOfFundation = 1932,
                    TotalNetWorth = 1400000000,
                    FieldOfWork = "Food and Clothing",
                    ContractDescription = "Lidl wants to support smaller, local farms, but doesn't have the manpower to allocate to this project. They are asking your company to find the best local farmers close to their big distribution centers. \nThey propose a 6 month trial period with their biggest warehouse, based in Münich. If the sales nor the quality of the food goes down, they will ask to prolong this relationship. \nThey are willing to spend 500'000 euros ",
                    GoodForEnvironment = 8,
                    ifAccepted = "You've made the right choice, this is a great deal"
                },
                new()
                {
                    CompanyName = "Apple",
                    CompanyShortDescription = "Most valued company in the world",
                    YearOfFundation = 1989,
                    TotalNetWorth = 3000000000000,
                    FieldOfWork = "Smart Devices",
                    ContractDescription = "Apple proposed to work together with you on it's new meal preping application. They want to learn the habits of people when it comes to making food. \nThey propose a 2 year long contract, where you work together to develop an add, that's helping people get the best nutrition. \nThe contract proposes that you get 1,5 million dollars at the end of the first year, and than 25% of the app's revenue.",
                    GoodForEnvironment = 6,
                    ifAccepted = "Good choice blablabla"
                }
            };

        private void ContractReview()
        {
            this.PrintContract();

            Console.WriteLine("Would you like to accept this contract? (y/n)");
            bool signedContract = false;
            while (!signedContract)
            {
                Console.Write(">");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine("");
                if (key.KeyChar == 'y' || key.KeyChar == 'n')
                {

                    Console.WriteLine("");
                    if (key.KeyChar == 'y')
                    {
                        Console.WriteLine(this.ifAccepted);
                    }
                    else if (key.KeyChar == 'n')
                    {
                        Console.WriteLine("You rejected this opportunity. Hope it wasn't a mistake");
                    }

                    signedContract = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again!");
                }
            }
        }

        public static void ContractReviewWork() {
            const int NUMBEROFCONTRACTS = 3;
            for(int i = ContractNamesIndex; i < ContractNamesIndex + NUMBEROFCONTRACTS; i++) {
                ContractNames[i%ContractNames.Count].ContractReview();
            }
            ContractNamesIndex += NUMBEROFCONTRACTS;
            ContractNamesIndex %= ContractNames.Count;
        }

        public void PrintContract()
        {
            Console.WriteLine($"");
            Console.WriteLine($"Company Name: {this.CompanyName}");
            Console.WriteLine($"Short Description: {this.CompanyShortDescription}");
            Console.WriteLine($"");
            Console.WriteLine($"Year of Fundation: {this.YearOfFundation}");
            Console.WriteLine($"Total Net Worth: {this.TotalNetWorth} dollars");
            Console.WriteLine($"Field of Work: {this.FieldOfWork}");
            Console.WriteLine($"");
            Console.WriteLine($"Contract: {this.ContractDescription}");
            Console.WriteLine($"");
        }
    }

}