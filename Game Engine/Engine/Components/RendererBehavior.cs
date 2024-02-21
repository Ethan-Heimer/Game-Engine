using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components
{
    public class RendererBehavior : Behavior
    {
        public Sprite Sprite;
        public Color Color = Color.White;

        public virtual Rectangle GetBounds()
        {
            if (Sprite == null)
                return Rectangle.Empty;

            return Sprite.Bounds;
        }
    }
}
