using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Utilities
{
    public static class RectangleExtention
    {
        public static bool Intersects(this Rectangle rectangle, Vector2 point)
        {
            bool xBetween = rectangle.Left < point.X && point.X < rectangle.Right;
            bool yBetween = rectangle.Top < point.Y && point.Y < rectangle.Bottom;

            return xBetween && yBetween;
        }
    }
}
