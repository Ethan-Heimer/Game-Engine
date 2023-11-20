using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Engine.Events
{
    /*
     * Why am i doing events like this? 
     * I Wanted a central place for events to be handled for cleaner code and design
     * sure, i could use static events, but that, i feel like, still couples code, which this system fixes.
     * Also, yeah it looks stragnge that im using field info and not event info but this is so i can manipulate the delegate.
     */

    public static class EngineEventManager
    {
        static Dictionary<Type, FieldInfo> events  = new Dictionary<Type, FieldInfo>();

        public static void Init()
        {
            List<Type> typesWithEvents = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttribute(typeof(ContainsEventsAttribute)) != null).ToList();

            foreach (Type type in typesWithEvents) 
            {
                var e = GetEvents(type); 

                foreach(var evnt in e)
                    AddEvent(evnt);
            }
        }

        public static void AddEventListener<TEventArgs>(EngineEvent<TEventArgs> method) where TEventArgs : IEventArgs 
        {
            FieldInfo e = events[typeof(TEventArgs)];
            Delegate d = (Delegate)e.GetValue(null);

            if (d != null)
            {
                var methods = d.GetInvocationList();
                EngineEvent<TEventArgs> newDelegate = null;

                foreach (var m in methods)
                    newDelegate += (EngineEvent<TEventArgs>)m;
                newDelegate += method;

                e.SetValue(null, newDelegate);
            }
            else
            {
                e.SetValue(null, method);
            }
        }

        public static void RemoveEventListener<TEventArgs>(EngineEvent<TEventArgs> method) where TEventArgs : IEventArgs
        {
            FieldInfo e = events[typeof(TEventArgs)];
            Delegate d = (Delegate)e.GetValue(null);

            if (d != null)
            {
                var methods = d.GetInvocationList();
                EngineEvent<TEventArgs> newDelegate = null;

                foreach (var m in methods)
                {
                    if (m.Method == method.Method)
                        continue;

                    newDelegate += (EngineEvent<TEventArgs>)m;
                }
            

                e.SetValue(null, newDelegate);
            }
        }

        static FieldInfo[] GetEvents(Type type)
        {
            return type.GetFields(BindingFlags.Static | BindingFlags.NonPublic).Where(x => x.FieldType.Name == typeof(EngineEvent<>).Name).ToArray();
        }

        static void AddEvent(FieldInfo eventInfo)
        {
            Type eventType = eventInfo.FieldType.GetGenericArguments()[0];

            events.Add(eventType, eventInfo);
        }
    }

    public class ContainsEventsAttribute: Attribute { }
   
    public delegate void EngineEvent<TEventArgs>(TEventArgs eventData) where TEventArgs : IEventArgs;
}
