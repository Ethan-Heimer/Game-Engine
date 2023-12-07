using GameEngine.Debugging;
using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameEngine.Editor.Windows
{
    [Note(note = "UI will be turbo fucked, default style sizes can be set in json file and changed by engine settings")]
    public class EditorGUIDrawer : DynamicObject
    {
        private EditorWindow window;
        
        private StackPanel defaultContainer;
        private Stack<StackPanel> containerStack = new Stack<StackPanel>();

        public EditorGUIDrawer(EditorWindow window, StackPanel _content) 
        {
            defaultContainer = _content;
            containerStack.Push(defaultContainer);

            this.window = window;
        }

        public TextBlock DrawText(string text)
        {
            return DrawText(text, "", new ElementStyle());
        }

        public TextBlock DrawText(string text, string tag)
        {
            return DrawText(text, tag, new ElementStyle());
        }

        public TextBlock DrawText(string text, ElementStyle style)
        {
            return DrawText(text, "", style);
        }
        public TextBlock DrawText(string text, string tag, ElementStyle style)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                Name = tag,
                FontSize = style.FontSize,
                Foreground = style.FontColor,
                Background = style.BackGroundColor,
                Padding = style.Padding,
                Margin = style.Margin,
            };

            containerStack.Peek().Children.Add(textBlock);

            return textBlock;
        }

        public Button DrawButton(string text, ButtonClick onClick)
        {
            return DrawButton(text, onClick, "", new ElementStyle());
        }
        
        public Button DrawButton(string text, string tag, ButtonClick onClick)
        {
            return DrawButton(text, onClick, tag, new ElementStyle());
        } 
        
        public Button DrawButton(string text, ButtonClick onClick, ElementStyle style)
        {
            return DrawButton(text, onClick, "", style);
        }

        public Button DrawButton(string text, ButtonClick onClick, string tag, ElementStyle style)
        {
            Button button = new Button()
            {
                Content = text,

                Name = tag,
                FontSize = style.FontSize,
                Foreground = style.FontColor,
                Background = style.BackGroundColor,
                BorderBrush = style.BorderColor,
                Padding = style.Padding,
                Margin = style.Margin,
                Width = style.Width,
                Height = style.Height
            };

            button.Click += (s, e) => onClick.Invoke(s, e);

            containerStack.Peek().Children.Add(button);

            return button;
        }

        public TextBox DrawField(Action<string> onValueChanged)
        {
            return DrawField("", onValueChanged, "", new ElementStyle());
        }

        public TextBox DrawField(string defaultText, Action<string> onValueChanged)
        {
           return DrawField(defaultText, onValueChanged, "", new ElementStyle());
        }

        public TextBox DrawField(Action<string> onValueChanged, ElementStyle style)
        {
            return DrawField("", onValueChanged, "",  style);
        }

        public TextBox DrawField(Action<string> onValueChanged, string tag, ElementStyle style)
        {
            return DrawField("", onValueChanged, tag,  style);
        }

        public TextBox DrawField(string defaultText, Action<string> onValueChanged, string tag,  ElementStyle style)
        {
            TextBox textBox = new TextBox()
            {
                Text = defaultText,

                Name = tag,
                FontSize = style.FontSize,
                Foreground = style.FontColor,
                Background = style.BackGroundColor,
                BorderBrush = style.BorderColor,
                Padding = style.Padding,
                Margin = style.Margin,
                Width = style.Width,
                Height = style.Height,
            };

            textBox.TextChanged += (s, e) => { onValueChanged(textBox.Text); };

            containerStack.Peek().Children.Add(textBox);

            return textBox;
        }

        public ContextMenu DrawContextMenu()
        {
            ContextMenu menu = new ContextMenu();

            var test = new MenuItem()
            {
                Header = "Test",
            };

            menu.Items.Add(test);

            containerStack.Peek().ContextMenu = menu;

            return menu;
        }

        public StackPanel StartHorizontalGroup()
        {
            return StartHorizontalGroup(new ElementStyle());
        }

        public StackPanel StartHorizontalGroup(ElementStyle style)
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Horizontal,

                Background = style.BackGroundColor,
                Margin = style.Margin,
            };

            containerStack.Peek().Children.Add(container);
            containerStack.Push(container);

            return container;
        }

        public StackPanel StartVerticalGroup()
        {
            return StartVerticalGroup(new ElementStyle());
        }

        public StackPanel StartVerticalGroup(ElementStyle style) 
        {
            StackPanel container = new StackPanel()
            {
                Orientation = Orientation.Vertical,

                Background = style.BackGroundColor,
                Margin = style.Margin,
            };

            containerStack.Peek().Children.Add(container);
            containerStack.Push(container);

            return container;
        }

        public void EndGroup() => containerStack.Pop();

        //the component can be removed
        public void Draw<T>(params object[] args) where T : UIComponent
        {
            T component = Activator.CreateInstance<T>();
            component.Draw(this, args);
        }

        public void Draw(UIComponent component, params object[] args) => component.Draw(this, args);

        public void Clear()
        {
            defaultContainer.Children.Clear();
        }

        public StackPanel GetContainer()
        {
            return defaultContainer;
        }
    }

    public class ElementStyle
    {
        public readonly static Brush PrimaryBackgroundColor = new SolidColorBrush(Color.FromRgb(77, 80, 97));
        public readonly static Brush AccentBackgroundColor = new SolidColorBrush(Color.FromRgb(48, 50, 61));

        public readonly static int LargeTextSize = 32;
        public readonly static int MediumTextSize = 24;
        public readonly static int SmallTextSize = 16;

        public static Brush GetColor(byte r, byte  g, byte b) => new SolidColorBrush(Color.FromRgb(r, g, b));

        public readonly static Thickness ZeroThickness = new Thickness()
        {
            Left = 0,
            Top = 0,
            Right = 0,
            Bottom = 0
        };

        public int FontSize = SmallTextSize;
        public Brush BackGroundColor;
        public Brush FontColor = Brushes.White;
        public Brush BorderColor = Brushes.White;
        public Thickness Padding = ZeroThickness;
        public Thickness Margin = ZeroThickness;
        public double Width = 100;
        public double Height = 25;

        public ElementStyle OverrideFontSize(int value)
        {
            FontSize = value;
            return this;
        }

        public ElementStyle OverrideBackGroundColor(Brush value)
        {
            BackGroundColor = value;
            return this;
        }

        public ElementStyle OverrideFontColor(Brush value)
        {
            FontColor = value;
            return this;
        }

        public ElementStyle OverrideBorderColor(Brush value)
        {
            BorderColor = value;
            return this;
        }

        public ElementStyle OverridePadding(Thickness value)
        {
            Padding = value; 
            return this;
        }

        public ElementStyle OverrideMargin(Thickness value)
        {
            Margin = value;
            return this;
        }

        public ElementStyle OverrideWidth(double value)
        {
            Width = value;
            return this;
        }

        public ElementStyle OverrideHeight(double value)
        {
            Height = value;
            return this;
        }

        public static ElementStyle DefaultTextStyle()
        {
            return new ElementStyle()
            {
                FontSize = SmallTextSize,
                Padding = new Thickness(1, 1, 10, 1)
            };
        }

        public static ElementStyle DefaultFieldStyle()
        {
            return new ElementStyle()
            {
                BackGroundColor = AccentBackgroundColor,
                Padding = new Thickness(1),
                Margin = new Thickness(10, 0, 10, 0)
            };
        }

    }

    public delegate void ButtonClick(object sender, RoutedEventArgs e); 
}
