using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace SandboxRpg.Systems
{
    public class PlayerMovementSystem : EntityProcessingSystem
    {
        private ComponentMapper<Transform2> transformMapper;

        public PlayerMovementSystem()
            : base(Aspect.All(typeof(Player), typeof(Transform2)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var currentKeyboardState = Keyboard.GetState();
            var transform = transformMapper.Get(entityId);

            Move(gameTime, currentKeyboardState, transform);
        }

        private void Move(GameTime gameTime, KeyboardState keyboardState, Transform2 transform)
        {
            const int speed = 200;

            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection(keyboardState);

            transform.Position += speed * movementDirection * seconds;
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
    }
}
