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

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private TiledMap tiledMap;
        private TiledMapRenderer tiledMapRenderer;
        private OrthographicCamera camera;
        private Vector2 cameraPosition = new Vector2(550, 600);

        public GameApp()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                // TODO: This is not working. It's possible that the i3 tile manager is causing
                //       a problem, or that windowed mode is not supported by Linux.
                IsFullScreen = false,
                PreferredBackBufferWidth = WindowWidth,
                PreferredBackBufferHeight = WindowHeight
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, WindowWidth, WindowHeight);
            camera = new OrthographicCamera(viewportAdapter);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tiledMap = Content.Load<TiledMap>("Maps/Home");
            tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, tiledMap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            tiledMapRenderer.Update(gameTime);

            MoveCamera(gameTime);

            camera.LookAt(cameraPosition);

            base.Update(gameTime);
        }

        private static Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }

            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize a zero vector, so make sure we check for it
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            const int speed = 200;

            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();

            cameraPosition += speed * movementDirection * seconds;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            tiledMapRenderer.Draw(camera.GetViewMatrix());

            base.Draw(gameTime);
        }
    }
}
