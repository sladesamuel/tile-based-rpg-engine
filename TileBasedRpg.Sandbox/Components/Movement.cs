using Microsoft.Xna.Framework;

namespace TileBasedRpg.Sandbox.Components
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
        public float LerpAmount { get; set; }

        public bool ShouldStop { get; private set; }

        public void Stop() => ShouldStop = true;
    }
}
