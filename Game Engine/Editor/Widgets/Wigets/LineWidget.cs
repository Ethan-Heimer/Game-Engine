using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Editor.Widgets
{
    public class LineWidget : Widget
    {
        public Vector2 Position2;
        protected override void OnDraw()
        {
            Renderer.DrawLine(Position, Position2);
        }
    }
}
