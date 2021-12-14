using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using TileBasedRpg.Engine.Components;
using TileBasedRpg.Engine.Systems;
using Shouldly;
using Xunit;

namespace TileBasedRpg.Engine.Tests.Systems
{
    public class MovementSystemTests
    {
        private delegate void UpdateAction(GameTime gameTime);

        [Theory]
        [InlineData(1, 0)]  // Right
        [InlineData(-1, 0)] // Left
        [InlineData(0, 1)]  // Down
        [InlineData(0, -1)] // Up
        public void MovementReachesTargetPositionAfter60FramesIn1Second(int x, int y)
        {
            var currentPosition = Vector2.Zero;
            var targetPosition = new Vector2(
                currentPosition.X + (x * 16f),
                currentPosition.Y + (y * 16f)
            );

            var system = new MovementSystem();

            var world = new WorldBuilder()
                .AddSystem(system)
                .Build();

            var entity = world.CreateEntity();

            var transform = new Transform2(currentPosition);
            entity.Attach(transform);

            var movement = new Movement(currentPosition, targetPosition);
            entity.Attach(movement);

            RunXFramesPerSecond(framesPerSecond: 60, seconds: 1, world.Update);

            // Check that the entity has been moved to the target position
            transform.Position.ShouldBeEquivalentTo(targetPosition);

            world.Dispose();
        }

        private static void RunXFramesPerSecond(int framesPerSecond, int seconds, UpdateAction updateAction)
        {
            float elapsedMillisecondsPerFrame = (seconds / (float)framesPerSecond) * 1000f;
            float totalElapsedMilliSeconds = 0f;
            int totalFrames = framesPerSecond * seconds;

            for (int frameNumber = 0; frameNumber < totalFrames; frameNumber++)
            {
                totalElapsedMilliSeconds += elapsedMillisecondsPerFrame;

                var elapsedGameTime = TimeSpan.FromMilliseconds(totalElapsedMilliSeconds);
                var elapsedFrameTime = TimeSpan.FromMilliseconds(elapsedMillisecondsPerFrame);

                var gameTime = new GameTime(elapsedGameTime, elapsedFrameTime);
                updateAction(gameTime);
            }
        }
    }
}
