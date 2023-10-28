using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace WorldOfZuul
{
    public class Npc
    {
        private List<string> greeting;
        private Dictionary<string, Dictionary<string, DialogData>> npcData;
        private Dictionary<string, int> npcIndexes;
        private bool talking = true;

        public Npc(string jsonFilePath)
        {
            greeting = new List<string>() { "Hi", "Hello", "How do you do" };
            LoadDialogsFromJson(jsonFilePath);
            npcIndexes = new Dictionary<string, int>();
        }

        private void LoadDialogsFromJson(string jsonFilePath)
        {
            try
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                npcData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, DialogData>>>(jsonData);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: JSON file not found.");
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: Invalid JSON format.");
            }
        }

        private void PrintChoices(DialogData dialogData)
        {
            if (dialogData.Choices != null)
            {
                int choiceNumber = 1;
                foreach (var choice in dialogData.Choices)
                {
                    PrintSlowly($"Choice number {choiceNumber}: {choice.Value.Description}");
                    choiceNumber++;
                }
            }
        }

        private int GetPlayerChoice(Dictionary<string, DialogChoice> choices)
        {
            string[] choiceIndices = choices.Keys.ToArray();
            int numberOfChoices = choiceIndices.Length;

            int choice;
            bool validChoice = false;
            do
            {
                Console.Write($"Enter your choice (1-{numberOfChoices}): ");
                string input = Console.ReadLine();
                validChoice = Int32.TryParse(input, out choice);
                if (!validChoice || choice < 1 || choice > numberOfChoices)
                {
                    PrintSlowly($"Invalid input. Please enter a number between 1 and {numberOfChoices}.");
                }
            } while (!validChoice || choice < 1 || choice > numberOfChoices);

            
            return Int32.Parse(choiceIndices[choice - 1]);
        }

        public void NpcTalk(string npcName)
        {
            if (npcData != null && npcData.ContainsKey(npcName))
            {
                if (!npcIndexes.ContainsKey(npcName))
                {
                    npcIndexes[npcName] = 1;
                }

                var currentNpcTalking = npcData[npcName];
                var index = npcIndexes[npcName].ToString(); 

                while (talking && currentNpcTalking.ContainsKey(index))
                {
                    RandomGreeting();
                    var dialogData = currentNpcTalking[index];
                    PrintSlowly(dialogData.Dialog);
                    PrintChoices(dialogData);
                    index = GetPlayerChoice(dialogData.Choices).ToString();
                    npcIndexes[npcName] = Int32.Parse(index); 
                }
            }
            else
            {
                Console.WriteLine($"Error: NPC '{npcName}' not found in the loaded data.");
            }
        }

        private void RandomGreeting()
        {
            Random greetings = new Random();
            int greetingIndex = greetings.Next(greeting.Count);
            PrintSlowly(greeting[greetingIndex]);
        }

        private void PrintSlowly(string text)
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

    public class DialogData
    {
        public string Dialog { get; set; }
        public Dictionary<string, DialogChoice> Choices { get; set; }
    }

    public class DialogChoice
    {
        public string Description { get; set; }
        public string JumpDialogIndex { get; set; }
    }
}
