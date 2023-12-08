using System.Text;
using System.Diagnostics;
using System.Threading;

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

            var linesBot = intString[bot].Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            var linesPlayer = intString[player].Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);


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
                    Console.Write(new string(linePlayer.ToCharArray().Select(c => FlipSign(c)).Reverse().ToArray()));
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

        /*
        ################################################################################


        UNDER THIS LINE ARE NOT OUR GAMES. 
        WE GOT IT FROM ZACHARY PATTEN'S GITHUB RESPORITORY, AND THEY ARE NOT TO BE EVELUATED


        ################################################################################    
        */






















        public static void WhackAMole()
        {
            
            string Board =
    @" ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗" + '\n' +
    @" ║ 7 ║       ║ ║ 8 ║       ║ ║ 9 ║       ║" + '\n' +
    @" ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ╚═══════╝     ╚═══════╝     ╚═══════╝" + '\n' +
    @" ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗" + '\n' +
    @" ║ 4 ║       ║ ║ 5 ║       ║ ║ 6 ║       ║" + '\n' +
    @" ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ╚═══════╝     ╚═══════╝     ╚═══════╝" + '\n' +
    @" ╔═══╦═══════╗ ╔═══╦═══════╗ ╔═══╦═══════╗" + '\n' +
    @" ║ 1 ║       ║ ║ 2 ║       ║ ║ 3 ║       ║" + '\n' +
    @" ╚═══╣       ║ ╚═══╣       ║ ╚═══╣       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ║       ║     ║       ║     ║       ║" + '\n' +
    @"     ╚═══════╝     ╚═══════╝     ╚═══════╝";

            string JavaNoob =
                @" ╔══─┐ " + '\n' +
                @" │o-o│ " + '\n' +
                @"┌└───┘┐" + '\n' +
                @"││ J ││";

            string Empty =
                @"       " + '\n' +
                @"       " + '\n' +
                @"       " + '\n' +
                @"       ";

            TimeSpan playTime = TimeSpan.FromSeconds(30);

            if (OperatingSystem.IsWindows())
            {
                Console.WindowWidth = Math.Max(Console.WindowWidth, 50);
                Console.WindowHeight = Math.Max(Console.WindowHeight, 22);
            }

            
            
                Console.Clear();
                Console.WriteLine("Whack A Mole");
                Console.WriteLine();
                Console.WriteLine(
                    $"You have {(int)playTime.TotalSeconds} seconds to whack as many moles as you " +
                    "can before the timer runs out. Use the number keys 1-9 to whack. Are you ready? ");
                Console.WriteLine();
                Console.WriteLine("Play [enter], or quit [escape]?");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        Play();
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("Whack A Mole was closed...");
                    break;   
                }

            

            void Play()
            {
                Console.Clear();
                Console.WriteLine("Whack A Mole");
                Console.WriteLine();
                Console.WriteLine(Board);
                DateTime start = DateTime.Now;
                int score = 0;
                int moleLocation = Random.Shared.Next(1, 10);
                Console.CursorVisible = false;
                while (DateTime.Now - start < playTime)
                {
                    var (left, top) = Map(moleLocation);
                    Console.SetCursorPosition(left, top);
                    Render(JavaNoob);
                    int selection;
                GetInput:
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1 or ConsoleKey.NumPad1: selection = 1; break;
                        case ConsoleKey.D2 or ConsoleKey.NumPad2: selection = 2; break;
                        case ConsoleKey.D3 or ConsoleKey.NumPad3: selection = 3; break;
                        case ConsoleKey.D4 or ConsoleKey.NumPad4: selection = 4; break;
                        case ConsoleKey.D5 or ConsoleKey.NumPad5: selection = 5; break;
                        case ConsoleKey.D6 or ConsoleKey.NumPad6: selection = 6; break;
                        case ConsoleKey.D7 or ConsoleKey.NumPad7: selection = 7; break;
                        case ConsoleKey.D8 or ConsoleKey.NumPad8: selection = 8; break;
                        case ConsoleKey.D9 or ConsoleKey.NumPad9: selection = 9; break;
                        case ConsoleKey.Escape:
                            Console.Clear();
                            Console.WriteLine("Whack A Mole was closed...");
                            return;
                        default: goto GetInput;
                    }
                    if (moleLocation == selection)
                    {
                        score++;
                        Console.SetCursorPosition(left, top);
                        Render(Empty);
                        int newMoleLocation = Random.Shared.Next(1, 9);
                        moleLocation = newMoleLocation >= moleLocation ? newMoleLocation + 1 : newMoleLocation;
                    }
                }
                Console.CursorVisible = true;
                Console.Clear();
                Console.WriteLine("Whack A Mole");
                Console.WriteLine();
                Console.WriteLine(Board);
                Console.WriteLine();
                Console.WriteLine("Game Over. Score: " + score);
                Console.WriteLine("You are good at this hope next time you will do better");
                Console.WriteLine();
                Console.WriteLine("Press [Enter] To Continue...");
                Console.ReadLine();
            }

            (int Left, int Top) Map(int hole) =>
                hole switch
                {
                    1 => (06, 15),
                    2 => (20, 15),
                    3 => (34, 15),
                    4 => (06, 09),
                    5 => (20, 09),
                    6 => (34, 09),
                    7 => (06, 03),
                    8 => (20, 03),
                    9 => (34, 03),
                    _ => throw new NotImplementedException(),
                };

            void Render(string @string)
            {
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                foreach (char c in @string)
                {
                    if (c is '\n')
                    {
                        Console.SetCursorPosition(x, ++y);
                    }
                    else
                    {
                        Console.Write(c);
                    }
                }
            }
        }


        public static void   RunGame()
        {
            int width = 50;
            int height = 30;

            int windowWidth;
            int windowHeight;
            Random random = new();
            char[,] scene;
            int score = 0;
            int carPosition;
            int carVelocity;
            bool gameRunning;
            bool keepPlaying = true;
            bool consoleSizeError = false;
            int previousRoadUpdate = 0;

            Console.CursorVisible = false;
            try
            {
                Initialize();
                LaunchScreen();
                while (keepPlaying)
                {
                    InitializeScene();
                    while (gameRunning)
                    {
                        if (Console.WindowHeight < height || Console.WindowWidth < width)
                        {
                            consoleSizeError = true;
                            keepPlaying = false;
                            break;
                        }
                        HandleInput();
                        Update();
                        Render();
                        if (gameRunning)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(33));
                        }
                    }
                    if (keepPlaying)
                    {
                        GameOverScreen();
                    }
                }
                Console.Clear();
                if (consoleSizeError)
                {
                    Console.WriteLine("Console/Terminal window is too small.");
                    Console.WriteLine($"Minimum size is {width} width x {height} height.");
                    Console.WriteLine("Increase the size of the console window.");
                }
                Console.WriteLine("Drive was closed.");
                Console.ReadKey(true);
            }
            finally
            {
                Console.CursorVisible = true;
            }

            void Initialize()
            {
                windowWidth = Console.WindowWidth;
                windowHeight = Console.WindowHeight;
                if (OperatingSystem.IsWindows())
                {
                    if (windowWidth < width && OperatingSystem.IsWindows())
                    {
                        windowWidth = Console.WindowWidth = width + 1;
                    }
                    if (windowHeight < height && OperatingSystem.IsWindows())
                    {
                        windowHeight = Console.WindowHeight = height + 1;
                    }
                    Console.BufferWidth = windowWidth;
                    Console.BufferHeight = windowHeight;
                }
            }

            void LaunchScreen()
            {
                Console.Clear();
                Console.WriteLine("You decided to go for a run");
                Console.WriteLine();
                Console.WriteLine("Stay on the road!");
                Console.WriteLine();
                Console.WriteLine("Use A, W, and D to control your velocity.");
                Console.WriteLine();
                Console.Write("Press [enter] to start...");
                PressEnterToContinue();
            }

            void InitializeScene()
            {
                const int roadWidth = 10;
                gameRunning = true;
                carPosition = width / 2;
                carVelocity = 0;
                int leftEdge = (width - roadWidth) / 2;
                int rightEdge = leftEdge + roadWidth + 1;
                scene = new char[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (j < leftEdge || j > rightEdge)
                        {
                            scene[i, j] = '.';
                        }
                        else
                        {
                            scene[i, j] = ' ';
                        }
                    }
                }
            }

            void Render()
            {
                StringBuilder stringBuilder = new(width * height);
                for (int i = height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i == 1 && j == carPosition)
                        {
                            stringBuilder.Append(
                                !gameRunning ? 'X' :
                                carVelocity < 0 ? '<' :
                                carVelocity > 0 ? '>' :
                                '^');
                        }
                        else
                        {
                            stringBuilder.Append(scene[i, j]);
                        }
                    }
                    if (i > 0)
                    {
                        stringBuilder.AppendLine();
                    }
                }
                Console.SetCursorPosition(0, 0);
                Console.Write(stringBuilder);
            }

            void HandleInput()
            {
                while (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.A or ConsoleKey.LeftArrow:
                            carVelocity = -1;
                            break;
                        case ConsoleKey.D or ConsoleKey.RightArrow:
                            carVelocity = +1;
                            break;
                        case ConsoleKey.W or ConsoleKey.UpArrow or ConsoleKey.S or ConsoleKey.DownArrow:
                            carVelocity = 0;
                            break;
                        case ConsoleKey.Escape:
                            gameRunning = false;
                            keepPlaying = false;
                            break;
                        case ConsoleKey.Enter:
                            Console.ReadLine();
                            break;
                    }
                }
            }

            void GameOverScreen()
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Run Over");
                Console.WriteLine($"Distance: {score} meters");
                Console.WriteLine($"Run Again (Y/N)?");
            GetInput:
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Y:
                        keepPlaying = true;
                        break;
                    case ConsoleKey.N or ConsoleKey.Escape:
                        keepPlaying = false;
                        break;
                    default:
                        goto GetInput;
                }
            }

            void Update()
            {
                for (int i = 0; i < height - 1; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        scene[i, j] = scene[i + 1, j];
                    }
                }
                int roadUpdate = 
                    random.Next(5) < 4 ? previousRoadUpdate :
                    random.Next(3) - 1;
                if (roadUpdate is -1 && scene[height - 1, 0] is ' ') roadUpdate = 1;
                if (roadUpdate is 1 && scene[height - 1, width - 1] is ' ') roadUpdate = -1;
                switch (roadUpdate)
                {
                    case -1: // left
                        for (int i = 0; i < width - 1; i++)
                        {
                            scene[height - 1, i] = scene[height - 1, i + 1];
                        }
                        scene[height - 1, width - 1] = '.';
                        break;
                    case 1: // right
                        for (int i = width - 1; i > 0; i--)
                        {
                            scene[height - 1, i] = scene[height - 1, i - 1];
                        }
                        scene[height - 1, 0] = '.';
                        break;
                }
                previousRoadUpdate = roadUpdate;
                carPosition += carVelocity;
                if (carPosition < 0 || carPosition >= width || scene[1, carPosition] is not ' ')
                {
                    gameRunning = false;
                }
                score++;
            }

            void PressEnterToContinue()
            {
            GetInput:
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Escape:
                        keepPlaying = false;
                        break;
                    default: goto GetInput;
                }
            }
        }
        public static void ConnectFour()
        {
            

            Exception? exception = null;

            bool?[,] board = new bool?[7, 6];
            bool player1Turn;
            bool player1MovesFirst = true;
            Random random = new();

            const int moveMinI = 5;
            const int moveJ = 2;

            try
            {
                Console.CursorVisible = false;
            PlayAgain:
                player1Turn = player1MovesFirst;
                player1MovesFirst = !player1MovesFirst;
                ResetBoard();
                while (true)
                {
                    (int I, int J) move = default;
                    if (player1Turn)
                    {
                        RenderBoard();
                        int i = 0;
                        Console.SetCursorPosition(moveMinI, moveJ);
                        Console.Write('v');
                    GetPlayerInput:
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.LeftArrow:
                                Console.SetCursorPosition(i * 2 + moveMinI, moveJ);
                                Console.Write(' ');
                                i = Math.Max(0, i - 1);
                                Console.SetCursorPosition(i * 2 + moveMinI, moveJ);
                                Console.Write('v');
                                goto GetPlayerInput;
                            case ConsoleKey.RightArrow:
                                Console.SetCursorPosition(i * 2 + moveMinI, moveJ);
                                Console.Write(' ');
                                i = Math.Min(board.GetLength(0) - 1, i + 1);
                                Console.SetCursorPosition(i * 2 + moveMinI, moveJ);
                                Console.Write('v');
                                goto GetPlayerInput;
                            case ConsoleKey.Enter:
                                if (board[i, board.GetLength(1) - 1] != null)
                                {
                                    goto GetPlayerInput;
                                }
                                for (int j = board.GetLength(1) - 1; ; j--)
                                {
                                    if (j is 0 || board[i, j - 1].HasValue)
                                    {
                                        board[i, j] = true;
                                        move = (i, j);
                                        break;
                                    }
                                }
                                break;
                            case ConsoleKey.Escape:
                                Console.Clear();
                                return;
                            default: goto GetPlayerInput;
                        }
                        if (CheckFor4(move.I, move.J))
                        {
                            RenderBoard();
                            Console.WriteLine();
                            Console.WriteLine("   You Win!");
                            goto PlayAgainCheck;
                        }
                    }
                    else
                    {
                        int[] moves = Enumerable.Range(0, board.GetLength(0)).Where(i => !board[i, board.GetLength(1) - 1].HasValue).ToArray();
                        int randomMove = moves[random.Next(moves.Length)];
                        for (int j = board.GetLength(1) - 1; ; j--)
                        {
                            if (j is 0 || board[randomMove, j - 1].HasValue)
                            {
                                board[randomMove, j] = false;
                                move = (randomMove, j);
                                break;
                            }
                        }
                        if (CheckFor4(move.I, move.J))
                        {
                            RenderBoard();
                            Console.WriteLine();
                            Console.WriteLine($"   You Lose!");
                            goto PlayAgainCheck;
                        }
                    }
                    if (CheckForDraw())
                    {
                        RenderBoard();
                        Console.WriteLine();
                        Console.WriteLine($"   Draw!");
                        goto PlayAgainCheck;
                    }
                    else
                    {
                        player1Turn = !player1Turn;
                    }
                }
            PlayAgainCheck:
                Console.WriteLine("   Play Again [enter], or quit [escape]?");
            GetInput:
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter: goto PlayAgain;
                    case ConsoleKey.Escape: Console.Clear(); return;
                    default: goto GetInput;
                }
            }
            catch (Exception e)
            {
                exception = e;
                throw;
            }
            finally
            {
                Console.CursorVisible = true;
                Console.Clear();
                Console.WriteLine(exception?.ToString() ?? "Connect 4 was closed.");
            }

            void ResetBoard()
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        board[i, j] = null;
                    }
                }
            }

            void RenderBoard()
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("   ╔" + new string('-', board.GetLength(0) * 2 + 1) + "╗");
                Console.Write("   ║ ");
                int iOffset = Console.CursorLeft;
                int jOffset = Console.CursorTop;
                Console.WriteLine(new string(' ', board.GetLength(0) * 2) + "║");
                for (int j = 1; j < board.GetLength(1) * 2; j++)
                {
                    Console.WriteLine("   ║" + new string(' ', board.GetLength(0) * 2 + 1) + "║");
                }
                Console.WriteLine("   ╚" + new string('═', board.GetLength(0) * 2 + 1) + "╝");
                int iFinal = Console.CursorLeft;
                int jFinal = Console.CursorTop;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        Console.SetCursorPosition(i * 2 + iOffset, (board.GetLength(1) - j) * 2 + jOffset - 1);
                        if (board[i, j] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write('█');
                            Console.ResetColor();
                        }
                        else if (board[i, j] == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write('█');
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ResetColor();
                            Console.Write(' ');
                        }
                    }
                }
                Console.SetCursorPosition(iFinal, jFinal);
            }

            bool CheckFor4(int i, int j)
            {
                bool player = board[i, j]!.Value;
                { // horizontal
                    int inARow = 0;
                    for (int _i = 0; _i < board.GetLength(0); _i++)
                    {
                        inARow = board[_i, j] == player ? inARow + 1 : 0;
                        if (inARow >= 4) return true;
                    }
                }
                { // vertical
                    int inARow = 0;
                    for (int _j = 0; _j < board.GetLength(1); _j++)
                    {
                        inARow = board[i, _j] == player ? inARow + 1 : 0;
                        if (inARow >= 4) return true;
                    }
                }
                { // postive slope diagonal
                    int inARow = 0;
                    int min = Math.Min(i, j);
                    for (int _i = i - min, _j = j - min; _i < board.GetLength(0) && _j < board.GetLength(1); _i++, _j++)
                    {
                        inARow = board[_i, _j] == player ? inARow + 1 : 0;
                        if (inARow >= 4) return true;
                    }
                }
                { // negative slope diagonal
                    int inARow = 0;
                    int offset = Math.Min(i, board.GetLength(1) - (j + 1));
                    for (int _i = i - offset, _j = j + offset; _i < board.GetLength(0) && _j >= 0; _i++, _j--)
                    {
                        inARow = board[_i, _j] == player ? inARow + 1 : 0;
                        if (inARow >= 4) return true;
                    }
                }
                return false;
            }

            bool CheckForDraw()
            {
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    if (!board[i, board.GetLength(1) - 1].HasValue)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void Pong()
        {
            
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            float multiplier = 1.1f;
            TimeSpan delay = TimeSpan.FromMilliseconds(10);
            TimeSpan enemyInputDelay = TimeSpan.FromMilliseconds(100);
            int paddleSize = height / 4;
            Stopwatch stopwatch = new();
            Stopwatch enemyStopwatch = new();
            int scoreA = 0;
            int scoreB = 0;
            Ball ball;
            int paddleA = height / 3;
            int paddleB = height / 3;

            Console.Clear();
            stopwatch.Restart();
            enemyStopwatch.Restart();
            Console.CursorVisible = false;
            while (scoreA < 3 && scoreB < 3)
            {
                ball = CreateNewBall();
                while (true)
                {
                    #region Update Ball

                    // Compute Time And New Ball Position
                    float time = (float)stopwatch.Elapsed.TotalSeconds * 15;
                    var (X2, Y2) = (ball.X + (time * ball.dX), ball.Y + (time * ball.dY));

                    // Collisions With Up/Down Walls
                    if (Y2 < 0 || Y2 > height)
                    {
                        ball.dY = -ball.dY;
                        Y2 = ball.Y + ball.dY;
                    }

                    // Collision With Paddle A
                    if (Math.Min(ball.X, X2) <= 2 && 2 <= Math.Max(ball.X, X2))
                    {
                        int ballPathAtPaddleA = height - (int)GetLineValue(((ball.X, height - ball.Y), (X2, height - Y2)), 2);
                        ballPathAtPaddleA = Math.Max(0, ballPathAtPaddleA);
                        ballPathAtPaddleA = Math.Min(height - 1, ballPathAtPaddleA);
                        if (paddleA <= ballPathAtPaddleA && ballPathAtPaddleA <= paddleA + paddleSize)
                        {
                            ball.dX = -ball.dX;
                            ball.dX *= multiplier;
                            ball.dY *= multiplier;
                            X2 = ball.X + (time * ball.dX);
                        }
                    }

                    // Collision With Paddle B
                    if (Math.Min(ball.X, X2) <= width - 2 && width - 2 <= Math.Max(ball.X, X2))
                    {
                        int ballPathAtPaddleB = height - (int)GetLineValue(((ball.X, height - ball.Y), (X2, height - Y2)), width - 2);
                        ballPathAtPaddleB = Math.Max(0, ballPathAtPaddleB);
                        ballPathAtPaddleB = Math.Min(height - 1, ballPathAtPaddleB);
                        if (paddleB <= ballPathAtPaddleB && ballPathAtPaddleB <= paddleB + paddleSize)
                        {
                            ball.dX = -ball.dX;
                            ball.dX *= multiplier;
                            ball.dY *= multiplier;
                            X2 = ball.X + (time * ball.dX);
                        }
                    }

                    // Collisions With Left/Right Walls
                    if (X2 < 0)
                    {
                        scoreB++;
                        break;
                    }
                    if (X2 > width)
                    {
                        scoreA++;
                        break;
                    }

                    // Updating Ball Position
                    Console.SetCursorPosition((int)ball.X, (int)ball.Y);
                    Console.Write(' ');
                    ball.X += time * ball.dX;
                    ball.Y += time * ball.dY;
                    Console.SetCursorPosition((int)ball.X, (int)ball.Y);
                    Console.Write('O');

                    #endregion

                    #region Update Player Paddle

                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.UpArrow: paddleA = Math.Max(paddleA - 1, 0); break;
                            case ConsoleKey.DownArrow: paddleA = Math.Min(paddleA + 1, height - paddleSize - 1); break;
                            case ConsoleKey.Escape:
                                Console.Clear();
                                Console.Write("Pong was closed.");
                                return;
                        }
                    }
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    #endregion

                    #region Update Computer Paddle

                    if (enemyStopwatch.Elapsed > enemyInputDelay)
                    {
                        if (ball.Y < paddleB + (paddleSize / 2) && ball.dY < 0)
                        {
                            paddleB = Math.Max(paddleB - 1, 0);
                        }
                        else if (ball.Y > paddleB + (paddleSize / 2) && ball.dY > 0)
                        {
                            paddleB = Math.Min(paddleB + 1, height - paddleSize - 1);
                        }
                        enemyStopwatch.Restart();
                    }

                    #endregion

                    #region Render Paddles

                    for (int i = 0; i < height; i++)
                    {
                        Console.SetCursorPosition(2, i);
                        Console.Write(paddleA <= i && i <= paddleA + paddleSize ? '█' : ' ');
                        Console.SetCursorPosition(width - 2, i);
                        Console.Write(paddleB <= i && i <= paddleB + paddleSize ? '█' : ' ');
                    }

                    #endregion

                    stopwatch.Restart();
                    Thread.Sleep(delay);
                }
                Console.SetCursorPosition((int)ball.X, (int)ball.Y);
                Console.Write(' ');
            }
            Console.Clear();
            if (scoreA > scoreB)
            {
                Console.Write("You win.");
            }
            if (scoreA < scoreB)
            {
                Console.Write("You lose.");
            }

            Ball CreateNewBall()
            {
                float randomFloat = (float)Random.Shared.NextDouble() * 2f;
                float dx = Math.Max(randomFloat, 1f - randomFloat);
                float dy = 1f - dx;
                float x = width / 2;
                float y = height / 2;
                if (Random.Shared.Next(2) is 0)
                {
                    dx = -dx;
                }
                if (Random.Shared.Next(2) is 0)
                {
                    dy = -dy;
                }
                return new Ball
                {
                    X = x,
                    Y = y,
                    dX = dx,
                    dY = dy,
                };
            }

            float GetLineValue(((float X, float Y) A, (float X, float Y) B) line, float x)
            {
                // order points from least to greatest X
                if (line.B.X < line.A.X)
                {
                    (line.A, line.B) = (line.B, line.A);
                }
                // find the slope
                float slope = (line.B.Y - line.A.Y) / (line.B.X - line.A.X);
                // find the y-intercept
                float yIntercept = line.A.Y - line.A.X * slope;
                // find the function's value at parameter "x"
                return x * slope + yIntercept;
            }
        }

        public class Ball
        {
            public float X;
            public float Y;
            public float dX;
            public float dY;
        }
    }
}



