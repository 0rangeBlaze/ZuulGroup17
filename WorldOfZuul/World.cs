using System.Text.Json;

namespace WorldOfZuul
{
    public class World
    {
        Dictionary<string, Area> areas;

        public World(string path="assets/world.json") {
            areas = new Dictionary<string, Area> {};

            JsonDocument doc;
            string jsonString;
            jsonString = File.ReadAllText(path);
            doc = JsonDocument.Parse(jsonString);
            

            //enumerating areas
            foreach (var area in doc.RootElement.GetProperty("areas").EnumerateObject())
            {
                Dictionary<string, Room> rooms = new Dictionary<string, Room> {};
                string defaultRoom = area.Value.GetProperty("default").GetString();

                //enumerating rooms
                foreach (var room in area.Value.GetProperty("rooms").EnumerateObject()){
                    Dictionary<string, string> exits = new Dictionary<string, string> {};
                    string longDescription = room.Value.GetProperty("longDescription").GetString();

                    //enumerating exits
                    foreach(JsonProperty property in room.Value.GetProperty("exits").EnumerateObject()){
                        exits[property.Name] = property.Value.GetString();
                    }

                    rooms[room.Name] = new Room(room.Name, longDescription, exits);
                }
                areas[area.Name] = new Area(area.Name, rooms, defaultRoom);
            }                
            doc.Dispose();
        }

        public Room GetRoom(string area, string room = "") {
            return (room == "") ? areas[area].Rooms[areas[area].DefaultRoom] : areas[area].Rooms[room];
        }
    }
}