using System.Text.Json;
using System.Text.Json.Serialization;

namespace WorldOfZuul
{
    public class World
    {
        public Dictionary<string, Area> Areas {get; private set;}
        public bool Loaded {get; private set;}
        public int PopulationWelfare{get; set;}
        public int Environment{get; set;}
        public int PreviousEnvironment{get; set;}
        public int PreviousPopulationWelfare{get; set;}

        public World(string path="assets/world.json") {
            Loaded = false;
            Areas = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase) {};
            Environment = PopulationWelfare = PreviousEnvironment = PreviousPopulationWelfare = 50;
            Load(path);
        }

        private void Load(string path){
            try{
                JsonDocument doc;
                string jsonString;
                jsonString = File.ReadAllText(path);
                doc = JsonDocument.Parse(jsonString);
            

                JsonElement areasElement;
                if(!doc.RootElement.TryGetProperty("areas", out areasElement))
                    throw new Exception($"Property \"areas\" is missing in root element");

                //enumerating areas
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
                        Dictionary<string, Npc> npcs = new(StringComparer.OrdinalIgnoreCase);
                        Dictionary<int, string> longDescriptions = new Dictionary<int, string>();

                        JsonElement shortDescriptionElement;
                        if(!room.Value.TryGetProperty("shortDescription", out shortDescriptionElement))
                            throw new Exception($"Property \"shortDescription\" is missing in room \"{room.Name}\", in area \"{area.Name}\".");
                        string shortDesc = shortDescriptionElement.GetString() ?? 
                            throw new Exception($"Value of property \"shortDescription\" is null in room \"{room.Name}\", in area \"{area.Name}\".");


                        JsonElement longDescriptionElement;
                        if(!room.Value.TryGetProperty("longDescription", out longDescriptionElement))
                            throw new Exception($"Property \"longDescription\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        foreach(JsonProperty longDesc in longDescriptionElement.EnumerateObject()){
                            int environmentValue;
                            if(!int.TryParse(longDesc.Name, out environmentValue))
                                throw new Exception($"Description must contain integers as keys in room \"{room.Name}\", in area \"{area.Name}\"");
                            longDescriptions[environmentValue] = longDesc.Value.GetString() ??
                                throw new Exception($"Description with value \"{longDesc.Name}\" has value null in room \"{room.Name}\", in area \"{area.Name}\"");
                        }

                        JsonElement actionsElement;
                        if(!room.Value.TryGetProperty("actions", out actionsElement))
                            throw new Exception($"Property \"actions\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        if(actionsElement.ValueKind != JsonValueKind.Array)
                            throw new Exception($"Property \"actions\" is not an array in room \"{room.Name}\", in area \"{area.Name}\"");

                        string[] actions = JsonSerializer.Deserialize<string[]>(actionsElement) ?? new string[0];

                        JsonElement eventsElement;
                        if(!room.Value.TryGetProperty("events", out eventsElement))
                            throw new Exception($"Property \"actions\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        if(eventsElement.ValueKind != JsonValueKind.Array)
                            throw new Exception($"Property \"actions\" is not an array in room \"{room.Name}\", in area \"{area.Name}\"");

                        List<string> events = JsonSerializer.Deserialize<List<string>>(eventsElement) ?? new List<string>{};

                        JsonElement exitsElement;
                        if(!room.Value.TryGetProperty("exits", out exitsElement))
                            throw new Exception($"Property \"exits\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");

                        //enumerating exits
                        foreach(JsonProperty property in exitsElement.EnumerateObject()){
                            exits[property.Name] = property.Value.GetString() ?? 
                            throw new Exception($"Exit \"{property.Name}\" has value null in room \"{room.Name}\", in area \"{area.Name}\"");
                        }
                        
                        JsonElement npcsElement;
                        if(!room.Value.TryGetProperty("npcs", out npcsElement))
                            throw new Exception($"Property \"npcs\" is missing in room \"{room.Name}\", in area \"{area.Name}\"");
                        
                        //enumerating npcs
                        foreach(JsonElement pathNpc in npcsElement.EnumerateArray()){
                            Npc npc = new Npc(pathNpc.GetString() ?? 
                            throw new Exception($"Npcs contains a non-string element in room \"{room.Name}\", in area \"{area.Name}\""));
                            if(!npc.Loaded) throw new Exception($"Error loading in NPC:(${pathNpc.GetString()}) in room \"{room.Name}\", in area \"{area.Name}\"");
                            npcs[npc.Name] = npc;
                        }
                        
                        rooms[room.Name] = new Room(room.Name, shortDesc, longDescriptions, actions, exits, npcs, events);
                    }
                    Areas[area.Name] = new Area(area.Name, rooms, defaultRoom);
                }
                doc.Dispose();
                Loaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public Room GetRoom(string area, string? room = "") {
            return string.IsNullOrEmpty(room) ? Areas[area].Rooms[Areas[area].DefaultRoom] : Areas[area].Rooms[room];
        }

        [JsonConstructorAttribute]
        public World(
            Dictionary<string, Area> areas,
            int populationWelfare,
            int environment,
            int previousEnvironment,
            int previousPopulationWelfare,
            bool loaded
        ) {
            this.Areas = new(areas, StringComparer.OrdinalIgnoreCase);
            this.PopulationWelfare = populationWelfare;
            this.Environment = environment;
            this.PreviousEnvironment = previousEnvironment;
            this.PreviousPopulationWelfare = previousPopulationWelfare;
            this.Loaded = loaded;
        }

        public void BetterEnvironment()
        {
            Environment += 5;
        }
        public void WorseEnvironment()
        {
            Environment += 5;
        }
    }
}