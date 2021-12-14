using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using TileBasedRpg.Engine.Components;

namespace TileBasedRpg.Engine.Systems
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
            const int speed = 4;
            float amount = elapsedSeconds * speed;

            movement.IncrementLerpAmount(amount);

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

            if (IsCloseEnough(transform.Position, movement.TargetPosition))
            {
                transform.Position = movement.TargetPosition;
            }
        }

        private static bool IsCloseEnough(Vector2 position, Vector2 targetPosition)
        {
            const float tolerance = 0.01f;

            var acceptableMinimum = new Vector2(
                targetPosition.X - tolerance, targetPosition.Y - tolerance);

            var acceptableMaximum = new Vector2(
                targetPosition.X + tolerance, targetPosition.Y + tolerance);

            bool isCloseEnoughX = position.X >= acceptableMinimum.X && position.X <= acceptableMaximum.X;
            bool isCloseEnoughY = position.Y >= acceptableMinimum.Y && position.Y <= acceptableMaximum.Y;

            return isCloseEnoughX && isCloseEnoughY;
        }

        private void DeleteMovementComponentIfComplete(int entityId, Transform2 transform, Movement movement)
        {
            if (HasReachedTarget(transform, movement))
            {
                movementMapper.Delete(entityId);
            }
        }

        private static bool HasReachedTarget(Transform2 transform, Movement movement) =>
            transform.Position == movement.TargetPosition;
    }
}
