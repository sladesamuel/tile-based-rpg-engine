using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace TileBasedRpg.Engine
{
    public static class MovementCalculations
    {
        public static (Vector2, Vector2) DetermineCurrentAndTargetPosition(Transform2 transform, Vector2 direction, Size tileSize)
        {
            var currentPosition = transform.Position;

            var currentTile = TileSupport.ConvertScreenToTilePosition(transform.Position, tileSize);
            var nextTile = new Point(
                currentTile.X + (int)direction.X,
                currentTile.Y + (int)direction.Y
            );

            // DEBUG: Code to support visual debugging.
            // TODO: Either remove or change to be usable in a generic way.
            // HighlightTile(currentTile, Color.Red);
            // HighlightTile(nextTile, Color.Green);

            var targetPosition = TileSupport.ConvertTileToScreenPosition(nextTile, tileSize);

            return (currentPosition, targetPosition);
        }

        // DEBUG: Code to support visual debugging.
        // TODO: Either remove or change to be usable in a generic way.
        // private void HighlightTile(Point tileCoordinates, Color tileColor)
        // {
        //     var entity = CreateEntity();

        //     var position = TileSupport.ConvertTileToScreenPosition(tileCoordinates);

        //     entity.Attach(new Sprite(TileSupport.TileHighlightingTexture)
        //     {
        //         Color = tileColor,
        //         Origin = Vector2.Zero
        //     });

        //     entity.Attach(new Transform2(position));
        // }
    }
}
