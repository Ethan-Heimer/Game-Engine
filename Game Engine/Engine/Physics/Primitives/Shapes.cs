using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Engine.Physics
{
    public struct Line
    {
        public Vector2 Start;
        public Vector2 End; 

        public float LengthSquared
        {
            get
            {
                return (End - Start).LengthSquared();
            }
        }
    }

    public struct Circle
    {
        public float Radius;
        public float RadiusSquared
        {
            get
            {
                return Radius * Radius;
            }
        }

        public Vector2 Center;
    }
}
