using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

namespace SandboxRpg.Systems
{
    public class AnimationSystem : EntityUpdateSystem
    {
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;

        public AnimationSystem()
            : base(Aspect.All(typeof(AnimatedSprite)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var animatedSprite = animatedSpriteMapper.Get(entity);
                animatedSprite.Update(gameTime);
            }
        }
    }
}
