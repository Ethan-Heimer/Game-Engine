using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    public class Box2D : Collider
    {
       

        public Vector2 LocalMin
        {
            get
            {
                return transform.Position - halfSize;
            }
        }

        public Vector2 LocalMax
        {
            get
            {
                return transform.Position + halfSize;
            }
        }
    }
}
