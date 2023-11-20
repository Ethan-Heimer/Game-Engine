using GameEngine.ComponentManagement;
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

namespace GameEngine.Engine.Rendering
{
    public static class Renderer
    {
        static GraphicsDevice graphicDevice;
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        static Drawer renderer;
        public static void Init(Game1 game, GraphicsDeviceManager _graphicsDeviceManager, GraphicsDevice _graphicDevice)
        {
            graphics = _graphicsDeviceManager;
            graphicDevice = _graphicDevice;
            
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1280, 700);
            Resolution.SetResolution(1920, 1080, false);

            spriteBatch = new SpriteBatch(_graphicDevice);

            renderer = new Drawer(spriteBatch);

            EngineEventManager.AddEventListener<OnEngineDrawEvent>((e) => Draw());
        }

        static void Draw()
        {
            graphicDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            BehaviorFunctionExecuter.Execute.OnDraw(renderer);
            spriteBatch.End();
        }
    }
}
