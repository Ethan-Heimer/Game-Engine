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

        public Sprite Sprite = new Sprite("");
        public Rectangle Bounds 
        {
            get
            {
                return Sprite.Bounds;
            }
        }

        public void OnDraw()
        {
            Renderer.RenderTexture(Sprite, transform, Color.White);
        }

        
    }
}
