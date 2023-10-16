using System;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;

namespace WorldOfZuul
{
    public class Area
    {
        List<Room>? rooms;
        private string? name;

        public Area(String? name)
        {
            this.name = name;
        }
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
            if(travelCommand == "car")
            {

            }
            else if(travelCommand == "walk")
            {

            }
            else if(travelCommand == "public transport")
            {

            }
            else
            {
                ErrorMessage();
            }
        }
        //Generic error message, can later be moved somewhere else
        public static void ErrorMessage()
        {
            Console.WriteLine("Invalid Input, Please try again!");
        }
    }
}