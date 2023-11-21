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
        readonly GraphicsDevice graphicsDevice;

        Texture2D pixel;

        public Drawer(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            pixel = new Texture2D(graphicsDevice, 1, 1, false,
            SurfaceFormat.Color);

            Int32[] data = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            pixel.SetData<Int32>(data, 0, pixel.Width * pixel.Height);
        }

        public void RenderTexture(Sprite texture, Vector2 position)
        {
            RenderTexture(texture, position, Color.White);
        }

        public void RenderTexture(Sprite texture, Vector2 position, Color color)
        {
            RenderTexture(texture, position, 0f, 1f, color, Vector2.Zero);
        }
        
        public void RenderTexture(Sprite texture, Transform transform, Color color)
        {
            RenderTexture(texture, transform, color, Vector2.One);
        }

        public void RenderTexture(Sprite texture, Transform transform, Color color, Vector2 origin)
        {
            RenderTexture(texture, transform.Position, transform.Rotation, transform.Scale, color, origin);
        }

        public void RenderTexture(Sprite texture, Vector2 position, float rotation, float scale, Color color, Vector2 origin)
        {
            spriteBatch.Draw(texture.Texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2)
        {
           DrawLine(pos1, pos2, Color.White);
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2, Color color)
        {
            float lineAngle = (float)(Math.Atan2(pos2.Y-pos1.Y, pos2.X-pos1.X));
            int length = (int)Math.Sqrt(Math.Pow(pos2.Y - pos1.Y, 2) + Math.Pow(pos2.X - pos1.X, 2));

            // Paint a 100x1 line starting at 20, 50
            spriteBatch.Draw(pixel, new Rectangle((int)pos1.X, (int)pos1.Y, length, 1), null, color, lineAngle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
