using Microsoft.Xna.Framework;

namespace SandboxRpg.Components
{
    public class Movement
    {
        public Movement(Vector2 direction)
        {
            Direction = direction;
        }

        public Vector2 Direction { get; }
    }
}
