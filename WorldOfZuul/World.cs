using System.Text.Json;

namespace WorldOfZuul
{
    public class World
    {
        Dictionary<string, Area> areas;

        public World(string path="assets/world.json") {
            areas = new Dictionary<string, Area> {};

            JsonDocument doc;
            JsonElement areasElement;
            string jsonString;
            jsonString = File.ReadAllText(path);
            doc = JsonDocument.Parse(jsonString);
            

            if(doc.RootElement.TryGetProperty("areas", out areasElement)) {
                foreach (var area in areasElement.EnumerateObject())
                {
                    JsonElement roomsElement;
                    Dictionary<string, Room> rooms = new Dictionary<string, Room> {};
                    if(area.Value.TryGetProperty("rooms", out roomsElement)) {
                        foreach (var room in roomsElement.EnumerateObject()){
                            Dictionary<string, string> exits = new Dictionary<string, string> {};

                            JsonElement LongDescriptionElement, exitsElement;
                            room.Value.TryGetProperty("longDescription", out LongDescriptionElement);

                            if(room.Value.TryGetProperty("exits", out exitsElement)) {
                                foreach(JsonProperty property in exitsElement.EnumerateObject()){
                                    exits[property.Name] = property.Value.GetString();
                                }
                            }

                            rooms[room.Name] = new Room(room.Name, LongDescriptionElement.GetString(), exits);
                        }
                    }
                    areas[area.Name] = new Area(area.Name, rooms);
                }                
            }
            doc.Dispose();
        }

        public Room GetRoom(string area, string room) {
            return areas[area].Rooms[room];
        }
    }
}