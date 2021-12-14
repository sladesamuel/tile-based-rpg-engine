using System;

namespace TileBasedRpg.Sandbox
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new GameApp())
            {
                game.Run();
            }
        }
    }
}
