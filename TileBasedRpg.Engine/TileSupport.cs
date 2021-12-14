using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TileBasedRpg.Engine
{
    public static class TileSupport
    {
        public static Texture2D TileHighlightingTexture { get; set; }

        public static Vector2 ConvertTileToScreenPosition(Point tilePosition, Size tileSize) =>
            new Vector2(
                tilePosition.X * tileSize.Width,
                tilePosition.Y * tileSize.Height
            );

        public static Point ConvertScreenToTilePosition(Vector2 screenPosition, Size tileSize) =>
            new Point(
                (int)(screenPosition.X / tileSize.Width),
                (int)(screenPosition.Y / tileSize.Height)
            );
    }
}
