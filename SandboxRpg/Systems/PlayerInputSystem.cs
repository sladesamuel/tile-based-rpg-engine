using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using SandboxRpg.Components;

namespace SandboxRpg.Systems
{
    public class PlayerInputSystem : EntityProcessingSystem
    {
        private ComponentMapper<Movement> movementMapper;
        private ComponentMapper<Animation> animationMapper;
        private ComponentMapper<Transform2> transformMapper;

        public PlayerInputSystem()
            : base(Aspect.All(typeof(Player)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            movementMapper = mapperService.GetMapper<Movement>();
            animationMapper = mapperService.GetMapper<Animation>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var currentKeyboardState = Keyboard.GetState();

            CheckIfMoving(currentKeyboardState, entityId);
        }

        private void CheckIfMoving(KeyboardState keyboardState, int entityId)
        {
            var (direction, animationName) = GetMovement(keyboardState);

            if (direction == Vector2.Zero)
            {
                if (movementMapper.Has(entityId))
                {
                    // Stop the player moving
                    var movement = movementMapper.Get(entityId);
                    movement.Stop();
                }
            }
            else
            {
                // Start moving the player
                var targetPosition = DetermineTargetPosition(entityId, direction);
                movementMapper.Put(entityId, new Movement(targetPosition));
            }

            animationMapper.Put(entityId, new Animation(animationName));
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

        private Vector2 DetermineTargetPosition(int entityId, Vector2 direction)
        {
            var transform = transformMapper.Get(entityId);

            var currentTile = TileSupport.ConvertScreenToTilePosition(transform.Position);
            var nextTile = new Point(
                currentTile.X + (int)direction.X,
                currentTile.Y + (int)direction.Y
            );

            return TileSupport.ConvertTileToScreenPosition(nextTile);
        }
    }
}
