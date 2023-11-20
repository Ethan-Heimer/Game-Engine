using GameEngine.Editor;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Rendering
{
    [Serializable]
    public class Sprite
    {
        string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                
            }
        }

        [NonSerialized]
        Texture2D _texture;
        public Texture2D Texture
        {
            get
            {
                if(_texture == null)
                {
                    _texture = AssetManager.LoadContent<Texture2D>(_path);
                }

                return _texture;
            }
        }

        public Sprite(string path)
        {
            this.Path = path;
        }

       
    }
}
