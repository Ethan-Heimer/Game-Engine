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
using GameEngine.Pointers;
using GameEngine.Debugging;

namespace GameEngine
{
    [Serializable]
    [Note(note ="IPointerManipulatable might wanna be seperated into multiple interfaced when starting on capture system")]
    public class GameObject : ISerializable, IPointerManiplatable
    {
        public string Name = "New Game Object";

        public List<Component> components = new List<Component>();

        public GameObject parent;
        
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

        TextureRenderer _renderer;
        public TextureRenderer renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<TextureRenderer>();
                return _renderer;
            }
        }

        public GameObject() 
        {
            AddComponent<Transform>();
            GameObjectManager.RegisterGameobject(this);
        }

        public GameObject(SerializationInfo info, StreamingContext context)
        {
            try
            {
                components = (List<Component>)info.GetValue("Components", typeof(List<Component>));
                children = (List<GameObject>)info.GetValue("Children", typeof(List<GameObject>));
                parent = (GameObject)info.GetValue("Parent", typeof(GameObject));  
                Name = (string)info.GetValue("Name", typeof(string));
            }
            catch { }
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
            var component = Component.BindComponent(behavior, this);

            components.Add(component);
            return (T)behavior;
        }

        public T AddComponent<T>(Component component) where T : Behavior
        {
            Behavior behavior = component.BindingBehavior;

            components.Add(Component.BindComponent(behavior, this));
            return (T)behavior;
        }

        public Behavior AddComponent(Type type) 
        {
            if(type.BaseType != typeof(Behavior))
                return null;

            var behavior = (Behavior)Activator.CreateInstance(type);
            components.Add(Component.BindComponent(behavior, this));

            return behavior;
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

        public void AddChild(GameObject child)
        {
            child.parent = this;
            children.Add(child);

            GameObjectManager.AlertTreeChange(this);
        }

        public void RemoveChild(GameObject child)
        {
            child.parent = null;
            children.Remove(child);

            GameObjectManager.AlertTreeChange(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext content)
        {
            info.AddValue("Components", components);
            info.AddValue("Children", children);
            info.AddValue("Parent", parent);
            info.AddValue("Name", Name);
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


       

    }
}
