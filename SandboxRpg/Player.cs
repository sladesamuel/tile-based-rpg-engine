using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace SandboxRpg
{
    public class Player
    {
        private const int SpriteWidth = 48;
        private const int SpriteHeight = 64;
        private static readonly Rectangle Area = new Rectangle(0, 0, SpriteWidth, SpriteHeight);

        private readonly Texture2D texture;
        private Vector2 position = Vector2.Zero;

        private Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public Vector2 Position => position;

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            Move(gameTime, keyboardState);
        }

        private void Move(GameTime gameTime, KeyboardState keyboardState)
        {
            const int speed = 200;

            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection(keyboardState);

            position += speed * movementDirection * seconds;
        }

        private static Vector2 GetMovementDirection(KeyboardState keyboardState)
        {
            var movementDirection = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
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

        public void Draw(SpriteBatch sprites)
        {
            sprites.Draw(
                texture,
                destinationRectangle: new Rectangle((int)position.X, (int)position.Y, 16, 24),
                sourceRectangle: Area,
                Color.White
            );
        }

        public static Player Load(ContentManager content, Viewport viewport)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var texture = content.Load<Texture2D>("Spritesheets/player");
            var position = new Vector2(
                (viewport.Width / 2f) - (SpriteWidth / 2f),
                (viewport.Height / 2f) - (SpriteHeight / 2f)
            );

            return new Player(texture, position);
        }
    }
}
