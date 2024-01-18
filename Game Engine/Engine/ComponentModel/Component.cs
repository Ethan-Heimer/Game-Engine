using GameEngine.ComponentManagement;
using GameEngine.Engine.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    [Serializable]
    public class Component : ISerializable, ICloneable
    {
        public Behavior BindingBehavior;

        GameObject _gameObject;
        public GameObject GameObject
        {
            get { return _gameObject; }
            set
            {
                _gameObject = value;
                BindingBehavior.gameObject = value;
            }
        }
         
        public Component(Behavior behavior) 
        { 
            BindingBehavior = behavior;
        }

        public Component(SerializationInfo info, StreamingContext context) 
        {
            Type type = Type.GetType(info.GetString("Behavior Type"));
            BindingBehavior = (Behavior)Activator.CreateInstance(type);

            foreach (var o in type.GetFields())
            {
                if (!o.FieldType.IsSerializable || o.FieldType == typeof(Component))
                    continue;
                try
                {
                    object obj = info.GetValue(o.Name, o.FieldType);
                    if (obj != null)
                    {
                        o.SetValue(BindingBehavior, obj);
                    }
                }
                catch { }
                
            }

            GameObject = BindingBehavior.gameObject;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] fields = GetFields(); 

            foreach(var o in fields) 
            {
                 info.AddValue(o.Name, o.GetValue(BindingBehavior));
            }

            info.AddValue("Behavior Type", BindingBehavior.GetType().FullName);
        }

        public FieldInfo[] GetFields()
        {
            return BindingBehavior.GetType().GetFields().Where(x => x.FieldType.IsSerializable && x.FieldType != typeof(Component)).ToArray();
        }

        public object Clone()
        {
            Behavior behavior = (Behavior)BindingBehavior.Clone();
            Component clone = new Component(behavior);
            clone.GameObject = GameObject;

            return clone;

        }

        public static Component BindComponent(Behavior behavior, GameObject obj)
        {
            FieldInfo gameObjectField = behavior.GetType().GetField("gameObject");
            gameObjectField.SetValue(behavior, obj);

            Component component = new Component(behavior);
            component.GameObject = obj;

            return component;
        }
    }
}
