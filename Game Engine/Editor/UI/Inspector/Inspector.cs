using GameEngine.Editor.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.UI.Inspector
{
    public class Inspector
    {
        

        public Inspector(GameObject gameObject, EditorGUIDrawer drawer) 
        {
            InspectorUIFactory factory = new InspectorUIFactory();
            DataBinderFactory binderFactory = new DataBinderFactory();

            foreach (GameEngine.Component o in gameObject.GetAllComponents())
            {
                drawer.DrawText(o.BindingBehavior.GetType().Name);
                foreach (FieldInfo f in o.GetFields())
                {
                    UIComponent component;
                    if (factory.TryGetUI(f, out component))
                    {
                        Type binderType = binderFactory.TryGetBinder(f);
                        drawer.Draw(component, binderType, f, o.BindingBehavior);
                    }
                    Console.WriteLine(component + " Component");
                }
            }
        }

        void Update()
        {

        }
    }
}
