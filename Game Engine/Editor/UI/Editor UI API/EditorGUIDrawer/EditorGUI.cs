using GameEngine.Debugging;
using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameEngine.Editor.Windows
{
    [Note(note = "Split Class Into formatter and drawer? // cache frequent classes like Thickness? || draw methods and get methods (returns without drawing)?")]
    public partial class EditorGUIDrawer
    {
        private EditorWindow window;
        
        private StackPanel defaultContainer;
        private Stack<StackPanel> containerStack = new Stack<StackPanel>();

        public ContextManager contextMenuManager;

        const int indentMultiplyer = 10;
        int indentLevel;

        public EditorGUIDrawer(EditorWindow window, StackPanel _content) 
        {
            defaultContainer = _content;
            containerStack.Push(defaultContainer);

            contextMenuManager = new ContextManager(window);

            this.window = window;
            window.AllowDrop = true;
        }

        public void Indent() => indentLevel++;
        public void Outdent() => indentLevel--;
       
        public StackPanel StartHorizontalGroup()
        {
            return StartHorizontalGroup(new ElementStyle().OverrideDynamicSize(true));
        }

        public StackPanel StartHorizontalGroup(ElementStyle style)
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Horizontal,

                Background = style.Background,
                Margin = style.Margin,
            };

            Render(container);
            containerStack.Push(container);

            return container;
        }

        public StackPanel StartVerticalGroup()
        {
            return StartVerticalGroup(new ElementStyle().OverrideDynamicSize(true));
        }

        public StackPanel StartVerticalGroup(ElementStyle style) 
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Vertical,

                Background = style.Background,
                Margin = style.Margin,
            };

            Render(container);
            containerStack.Push(container);

            return container;
        }

        public ContextManager AddContextItem(string title, RoutedEventHandler onClick)
        {
            contextMenuManager.AddOption(title, onClick);
            Console.WriteLine(title);
            return contextMenuManager;
        }

        public void EndGroup() => containerStack.Pop();

        public void Clear()
        {
            defaultContainer.Children.Clear();
        }

        public StackPanel GetContainer()
        {
            return defaultContainer;
        }

        void ApplyStyle(FrameworkElement element, ElementStyle style)
        {
            MemberInfo[] styleFields = style.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            MemberInfo[] elementFields = element.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string[] commonStyles = FindCommonStyles(styleFields, elementFields);
        
            foreach(var o in commonStyles)
            {
                if (style.DynamicSize && (o == "Width" || o == "Height"))
                   continue;

                var styleData = style.GetType().GetField(o).GetValue(style);
                element.GetType().GetProperty(o).SetValue(element, styleData);
            }

            if(indentLevel > 0)
            {
                var indent = (indentLevel * indentMultiplyer);
                element.Margin = new Thickness(element.Margin.Left + indent, element.Margin.Top, element.Margin.Right, element.Margin.Bottom);
            }
        }

        string[] FindCommonStyles(MemberInfo[] fields1, MemberInfo[] fields2)
        {
            List<string> intersecton = new List<string>();
            HashSet<string> H = new HashSet<string>();

            foreach (var o in fields1)
                H.Add(o.Name);

            foreach(var o in fields2)
            {
                if (H.Contains(o.Name))
                    intersecton.Add(o.Name);
            }

            return intersecton.ToArray();
        }

        void Render(FrameworkElement element)
        {
            containerStack.Peek().Children.Add(element);
        }
    }

    public class ContextManager
    {
        ContextMenu menu = new ContextMenu();
        FrameworkElement element;

        public ContextManager(FrameworkElement element)
        {
            this.element = element;
            element.ContextMenu = menu;
        }

        public ContextManager AddOption(string Title, RoutedEventHandler OnClick)
        {
            MenuItem menuItem = new MenuItem()
            {
                Background = ElementStyle.AccentBackgroundColor,
            };

            menuItem.Header = Title;
            menuItem.Click += OnClick;

            element.ContextMenu.Items.Add(menuItem);
            Console.WriteLine("added " + Title);

            return this;
        }
    }

    public delegate void ButtonClick(object sender, RoutedEventArgs e); 
}
