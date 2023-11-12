namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set;}
        public Dictionary<string, string> Exits { get; private set; }
        public string[] Actions {get; private set;}
        public Dictionary<string, Npc> Npcs {get; private set;}

        public Room(string shortDesc, string longDesc, string[]? actions = null, Dictionary<string, string>? exits = null, Dictionary<string, Npc>? npcs = null)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            Exits = exits ?? new(StringComparer.OrdinalIgnoreCase) {};
            Actions = actions ?? new string[0];
            Npcs = npcs ?? new(StringComparer.OrdinalIgnoreCase);
        }

        public void Describe()
        {
            Console.WriteLine(this.LongDescription);

            if (Npcs.Count > 0)
            {
                Console.WriteLine("You see the following people in the room:");
                foreach (var npc in Npcs)
                {
                    Console.WriteLine(" " + npc.Value.Name)
                }
            }
                if (Exits.Count > 0)
            {
                Console.WriteLine("Exits:");
                foreach (var exit in Exits)
                {
                    Console.WriteLine("- " + exit.Key);
                }
            }
            else 
            {
                Console.WriteLine("You cannot exit");
            }
        }
    }
}
