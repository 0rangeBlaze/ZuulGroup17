using System.Text.Json;

namespace WorldOfZuul
{
    public class World
    {
        public List<Area> areas;

        public World(string path="assets/world.json") {
            areas = new List<Area> {};

            JsonDocument doc;
            JsonElement areasElement;
            string jsonString;
            jsonString = File.ReadAllText(path);
            doc = JsonDocument.Parse(jsonString);
            

            if(doc.RootElement.TryGetProperty("areas", out areasElement) && areasElement.ValueKind == JsonValueKind.Array) {
                foreach (var element in areasElement.EnumerateArray())
                {
                    // Access and work with individual elements
                    JsonElement roomsElement;
                    JsonElement nameElement;
                    Dictionary<string, Room> rooms = new Dictionary<string, Room> {};
                    element.TryGetProperty("name", out nameElement);
                    if(element.TryGetProperty("rooms", out roomsElement)) {
                        Dictionary<string, string> exits = new Dictionary<string, string> {};
                        foreach (var room in roomsElement.EnumerateArray()){
                          JsonElement idElement;
                          JsonElement LongDescriptionElement;
                          JsonElement exitsElement;
                          room.TryGetProperty("shortDescription", out idElement);
                          room.TryGetProperty("longDescription", out LongDescriptionElement);
                          if(room.TryGetProperty("exits", out exitsElement)) {
                            foreach(JsonProperty property in exitsElement.EnumerateObject()){
                                exits[property.Name] = property.Value.GetString();
                            }
                          }
                          rooms[idElement.GetString()] = new Room(idElement.GetString(), LongDescriptionElement.GetString(), exits);
                        }
                    }
                    areas.Add(new Area(nameElement.GetString(), rooms));
                }                
            }
            doc.Dispose();
        }

        public Room GetRoom(int area, string room) {
            return areas[area].Rooms[room];
        }
    }
}