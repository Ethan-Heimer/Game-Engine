using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.ComponentManagement;
using GameEngine.Engine.Events;

namespace GameEngine.Engine
{
    [ContainsEvents]
    public static class GameExecuter
    {
        static EngineEvent<OnPlayModeEnter> OnEnterPlayMode;
        static EngineEvent<OnPlayModeExit> OnExitPlayMode;

        static bool _play = true;
        public static bool play
        {
            get { return _play; }
            set
            {
                _play = value;

                if (!value)
                {
                    started = false;
                    OnExitPlayMode?.Invoke(new OnPlayModeExit());
                }
                else
                    OnEnterPlayMode?.Invoke(new OnPlayModeEnter());
            }
        }

        static bool started;



        public static void Init()
        {
            EngineEventManager.AddEventListener<SceneLoadedEvent>((e) => ExecuteStart());
            EngineEventManager.AddEventListener<OnEngineTickEvent>((e) => Tick());
        }

        public static void Tick()
        {
            if (!play) return;

            if(!started) 
            {
                BehaviorFunctionExecuter.Execute.Awake();
                BehaviorFunctionExecuter.Execute.Start();

                started = true;
            }

            BehaviorFunctionExecuter.Execute.Update();
        }

        static void ExecuteStart() => started = false;
    }

    public struct OnPlayModeExit : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }

    public struct OnPlayModeEnter : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
}
