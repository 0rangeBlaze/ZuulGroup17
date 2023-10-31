using System.Text.Json;

namespace WorldOfZuul
{
    public class World
    {
        public Dictionary<string, Area> Areas {get; private set;}
        public bool loaded = false;
        public int PopulationWelfare{get; set;}
        public int Environment{get; set;}

        public World(string path="assets/world.json") {
            Areas = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase) {};
            Environment = PopulationWelfare = 0;
            Load(path);
        }

        private void Load(string path){
            try{
                JsonDocument doc;
                string jsonString;
                jsonString = File.ReadAllText(path);
                doc = JsonDocument.Parse(jsonString);
            

                //enumerating areas
                JsonElement areasElement;
                if(!doc.RootElement.TryGetProperty("areas", out areasElement))
                    throw new Exception($"Property \"areas\" is missing in root element");

                foreach (JsonProperty area in areasElement.EnumerateObject())
                {
                    Dictionary<string, Room> rooms = new Dictionary<string, Room> {};

                    JsonElement defaultRoomElement;
                    if(!area.Value.TryGetProperty("default", out defaultRoomElement))
                        throw new Exception($"Property \"default\" is missing in area \"{area.Name}\"");
                    string defaultRoom = defaultRoomElement.GetString() ?? 
                        throw new Exception($"Value of property \"default\" is null in area \"{area.Name}\"");

                    JsonElement roomsElement;
                    if(!area.Value.TryGetProperty("rooms", out roomsElement))
                        throw new Exception($"Property \"rooms\" is missing in area \"{area.Name}\"");

                    //enumerating rooms
                    foreach (JsonProperty room in roomsElement.EnumerateObject()){
                        Dictionary<string, string> exits = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {};

                        JsonElement longDescriptionElement;
                        if(!room.Value.TryGetProperty("longDescription", out longDescriptionElement))
                            throw new Exception($"Property \"longDescription\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        string longDescription = longDescriptionElement.GetString() ?? 
                            throw new Exception($"Value of property \"longDescription\" is null in room \"{room.Name}\", in area \"{area.Name}\"");

                        JsonElement actionsElement;
                        if(!room.Value.TryGetProperty("actions", out actionsElement))
                            throw new Exception($"Property \"actions\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        if(actionsElement.ValueKind != JsonValueKind.Array)
                            throw new Exception($"Property \"actions\" is not an array in room \"{room.Name}\", in area \"{area.Name}\"");

                        string[] actions = JsonSerializer.Deserialize<string[]>(actionsElement);

                        JsonElement exitsElement;
                        if(!room.Value.TryGetProperty("exits", out exitsElement))
                            throw new Exception($"Property \"exits\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");

                        //enumerating exits
                        foreach(JsonProperty property in exitsElement.EnumerateObject()){
                            exits[property.Name] = property.Value.GetString() ?? 
                            throw new Exception($"Exit \"{property.Name}\" has value null in room \"{room.Name}\", in area \"{area.Name}\"");
                        }

                        rooms[room.Name] = new Room(room.Name, longDescription, actions, exits);
                    }
                    Areas[area.Name] = new Area(area.Name, rooms, defaultRoom);
                }
                doc.Dispose();
                loaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Room GetRoom(string area, string? room = "") {
            return string.IsNullOrEmpty(room) ? Areas[area].Rooms[Areas[area].DefaultRoom] : Areas[area].Rooms[room];
        }
    }
}