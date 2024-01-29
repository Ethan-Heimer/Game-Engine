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
using System.Runtime.InteropServices;

namespace GameEngine
{
    [Serializable]
    [Note(note ="IPointerManipulatable might wanna be seperated into multiple interfaced when starting on capture system")]
    public class GameObject : ISerializable, IPointerManiplatable, ICloneable
    {
        public string Name = "New Game Object";

        public List<Component> components = new List<Component>();

        public GameObject Parent;
        public Icon Icon;
        
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
            RegisterGameobject(false);

            AssetManager.GetIcon("Cube", out Icon);
        }

        public GameObject(string name)
        {
            AddComponent<Transform>();
            RegisterGameobject(false);

            AssetManager.GetIcon("Cube", out Icon);

            Name = name;
        }

        public GameObject(bool empty)
        {
            if (!empty)
            {
                AddComponent<Transform>();
            }

            RegisterGameobject(false);
            AssetManager.GetIcon("Cube", out Icon);
        }

        public GameObject(string name, bool empty)
        {
            if (!empty)
            {
                AddComponent<Transform>();
            }

            RegisterGameobject(false);
            AssetManager.GetIcon("Cube", out Icon);
            Name = name;
        }

        public GameObject(SerializationInfo info, StreamingContext context)
        {
            try
            {
                InitData(info, context);
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
            ComponentCacheManager.AddCache(behavior);

            return (T)behavior;
        }

        public Behavior AddComponent(Type type) 
        {
            if(type.BaseType != typeof(Behavior))
                return null;

            var behavior = (Behavior)Activator.CreateInstance(type);
            components.Add(Component.BindComponent(behavior, this));
            ComponentCacheManager.AddCache(behavior);

            return behavior;
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
            ComponentCacheManager.AddCache(component.BindingBehavior);
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
            ComponentCacheManager.RemoveCache(component.BindingBehavior);
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
            child.Parent = this;
            children.Add(child);

            GameObjectManager.AlertTreeChange(this);
        }

        public void RemoveChild(GameObject child)
        {
            child.Parent = null;
            children.Remove(child);

            GameObjectManager.AlertTreeChange(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext content)
        {
            info.AddValue("Components", components);
            info.AddValue("Children", children);
            info.AddValue("Parent", Parent);
            info.AddValue("Name", Name);
            info.AddValue("Icon", Icon);
            components.ForEach(x => Console.WriteLine(x + " on serialized"));
        }

        public void Destroy()
        {
            GameObjectManager.DispatchGameobject(this);
            Parent?.RemoveChild(this);

            while(children.Count > 0)
                children[0].Destroy();
        }

        public void ClearComponents()
        {
            foreach(Component o in components)
                ComponentCacheManager.RemoveCache(o.BindingBehavior);

            components.Clear();
        }


        public object Clone()
        {
            //GameObject clone = new GameObject(Name + " (Clone)", false);
            GameObject clone = new GameObject(Name + " (Clone)", true);
            
            foreach(Component o in components)
            {
                Component component = (Component)o.Clone();
                component.GameObject = clone;

                clone.AddComponent(component); 
            }

            foreach(GameObject o in children)
            {
                var child = (GameObject)o.Clone();
                clone.AddChild(child);
            }
            00

            return clone;
        }

        async void RegisterGameobject(bool yeild)
        {
            if(yeild)
                await Task.Yield();

            GameObjectManager.RegisterGameobject(this);
        }

        async void InitData(SerializationInfo info, StreamingContext content)
        {
            components = (List<Component>)info.GetValue("Components", typeof(List<Component>));
            children = (List<GameObject>)info.GetValue("Children", typeof(List<GameObject>));
            Parent = (GameObject)info.GetValue("Parent", typeof(GameObject));
            Icon = (Icon)info.GetValue("Icon", typeof(Icon));
            Name = (string)info.GetValue("Name", typeof(string));

            while (components[0] == null)
            {
                await Task.Yield(); 
            }

            RegisterGameobject(true);
        }

    }
}
