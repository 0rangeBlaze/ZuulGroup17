namespace WorldOfZuul
{
    static class MiniGames
    {
        static Random random = new Random();
        static int botPoints = 0;
        static int playerPoints = 0;
        public static void RockPaperScizors()
        {  
            botPoints = 0;
            playerPoints = 0;
            
            Dictionary<int, string> dict = new Dictionary<int, string>()
            {
                {0,"Rock" },
                {1,"Paper" },
                {2,"Scizors" }
            };

            Utilities.PrintSlowlyCenter("How many rounds are we gonna play\n");

            Console.CursorLeft = Console.WindowWidth / 2;

            string? srounds = Console.ReadLine();

            int rounds;
            while (srounds == null || int.TryParse(srounds, out _) == false)
            {
                Utilities.CenterColor("You typed something wrong try again", "red");
                srounds = Console.ReadLine();
            }
            int.TryParse(srounds, out rounds);
            Console.Clear();
            var list = dict.Values.ToList();
            string question = "What are gonna show";
            for (int i = 0; i < rounds; i++)
            {
                int bot = random.Next(3);
                int player = Utilities.SelectOption(question, list);
                WhoWins(bot, player);
                Console.CursorVisible = false;
            }
            Utilities.CenterText("So who won ?\n");
            if (botPoints > playerPoints)
            {
                Utilities.CenterColor("You lost ;(", "red");
            }
            else if (botPoints < playerPoints)
            {
                Utilities.CenterColor("You won :)", "green");
            }
            else
            {
                Utilities.CenterColor("It was a draw", "yellow");
            }
            Thread.Sleep(4000);
            Console.Clear() ;
            Console.CursorVisible = true;
        }

        private static void ShowPictures(int bot, int player, string decision)
        {

            string rock = @"
                    _______  
                ---'   ____) 
                      (_____)
                      (_____)
                      (____) 
                ---.__(___)  
            ";

            string paper = @"
                _______       
            ---'   ____)____  
                      ______) 
                     _______) 
                     _______) 
            ---.__________)   
            ";

            string scissors = @"
                _______       
            ---'   ____)____  
                      ______) 
                   __________)
                  (____)      
            ---.__(___)       
            ";

            Dictionary<int, string> intString = new Dictionary<int, string>()
            {
                {0,rock },
                {1, paper },
                {2, scissors }
            };

            List<string> listOfAcsi = new List<string>()
            {
                {scissors},
                {paper},
                {rock}
            };

            CountDown(3, listOfAcsi);

            var linesBot = intString[bot].Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var linesPlayer = intString[player].Split(new[] { Environment.NewLine }, StringSplitOptions.None);


            int maxLines = Math.Max(linesBot.Length, linesPlayer.Length);

            int totalWidth = linesBot.Max(line => line.Length) + decision.Length + linesPlayer.Max(line => line.Length) + 8;

            for (int i = 0; i < maxLines; i++)
            {
                string lineBot = i < linesBot.Length ? linesBot[i].PadRight(linesBot.Max(line => line.Length)) : string.Empty;
                string linePlayer = i < linesPlayer.Length ? linesPlayer[i].PadLeft(linesPlayer.Max(line => line.Length)) : string.Empty;

                if (i == 0)
                {
                    Utilities.CenterText(decision);
                }
                else
                {
                    Console.Write(lineBot);
                    Console.CursorLeft = Console.WindowWidth - linePlayer.Length;
                    Console.WriteLine(new string(linePlayer.ToCharArray().Select(c => FlipSign(c)).Reverse().ToArray()));
                }
            }

            Console.Write($"Bob Points: {botPoints}");
            Console.CursorLeft = Console.WindowWidth - $"Player Points: {playerPoints}".Length;
            Console.WriteLine($"Player Points: {playerPoints}");
            Thread.Sleep(3000);
            Console.Clear();
            char FlipSign(char c) => c == ')' ? '(' : c;

        }


        private static void CountDown(int count, List<string> list)
        {
            for (int i = count; i >= 0; i--)
            {

                if (i != 0)
                {
                    Utilities.CenterText(list[i - 1]);
                    Utilities.CenterText($"Countdown {i}");
                }
                else
                {
                    Utilities.CenterText("Show Time");
                }

                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        private static void WhoWins(int bot, int chosed)
        {
            Dictionary<string, bool> botwins = new Dictionary<string, bool>()
            {
                {"10", true},
                {"20", false},
                {"01", false},
                {"02", true},
                {"21", true},
                {"12", false}
            };

            string decision = "";
            string dicKey = bot.ToString() + chosed.ToString();



            if (bot == chosed)
            {
                decision = "It's a draw";
            }
            else if (botwins[dicKey] == true)
            {
                decision = "Bob wins";
                botPoints++;
            }
            else
            {
                decision = "You won";
                playerPoints++;
            }
            ShowPictures(bot, chosed, decision);
        }

    }
}