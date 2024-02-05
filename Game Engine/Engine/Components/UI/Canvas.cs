using GameEngine.Engine.Rendering;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Components
{
    public class Canvas : Behavior
    {
        public int Width
        {
            get
            {
                return (int)GameWindowManager.WindowSize().X;
            }
        }
        public int Height
        {
            get
            {
                return (int)GameWindowManager.WindowSize().Y;
            }
        }

        public Vector2 TopLeft
        {
            get
            {
                return new Vector2(0,0);
            }
        }
        public Vector2 TopRight
        {
            get
            {
                return new Vector2(Width, 0);
            }
        }

        public Vector2 BottomLeft
        {
            get
            {
                return new Vector2(0, Height);
            }
        }

        public Vector2 BottomRight
        {
            get
            {
                return new Vector2(Width, Height);
            }
        }
        public int CenterWidth
        {
            get
            {
                return Width / 2;
            }
        }
        public int CenterHeight
        {
            get
            {
                return Height / 2;
            }
        }

        public float PercentWidth(float percent) => Width * percent; 
        public float PercentHeight(float percent) => Height * percent; 

        public void Start()
        {
            transform.Position = new Vector2(0, 0);
        }

        public void WhileInEditor()
        {
            Vector2[] verticies = new Vector2[]
            {
                new Vector2(transform.WorldPosition.X ,transform.WorldPosition.Y),
                new Vector2(Width + transform.WorldPosition.X, transform.WorldPosition.Y),
                new Vector2(Width + transform.WorldPosition.X, Height + transform.WorldPosition.Y),
                new Vector2(transform.WorldPosition.X, Height + transform.WorldPosition.Y),
            };

            Renderer.DrawWireframe(verticies, Color.Aqua);
        }
    }
}
