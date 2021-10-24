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

        private GraphicsDeviceManager graphics;

        private World world;
        private OrthographicCamera camera;
        private TileMapRenderSystem tileMapRenderSystem;

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
                .AddSystem(tileMapRenderSystem = new TileMapRenderSystem(GraphicsDevice, camera))
                .AddSystem(new RenderSystem(GraphicsDevice, camera))
                .Build();

            Components.Add(world);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var tiledMap = Content.Load<TiledMap>("Maps/Home");
            tileMapRenderSystem.LoadMap(tiledMap);

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

            base.Update(gameTime);
        }
    }
}
