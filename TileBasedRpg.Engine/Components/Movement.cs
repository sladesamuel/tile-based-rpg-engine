using System;
using Microsoft.Xna.Framework;

namespace TileBasedRpg.Engine.Components
{
    public class Movement
    {
        public Movement(Vector2 currentPosition, Vector2 targetPosition)
        {
            CurrentPosition = currentPosition;
            TargetPosition = targetPosition;
        }

        public Vector2 CurrentPosition { get; }
        public Vector2 TargetPosition { get; }
        public float LerpAmount { get; private set; }

        public void IncrementLerpAmount(float amount)
        {
            float lerpAmount = LerpAmount + amount;
            LerpAmount = Clamp(lerpAmount, 0f, 1f);
        }

        private static float Clamp(float amount, float min, float max)
        {
            // TODO: For some reason, using Math.Clamp fails to compile. It has worked previously,
            // so I suspect it may be related to the .NET SDK version in use. Will need to
            // investigate separately.

            return Math.Min(max, Math.Max(min, amount));
        }
    }
}
