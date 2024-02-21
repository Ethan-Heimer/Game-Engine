using GameEngine.Engine.ComponentModel;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Editor
{
    [ContainsEvents]
    public static class EditorEventManager
    {
        static EngineEvent<OnObjectSelected> onGameObjectSelected;

        static EngineEvent<OnObjectCaptured> onGameObjectCaptured;
        static EngineEvent<WhileObjectCaptured> whileGameObjectCaptured;
        static EngineEvent<OnObjectReleased> onGameObjectReleased;

        static OnObjectSelected onObjectSelectedData = new OnObjectSelected();

        static OnObjectCaptured onObjectCapuredData = new OnObjectCaptured();
        static WhileObjectCaptured whileObjectCapuredData = new WhileObjectCaptured();
        static OnObjectReleased onObjectReleasedData = new OnObjectReleased();

        public static GameObject CapturedGameObject { get; private set; }

        static GameObject _gameObject;
        public static GameObject SelectedObject
        {
            get { return _gameObject; }
            set { 
                _gameObject = value;

                onObjectSelectedData.SelectedObject = value;
                onGameObjectSelected?.Invoke(onObjectSelectedData);
            }
        }

        public static void CaputureGameObject(GameObject go)
        {
            CapturedGameObject = go;

            onObjectCapuredData.CapturedObject = go;
            onGameObjectCaptured?.Invoke(onObjectCapuredData);
        }


        public static void DragCaptured()
        {
            if (CapturedGameObject == null)
                return;

            whileObjectCapuredData.CapturedObject = CapturedGameObject;
            whileGameObjectCaptured?.Invoke(whileObjectCapuredData);
        }

        public static void ReleaseCapture()
        {
            if (CapturedGameObject == null)
                return;

            onObjectReleasedData.ReleasedObject = CapturedGameObject;
            onGameObjectReleased?.Invoke(onObjectReleasedData);

            CapturedGameObject = null;
        }
    }

    public class OnObjectSelected : IEventArgs
    {
        public GameObject SelectedObject;
        public object Sender
        {
            get;
            set;
        }
    }

    public class OnObjectCaptured : IEventArgs
    {
        public GameObject CapturedObject;
        public object Sender
        {
            get;
            set;
        }
    }

    public class WhileObjectCaptured : IEventArgs
    {
        public GameObject CapturedObject;
        public object Sender
        {
            get;
            set;
        }
    }

    public class OnObjectReleased : IEventArgs
    {
        public GameObject ReleasedObject;
        public object Sender
        {
            get;
            set;
        }
    }

}
