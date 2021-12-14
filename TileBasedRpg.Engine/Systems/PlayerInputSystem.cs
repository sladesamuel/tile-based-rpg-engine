using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using TileBasedRpg.Engine.Components;
using Animation = TileBasedRpg.Engine.Components.Animation;

namespace TileBasedRpg.Engine.Systems
{
    public class PlayerInputSystem : EntityProcessingSystem
    {
        private readonly Size tileSize;

        private ComponentMapper<Movement> movementMapper;
        private ComponentMapper<Animation> animationMapper;
        private ComponentMapper<Transform2> transformMapper;

        public PlayerInputSystem(Size tileSize)
            : base(Aspect.All(typeof(Player)))
        {
            this.tileSize = tileSize;
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

            if (direction != Vector2.Zero && !IsAlreadyMoving(entityId))
            {
                StartMovement(entityId, direction, animationName);
            }
        }

        private bool IsAlreadyMoving(int entityId) => movementMapper.Has(entityId);

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

        private void StartMovement(int entityId, Vector2 direction, string animationName)
        {
            var transform = transformMapper.Get(entityId);
            var (currentPosition, targetPosition) =
                MovementCalculations.DetermineCurrentAndTargetPosition(transform, direction, tileSize);

            movementMapper.Put(entityId, new Movement(currentPosition, targetPosition));
            animationMapper.Put(entityId, new Animation(animationName));
        }
    }
}
