﻿
using Microsoft.Xna.Framework;
using GameEngine.Editor;
using GameEngine.Game;
using System;
using GameEngine.ComponentManagement;
using GameEngine.Engine;
using GameEngine.Engine.Rendering;
using GameEngine.Engine.Events;

namespace GameEngine
{
    [ContainsEvents]
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static event Action AfterInit;

        GraphicsDeviceManager graphics;
       
        static EngineEvent<OnEngineTickEvent> OnTick;
        OnEngineTickEvent OnEngineTickEvent = new OnEngineTickEvent();

        static EngineEvent<OnEngineDrawEvent> OnDraw;
        OnEngineDrawEvent OnEngineDrawEvent = new OnEngineDrawEvent();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            EngineEventManager.Init();
            ComponentCacheManager.Init();
            GameExecuter.Init();
            SceneManager.Init();
            InputManager.Init();
            Renderer.Init(this, graphics, GraphicsDevice);
            AssetManager.Init(Content);

            AfterInit.Invoke();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameObject gameObect = new GameObject();
            gameObect.Transform.Position = Vector2.One * 200;
            gameObect.AddComponent<TextureRenderer>().Path = "PlaceHolderTwo";
            gameObect.AddComponent<Move>().Speed = 9;
            gameObect.AddComponent<TestComponent>();
        }


        protected override void Update(GameTime gameTime)
        {
            OnEngineTickEvent.GameTime = gameTime;
            OnEngineTickEvent.Sender = this;

            OnTick?.Invoke(OnEngineTickEvent);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            OnEngineDrawEvent.Sender = this;
            OnDraw?.Invoke(OnEngineDrawEvent);
        }
    }

    public struct OnEngineTickEvent : IEventArgs
    {
        public GameTime GameTime;
        public object Sender
        {
            get; set;
        }
    }

    public struct OnEngineDrawEvent : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
}

