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
            }
        }

        private void Move(float elapsedSeconds, Movement movement, Transform2 transform)
        {
            // The Movement component may have been deleted from the entity mid-frame
            if (movement == null)
            {
                return;
            }

            const int speed = 200;
            transform.Position += speed * movement.Direction * elapsedSeconds;
        }
    }
}
