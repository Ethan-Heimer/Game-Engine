using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using GameEngine.Debugging;
using GameEngine.Engine;
using System.Data.SqlClient;
using System.Reflection;
using GameEngine.Engine.Settings;

namespace GameEngine.Editor.Windows
{
    [ContainsSettings]
    public class ElementStyle
    {
        [EngineSettings("Editor")]
        static Microsoft.Xna.Framework.Color PrimaryColor;

        [EngineSettings("Editor")]
        static Microsoft.Xna.Framework.Color AccentColor;

        [EngineSettings("Editor")]
        static Microsoft.Xna.Framework.Color TertiaryColor;

        public static Brush PrimaryBackgroundColor
        {
            get
            {
                return new SolidColorBrush(Color.FromRgb(PrimaryColor.R, PrimaryColor.G, PrimaryColor.B));
            }
        }
        public static Brush AccentBackgroundColor
        {
            get
            {
                return new SolidColorBrush(Color.FromRgb(AccentColor.R, AccentColor.G, AccentColor.B));
            }
        }
        public static Brush TertiaryBackgroundColor
        {
            get
            {
                return new SolidColorBrush(Color.FromRgb(TertiaryColor.R, TertiaryColor.G, TertiaryColor.B));
            }
        }

        public readonly static int LargeTextSize = 24;
        public readonly static int MediumTextSize = 16;
        public readonly static int SmallTextSize = 12;

        public static Brush GetColor(byte r, byte g, byte b) => new SolidColorBrush(Color.FromRgb(r, g, b));
        public static ImageBrush GetImage(Icon icon) => new ImageBrush(icon?.GetImage());

        public readonly static Thickness ZeroThickness = new Thickness()
        {
            Left = 0,
            Top = 0,
            Right = 0,
            Bottom = 0
        };

        public int FontSize = SmallTextSize;
        public Brush Background;
        public Brush Foreground = Brushes.White;
        public Brush BorderBrush = Brushes.Black;
        public Thickness Padding = ZeroThickness;
        public Thickness Margin = ZeroThickness;

        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Stretch;

        public double Width = 100;
        public double Height = 25;

        public bool DynamicSize = false;

        public bool DynamicWidth = false;
        public bool DynamicHeight = false;

        public bool HoverEfects = false;
        public bool DragEffects = false;

        public bool AppliedHover = false;

        public Brush OnHoverBackground;
        public Brush OnDragBackground;

        public ElementStyle OverrideFontSize(int value)
        {
            FontSize = value;
            return this;
        }

        public ElementStyle OverrideBackGroundColor(Brush value)
        {
            Background = value;
            return this;
        }

        public ElementStyle OverrideForeground(Brush value)
        {
            Foreground = value;
            return this;
        }

        public ElementStyle OverrideBorderBrush(Brush value)
        {
            BorderBrush = value;
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

        public ElementStyle OverrideHorzontalAlignment(HorizontalAlignment value)
        {
            HorizontalAlignment = value;
            return this;
        }

        public ElementStyle OverrideDynamicSize(bool value)
        {
            DynamicSize = value;
            return this;
        } 
        public ElementStyle OverrideDynamicWidth(bool value)
        {
            DynamicWidth = value;
            return this;
        }
        public ElementStyle OverrideDynamicHeight(bool value)
        {
            DynamicHeight = value;
            return this;
        }

        public ElementStyle OverrideHoverEffects(bool value)
        {
            HoverEfects = value;
            return this;
        }

        public ElementStyle OverrideHoverBackground(Brush color)
        {
            OnHoverBackground = color;
            return this;
        }

        public static ElementStyle DefaultTextStyle
        {
            get
            {
                return new ElementStyle()
                {
                    FontSize = SmallTextSize,
                    Padding = new Thickness(1, 1, 10, 1),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    DynamicSize = true,
                };

            }
        }

        public static ElementStyle DefaultFieldStyle
        {
            get
            {
                return new ElementStyle()
                {
                    Background = AccentBackgroundColor,
                    Padding = new Thickness(0),
                    Margin = new Thickness(0, 0, 1, 0),
                    Width = 40,
                    Height = 20
                };
            }
        }

        public static ElementStyle DefaultButtonStyle
        {
            get
            {
                return new ElementStyle()
                {
                    Padding = new Thickness(1),
                    Margin = new Thickness(10, 0, 10, 0),
                    DynamicSize = true,
                    Background = TertiaryBackgroundColor,
                    BorderBrush = Brushes.Black,
                };
            }
        }

        public static ElementStyle DefaultCheckboxStyle
        {
            get
            {
                return new ElementStyle()
                {
                    Foreground = Brushes.White,
                    Padding = new Thickness(1),
                };
            }
        }

        public static ElementStyle DefaultGroupStyle => new ElementStyle()
        {
            DynamicSize = true,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };

        public static void ApplyStyle(FrameworkElement element, ElementStyle style)
        {
            MemberInfo[] styleFields = style.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            MemberInfo[] elementFields = element.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string[] commonStyles = FindCommonStyles(styleFields, elementFields);

            foreach (var o in commonStyles)
            {
                if (style.DynamicSize && (o == "Width" || o == "Height"))
                    continue;
                else if (style.DynamicWidth && (o == "Width"))
                    continue;
                else if (style.DynamicHeight && (o == "Height"))
                    continue;

                var styleData = style.GetType().GetField(o).GetValue(style);
                element.GetType().GetProperty(o).SetValue(element, styleData);
            }

            if (style.HoverEfects)
            {
                element.IsHitTestVisible = true;
                element.MouseEnter += (s, e) =>
                {
                    var field = element.GetType().GetProperty("Background");

                    if (field != null)
                        field.SetValue(element, style.OnHoverBackground);
                };

                element.MouseLeave += (s, e) =>
                {
                    var field = element.GetType().GetProperty("Background");
                    if (field != null)
                        field.SetValue(element, style.Background);
                };
            }

            if (style.DragEffects)
            {
                element.DragEnter += (s, e) =>
                {
                    var field = element.GetType().GetProperty("Background");

                    if (field != null)
                        field.SetValue(element, style.OnDragBackground);
                };

                element.DragLeave += (s, e) =>
                {
                    var field = element.GetType().GetProperty("Background");
                    if (field != null)
                        field.SetValue(element, style.Background);
                };
            }
        }

        static string[] FindCommonStyles(MemberInfo[] fields1, MemberInfo[] fields2)
        {
            List<string> intersecton = new List<string>();
            HashSet<string> H = new HashSet<string>();

            foreach (var o in fields1)
                H.Add(o.Name);

            foreach (var o in fields2)
            {
                if (H.Contains(o.Name))
                    intersecton.Add(o.Name);
            }

            return intersecton.ToArray();
        }

    }
}
