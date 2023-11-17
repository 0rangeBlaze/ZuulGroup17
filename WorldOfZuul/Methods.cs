using System;

public class Methods
{   
    // You input the question and list.
    // Then the method show you question and menu from which you can select with arrows
    // It returns index of list
    // You need to proces the returned value yourself
    public string SelectOption(string question, List<string> temp)
    {
        int option = 1;
        int startingPosition = temp.Count;
        int endingPosition = (temp.Count + 1) - startingPosition;

        bool selected = false;
        Console.CursorVisible = false;
        if (question != null && temp.Count > 0)
        {
            while (!selected)
            {

                CenterText(question + "\n");

                for (int i = 0; i < temp.Count; i++)
                {
                    if (option == i + 1)
                    {
                        CenterColor(temp[i], "green");
                    }
                    else
                    {
                        CenterText(temp[i]);
                    }

                }

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    default:
                        Console.Clear();
                        break;
                    case ConsoleKey.DownArrow:
                        if (option < startingPosition)
                        {
                            option++;
                        }
                        Console.Clear();
                        break;
                    case ConsoleKey.UpArrow:
                        if (option > endingPosition)
                        {
                            option--;
                        }
                        Console.Clear();
                        break;
                    case ConsoleKey.Enter:
                        selected = true;
                        Console.Clear();
                        break;
                }
            }
            Console.CursorVisible = true;
            return temp[option - 1];
        }
        else
        {
            CenterColor("Wrong selection menu inputs", "red");
            return "";
        }
    }

    //It show text that is centered
    public void CenterText(string text)
    {
        Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        Console.WriteLine(text);
    }

    // Method for coloring the text
    // You in variables you write text and color you want your text to have
    // For now only green, blue , yellow, red
    // Can easly add more colors
    // It shows centered text
    public void CenterColor(string text, string color)
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
    public void SlowColor(string text, string color)
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
    public void PrintSlowly(string text)
    {
        int delay = 25;
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }

    //Print centered text slowly to add immersion
    public void PrintSlowlyCenter(string text)
    {
        Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
        int delay = 25;
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(delay);
        }
        Console.WriteLine();
    }
}
