using GameEngine.Pointers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    public class ForceRegistry
    {
        List<ForceRegistration> registry = new List<ForceRegistration>();

        public void Add(RigidBody body, IForceGenerator force)
        {
            ForceRegistration fr = new ForceRegistration(force, body);
            registry.Add(fr);
        }

        public void Remove(RigidBody body, IForceGenerator force) 
        {
            ForceRegistration fr = new ForceRegistration(force, body);
            registry.Remove(fr);    
        }

        public void Clear()
        {
            registry.Clear();
        }

        public void UpdateForces(float deltaTime)
        {
            foreach (ForceRegistration fr in registry) 
            {
                fr.force.UpdateForce(fr.body, deltaTime);
            }
        }

        /*
        public void ZeroForces()
        {
            foreach (ForceRegistration fr in registry) 
            {
                fr.force.ZeroForces();
            }
        }*/
    }
}
