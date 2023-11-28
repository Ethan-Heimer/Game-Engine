using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameEngine.Editor.Windows
{
    public class EditorGUIDrawer : DynamicObject
    {
        private StackPanel defaultContainer;
        private Stack<StackPanel> containerStack = new Stack<StackPanel>();

        public EditorGUIDrawer(StackPanel _content) 
        {
            defaultContainer = _content;
            containerStack.Push(defaultContainer);
        }

        public void DrawLabel(string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text
            };

            textBlock.Style = (Style)Application.Current.Resources["Label"];
            containerStack.Peek().Children.Add(textBlock);
        }

        public void DrawText(string text)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text
            };

            textBlock.Style = (Style)Application.Current.Resources["Text"];
            containerStack.Peek().Children.Add(textBlock);
        }

        public void DrawButton(string text, ButtonClick onClick)
        {
            Button button = new Button()
            {
                Content = text
            };

            button.Style = (Style)Application.Current.Resources["Button"];
            button.Click += (s, e) => onClick.Invoke(s, e);

            containerStack.Peek().Children.Add(button);
        }

        public void DrawField(Action<string> onValueChanged)
        {
            TextBox textBox = new TextBox();
            textBox.TextChanged += (s, e) => { onValueChanged(textBox.Text); };

            containerStack.Peek().Children.Add(textBox);
        }

        public void DrawField(string defaultText, Action<string> onValueChanged)
        {
            TextBox textBox = new TextBox()
            {
                Text = defaultText
            };

            textBox.TextChanged += (s, e) => { onValueChanged(textBox.Text); };

            containerStack.Peek().Children.Add(textBox);
        }

        public void StartHorizontalGroup()
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };

            containerStack.Peek().Children.Add(container);
            containerStack.Push(container);
        }

        public void EndGroup() => containerStack.Pop();

        public void StartVerticalGroup() 
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Vertical
            };

            containerStack.Peek().Children.Add(container);
            containerStack.Push(container);
        }

        public void Draw<T>(params object[] args) where T : Component
        {
            T component = Activator.CreateInstance<T>();
            component.Draw(this, args);
        }

        public void Clear()
        {
            defaultContainer.Children.Clear();
        }
    }

    public delegate void ButtonClick(object sender, RoutedEventArgs e); 
}
