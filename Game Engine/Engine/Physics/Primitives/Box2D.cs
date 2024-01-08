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
        Vector2 size
        {
            get
            {
                return transform.Size;
            }
        }
        Vector2 halfSize
        {
            get
            {
                return transform.Size/2;
            }
        }


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
