using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor
{
    [ContainsEvents]
    public static class EditorEventManager
    {
        static EngineEvent<OnObjectSelected> onGameObjectSelected;
        static OnObjectSelected onObjectSelectedData = new OnObjectSelected();

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
}
