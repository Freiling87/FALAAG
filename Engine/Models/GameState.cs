namespace Engine.Models
{
    public class GameState
    {
        public Player Player { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public int Z { get; init; }

        public GameState(Player player, int x, int y, int z)
        {
            Player = player;
            X = x;
            Y = y;
            Z = z;
        }
    }
}