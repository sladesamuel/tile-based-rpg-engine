using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;
using TileBasedRpg.Sandbox.Components;
using TileBasedRpg.Sandbox.Systems;

namespace TileBasedRpg.Sandbox
{
    public class GameApp : Game
    {
        private World world;
        private OrthographicCamera camera;
        private TileMapRenderSystem tileMapRenderSystem;

        public GameApp()
        {
            var graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = Constants.WindowWidth;
            graphics.PreferredBackBufferHeight = Constants.WindowHeight;
            graphics.ApplyChanges();

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(
                Window, GraphicsDevice, Constants.ViewportWidth, Constants.ViewportHeight);

            camera = new OrthographicCamera(viewportAdapter);

            world = new WorldBuilder()
                .AddSystem(new PlayerInputSystem())
                .AddSystem(new PlayerFollowSystem(camera))
                .AddSystem(new MovementSystem())
                .AddSystem(new AnimationSystem())
                .AddSystem(new PreRenderSystem(GraphicsDevice))
                .AddSystem(tileMapRenderSystem = new TileMapRenderSystem(GraphicsDevice, camera))
                .AddSystem(new RenderSystem(GraphicsDevice, camera))
                .Build();

            Components.Add(world);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            TileSupport.TileHighlightingTexture = Content.Load<Texture2D>("square-border");

            var tiledMap = Content.Load<TiledMap>("Maps/Home");
            tileMapRenderSystem.LoadMap(tiledMap);

            CreatePlayer();

            base.LoadContent();
        }

        private void CreatePlayer()
        {
            var viewport = GraphicsDevice.Viewport;
            var entity = world.CreateEntity();

            var spriteSheet = Content.Load<SpriteSheet>("Spritesheets/player.sf", new JsonContentLoader());

            var playerPosition = new Point(9, 15);
            var position = TileSupport.ConvertTileToScreenPosition(playerPosition);

            entity.Attach(new Player());
            entity.Attach(new AnimatedSprite(spriteSheet, "idle") { Origin = new Vector2(16f, 32f) });
            entity.Attach(new Transform2(position.X, position.Y, scaleX: 0.5f, scaleY: 0.5f));
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
