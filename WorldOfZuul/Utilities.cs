namespace WorldOfZuul {
    public static class Utilities {
        public static void GamePrint(string paragraph="") {
           PrintSlowly(WrapLine(paragraph)); 
        }

        public static string WrapLine(string paragraph = "", int tabSize = 8) {
            string[] lines = paragraph
                .Replace("\t", new String(' ', tabSize))
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            string result = "";
            for (int i = 0; i < lines.Length; i++) {
                string process = lines[i];
                List<String> wrapped = new List<string>();

                while (process.Length > Console.WindowWidth) {
                    int wrapAt = process.LastIndexOf(' ', Math.Min(Console.WindowWidth - 1, process.Length));
                    if (wrapAt <= 0) break;

                    wrapped.Add(process.Substring(0, wrapAt));
                    process = process.Remove(0, wrapAt + 1);
                }

                foreach (string wrap in wrapped) {
                    result += wrap + "\n";
                }
                result += process + (i < lines.Length-1 ? "\n" : "");
            }
            return result;
        }
        // You input the question and list.
        // Then the method show you question and menu from which you can select with arrows
        // It returns index of list
        // You need to proces the returned value yourself
        public static int SelectOption(string question, List<string> temp)
        {
            int option = 0;
            int startingPosition = 0;
            int endingPosition = temp.Count;

            bool selected = false;
            Console.CursorVisible = false;
            if (question != null && temp.Count > 0)
            {
                Console.Clear();
                PrintSlowlyCenter(WrapLine(question + "\n"));

                int top = Console.CursorTop;
                //Console.WriteLine(top);
                for (int i = 0; i < temp.Count; i++)
                {
                    if (option == i)
                    {
                        CenterColor(WrapLine(temp[i]), "green");
                    }
                    else
                    {
                        CenterText(WrapLine(temp[i]));
                    }
                }
                Console.CursorTop = top;

                while (!selected)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        default:
                            break;
                        case ConsoleKey.DownArrow:
                            if (option + 1 < endingPosition)
                            {
                                CenterText(WrapLine(temp[option]));
                                option++;
                                string selectedText = WrapLine(temp[option]);
                                CenterColor(selectedText, "green");
                                Console.CursorTop -= selectedText.Count(x => x == '\n')+1;
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (option - 1 >= startingPosition)
                            {
                                string deselected = WrapLine(temp[option]);
                                CenterText(deselected);
                                option--;
                                string selectedText = WrapLine(temp[option]);
                                Console.CursorTop -= selectedText.Count(x => x == '\n') + deselected.Count(x => x == '\n') + 2;
                                CenterColor(selectedText, "green");
                                Console.CursorTop -= selectedText.Count(x => x == '\n') + 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                    }
                }
                Console.CursorVisible = true;
                Console.Clear();
                return option;
            }
            else
            {
                CenterColor("Wrong selection menu inputs", "red");
                return -1;
            }
        }

        //It show text that is centered
        public static void CenterText(string paragraph)
        {
            string[] lines = paragraph.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach(string text in lines) {
                if((Console.WindowWidth-text.Length)/2 >= 0) {
                    Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
                }
                Console.WriteLine(text);
            }
        }

        // Method for coloring the text
        // You in variables you write text and color you want your text to have
        // For now only green, blue , yellow, red
        // Can easly add more colors
        // It shows centered text
        public static void CenterColor(string text, string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            CenterText(text);
            Console.ResetColor();
        }

        //The same as method that centers the colored text
        //Print text slowly for dialogs
        public static void SlowColor(string text, string color)
        {
            switch (color.ToLower())
            {
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }
            PrintSlowly(text);
            Console.ResetColor();
        }

        //Print text slowly to add immersion
        public static void PrintSlowly(string text, int delay = 35)
        {
            
            foreach (char c in text)
            {
                while(Console.KeyAvailable) {
                    Console.ReadKey(true);
                    delay = 0;
                }
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
            while(Console.KeyAvailable) {
                Console.ReadKey(true);
            }
        }

        //Print centered text slowly to add immersion
        public static void PrintSlowlyCenter(string paragraph, ConsoleColor color = ConsoleColor.White, int delay = 35)
        {
            string[] lines = paragraph.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach(string text in lines) {
                if((Console.WindowWidth-text.Length)/2 >= 0) {
                    Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
                }
                Console.ForegroundColor = color;
                foreach (char c in text)
                {
                    while(Console.KeyAvailable) {
                        Console.ReadKey(true);
                        delay = 0;
                    }
                    Console.Write(c);
                    Thread.Sleep(delay);
                }
                Console.ResetColor();
                Console.WriteLine();
                while(Console.KeyAvailable) {
                    Console.ReadKey(true);
                }
            }
        }
    }
}