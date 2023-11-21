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

namespace GameEngine.Rendering
{
    [ContainsEvents]
    public static class Renderer
    {
        static GraphicsDevice graphicDevice;
        static GraphicsDeviceManager graphics;
        static SpriteBatch spriteBatch;
        static Drawer renderer;

        static EngineEvent<OnEngineDrawEvent> OnDraw;
        static OnEngineDrawEvent OnEngineDrawEvent = new OnEngineDrawEvent();

    public static void Init(Game1 game, GraphicsDeviceManager _graphicsDeviceManager, GraphicsDevice _graphicDevice)
        {
            graphics = _graphicsDeviceManager;
            graphicDevice = _graphicDevice;
            
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1280, 700);
            Resolution.SetResolution(1920, 1080, false);

            spriteBatch = new SpriteBatch(_graphicDevice);

            renderer = new Drawer(spriteBatch, graphicDevice);

            //EngineEventManager.AddEventListener<OnEngineDrawEvent>(e => Console.WriteLine("OnDraw"));
        }

        public static void Draw()
        {
            graphicDevice.Clear(new Color(55,55,55));

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, CameraManager.GetTransformantionMaxtrix());
            BehaviorFunctionExecuter.Execute.OnDraw(renderer);
            OnEngineDrawEvent.drawer = renderer;
            OnDraw?.Invoke(OnEngineDrawEvent);
            spriteBatch.End();

            
        }
    }

    public struct OnEngineDrawEvent : IEventArgs
    {
        public Drawer drawer;
        public object Sender
        {
            get; set;
        }
    }
}
