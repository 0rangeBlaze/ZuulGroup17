using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace WorldOfZuul
{
    public class Npc
    {
        public string Name {get; private set;}
        public string CurrentDialog {get; set;}
        public List<string> Greeting {get; private set;}
        public Dictionary<string, DialogData> NpcData {get; private set;}
        public bool Talking {get; private set;}
        public bool Loaded {get; private set;}

        public Npc(string jsonFilePath)
        {
            Loaded = false;
            Greeting = new List<string>() { "Hi", "Hello", "How do you do" };
            CurrentDialog = "index1";
            LoadDialogsFromJson(jsonFilePath);
            Name = Name ?? "";
            NpcData = NpcData ?? new();
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
                NpcData = JsonSerializer.Deserialize<Dictionary<string, DialogData>>(dialogsElement.ToString()) ?? new();
                Loaded=true;
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
            string question = dialogData.Dialog.ToString();

            List<KeyValuePair<string, DialogChoice>> choices = new(dialogData.Choices);
            List<string> descriptions = new List<string>();

            foreach (var choice in choices)
            {
                descriptions.Add(choice.Value.Description.ToString());
            }
            descriptions.Add("Quit conversation.");

            int chosen = Utilities.SelectOption(question, descriptions);

            if(chosen == descriptions.Count-1)
            {
                Talking = false;
                return CurrentDialog;
            }

            if(choices[chosen].Key[0] == '#') {
                Talking = false;
            }
            choices[chosen].Value.HandleActions(game);

            return choices[chosen].Value.JumpDialogIndex;
        }

        public void NpcTalk(Game game)
        {
            if (NpcData != null)
            {
                Talking = true;
                Console.Clear();
                RandomGreeting();
                Console.ReadKey(true);
                while (Talking)
                {
                    if (NpcData.ContainsKey(CurrentDialog))
                    {
                        DialogData currentDialogData = NpcData[CurrentDialog];
                        CurrentDialog = SelectChoice(currentDialogData, game);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {Name} doesn't have dialog with index '{CurrentDialog}'");
                        Talking = false;
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
            int greetingIndex = greetings.Next(Greeting.Count);
            Utilities.PrintSlowlyCenter(Greeting[greetingIndex], ConsoleColor.Green);
        }

        [JsonConstructorAttribute]
        public Npc(string name,
        string currentDialog,
        List<string> greeting,
        Dictionary<string, DialogData> npcData,
        bool talking
        ) {
           Name = name;
           CurrentDialog = currentDialog;
           Greeting = greeting;
           NpcData = npcData; 
           Talking = talking;
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
