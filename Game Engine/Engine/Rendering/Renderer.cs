using GameEngine.ComponentManagement;
using GameEngine.Debugging;
using GameEngine.Editor;
using GameEngine.Engine.Events;
using GameEngine.Engine.Settings;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.src.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameEngine.Rendering
{
    [ContainsEvents]
    [ContainsSettings]
    [Note(note = "The render uses a buffer which i feel might use alot of memory. when optimizing this might be a good place to start. a fun approch could be rendering with async, blocking the fuctions rendering calls until some blocking method is released")]
    public static class Renderer
    {
        [EngineSettings("Rendering")]
        public static float scale = 1;

        static GameWindow window;

        static GraphicsDevice graphicsDevice;
        static GraphicsDeviceManager graphics;

        static SpriteBatch spriteBatch;
        static SpriteBatch uiBatch;

        static Texture2D pixel;
        static Texture2D circle;

        static EngineEvent<OnEngineDrawEvent> OnDraw;
        static OnEngineDrawEvent OnEngineDrawEvent = new OnEngineDrawEvent();

        static Stack<IBufferData> buffer = new Stack<IBufferData>();

        public static void Init(Game1 game, GraphicsDeviceManager _graphicsDeviceManager, GraphicsDevice _graphicDevice)
        {
            graphics = _graphicsDeviceManager;
            graphicsDevice = _graphicDevice;

            int width = (int)(1920 / scale);
            int height = (int)(1080 / scale);
            
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1280, 700);
            Resolution.SetResolution(width, height, false);

            spriteBatch = new SpriteBatch(_graphicDevice);
            uiBatch = new SpriteBatch(_graphicDevice);

            pixel = new Texture2D(graphicsDevice, 1, 1, false,
            SurfaceFormat.Color);

            Int32[] data = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            pixel.SetData<Int32>(data, 0, pixel.Width * pixel.Height);

            circle = CreateCircleText(1000);

            window = game.Window;
        }

        public static void Draw()
        {
            graphicsDevice.Clear(new Color(55,55,55));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, CameraManager.GetTransformantionMaxtrix());
            uiBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null);

            BehaviorFunctionExecuter.Execute.OnDraw();
            OnDraw?.Invoke(OnEngineDrawEvent);

            RenderBuffer();

            spriteBatch.End();
            uiBatch.End();
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
            RenderTexture(texture, transform.WorldPosition, (float)transform.WorldRotation, transform.WorldScale, color, transform.Origin);
        }

        public static void RenderTexture(Sprite texture, Vector2 position, float rotation, float scale, Color color, Vector2 origin)
        {
            RenderTexture(texture, position, rotation, scale, color, origin, Layer.Game);
        }

        public static void RenderTexture(Sprite texture, Vector2 position, float rotation, float scale, Color color, Vector2 origin, Layer layer)
        {
            buffer.Push(new SpriteData()
            {
                texture = texture.Texture,
                position = position,
                rotation = rotation,
                scale = scale,
                color = color,
                origin = origin,
                layer = layer
            });
        }

        public static void DrawLine(Vector2 pos1, Vector2 pos2)
        {
            DrawLine(pos1, pos2, Color.White);
        }

        public static void DrawLine(Vector2 pos1, Vector2 pos2, Color color)
        {
            DrawLine(pos1, pos2, color, Layer.Game);
        }

        public static void DrawLine(Vector2 pos1, Vector2 pos2, Color color, Layer layer)
        {
            buffer.Push(new LineData()
            {
                texture = pixel,
                color = color,
                positionOne = pos1,
                positionTwo = pos2,
                layer = layer
            });
        }


        public static void RenderRect(Rectangle rect)
        {
            RenderRect(rect, Color.White);
        }

        public static void RenderRect(Rectangle rect, Color color)
        {
            RenderRect(rect, color, Layer.Game);
        }

        public static void RenderRect(Rectangle rect, Color color, Layer layer)
        {
            buffer.Push(new RectData()
            {
                texture = pixel,
                color = color,
                rectangle = rect,
                layer = layer
            });
        }

        public static void DrawCircle(Vector2 center, float radius, Color color)
        {
           DrawCircle(center, radius, color, Layer.Game);
        }

        public static void DrawCircle(Vector2 center, float radius, Color color, Layer layer)
        {
            buffer.Push(new CircleData()
            {
                texture = circle,
                color = color,
                center = center,
                radius = radius,
                layer = layer
            });
        }

        public static void DrawWireframe(Vector2[] verticies)
        {
            for (int i = 0; i < verticies.Length; i++)
            {
                DrawLine(verticies[i], verticies[(i + 1) % verticies.Length], Color.White);
            }
        }

        public static void DrawWireframe(Vector2[] verticies, Color color)
        {
            for (int i = 0; i < verticies.Length; i++)
            {
                DrawLine(verticies[i], verticies[(i + 1) % verticies.Length], color, Layer.Game);
            }
        }

        public static void DrawWireframe(Vector2[] verticies, Color color, Layer layer)
        {
            for (int i = 0; i < verticies.Length; i++)
            {
                DrawLine(verticies[i], verticies[(i + 1) % verticies.Length], color, layer);
            }
        }

        public static void DrawWireRect(Rectangle rect) => DrawWireRect(rect, Color.White);
        public static void DrawWireRect(Rectangle rect, Color color) => DrawWireRect(rect, color, Layer.Game);

        public static void DrawWireRect(Rectangle rect, Color color, Layer layer)
        {
            Vector2[] verticies = new Vector2[]
            {
                new Vector2(rect.Left, rect.Top),
                new Vector2(rect.Right, rect.Top),
                new Vector2(rect.Right, rect.Bottom),
                new Vector2(rect.Left, rect.Bottom),
            };

            DrawWireframe(verticies, color, layer);
        }

        public static void DrawText(SpriteFont font, string text)
        {
            DrawText(font, text, Vector2.Zero);
        }

        public static void DrawText(SpriteFont font, string text, Vector2 position)
        {
            DrawText(font, text, position, Color.White);
        }

        public static void DrawText(SpriteFont font, string text, Vector2 position, Color color)
        {
            DrawText(font, text, position, color, Layer.Game);
        }

        public static void DrawText(SpriteFont font, string text, Vector2 position, Color color, Layer layer)
        {
            buffer.Push(new TextData
            {
                color = color,
                Text = text,
                Font = font,
                layer = layer,
                Position = position
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

                switch (data.layer)
                {
                    case Layer.Game:
                        data.Draw(spriteBatch);
                        break;

                    case Layer.UI:
                        data.Draw(uiBatch);
                        break;
                }
            }
        }
    }

    public interface IBufferData
    {
        Color color { get; set; }

        Layer layer { get; set; }

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

        public Layer layer { get; set; }

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

        public Layer layer { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (positionOne.X == positionTwo.X)
                spriteBatch.Draw(texture, new Rectangle((int)positionOne.X, (int)positionOne.Y, 1, (int)(positionTwo.Y - positionOne.Y)), null, color, 0, Vector2.Zero, SpriteEffects.None, 0);
            else if(positionOne.Y == positionTwo.Y)
                spriteBatch.Draw(texture, new Rectangle((int)positionOne.X, (int)positionOne.Y, (int)(positionTwo.X - positionOne.X), 1), null, color, 0, Vector2.Zero, SpriteEffects.None, 0);
            else
            {
                int length = (int)Math.Sqrt(Math.Pow(positionTwo.Y - positionOne.Y, 2) + Math.Pow(positionTwo.X - positionOne.X, 2));
                Rectangle line = new Rectangle((int)positionOne.X, (int)positionOne.Y, length, 1);
                float angle = (float)(Math.Atan2(positionTwo.Y - positionOne.Y, positionTwo.X - positionOne.X));

                spriteBatch.Draw(texture, line, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
            }

        }
    }

    public struct RectData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }
        public Rectangle rectangle;

        public Layer layer { get; set; }

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, rectangle, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
    }

    public struct CircleData : IBufferData
    {
        public Texture2D texture { get; set; }
        public Color color { get; set; }

        public Vector2 center;

        public float radius;
        public Layer layer { get; set; }

        public void Draw(SpriteBatch spriteBatch) => spriteBatch.Draw(texture, center, null, color, 0f, new Vector2(texture.Width/2, texture.Height/2), radius/1000, SpriteEffects.None, 0);
    }

    public struct TextData : IBufferData
    {
        public Color color { get; set; }
        public Layer layer { get; set; }

        public string Text;
        public SpriteFont Font;

        public Vector2 Position;
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, color);
        }
    }

    public struct OnEngineDrawEvent : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }

    public enum Layer
    {
        Game, UI
    }
}
