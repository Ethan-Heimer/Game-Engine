using GameEngine.ComponentManagement;
using GameEngine.Debugging;
using GameEngine.Engine.Events;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GameEngine.Rendering
{
    [ContainsEvents]
    [Note(note = "The render uses a buffer which i feel might use alot of memory. when optimizing this might be a good place to start. a fun approch could be rendering with async, blocking the fuctions rendering calls until some blocking method is released")]
    public static class Renderer
    {
        static GraphicsDevice graphicsDevice;
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;

        static Texture2D pixel;
        static Texture2D circle;

        static EngineEvent<OnEngineDrawEvent> OnDraw;
        static OnEngineDrawEvent OnEngineDrawEvent = new OnEngineDrawEvent();

        static Stack<IBufferData> buffer = new Stack<IBufferData>();

        public static void Init(Game1 game, GraphicsDeviceManager _graphicsDeviceManager, GraphicsDevice _graphicDevice)
        {
            graphics = _graphicsDeviceManager;
            graphicsDevice = _graphicDevice;
            
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1280, 700);
            Resolution.SetResolution(1920, 1080, false);

            spriteBatch = new SpriteBatch(_graphicDevice);

            pixel = new Texture2D(graphicsDevice, 1, 1, false,
            SurfaceFormat.Color);

            Int32[] data = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            pixel.SetData<Int32>(data, 0, pixel.Width * pixel.Height);

            circle = CreateCircleText(1000);
        }

        public static void Draw()
        {
            graphicsDevice.Clear(new Color(55,55,55));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, CameraManager.GetTransformantionMaxtrix());

            BehaviorFunctionExecuter.Execute.OnDraw();
            OnDraw?.Invoke(OnEngineDrawEvent);
            RenderBuffer();

            spriteBatch.End();
        }

        public static void RenderTexture(Sprite texture, Vector2 position)
        {
            RenderTexture(texture, position, Color.White);
        }

        public static void RenderTexture(Sprite texture, Vector2 position, Color color)
        {
            RenderTexture(texture, position, 0f, 1f, color, Vector2.Zero);
        }

        public static void RenderTexture(Sprite texture, Transform transform, Color color)
        {
            RenderTexture(texture, transform.Position, (float)transform.GetRotationInRad(), transform.Scale, color, transform.Origin);
        }

        public static void RenderTexture(Sprite texture, Vector2 position, float rotation, float scale, Color color, Vector2 origin)
        {
            buffer.Push(new SpriteData()
            {
                texture = texture.Texture,
                position = position,
                rotation = rotation,
                scale = scale,
                color = color,
                origin = origin
            });
        }

        public static void DrawLine(Vector2 pos1, Vector2 pos2)
        {
            DrawLine(pos1, pos2, Color.White);
        }

        public static void DrawLine(Vector2 pos1, Vector2 pos2, Color color)
        {
            buffer.Push(new LineData()
            {
                texture = pixel,
                color = color,
                positionOne = pos1,
                positionTwo = pos2,
            });
        }

        public static void RenderRect(Rectangle rect)
        {
            RenderRect(rect, Color.White);
        }

        public static void RenderRect(Rectangle rect, Color color)
        {
            buffer.Push(new RectData()
            {
                texture = pixel,
                color = color,
                rectangle = rect,
            });
        }

        public static void DrawCircle(Vector2 center, float radius, Color color)
        {
            buffer.Push(new CircleData()
            {
                texture = circle,
                color = color,
                center = center,
                radius = radius
            });
        }    

        static Texture2D CreateCircleText(int radius)
        {
            Texture2D texture = new Texture2D(graphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius/2;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        static void RenderBuffer()
        {
            while(buffer.Count > 0)
            {
                var data = buffer.Pop();
                data.Draw(spriteBatch);
            }
        }
    }

    public interface IBufferData
    {
        Texture2D texture { get; set; }
        Color color { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }

    public struct SpriteData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }

        public Vector2 position;
        public float rotation;
        public float scale;
        public Vector2 origin;

        public void Draw(SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 1);
        }
    }

    public struct LineData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }

        public Vector2 positionOne;
        public Vector2 positionTwo;
        
        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, new Rectangle((int)positionOne.X, (int)positionOne.Y, 
            (int)Math.Sqrt(Math.Pow(positionTwo.Y - positionOne.Y, 2) + Math.Pow(positionTwo.X - positionOne.X, 2)), 1), 
            null, color,
            (float)(Math.Atan2(positionTwo.Y - positionOne.Y, positionTwo.X - positionOne.X)), 
            Vector2.Zero, SpriteEffects.None, 0);
    }

    public struct RectData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }
        public Rectangle rectangle;

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, rectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
    }

    public struct CircleData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }

        public Vector2 center;

        public float radius;

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, center, null, color, 0f, new Vector2(texture.Width/2, texture.Height/2), radius/1000, SpriteEffects.None, 0);
    }

    public struct OnEngineDrawEvent : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
}
