using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Rendering
{
    public class Drawer
    {
        readonly SpriteBatch spriteBatch;

        public Drawer(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void RenderTexture(Texture2D texture, Vector2 position)
        {
            RenderTexture(texture, position, Color.White);
        }

        public void RenderTexture(Texture2D texture, Vector2 position, Color color)
        {
            RenderTexture(texture, position, 0f, 1f, color, Vector2.Zero);
        }
        
        public void RenderTexture(Texture2D texture, Transform transform, Color color)
        {
            RenderTexture(texture, transform, color, Vector2.One);
        }

        public void RenderTexture(Texture2D texture, Transform transform, Color color, Vector2 origin)
        {
            RenderTexture(texture, transform.Position, transform.Rotation, transform.Scale, color, origin);
        }

        public void RenderTexture(Texture2D texture, Vector2 position, float rotation, float scale, Color color, Vector2 origin)
        {
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
        }
    }
}
