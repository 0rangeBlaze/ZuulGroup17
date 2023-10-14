using System;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace WorldOfZuul
{
    public class Area
    {
        Area area = new(rooms);
        public static void TravelCheck()
        {
            string? travelCommand;

            while(true)
            {
                Console.WriteLine("Choosing type of transport");
                travelCommand = Console.ReadLine()?.ToLower();

                try
                {
                    Travel(travelCommand);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"You can't travel by {ex.Message}");
                }
            } 
        }
        public static void Travel(string? travelCommand)
        {
            if(travelCommand == rooms)
            {

            }
        }
    }
}