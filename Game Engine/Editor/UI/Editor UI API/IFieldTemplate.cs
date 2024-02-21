using GameEngine.Debugging;
using GameEngine.Editor.Windows;
using GameEngine.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;

namespace GameEngine.Editor.UI.Inspector
{
    [Note(note = "Instead of tagging templates with interfaces, it might be better to use attributes")]
    public abstract class FieldTemplate<T> : IFieldTemplate
    {
        protected IFieldBinder<T> data;
        protected StackPanel template;

        protected T Value;
        protected object owner;

        EditorGUIDrawer drawer;

        bool hovered;

        protected ElementStyle templateStyle = new ElementStyle()
        {
            DynamicSize = true,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        public FieldTemplate(Type binderType, MemberValue field, object owner)
        {
            data = (IFieldBinder<T>)Activator.CreateInstance(binderType, new object[] { field, owner });
            Value = data.GetValue();

            this.owner = owner;
        }

        public void Display(EditorGUIDrawer drawer, params object[] args)
        {
            template = drawer.StartHorizontalGroup(templateStyle);
            Template(drawer, args);
            drawer.EndGroup();

            template.MouseEnter += (s, e) => OnMouseEnter(drawer);
            template.MouseLeave += (s, e) => OnMouseExit(drawer);

            template.MouseUp += (s, e) => OnMouseUp(drawer);

           

            this.drawer = drawer;

            OnValueChanged();
        }

        public void Update(EditorGUIDrawer drawer)
        {
            if (template.IsFocused)
            {
                WhileFocused(drawer);
                return;
            }

            OnUpdate(drawer);


            if (data.HasValueChange())
                OnValueChanged();
        }

        protected void EnableDragDrop(object data)
        {
            EnableDragDrop(template, data);
        }

        protected void EnableDragDrop(FrameworkElement source, object data)
        {
            source.IsHitTestVisible = true;

            source.AllowDrop = true;
            source.MouseMove += (s, e) => DragMouseMove(s, e, data);
            source.DragEnter += (s, e) =>  DragEnter(s, e, data.GetType());
            source.DragEnter += (s, e) => DragEnter(s, e, data.GetType());
            source.DragOver += (s, e) => DragOver(s, e, data.GetType());
            source.DragLeave += (s, e) => DragLeave(s, e, data.GetType());
            source.Drop += (s, e) => Drop(s, e, data.GetType());
        }

        void DragMouseMove(object sender, MouseEventArgs e, object data)
        {
            var element = sender as FrameworkElement;

            element.Focus();
            if (element != null && e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DragDrop.DoDragDrop(element, data, DragDropEffects.Move);
                }
                catch { }
            }
        }
        void DragEnter(object sender, DragEventArgs e, Type dataType)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                OnDragEnter(drawer, e.Data.GetData(dataType));
            }
        }
        void DragOver(object sender, DragEventArgs e, Type dataType)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                OnDragOver(drawer, e.Data.GetData(dataType));
            }
        }
        void DragLeave(object sender, DragEventArgs e, Type dataType)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                OnDragLeave(drawer, e.Data.GetData(dataType));
            }
        }
        void Drop(object sender, DragEventArgs e, Type dataType)
        {
            var element = sender as FrameworkElement;
            if (element != null)
            {
                OnDrop(drawer, e.Data.GetData(dataType));
            }
        }

        protected virtual void OnUpdate(EditorGUIDrawer drawer) { }
        protected virtual void OnValueChanged() { }
        protected virtual void WhileFocused(EditorGUIDrawer drawer) { }
        
        protected virtual void OnMouseEnter(EditorGUIDrawer drawer) 
        {
            hovered = true;
        }
        protected virtual void OnMouseExit(EditorGUIDrawer drawer) {}
        protected virtual void OnMouseClick(EditorGUIDrawer drawer) { }
        protected virtual void OnMouseUp(EditorGUIDrawer drawer) { }
        protected virtual void OnMouseHover(EditorGUIDrawer drawer) { }

        protected virtual void OnDragEnter(EditorGUIDrawer drawer, object data) { }
        protected virtual void OnDragOver(EditorGUIDrawer drawer, object data) { }
        protected virtual void OnDragLeave(EditorGUIDrawer drawer, object data) { }
        protected virtual void OnDrop(EditorGUIDrawer drawer, object data) { }
        
        protected abstract void Template(EditorGUIDrawer drawer, object[] args);

        protected T2 FindElementInTemplate<T2>(string name) where T2 : DependencyObject
        {
            return template.FindChild<T2>(name);
        }
    } 
    
    public interface IFieldTemplate
    {
        void Display(EditorGUIDrawer drawer, params object[] args);
        void Update(EditorGUIDrawer drawer);
    }

    public class InspectingFieldTemplateAttribute : UITemplateAttribute { }
    public class HiarchyFieldTemplateAttribute : UITemplateAttribute { }

    public class UITemplateAttribute : Attribute { }
}
