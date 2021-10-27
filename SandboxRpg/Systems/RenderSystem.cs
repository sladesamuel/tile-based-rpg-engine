using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled.Renderers;

namespace SandboxRpg.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly Camera<Vector2> camera;
        private readonly SpriteBatch spriteBatch;

        private ComponentMapper<Sprite> spriteMapper;
        private ComponentMapper<AnimatedSprite> animatedSpriteMapper;
        private ComponentMapper<Transform2> transformMapper;

        public RenderSystem(GraphicsDevice graphicsDevice, Camera<Vector2> camera)
            : base(Aspect.All(typeof(Transform2)).One(typeof(Sprite), typeof(AnimatedSprite)))
        {
            this.graphicsDevice = graphicsDevice
                ?? throw new ArgumentNullException(nameof(graphicsDevice));

            this.camera = camera
                ?? throw new ArgumentNullException(nameof(camera));

            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            spriteMapper = mapperService.GetMapper<Sprite>();
            animatedSpriteMapper = mapperService.GetMapper<AnimatedSprite>();
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            graphicsDevice.BlendState = BlendState.AlphaBlend;

            var viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);

            foreach (var entity in ActiveEntities)
            {
                // We should either have a Sprite or AnimatedSprite component
                var sprite = spriteMapper.Get(entity) ?? animatedSpriteMapper.Get(entity);
                var transform = transformMapper.Get(entity);

                sprite.Draw(
                    spriteBatch,
                    transform.Position,
                    transform.Rotation,
                    transform.Scale
                );
            }

            spriteBatch.End();
        }
    }
}
