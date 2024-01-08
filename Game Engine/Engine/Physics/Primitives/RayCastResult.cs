using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics.Primitives
{
    public class RayCastResult
    {
        public Vector2 Point { get; private set; }
        public Vector2 Normal { get; private set; }

        public bool Hit { get; private set; }
        float t;

        public RayCastResult()
        {
            Hit = false;
            t = -1;
        }

        public void Init(Vector2 point, Vector2 normal, float t, bool hit)
        {
            this.Point = point;
            this.Normal = normal;
            this.t = t;
            this.Hit = hit;
        }

        public void Reset()
        {
            this.Point = Vector2.Zero;
            this.Normal = Vector2.Zero;
            Hit = false;
            t = -1;
        }
    }
}
