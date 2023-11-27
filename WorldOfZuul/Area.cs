using System;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace WorldOfZuul
{
    public class Area
    {
        public Dictionary<string, Room> Rooms {get; private set;}
        public string? Name {get; private set;}
        public string DefaultRoom {get; private set;}

        public Area(string? name, Dictionary<string, Room>? rooms, string defaultRoom)
        {
            Name = name;
            DefaultRoom = defaultRoom;
            Rooms = rooms ?? new Dictionary<string,Room>();
        }
       
    }
}