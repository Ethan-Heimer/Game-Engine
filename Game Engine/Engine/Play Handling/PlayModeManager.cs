using GameEngine.Engine.Events;
using GameEngine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Engine
{
    [ContainsEvents]
    public static class PlayModeManager
    {
        static EngineEvent<WhileInEditMode> WhileEditMode; 
        static WhileInEditMode editModeArgs = new WhileInEditMode();

        static EngineEvent<OnEnterEditMode> EnterEditMode;
        static OnEnterEditMode enterEditModeArgs = new OnEnterEditMode();

        static EngineEvent<WhileInPlayMode> WhilePlayMode; 
        static WhileInPlayMode playModeArgs = new WhileInPlayMode();

        static EngineEvent<OnEnterPlayMode> EnterPlayMode;
        static OnEnterPlayMode enterPlayModeArgs = new OnEnterPlayMode();

        static EngineEvent<WhileInPauseMode> WhilePauseMode; 
        static WhileInPauseMode pauseModeArgs = new WhileInPauseMode();

        static EngineEvent<OnEnterPauseMode> EnterPauseMode;
        static OnEnterPauseMode enterPauseModeArgs = new OnEnterPauseMode();

        public enum PlayMode
        {
            Play,
            Edit,
            Pause,
        }
        public static PlayMode State
        {
            get;
            private set;
        }

        public static void Init()
        {
            EngineEventManager.AddEventListener<OnEngineTickEvent>(e => Tick()); 
        }

        public static void SetMode(PlayMode state)
        {
            switch (state)
            {
                case PlayMode.Edit:
                    EnterEditMode?.Invoke(enterEditModeArgs);
                    break;
                case PlayMode.Pause:
                    EnterPauseMode?.Invoke(enterPauseModeArgs);
                    break;
                case PlayMode.Play:
                    EnterPlayMode?.Invoke(enterPlayModeArgs);
                    break;
            }

            State = state;
        }

        static void Tick()
        {
            switch(State) 
            {
                case PlayMode.Edit:
                    WhileEditMode?.Invoke(editModeArgs);
                    break;
                case PlayMode.Pause:
                    WhilePauseMode?.Invoke(pauseModeArgs);
                    break;
                case PlayMode.Play:
                    WhilePlayMode?.Invoke(playModeArgs);
                    break;
            }
        }
    }

    public struct OnEnterPlayMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
    public struct WhileInPlayMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
    public struct OnEnterEditMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
    public struct WhileInEditMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
    public struct OnEnterPauseMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
    public struct WhileInPauseMode : IEventArgs
    {
        public object Sender
        {
            get; set;
        }
    }
}
