using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Shouldly;
using Xunit;

namespace TileBasedRpg.Engine.Tests
{
    public class MovementCalculationsTests
    {
        // TODO: Should tile size be moved to some form of engine-level configuration?
        private Size tileSize = new Size(height: 16, width: 16);

        [Fact]
        public void CalculatesPositionOfAdjacentTileToTheRight()
        {
            var currentTile = new Point(10, 15);

            var expectedCurrentPosition = TileSupport.ConvertTileToScreenPosition(currentTile, tileSize);
            var expectedTargetPosition = new Vector2(
                expectedCurrentPosition.X + tileSize.Width,
                expectedCurrentPosition.Y
            );

            var transform = new Transform2(expectedCurrentPosition);

            var direction = Vector2.UnitX;

            var (actualCurrentPosition, actualTargetPosition) =
                MovementCalculations.DetermineCurrentAndTargetPosition(transform, direction, tileSize);

            actualCurrentPosition.ShouldBeEquivalentTo(expectedCurrentPosition);
            actualTargetPosition.ShouldBeEquivalentTo(expectedTargetPosition);
        }

        [Fact]
        public void CalculatesPositionOfAdjacentTileToTheLeft()
        {
            var currentTile = new Point(10, 15);

            var expectedCurrentPosition = TileSupport.ConvertTileToScreenPosition(currentTile, tileSize);
            var expectedTargetPosition = new Vector2(
                expectedCurrentPosition.X - tileSize.Width,
                expectedCurrentPosition.Y
            );

            var transform = new Transform2(expectedCurrentPosition);

            var direction = -Vector2.UnitX;

            var (actualCurrentPosition, actualTargetPosition) =
                MovementCalculations.DetermineCurrentAndTargetPosition(transform, direction, tileSize);

            actualCurrentPosition.ShouldBeEquivalentTo(expectedCurrentPosition);
            actualTargetPosition.ShouldBeEquivalentTo(expectedTargetPosition);
        }

        [Fact]
        public void CalculatesPositionOfAdjacentTileDown()
        {
            var currentTile = new Point(10, 15);

            var expectedCurrentPosition = TileSupport.ConvertTileToScreenPosition(currentTile, tileSize);
            var expectedTargetPosition = new Vector2(
                expectedCurrentPosition.X,
                expectedCurrentPosition.Y + tileSize.Height
            );

            var transform = new Transform2(expectedCurrentPosition);

            var direction = Vector2.UnitY;

            var (actualCurrentPosition, actualTargetPosition) =
                MovementCalculations.DetermineCurrentAndTargetPosition(transform, direction, tileSize);

            actualCurrentPosition.ShouldBeEquivalentTo(expectedCurrentPosition);
            actualTargetPosition.ShouldBeEquivalentTo(expectedTargetPosition);
        }

        [Fact]
        public void CalculatesPositionOfAdjacentTileUp()
        {
            var currentTile = new Point(10, 15);

            var expectedCurrentPosition = TileSupport.ConvertTileToScreenPosition(currentTile, tileSize);
            var expectedTargetPosition = new Vector2(
                expectedCurrentPosition.X,
                expectedCurrentPosition.Y - tileSize.Height
            );

            var transform = new Transform2(expectedCurrentPosition);

            var direction = -Vector2.UnitY;

            var (actualCurrentPosition, actualTargetPosition) =
                MovementCalculations.DetermineCurrentAndTargetPosition(transform, direction, tileSize);

            actualCurrentPosition.ShouldBeEquivalentTo(expectedCurrentPosition);
            actualTargetPosition.ShouldBeEquivalentTo(expectedTargetPosition);
        }
    }
}
