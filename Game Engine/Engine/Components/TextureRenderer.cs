using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Editor;
using GameEngine.Debugging;
using GameEngine.Engine.Components;

namespace GameEngine
{
    [Note(note = "The Standard Rectangle Only Allows for int values, a coustom one with floting point values might be better")]
    public class TextureRenderer : RendererBehavior
    {
        
        public void OnDraw()
        {
            if(Sprite != null)
                Renderer.RenderTexture(Sprite, transform, Color);
        }

        
    }
}
