using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    public class ForceRegistration
    {
        public IForceGenerator force;
        public RigidBody body;

        public ForceRegistration(IForceGenerator force, RigidBody body)
        {
            this.force = force;
            this.body = body;
        }

        public override bool Equals(object obj)
        {
            if(obj == null)
                return false;

            if(obj.GetType() != typeof(ForceRegistration)) return false;

            ForceRegistration forceRegistration = (ForceRegistration)obj;
            return forceRegistration.force == this.force && forceRegistration.body == this.body;
        }
    }
}
