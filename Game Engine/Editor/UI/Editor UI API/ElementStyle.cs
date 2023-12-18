using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using GameEngine.Debugging;

namespace GameEngine.Editor.Windows
{
    [Note(note = "Make Defaults are properties instead of functions")]
    public class ElementStyle
    {
        public readonly static Brush PrimaryBackgroundColor = new SolidColorBrush(Color.FromRgb(63, 65, 79));
        public readonly static Brush AccentBackgroundColor = new SolidColorBrush(Color.FromRgb(48, 50, 61));
        public readonly static Brush TertiaryBackgroundColor = new SolidColorBrush(Color.FromRgb(77, 80, 97));

        public readonly static int LargeTextSize = 32;
        public readonly static int MediumTextSize = 24;
        public readonly static int SmallTextSize = 16;

        public static Brush GetColor(byte r, byte g, byte b) => new SolidColorBrush(Color.FromRgb(r, g, b));

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

        public HorizontalAlignment HorizontalAlignment = HorizontalAlignment.Center;

        public double Width = 100;
        public double Height = 25;

        public bool DynamicSize = false;

        public Brush OnHoverBackground;

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
                    Padding = new Thickness(1),
                    Margin = new Thickness(10, 0, 10, 0)
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

    }
}
