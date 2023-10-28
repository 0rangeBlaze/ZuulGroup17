using System;

public class Dialogs
{
    private string name { get; set; }
    private Dictionary<string, string> dialogs;
    public Dialogs(string name, Dictionary<string, string> dialogs)
    {
        this.name = name;
        this.dialogs = dialogs;
    }
}