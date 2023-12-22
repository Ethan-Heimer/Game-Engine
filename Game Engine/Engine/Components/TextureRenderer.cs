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

namespace GameEngine
{
    [Note(note = "The Standard Rectangle Only Allows for int values, a coustom one with floting point values might be better")]
    public class TextureRenderer : Behavior
    {
        string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Sprite.Path = _path;
            }
        }

        public Sprite Sprite;
        public Color Color = Color.White;

        public Rectangle Bounds 
        {
            get
            {
                if(Sprite == null)
                    return Rectangle.Empty;
                return Sprite.Bounds;
            }
        }

        public void OnDraw()
        {
            if(Sprite != null)
                Renderer.RenderTexture(Sprite, transform, Color);
        }

        
    }
}
