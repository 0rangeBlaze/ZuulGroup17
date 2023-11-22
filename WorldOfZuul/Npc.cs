using System;
using System.Text.Json;


namespace WorldOfZuul
{
    public class Npc
    {
        public string Name {get; private set;}
        private string currentDialog;
        private List<string> greeting;
        private Dictionary<string, DialogData> npcData;
        private bool talking = true;

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

        private string SelectChoice(DialogData dialogData, Game game)
        {
            string jumpDialog = "";
            string actionKey = "";
           
            string question = dialogData.Dialog.ToString();

            List<string> choices = new List<string>();

            foreach (var choice in dialogData.Choices)
            {
                choices.Add(choice.Value.Description.ToString());
            }

            string chosed = Utilities.SelectOption(question, choices);


            foreach (var choice in dialogData.Choices)
            {
                if (choice.Value.Description.ToString() == chosed)
                {
                    jumpDialog = choice.Value.JumpDialogIndex.ToString();
                    actionKey = choice.Key.ToString();
                    if (choice.Key[0] == '#')
                    {
                        talking = false;
                    }
                    break;
                }
            }

            dialogData.Choices[actionKey].HandleActions(game);

            return jumpDialog;
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
                        currentDialog = SelectChoice(currentDialogData, game);
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
            Utilities.SlowColor(greeting[greetingIndex],"green");
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
