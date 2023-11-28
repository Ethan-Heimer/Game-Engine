using GameEngine.Engine.Events;
using GameEngine.Pointer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameEngine.Pointers
{
    public class Pointer<T> : IPointer<T> where T : IPointerManiplatable 
    {
        protected PointerEvent<TargetSelected<T>> OnTargetSelected;
        protected PointerEvent<TargetDeselected<T>> OnTargetDeselected;
        protected PointerEvent<TargetDragged<T>> OnDragged;
        protected PointerEvent<PointerUpdated<T>> OnUpdate;

        public T Target
        {
            get; set;
        }

        public virtual void Update() 
        {
            OnUpdate?.Invoke(new PointerUpdated<T> { Target = Target, pointerPosition = InputManager.MousePositionCamera() });
        }

        public Pointer<T> AddManipulator(IPointerManipulator<T> pointerManipulator)
        {
            pointerManipulator.RegisterCallbacks(this);
            return this;
        }

        public Pointer<T> RemoveManipulator(IPointerManipulator<T> pointerManipulator)
        {
            pointerManipulator.UnregisterCallbacks(this);
            return this;
        }

        public void RegisterCallback<T2>(PointerEvent<T2> callback) where T2 : IPointerEvent
        {
            FieldInfo e = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => x.FieldType.IsGenericType && x.FieldType.GetGenericArguments()?[0] == typeof(T2));
            Delegate d = (Delegate)e.GetValue(this);

            if (d != null)
            {
                var methods = d.GetInvocationList();
                PointerEvent<T2> newDelegate = null;

                foreach (var m in methods)
                    newDelegate += (PointerEvent<T2>)m;
                newDelegate += callback;

                e.SetValue(this, newDelegate);
            }
            else
            {
                e.SetValue(this, callback);
            }
        }

        public void UnregisterCallback<T2>(PointerEvent<T2> callback) where T2 : IPointerEvent
        {
            FieldInfo e = GetType().GetFields().FirstOrDefault(x => x.FieldType.GetGenericArguments()[0] == typeof(T2));
            Delegate d = (Delegate)e.GetValue(this);

            if (d != null)
            {
                var methods = d.GetInvocationList();
                PointerEvent<T2> newDelegate = null;

                foreach (var m in methods)
                {
                    if (m.Method == callback.Method)
                        continue;

                    newDelegate += (PointerEvent<T2>)m;
                }


                e.SetValue(this, newDelegate);
            }
        }
    }

    public delegate void PointerEvent<T>(T args) where T : IPointerEvent;

    public interface IPointerEvent : IEventArgs { }
    public interface IPointerEvent<T> : IPointerEvent
    {
        T Target { get; }
    }

    public class TargetSelected<T> : IPointerEvent<T>
    {
        public T Target
        {
            get; set;
        }

        public object Sender
        {
            get; set;
        }
    }

    public class TargetDeselected<T> : IPointerEvent<T>
    {
        public T Target
        {
            get; set;
        }

        public object Sender
        {
            get; set;
        }
    }

    public class TargetDragged<T> : IPointerEvent<T>
    {
        public T Target
        {
            get; set;
        }

        public Vector2 dragDelta
        {
            get; set;
        }

        public Vector2 pointerPosition
        {
            get; set;
        }

        public object Sender
        {
            get; set;
        }
    }

    public class PointerUpdated<T> : IPointerEvent<T>
    {
        public T Target
        {
            get; set;
        }

        public Vector2 pointerPosition
        {
            get; set;
        }

        public object Sender
        {
            get; set;
        }
    }
}
