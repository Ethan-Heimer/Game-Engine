using GameEngine.Editor.Windows;
using GameEngine.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;

namespace GameEngine.Editor.UI.Inspector
{
    public abstract class FieldTemplate<T> : IFieldTemplate
    {
        protected IFieldBinder<T> data;
        protected FrameworkElement ui;

        public FieldTemplate(Type binderType, FieldInfo field, object owner)
        {
            data = (IFieldBinder<T>)Activator.CreateInstance(binderType, new object[] { field, owner });
        }

        
        public void Display(EditorGUIDrawer drawer)
        {
            ui = Template(drawer);
        }

        public void Update(EditorGUIDrawer drawer)
        {
            if (ui.IsFocused)
                return;

            OnUpdate(drawer);

            if (data.HasValueChange())
                OnValueChanged(drawer);
        }

        protected virtual void OnUpdate(EditorGUIDrawer drawer) { }
        protected virtual void OnValueChanged(EditorGUIDrawer drawer) { }
        protected virtual void WhileFocused(EditorGUIDrawer drawer) { }
        
        protected abstract FrameworkElement Template(EditorGUIDrawer drawer);

        protected T2 FindElementInTemplate<T2>(string name) where T2 : DependencyObject
        {
            return UIHelper.FindChild<T2>(ui, name);
        }
    } 
    
    public interface IFieldTemplate
    {
        void Display(EditorGUIDrawer drawer);
        void Update(EditorGUIDrawer drawer);
    }
}
