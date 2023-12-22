using GameEngine.Debugging;
using GameEngine.Engine;
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
        private Stack<Panel> containerStack = new Stack<Panel>();

        public ContextManager contextMenuManager;

        public EditorGUIDrawer(EditorWindow window, StackPanel _content) 
        {
            defaultContainer = _content;
            containerStack.Push(defaultContainer);

            contextMenuManager = new ContextManager(window);

            this.window = window;
            window.AllowDrop = true;
        }
       
        public StackPanel StartHorizontalGroup()
        {
            return StartHorizontalGroup(ElementStyle.DefaultGroupStyle);
        }

        public StackPanel StartHorizontalGroup(ElementStyle style)
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
            };

            ElementStyle.ApplyStyle(container, style);
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
            };

            ElementStyle.ApplyStyle(container, style);
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

        public ContextManager AddContextItem(string title, string icon, RoutedEventHandler onClick)
        {
            contextMenuManager.AddOption(title, icon, onClick);
            Console.WriteLine(title);
            return contextMenuManager;
        }

        public void ClearContextMenu() => contextMenuManager.ClearMenu();

        public void EndGroup() => containerStack.Pop();

        public void Clear()
        {
            defaultContainer.Children.Clear();
        }

        public StackPanel GetContainer()
        {
            return defaultContainer;
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

        public void ClearMenu()
        {
            menu.Items.Clear();
        }

        public ContextManager AddOption(string Title, RoutedEventHandler OnClick)
        {
            AddOption(Title, "", OnClick);
            return this;
        }

        public ContextManager AddOption(string Title, string iconName, RoutedEventHandler OnClick)
        {
            MenuItem menuItem = new MenuItem()
            {
                Background = ElementStyle.AccentBackgroundColor,
            };

            Image image = new Image();
            
            if(AssetManager.GetIcon(iconName, out Icon icon))
            {
                image.Source = icon.GetImage();
                menuItem.Icon = image;
            }

            menuItem.Header = Title;
            menuItem.Click += OnClick;

            element.ContextMenu.Items.Add(menuItem);
            Console.WriteLine("added " + Title);

            return this;
        }
    }

    public delegate void ButtonClick(object sender, RoutedEventArgs e); 
}
