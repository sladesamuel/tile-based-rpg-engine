using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace SandboxRpg
{
    public class GameApp : Game
    {
        private const int WindowWidth = 720;
        private const int WindowHeight = 480;
        private const int ViewportWidth = 360;
        private const int ViewportHeight = 240;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRenderer;
        private OrthographicCamera camera;

        private Player player;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tiledMap = Content.Load<TiledMap>("Maps/Home");
            tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, tiledMap);

            player = Player.Load(Content, GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            var currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            tiledMapRenderer.Update(gameTime);
            player.Update(gameTime, currentKeyboardState);

            camera.LookAt(player.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var viewMatrix = camera.GetViewMatrix();
            tiledMapRenderer.Draw(viewMatrix);

            spriteBatch.Begin(transformMatrix: viewMatrix);

            player.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
