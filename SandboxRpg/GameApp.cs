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
using SandboxRpg.Components;
using SandboxRpg.Systems;

namespace SandboxRpg
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
            var screenPosition = new Vector2(
                (viewport.Width / 2f) - (Constants.SpriteWidth / 2f),
                (viewport.Height / 2f) - (Constants.SpriteHeight / 2f)
            );

            var position = camera.WorldToScreen(screenPosition);

            var animatedSprite = new AnimatedSprite(spriteSheet);
            animatedSprite.Play("idle");

            entity.Attach(new Player());
            entity.Attach(animatedSprite);
            entity.Attach(new Transform2(position));
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
