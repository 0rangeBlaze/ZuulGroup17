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

        /*public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("north", north);
            SetExit("east", east);
            SetExit("south", south);
            SetExit("west", west);
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }*/
    }
}
