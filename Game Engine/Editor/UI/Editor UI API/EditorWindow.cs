using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace GameEngine.Editor.Windows
{
    public abstract class EditorWindow : Window
    {
        Vector2 _dpi;
        Vector2 DPI
        {
            get
            {
                if(_dpi == Vector2.Zero)
                {
                    PresentationSource source = PresentationSource.FromVisual(this);

                    double dpiX = 0, dpiY = 0;
                    if (source != null)
                    {
                        dpiX = source.CompositionTarget.TransformToDevice.M11;
                        dpiY = source.CompositionTarget.TransformToDevice.M22;
                    }

                    _dpi = new Vector2((float)dpiX, (float)dpiY);
                }

                return _dpi;
            }
        }

        public RelativeWindowPosition RelativePosition
        {
            get;
            protected set;
        }

        public Vector2 Position
        {
            get
            {
                var pos = new Vector2((float)Left, (float)Top);
                return pos;
            }

            set
            {
                Left = value.X * (1/DPI.X);
                Top = value.Y * (1/DPI.Y);
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2((float)Width, (float)Height);
            }

            set
            {
                Width = value.X * (1 / DPI.X);
                Height = value.Y * (1 / DPI.Y);

            }
        }

        public EditorWindow()
        {
            Background = ElementStyle.PrimaryBackgroundColor;
        }

        public abstract void OnGUI(EditorGUIDrawer drawer);
        public virtual void OnUpdateGUI(EditorGUIDrawer drawer) { }

        
    }

    public enum RelativeWindowPosition
    {
        Left,
        Right,
        Top,
        Bottom,

        Float
    }
}
