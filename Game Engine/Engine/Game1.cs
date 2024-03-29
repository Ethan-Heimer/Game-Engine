﻿
using Microsoft.Xna.Framework;
using GameEngine.Editor;
using GameEngine.Game;
using System;
using GameEngine.ComponentManagement;
using GameEngine.Engine;
using GameEngine.Engine.Events;
using GameEngine.Rendering;
using GameEngine.Debugging;
using GameEngine.Engine.Physics;
using GameEngine.Engine.Rendering;
using GameEngine.Engine.Settings;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Documents;
using System.Threading;
using System.Collections.Generic;
using GameEngine.Engine.Components.UI;
using GameEngine.Engine.Utilities;

namespace GameEngine
{
    [ContainsEvents]
    [Note(note = "Theres a lot of init functions. Maybe a system to call static init functions might be better?")]
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static event Action AfterInit;

        GraphicsDeviceManager graphics;
       
        static EngineEvent<OnEngineTickEvent> OnTick;
        OnEngineTickEvent OnEngineTickEvent = new OnEngineTickEvent();

        static EngineEvent<OnEngineExitEvent> OnExit;
        OnEngineExitEvent OnEngineExitEvent = new OnEngineExitEvent();

       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            List<Thread> threads = new List<Thread>();

            EngineEventManager.Init();
            ComponentCacheManager.Init();
            EngineSettings.Init();

            Thread loadThread = new Thread(() =>
            {
                SceneManager.Init();
            });

            Thread renderThread = new Thread(() =>
            {
                Renderer.Init(this, graphics, GraphicsDevice);
                CameraManager.Init();
            });

            renderThread.Start();
            loadThread.Start();

            renderThread.Join();
            loadThread.Join();

            AssetManager.Init(Content);
            GameExecuter.Init();
            InputManager.Init();
            PlayModeManager.Init();
            TempFileHandler.Init();
            NotesManager.Init();
            PhysicsSystem.Init(.1f);
            GameWindowManager.Init(this.Window);

          
            
            this.IsMouseVisible = true;
            AfterInit?.Invoke();

            base.Initialize();
            Console.WriteLine(typeof(Panel).GetRootType());
        }

        protected override void LoadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            OnEngineTickEvent.GameTime = gameTime;
            OnEngineTickEvent.Sender = this;

            OnTick?.Invoke(OnEngineTickEvent);

        }
        
        protected override void Draw(GameTime gameTime)
        {
            Renderer.Draw();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            OnExit?.Invoke(OnEngineExitEvent);   
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

    public struct OnEngineExitEvent : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
}

