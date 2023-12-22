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
       
        public void RegisterCallbacks(IPointer<GameObject> pointer) 
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(Select);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer) 
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(Select);
        }

        void Select(TargetSelected<GameObject> e)
        {
            EditorEventManager.SelectedObject = e.Target;
        }
    }

    
}
