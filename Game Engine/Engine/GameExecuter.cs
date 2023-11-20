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
    public static class GameExecuter
    {
        static bool _play = true;
        public static bool play
        {
            get { return _play; }
            set
            {
                _play = value;

                if (!value)
                    started = false;
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
}
