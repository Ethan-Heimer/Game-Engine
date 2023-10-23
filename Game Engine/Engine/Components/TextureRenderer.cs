using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;

namespace GameEngine
{
    public class TextureRenderer : Behavior
    {
        public Texture2D texture;

        public void OnDraw(Drawer g)
        {
            g.RenderTexture(texture, transform, Color.White);
        }
    }
}
