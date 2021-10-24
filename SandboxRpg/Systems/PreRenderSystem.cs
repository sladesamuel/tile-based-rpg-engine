using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities.Systems;

namespace SandboxRpg.Systems
{
    public class PreRenderSystem : DrawSystem
    {
        private readonly GraphicsDevice graphicsDevice;

        public PreRenderSystem(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice ??
                throw new ArgumentNullException(nameof(graphicsDevice));
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.Black);
        }
    }
}
