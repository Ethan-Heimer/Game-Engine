using GameEngine.Engine.Events;
using GameEngine.Pointer;
using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor
{
    [ContainsEvents]
    public class OnGameObjectSelectManipulator : IPointerManipulator<GameObject>
    {
        static EngineEvent<OnObjectSelectedEditorEvent> onGameObjectSelected;
        public void RegisterCallbacks(IPointer<GameObject> pointer) 
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(Invoke);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer) 
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(Invoke);
        }

        void Invoke(TargetSelected<GameObject> e)
        {
            onGameObjectSelected?.Invoke(new OnObjectSelectedEditorEvent
            {
                SelectedObject = e.Target,
                Sender = e.Sender
            });
        }
    }

    public class OnObjectSelectedEditorEvent : IEventArgs
    {
        public GameObject SelectedObject;
        public object Sender
        {
            get;
            set;
        }
    }
}
