using System;
using System.Diagnostics;

namespace WorldOfZuul
{
    public class Area
    {
        public static void Mall()
        {
            string? commandMall;

            while(true)
            {
                Console.WriteLine("Where would you like to go: \n 1.Restaurant \n 2.Clothing Store \n 3.General Store");
                commandMall = Console.ReadLine()?.ToLower();

                try
                {
                    MallCommand(commandMall);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"There is no such place as {ex.Message}");
                }
            }
        }
        static void MallCommand(string? commandMall)
        {
            if(commandMall == "restaurant")
                Restaurant();
            else if(commandMall == "clothing store")
                ClothingStore();
            else if(commandMall == "general store")
                GeneralStore();
            else
                ErrorMessage();
        }
        static void FoodChoice(string? commandRestaurant)
        {
            if(commandRestaurant == "fast food")
                FastFood();
            else if(commandRestaurant == "healthy eats")
                HealthyFood();
            else if(commandRestaurant == "back")
                Mall();
            else
                ErrorMessage();
        }
        static void Restaurant()
        {
            string? commandRestaurant;
            while(true)
            {
                //possibly write more fluff here?
                Console.WriteLine("What would you like to eat? \n 1.Fast Food \n 2.Healthy Eats \n 3.Back");
                commandRestaurant = Console.ReadLine()?.ToLower();

                try
                {
                    FoodChoice(commandRestaurant);
                }
                catch
                {
                    ErrorMessage();
                }
            }
        }
        static void ClothingStore()
        {

        }
        static void GeneralStore()
        {

        }
        static void FastFood()
        {

        }
        static void HealthyFood()
        {

        }
        static void ErrorMessage()
        {
            Console.WriteLine("THere is no such place! \n Please try again.");
        }
    }
}