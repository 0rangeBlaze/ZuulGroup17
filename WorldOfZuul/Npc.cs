using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;

namespace WorldOfZuul
{
    public class Npc
    {   //Initializing list variables
        private List<string> greeting;
        private List<string> dialogs;
        private List<string> badOutcome;
        private List<string> goodOutcome;
        //Creating variables that will store index of dialogs
        public int dialogIndex = 0;
        public int outcomeIndex = 0;

        //Creating npc constructor that will take 4 paths as variables
        public Npc(string filePath1, string filePath2, string filePath3, string filePath4) 
        {   
            //initializing lists
            greeting = new List<string>();
            dialogs = new List<string>();
            badOutcome = new List<string>();
            goodOutcome = new List<string>();
            //Using function that will input texts from files to lists
            LoadDialogsFromFile(filePath1,filePath2,filePath3,filePath4) ;

        } 
        //Fucntion that input texts line by line from txt file to respective list
        private void LoadDialogsFromFile(string filePath1, string filePath2, string filePath3, string filePath4)
        {
            try 
            {
                greeting.AddRange(File.ReadAllLines(filePath1));
                dialogs.AddRange(File.ReadAllLines(filePath2));
                badOutcome.AddRange(File.ReadAllLines(filePath3));
                goodOutcome.AddRange(File.ReadAllLines(filePath4));
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"Error occured while trying to load dialogs {ex.Message}");
            }

        }
        //Random greeting function.Can be used before dialouge as greeting. 
        //Creating immersive gampley by using ranomnes of greeting
        public void RandomGreeting()
        {
            Random greetings = new Random();
            int greetingIndex = greetings.Next(greeting.Count);
            PrintSlowly(greeting[greetingIndex]);

        }

        //Npc talking Function
        public void NpcTalk(ref int dialogIndex)
        {
            for (int i = dialogIndex; i < dialogs.Count; i++)
            {
                //Print dialog with index of i
                RandomGreeting();
                PrintSlowly(dialogs[i]);
                dialogIndex++;
                //Use funticion print choices to print choices good and bad one
                PrintChoices(ref outcomeIndex);
                PrintSlowly("Choose your option by clicking 1 or 2");
                //Use funtion ReadKey to check what number player clicked
                int key = ReadKey();
                if (key == 1 && dialogIndex < goodOutcome.Count)
                {
                    PrintSlowly("You choosed option 1");
                    //Printing description of outcome
                    PrintSlowly(goodOutcome[outcomeIndex]);
                    outcomeIndex++;
                    //Use function implemented somewere else that calculates numbers
                    goodOutcome();
                    //then stop the loop so it wonn't go to another dialog
                    break;
                }
                else if (key == 2 && dialogIndex < badOutcome.Count)
                {
                    PrintSlowly("You choosed option 2");
                    //Printing description of outcome 2
                    PrintSlowly(badOutcome[outcomeIndex]);
                    outcomeIndex++;
                    //Use function implemented somewere else that calculates numbers
                    badOutcome();
                    //then stop the loop so it wonn't go to another dialog
                    break;
                }
                //Resseting loop to print the same thing when player click wrong key
                else
                {
                    PrintSlowly("Wrong key try again");
                    dialogIndex--;
                    i--;
                    Console.Clear();
                }

            }
        }

        //Function for pringting choices player have
        public void PrintChoices(ref int outcomeIndex)
        {
            for (int i = outcomeIndex; i < badOutcome.Count; i++)
            {
                PrintSlowly("You have 2 choices");
                PrintSlowly("Choice number 1");
                PrintSlowly(goodOutcome[i]);
                PrintSlowly("Choice number 2");
                PrintSlowly(badOutcome[i]);
                outcomeIndex++;
            }
          
        }

        //Function for reading key.
        public int ReadKey()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            return (int)key.Key;
        }

        //Slow printing function
        public void PrintSlowly(string text)
        {
            int delay = 300;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();


        }



    }
}