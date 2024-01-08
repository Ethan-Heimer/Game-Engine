using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Engine.Physics
{
    public class Collider : Behavior
    {
        RigidBody _ridgidBody;
        public RigidBody RigidBody
        {
            get
            {
                if (_ridgidBody == null)
                    _ridgidBody = gameObject.GetComponent<RigidBody>();

                return _ridgidBody; 
            }
        }
        //public abstract float getInertiaTensor()
    }
}
