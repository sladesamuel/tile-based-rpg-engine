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
            : base(Aspect.All(typeof(Components.Player), typeof(Transform2)))
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
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                return Vector2.UnitY;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                return -Vector2.UnitY;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                return -Vector2.UnitX;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                return Vector2.UnitX;
            }

            return Vector2.Zero;
        }
    }
}
