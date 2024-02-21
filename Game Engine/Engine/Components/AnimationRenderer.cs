using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components
{
    public class AnimationRenderer : RendererBehavior
    {
        public Vector2 SpriteSize;
        public float FramesPerSprite;
        public int XOffset;
        public int YOffset;

        int position;

        int frame;
        public void OnDraw()
        {
            if (frame >= FramesPerSprite || Sprite == null)
            {
                position = (int)(position + SpriteSize.X >= Sprite.Bounds.Width ? 0 : position + SpriteSize.X);
                frame = 0;
            }

            Rectangle rectangle = new Rectangle()
            {
                Width = (int)SpriteSize.X,
                Height = (int)SpriteSize.Y,

                X = position + XOffset,
                Y = YOffset
            };

           
            Renderer.RenderTexture(Sprite, transform.WorldPosition, rectangle, transform.WorldRotation, transform.WorldScale, Color.White, transform.Origin, Layer.Game);
            frame++;
        }

        public override Rectangle GetBounds()
        {
            return new Rectangle()
            {
                Width = (int)SpriteSize.X,
                Height = (int)SpriteSize.Y,
            };
        }

        public void SawpYLevel(int level)
        {
            YOffset = (int)(level * SpriteSize.Y);
        }
    }
}
