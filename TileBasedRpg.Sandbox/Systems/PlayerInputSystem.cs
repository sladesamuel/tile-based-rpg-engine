using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using TileBasedRpg.Sandbox.Components;
using Animation = TileBasedRpg.Sandbox.Components.Animation;

namespace TileBasedRpg.Sandbox.Systems
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
                StopMovement(entityId);
            }
            else
            {
                StartMovement(entityId, direction);
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

        private void StopMovement(int entityId)
        {
            if (movementMapper.Has(entityId))
            {
                // Stop the player moving
                var movement = movementMapper.Get(entityId);
                movement.Stop();
            }
        }

        private void StartMovement(int entityId, Vector2 direction)
        {
            var (currentPosition, targetPosition) = DetermineCurrentAndTargetPosition(entityId, direction);
            movementMapper.Put(entityId, new Movement(currentPosition, targetPosition));
        }

        private (Vector2, Vector2) DetermineCurrentAndTargetPosition(int entityId, Vector2 direction)
        {
            var transform = transformMapper.Get(entityId);
            var currentPosition = transform.Position;

            var currentTile = TileSupport.ConvertScreenToTilePosition(transform.Position);
            var nextTile = new Point(
                currentTile.X + (int)direction.X,
                currentTile.Y + (int)direction.Y
            );

            HighlightTile(currentTile, Color.Red);
            HighlightTile(nextTile, Color.Green);

            var targetPosition = TileSupport.ConvertTileToScreenPosition(nextTile);

            return (currentPosition, targetPosition);
        }

        private void HighlightTile(Point tileCoordinates, Color tileColor)
        {
            var entity = CreateEntity();

            var position = TileSupport.ConvertTileToScreenPosition(tileCoordinates);

            entity.Attach(new Sprite(TileSupport.TileHighlightingTexture)
            {
                Color = tileColor,
                Origin = Vector2.Zero
            });

            entity.Attach(new Transform2(position));
        }
    }
}
