using Microsoft.Xna.Framework;

namespace SandboxRpg
{
    public static class TileSupport
    {
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
