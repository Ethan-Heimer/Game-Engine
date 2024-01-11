using GameEngine.Engine.Events;
using GameEngine.Engine.Physics.Forces;
using GameEngine.Engine.Physics.Rigidbody;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics
{
    public static class PhysicsSystem
    {
        static ForceRegistry forceRegistry;
        static List<RigidBody> rigidBodies = new List<RigidBody>();
        static Gravity gravity;
        static float fixedUpdate;

        static List<RigidBody> bodies1 = new List<RigidBody>();
        static List<RigidBody> bodies2 = new List<RigidBody>();
        static List<CollisionManifold> collisions = new List<CollisionManifold>();

        const float impulseIterations = 6; 

        public static void Init(float fixedUpdateDeltaTime, Vector2 _gravity) 
        {
            forceRegistry = new ForceRegistry();
            gravity = new Gravity(_gravity);

            fixedUpdate = fixedUpdateDeltaTime;

            EngineEventManager.AddEventListener<WhileInPlayMode>((e) => Update(fixedUpdateDeltaTime));
        }

        //use engine hook
        public static void Update(float dt) 
        {
            FixedUpdate();
        }

        public static void FixedUpdate()
        {
            bodies1.Clear();
            bodies2.Clear();
            collisions.Clear();

            //find collision (look int quad tree, much faster than this)
            int size = rigidBodies.Count;
            for (int i = 0; i < size; i++) 
            {
                for(int  j = i; j < size; j++)
                {
                    if (i == j)
                        continue;

                    CollisionManifold result = new CollisionManifold();

                    RigidBody r1 = rigidBodies[i];
                    RigidBody r2 = rigidBodies[j];

                    Collider c1 = r1.Collider;
                    Collider c2 = r2.Collider;

                    if(c1 != null && c2 != null && !(r1.HasInfinateMass && r1.HasInfinateMass))
                    {
                        result = Collisions.FindCollisionFeatures(c1, c2);
                    }

                    if (result != null && result.IsColliding)
                    {
                        bodies1.Add(r1);
                        bodies2.Add(r2);
                        collisions.Add(result);
                    }
                }
            }

            //update forces
            forceRegistry.UpdateForces(fixedUpdate);

            //resolve collisions with iterative impulse resolution
            //itterate a certain amount of times to get an approx. solution

            for(int i =  0; i < impulseIterations; i++) 
            {
                for(int j = 0; j < collisions.Count; j++) 
                {
                    int kSize = collisions[j].ContactPoints.Length;
                    for(int k = 0; k < kSize; k++)
                    {
                        RigidBody r1 = bodies1[j];
                        RigidBody r2 = bodies2[j];

                        ApplyImpulse(r1, r2, collisions[j]);
                    }
                }
            }

            //FixedUpdate magic function can be ran here
            foreach (var o in rigidBodies)
            {
                o.PhysicsUpdate(fixedUpdate);
            }
        }

        public static void AddRigidBody(RigidBody rigidBody, bool useGravity) 
        {
            rigidBodies.Add(rigidBody);

            if(useGravity)
                forceRegistry.Add(rigidBody, gravity);
        }

        static void ApplyImpulse(RigidBody r1, RigidBody r2, CollisionManifold m)
        {
            Console.WriteLine("Trying to Apply Impulse");

            float invMass1 = r1.InverseMass;
            float invMass2 = r2.InverseMass;

            float invMassSum = invMass1 + invMass2;

            if (invMassSum == 0f)
            {
                Console.WriteLine("Sum is Zero");
                return;
            }

            Vector2 relVel = r2.Velocity - r1.Velocity;
            Vector2 relNormal = m.Normal;

            relNormal.Normalize();
            Console.WriteLine(relNormal);

            //moving away
            if(Vector2.Dot(relVel, relNormal) > 0)
            {
                Console.WriteLine("Objects are moving away");
                return;
            }

            float e = Math.Min(r1.COR, r2.COR);

            float n = (-(1f + e) * Vector2.Dot(relVel, relNormal));
            float J = n / invMassSum;

            if (m.ContactPoints.Length > 0 && J != 0f)
                J /= (float)m.ContactPoints.Length;

            Vector2 impulse = relNormal * J;

            r1.SetVelocity((r1.Velocity + impulse) * invMass1 * -1);
            r2.SetVelocity((r2.Velocity + impulse) * invMass2);
        }
    }
}
