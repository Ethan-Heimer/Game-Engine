using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    [Serializable]
    public class Component : ISerializable, ICloneable
    {
        public Behavior BindingBehavior;
         
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
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            Type type = BindingBehavior.GetType();

            foreach(var o in type.GetFields()) 
            {
                Console.WriteLine(o.Name);
                if(o.FieldType.IsSerializable && o.FieldType != typeof(Component))
                    info.AddValue(o.Name, o.GetValue(BindingBehavior));
            }

            info.AddValue("Behavior Type", BindingBehavior.GetType().FullName);
        }

        public object Clone()
        {
            return new Component(BindingBehavior.Clone() as Behavior);
        }
    }
}
