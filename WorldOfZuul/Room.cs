namespace WorldOfZuul
{
    public class Room
    {
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set;}
        public Dictionary<string, string> Exits { get; private set; }

        public Room(string shortDesc, string longDesc, Dictionary<string, string> exits = null)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            Exits = (exits == null) ? new() : exits;
        }
    }
}
