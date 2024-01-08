using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    public class Ray2D
    {
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }

        public Ray2D(Vector2 origin, Vector2 direction)
        {
            this.Origin = origin;
            this.Direction = direction;

            this.Direction.Normalize();
        }
    }
}
