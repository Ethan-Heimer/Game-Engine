using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics.Forces
{
    public class Gravity : IForceGenerator
    {
        Vector2 gravity; 
        public Gravity(Vector2 gravity) 
        {
            this.gravity = gravity;
        }

        public void UpdateForce(RigidBody rb, float dt)
        {
            //f = m * a

            rb.AddForce(gravity * rb.Mass);
        }
    }
}
