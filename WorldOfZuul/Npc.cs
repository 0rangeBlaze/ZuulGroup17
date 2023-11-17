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
        public string Name {get; private set;}
        private string currentDialog;
        private List<string> greeting;
        private Dictionary<string, DialogData> npcData;
        private bool talking = true;
        Methods methods = new Methods();

        public Npc(string jsonFilePath)
        {
            greeting = new List<string>() { "Hi", "Hello", "How do you do" };
            currentDialog = "index1";
            LoadDialogsFromJson(jsonFilePath);
            Name = Name ?? "";
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
                Name = nameElement.GetString() ?? "";
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
                Console.WriteLine($"Error loading npc: {e.Message} in {jsonFilePath}");
            }
        }

        private void PrintChoices(DialogData dialogData)
        {
            int choiceNumber = 1;
            foreach (var choice in dialogData.Choices)
            {
                methods.PrintSlowly($"Choice number {choiceNumber}: {choice.Value.Description}");
                choiceNumber++;
            }
            methods.PrintSlowly($"Choice number {choiceNumber}: Stop talking");
        }

        //to do refactor:
        private string GetPlayerChoice(Dictionary<string, DialogChoice> choices, Game game)
        {
            string[] choiceIndices = choices.Keys.ToArray();
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
                    methods.PrintSlowly($"Invalid input. Please enter a number between 1 and {numberOfChoices}.");
                }
            } while (!validChoice || choice < 1 || choice > numberOfChoices);

            if (choice == numberOfChoices)
            {
                talking = false;
                return currentDialog;
            }

            choices[choiceIndices[choice - 1]].HandleActions(game);

            if (choiceIndices[choice-1][0] == '#') {
                talking = false;
            }

            return choices[choiceIndices[choice - 1]].JumpDialogIndex;
        }

        public void NpcTalk(Game game)
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
                        methods.PrintSlowly(currentDialogData.Dialog);
                        PrintChoices(currentDialogData);
                        currentDialog = GetPlayerChoice(currentDialogData.Choices, game);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {Name} doesn't have dialog with index '{currentDialog}'");
                        talking = false;
                    }
                }

            }
            else
            {
                Console.WriteLine($"Error: NPC '{Name}' not found in the loaded data.");
            }
        }

        private void RandomGreeting()
        {
            Random greetings = new Random();
            int greetingIndex = greetings.Next(greeting.Count);
            methods.PrintSlowly(greeting[greetingIndex]);
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
        //the deserialization of the json file only works if Actions is public for some reason, however, either set or get can be set to private (they can't be both set to private) the code still works.
        public List<string> Actions {get; private set; } 
        
        public DialogChoice(string? description, string? jumpDialogIndex, List<string> actions) {
            Description = description ?? "";
            JumpDialogIndex = jumpDialogIndex ?? "";
            Actions = actions ?? new();
        }

        public void HandleActions(Game game) {
            foreach(var action in Actions) {
                CommandProcessor.Process(action, game);
            }
        }
    }
}
