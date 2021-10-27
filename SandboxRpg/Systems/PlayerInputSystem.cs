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
        private ComponentMapper<Animation> animationMapper;

        public PlayerInputSystem()
            : base(Aspect.All(typeof(Player)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            movementMapper = mapperService.GetMapper<Movement>();
            animationMapper = mapperService.GetMapper<Animation>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var currentKeyboardState = Keyboard.GetState();

            CheckIfMoving(currentKeyboardState, entityId);
        }

        private void CheckIfMoving(KeyboardState keyboardState, int entityId)
        {
            var movement = GetMovement(keyboardState);

            if (movement.Item1 == Vector2.Zero)
            {
                // Stop the player moving
                movementMapper.Delete(entityId);
            }
            else
            {
                // Start moving the player
                movementMapper.Put(entityId, new Movement(movement.Item1));
            }

            animationMapper.Put(entityId, new Animation(movement.Item2));
        }

        private static (Vector2, string) GetMovement(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                return (Vector2.UnitY, "walkDown");
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                return (-Vector2.UnitY, "walkUp");
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                return (-Vector2.UnitX, "walkLeft");
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                return (Vector2.UnitX, "walkRight");
            }

            return (Vector2.Zero, "idle");
        }
    }
}
