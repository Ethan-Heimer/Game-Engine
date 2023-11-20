using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Editor;

namespace GameEngine
{
    public class TextureRenderer : Behavior
    {
        string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                sprite.Path = _path;
            }
        }

        public Sprite sprite = new Sprite("");

        public void OnDraw(Drawer g)
        {
            g.RenderTexture(sprite, transform, Color.White);
        }
    }
}
