using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GameEngine.Editor;
using GameEngine.Editor.Windows;
using GameEngine.Engine.Events;
using GameEngine.Engine;
using GameEngine.ComponentManagement;
using System.Windows.Forms;

namespace GameEngine
{
    [Serializable]
    public class GameObject : ISerializable
    {
        public string Name;

        public List<Component> components = new List<Component>();
        List<GameObject> children = new List<GameObject>();

        Transform _transform;
        public Transform Transform
        {
            get 
            {
                if(_transform == null)
                    _transform = GetComponent<Transform>();
                return _transform;
            }
        }

        public GameObject() 
        {
            AddComponent<Transform>();
            GameObjectManager.RegisterGameobject(this);
        }

        public GameObject(SerializationInfo info, StreamingContext context)
        {
            components = (List<Component>)info.GetValue("Components", typeof(List<Component>));
            children = (List<GameObject>)info.GetValue("Children", typeof(List<GameObject>));
        }

        public T GetComponent<T>() where T : Behavior
        {
            foreach (var o in components) 
            {
                if(o.BindingBehavior.GetType() == typeof(T))
                    return o.BindingBehavior as T;
            }
           
            return null;
        }

        public T AddComponent<T>() where T : Behavior
        {
            Behavior behavior = (Behavior)Activator.CreateInstance(typeof(T));
            var component = BindComponent(behavior);

            components.Add(component);
            return (T)behavior;
        }

        public T AddComponent<T>(Component component) where T : Behavior
        {
            Behavior behavior = component.BindingBehavior;

            components.Add(BindComponent(behavior));
            return (T)behavior;
        }

        public void RemoveComponent<T>() where T : Behavior
        {
            Component component = components.First(x => x.BindingBehavior.GetType() == typeof(T));

            if(component != null)
                components.Remove(component);

            ComponentCacheManager.RemoveCache(component.BindingBehavior);
        }

        public Component[] GetAllComponents() => components.ToArray();

        public GameObject[] GetChildren() => children.ToArray();
        
        public void AddChild(GameObject child)  => children.Add(child);

        public void RemoveChild(GameObject child) => children.Remove(child);

        public void GetObjectData(SerializationInfo info, StreamingContext content)
        {
            info.AddValue("Components", components);
            info.AddValue("Children", children);
            components.ForEach(x => Console.WriteLine(x + " on serialized"));
        }

        public void Destroy()
        {
            GameObjectManager.DispatchGameobject(this);
        }

        public void ClearComponents()
        {
            foreach(Component o in components)
                ComponentCacheManager.RemoveCache(o.BindingBehavior);

            components.Clear();
        }

        Component BindComponent(Behavior behavior)
        {
            FieldInfo gameObjectField = behavior.GetType().GetField("gameObject");
            gameObjectField.SetValue(behavior, this);

            Component component = new Component(behavior);
            component.GameObject = this;

            return component;
        }

    }
}
