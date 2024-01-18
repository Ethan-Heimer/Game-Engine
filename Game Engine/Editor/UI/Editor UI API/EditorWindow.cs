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
                Console.WriteLine(pos + " " + this.GetType().Name);
                return pos;
            }

            set
            {
                Left = value.X;
                Top = value.Y;

               
            }
        }

        public Vector2 Size
        {
            get
            {
                var pos = new Vector2((float)Width, (float)Height);
                Console.WriteLine(pos + " " + this.GetType().Name);

                return new Vector2((float)Width, (float)Height);
            }

            set
            {
                Width = value.X;
                Height = value.Y;

            }
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
