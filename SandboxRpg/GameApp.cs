using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using SandboxRpg.Systems;

namespace SandboxRpg
{
    public class GameApp : Game
    {
        private const int WindowWidth = 720;
        private const int WindowHeight = 480;
        private const int ViewportWidth = 360;
        private const int ViewportHeight = 240;

        private World world;

        private GraphicsDeviceManager graphics;

        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRenderer;
        private OrthographicCamera camera;

        public GameApp()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, ViewportWidth, ViewportHeight);
            camera = new OrthographicCamera(viewportAdapter);

            world = new WorldBuilder()
                .AddSystem(new PlayerMovementSystem())
                .AddSystem(new PlayerFollowSystem(camera))
                .AddSystem(new PreRenderSystem(GraphicsDevice))
                .AddSystem(new RenderSystem(GraphicsDevice, camera))
                .Build();

            Components.Add(world);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            tiledMap = Content.Load<TiledMap>("Maps/Home");
            tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, tiledMap);

            // TODO: Tidy up loading of player.
            var playerEntity = world.CreateEntity();
            Player.AttachComponents(Content, GraphicsDevice.Viewport, camera, playerEntity);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            tiledMapRenderer.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: This needs to move to the end of the method call once we have moved everything to ECS.
            base.Draw(gameTime);

            // Render tilemap
            var viewMatrix = camera.GetViewMatrix();
            tiledMapRenderer.Draw(viewMatrix);
        }
    }
}
