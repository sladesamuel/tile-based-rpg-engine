using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBasedRpg.Sandbox
{
    public static class TileSupport
    {
        public static Texture2D TileHighlightingTexture { get; set; }

        public static Vector2 ConvertTileToScreenPosition(Point tilePosition) =>
            new Vector2(
                tilePosition.X * Constants.TileWidth,
                tilePosition.Y * Constants.TileHeight
            );

        public static Point ConvertScreenToTilePosition(Vector2 screenPosition) =>
            new Point(
                (int)(screenPosition.X / Constants.TileWidth),
                (int)(screenPosition.Y / Constants.TileHeight)
            );
    }
}
