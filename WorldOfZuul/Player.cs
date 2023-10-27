class Player {
    public string CurrentRoom {get; set;}
    public string? PreviousRoom {get; set;}
    public string CurrentArea {get; set;}
    public string? PreviousArea {get; set;}
    public int personalWelfare, populationWelfare, environment;

    public Player(string currentArea, string previousArea, string currentRoom) {
        CurrentArea = currentArea;
        PreviousArea = previousArea;
        CurrentRoom = currentRoom;
        personalWelfare = populationWelfare = environment = 0;
    }

}