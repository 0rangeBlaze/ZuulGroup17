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
        private string name;
        private string currentDialog;
        private List<string> greeting;
        private Dictionary<string, DialogData> npcData;
        private bool talking = true;

        public Npc(string jsonFilePath)
        {
            greeting = new List<string>() { "Hi", "Hello", "How do you do" };
            currentDialog = "index1";
            LoadDialogsFromJson(jsonFilePath);
            name = name ?? "";
            npcData = npcData ?? new();
        }

        private void LoadDialogsFromJson(string jsonFilePath)
        {
            try
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                JsonDocument npcDoc = JsonDocument.Parse(jsonData);
                JsonElement nameElement;
                npcDoc.RootElement.TryGetProperty("name", out nameElement);
                name = nameElement.GetString() ?? "";
                JsonElement dialogsElement;
                npcDoc.RootElement.TryGetProperty("dialogs", out dialogsElement);
                npcData = JsonSerializer.Deserialize<Dictionary<string, DialogData>>(dialogsElement.ToString()) ?? new();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: JSON file not found.");
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: Invalid JSON format.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading npc: {e.Message}");
            }
        }

        private void PrintChoices(DialogData dialogData)
        {
            int choiceNumber = 1;
            foreach (var choice in dialogData.Choices)
            {
                PrintSlowly($"Choice number {choiceNumber}: {choice.Value.Description}");
                choiceNumber++;
            }
            PrintSlowly($"Choice number {choiceNumber}: Stop talking");
        }

        private string GetPlayerChoice(Dictionary<string, DialogChoice> choices)
        {
            DialogChoice[] choiceIndices = choices.Values.ToArray();
            int numberOfChoices = choiceIndices.Length + 1;

            int choice;
            bool validChoice = false;
            do
            {
                Console.Write($"Enter your choice (1-{numberOfChoices}): ");
                string input = Console.ReadLine() ?? "";
                validChoice = Int32.TryParse(input, out choice);
                if (!validChoice || choice < 1 || choice > numberOfChoices)
                {
                    PrintSlowly($"Invalid input. Please enter a number between 1 and {numberOfChoices}.");
                }
            } while (!validChoice || choice < 1 || choice > numberOfChoices);

            if (choice == numberOfChoices)
            {
                talking = false;
                return currentDialog;
            }
            return choiceIndices[choice - 1].JumpDialogIndex;
        }

        public void NpcTalk()
        {
            if (npcData != null)
            {
                talking = true;
                RandomGreeting();
                while (talking)
                {
                    if (npcData.ContainsKey(currentDialog))
                    {
                        DialogData currentDialogData = npcData[currentDialog];
                        PrintSlowly(currentDialogData.Dialog);
                        PrintChoices(currentDialogData);
                        currentDialog = GetPlayerChoice(currentDialogData.Choices);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {name} doesn't have dialog with index '{currentDialog}'");
                    }
                }

            }
            else
            {
                Console.WriteLine($"Error: NPC '{name}' not found in the loaded data.");
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
            int delay = 25;
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
        public DialogData(string? dialog, Dictionary<string, DialogChoice>? choices) {
            Dialog = dialog ?? "";
            Choices = choices ?? new();
        }
    }

    public class DialogChoice
    {
        public string Description { get; set; }
        public string JumpDialogIndex { get; set; }
        public DialogChoice(string? description, string? jumpDialogIndex) {
            Description = description ?? "";
            JumpDialogIndex = jumpDialogIndex ?? "";
        }
    }
}
