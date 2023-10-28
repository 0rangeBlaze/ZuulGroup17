using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;

namespace WorldOfZuul
{
    public class Npc
    { 
        private List<string> greeting;
        private Dictionary<string, Dialogs> npc;
        private bool talking = true;
        private string index = "index1";

        public Npc(Dictionary<string,Dialogs> npc) 
        {   
            //initializing lists
            greeting = new List<string>() {"Hi","Helllo","How do you do" };
            this.npc = npc;
            
            

        }
        
        public void RandomGreeting()
        {
            Random greetings = new Random();
            int greetingIndex = greetings.Next(greeting.Count);
            PrintSlowly(greeting[greetingIndex]);

        }

        //Npc talking Function
        public void NpcTalk(string npcName)
        {
            while (talking)
            {
                RandomGreeting();


                
                

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