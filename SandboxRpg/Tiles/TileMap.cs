using System.Collections.Generic;

namespace SandboxRpg.Tiles
{
    public class TileMap
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int TileHeight { get; set; }
        public int TileWidth { get; set; }

        public List<Layer> Layers { get; set; } = new List<Layer>();
    }
}
