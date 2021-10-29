using Microsoft.Xna.Framework;

namespace SandboxRpg.Components
{
    public class Movement
    {
        public Movement(Vector2 targetPosition)
        {
            TargetPosition = targetPosition;
        }

        public Vector2 TargetPosition { get; }

        public bool ShouldStop { get; private set; }

        public void Stop() => ShouldStop = true;
    }
}
