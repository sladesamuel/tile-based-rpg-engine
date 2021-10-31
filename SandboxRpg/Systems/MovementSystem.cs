using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using SandboxRpg.Components;

namespace SandboxRpg.Systems
{
    public class MovementSystem : EntityUpdateSystem
    {
        private ComponentMapper<Movement> movementMapper;
        private ComponentMapper<Transform2> transformMapper;

        public MovementSystem()
            : base(Aspect.All(typeof(Movement), typeof(Transform2)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            movementMapper = mapperService.GetMapper<Movement>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = gameTime.GetElapsedSeconds();

            foreach (var entity in ActiveEntities)
            {
                var movement = movementMapper.Get(entity);
                var transform = transformMapper.Get(entity);

                Move(elapsedSeconds, movement, transform);
                DeleteMovementComponentIfComplete(entity, transform, movement);
            }
        }

        private static int PerformTileBasedRounding(float value) => (int)System.Math.Round(value);

        private void Move(float elapsedSeconds, Movement movement, Transform2 transform)
        {
            const int speed = 2;
            float amount = elapsedSeconds * speed;

            movement.LerpAmount += amount;

            float x = MathHelper.Lerp(
                movement.CurrentPosition.X,
                movement.TargetPosition.X,
                movement.LerpAmount
            );

            float y = MathHelper.Lerp(
                movement.CurrentPosition.Y,
                movement.TargetPosition.Y,
                movement.LerpAmount
            );

            transform.Position = new Vector2(x, y);
        }

        private void DeleteMovementComponentIfComplete(int entityId, Transform2 transform, Movement movement)
        {
            if (movement.ShouldStop && HasReachedTarget(transform, movement))
            {
                movementMapper.Delete(entityId);
            }
        }

        private static bool HasReachedTarget(Transform2 transform, Movement movement) =>
            transform.Position == movement.TargetPosition;
    }
}
