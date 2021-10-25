using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace SandboxRpg.Systems
{
    public class PlayerFollowSystem : EntityProcessingSystem
    {
        private readonly Camera<Vector2> camera;
        private ComponentMapper<Transform2> transformMapper;

        public PlayerFollowSystem(Camera<Vector2> camera)
            : base(Aspect.All(typeof(Components.Player), typeof(Transform2)))
        {
            this.camera = camera
                ?? throw new ArgumentNullException(nameof(camera));
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var transform = transformMapper.Get(entityId);
            camera.LookAt(transform.Position);
        }
    }
}
