using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using SandboxRpg.Components;

namespace SandboxRpg.Systems
{
    public class PlayerInputSystem : EntityProcessingSystem
    {
        private ComponentMapper<Movement> movementMapper;

        public PlayerInputSystem()
            : base(Aspect.All(typeof(Player)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            movementMapper = mapperService.GetMapper<Movement>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var currentKeyboardState = Keyboard.GetState();

            CheckIfMoving(currentKeyboardState, entityId);
        }

        private void CheckIfMoving(KeyboardState keyboardState, int entityId)
        {
            var movementDirection = GetMovementDirection(keyboardState);

            if (movementDirection == Vector2.Zero)
            {
                // Stop the player moving
                movementMapper.Delete(entityId);
            }
            else
            {
                // Start moving the player
                movementMapper.Put(entityId, new Movement(movementDirection));
            }
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
