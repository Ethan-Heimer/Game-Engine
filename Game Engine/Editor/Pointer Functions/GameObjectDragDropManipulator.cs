using GameEngine.Pointer;
using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Pointer_Functions
{
    internal class GameObjectDragDropManipulator : IPointerManipulator<GameObject>
    {
        public void RegisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.RegisterCallback<TargetSelected<GameObject>>(CaptureGameobject);
            pointer.RegisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.RegisterCallback<TargetDeselected<GameObject>>(OnDrop);
        }

        public void UnregisterCallbacks(IPointer<GameObject> pointer)
        {
            pointer.UnregisterCallback<TargetSelected<GameObject>>(CaptureGameobject);
            pointer.UnregisterCallback<TargetDragged<GameObject>>(OnDrag);
            pointer.UnregisterCallback<TargetDeselected<GameObject>>(OnDrop);
        }

        void CaptureGameobject(TargetSelected<GameObject> e)
        {
            EditorEventManager.CaputureGameObject(e.Target);
        }

        void OnDrag(TargetDragged<GameObject> e)
        {
            EditorEventManager.DragCaptured();
        }

        void OnDrop(TargetDeselected<GameObject> e) 
        {
            EditorEventManager.ReleaseCapture();
        }
    }
}
