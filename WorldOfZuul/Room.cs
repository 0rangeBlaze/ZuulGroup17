namespace WorldOfZuul
{
    public class Room
    {
        public string Name {get; private set;}
        public string ShortDescription { get; private set; }
        public Dictionary<int, string> LongDescriptions { get; private set; }
        public Dictionary<string, string> Exits { get; private set; }
        public string[] Actions {get; private set;}
        public Dictionary<string, Npc> Npcs {get; private set;}

        public Room(string name, string shortDescription, Dictionary<int, string>? longDescriptions, string[]? actions = null, Dictionary<string, string>? exits = null, Dictionary<string, Npc>? npcs = null)
        {
            Name = name;
            ShortDescription = shortDescription;
            LongDescriptions = longDescriptions ?? new();
            Exits = exits == null ? new(StringComparer.OrdinalIgnoreCase){} : new(exits, StringComparer.OrdinalIgnoreCase);
            Actions = actions ?? new string[0];
            Npcs = npcs == null ? new(StringComparer.OrdinalIgnoreCase){} : new(npcs, StringComparer.OrdinalIgnoreCase);
        }

        private string GetLongDescription(int environment) {
            int[] keys = LongDescriptions.Keys.ToArray();
            int maxValue = int.MinValue;
            //O(n) not the best but array won't be longer than 100 elements for sure, so doesn't matter
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] <= environment && keys[i] > maxValue)
                {
                    maxValue = keys[i];
                }
            }

            //a bit ugly, but it gets the job done if we don't find a value
            return LongDescriptions.ContainsKey(maxValue) ? LongDescriptions[maxValue] : LongDescriptions[keys[0]];
        }

        public void Describe(Game game)
        {
            Utilities.GamePrint(this.GetLongDescription(game.World.Environment));

            if (Npcs.Count > 0)
            {
                Utilities.GamePrint("You see the following people in the room:");
                foreach (var npc in Npcs)
                {
                    Utilities.GamePrint(" " + npc.Value.Name);
                }
            }
            if (Exits.Count > 0)
            {
                Utilities.GamePrint("Exits:");
                foreach (var exit in Exits)
                {
                    Utilities.GamePrint("- " + exit.Key);
                }
            }
            else 
            {
                Utilities.GamePrint("You cannot exit");
            }
        }
    }
}
