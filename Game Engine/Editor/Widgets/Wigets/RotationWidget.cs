using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Editor.Widgets
{
    public class RotationWidget : Widget
    {
        public Vector2 pointerPosition;
      
        protected override void OnDraw()
        {
            Renderer.DrawLine(Position, pointerPosition);

            int length = (int)Math.Sqrt(Math.Pow(pointerPosition.X - Position.X, 2) + Math.Pow(pointerPosition.Y - Position.Y, 2));
            Renderer.DrawCircle(Position, length * 2, Color.White * .1f);
        }
    }
}
