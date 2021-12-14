using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace TileBasedRpg.Engine.Systems
{
    public class TileMapRenderSystem : IUpdateSystem, IDrawSystem
    {
        private readonly Camera<Vector2> camera;
        private readonly TiledMapRenderer tiledMapRenderer;

        public TileMapRenderSystem(GraphicsDevice graphicsDevice, Camera<Vector2> camera)
        {
            this.camera = camera ?? throw new ArgumentNullException(nameof(camera));

            tiledMapRenderer = new TiledMapRenderer(graphicsDevice);
        }

        public void Initialize(World world)
        {
            // Nothing to do here
        }

        public void LoadMap(TiledMap tiledMap) => tiledMapRenderer.LoadMap(tiledMap);
        public void Update(GameTime gameTime) => tiledMapRenderer.Update(gameTime);

        public void Draw(GameTime gameTime)
        {
            var viewMatrix = camera.GetViewMatrix();
            tiledMapRenderer.Draw(viewMatrix);
        }

        public void Dispose() => tiledMapRenderer.Dispose();
    }
}
