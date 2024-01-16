﻿using System;
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
        static bool started;

        public static void Init()
        {
            EngineEventManager.AddEventListener<SceneLoadedEvent>((e) => ResetStart());

            EngineEventManager.AddEventListener<WhileInPlayMode>(e => Tick());
            EngineEventManager.AddEventListener<OnEnterEditMode>(e => ResetStart());
            EngineEventManager.AddEventListener<OnEnterEditMode>(e => ExecuteDisable());

            EngineEventManager.AddEventListener<WhileInEditMode>(e => ExecutePersistantUpdate());
        }

        public static void Tick()
        {
            //Console.WriteLine("Tick");

            if(!started) 
            {
                BehaviorFunctionExecuter.Execute.Awake();
                BehaviorFunctionExecuter.Execute.Start();

                started = true;
            }

            BehaviorFunctionExecuter.Execute.Update();
        }

        static void ExecuteDisable() => BehaviorFunctionExecuter.Execute.OnDisable();

        static void ExecutePersistantUpdate() => BehaviorFunctionExecuter.Execute.UpdateInEditor();

        static void ResetStart() => started = false;
    }
}
