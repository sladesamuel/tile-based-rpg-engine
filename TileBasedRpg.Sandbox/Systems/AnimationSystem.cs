using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using Animation = TileBasedRpg.Sandbox.Components.Animation;

namespace TileBasedRpg.Sandbox.Systems
{
    public class AnimationSystem : EntityUpdateSystem
    {
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;
        private ComponentMapper<Animation> animationMapper;

        public AnimationSystem()
            : base(Aspect.All(typeof(AnimatedSprite)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            animationMapper = mapperService.GetMapper<Animation>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var animatedSprite = animatedSpriteMapper.Get(entity);

                if (animationMapper.Has(entity))
                {
                    var animation = animationMapper.Get(entity);
                    animatedSprite.Play(animation.AnimationName);

                    // Remove the animation component so that we don't keep trying to play the same animation
                    animationMapper.Delete(entity);
                }

                animatedSprite.Update(gameTime);
            }
        }
    }
}
