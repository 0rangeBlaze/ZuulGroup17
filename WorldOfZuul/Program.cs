namespace WorldOfZuul
{
    public class Program
    {
        public static void Main()
        {   
            Game game = Game.Load();
            game.Play();
        }
    }
}