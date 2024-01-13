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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
                    RigidBody r1 = bodies1[j];
                    RigidBody r2 = bodies2[j];

                    if(r1.Mass != 0)
                        r1.transform.Position += (-collisions[j].Normal * collisions[j].Depth / 2);

                    if(r2.Mass != 0)
                        r2.transform.Position += (collisions[j].Normal * collisions[j].Depth / 2);

                    ApplyImpulseWithRotation(r1, r2, collisions[j]);
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
            float invMass1 = r1.InverseMass;
            float invMass2 = r2.InverseMass;

            float invMassSum = invMass1 + invMass2;

            if (invMassSum == 0f)
            {
                return;
            }

            Vector2 relVel = r2.Velocity - r1.Velocity;
            Vector2 relNormal = m.Normal;

            relNormal.Normalize();

            //moving away
            //it does not matter the order for a dot product
            if(Vector2.Dot(relVel, relNormal) > 0)
            {
                return;
            }

            float e = Math.Min(r1.COR, r2.COR);

            float n = (-(1f + e) * Vector2.Dot(relVel, relNormal));
            float J = n / invMassSum;

            Vector2 impulse = relNormal * J;

            r1.SetVelocity((r1.Velocity + impulse) * invMass1 * -1);
            r2.SetVelocity((r2.Velocity + impulse) * invMass2);   
        }

        public static void ApplyImpulseWithRotation(RigidBody r1, RigidBody r2, CollisionManifold m)
        {
            Vector2 normal = m.Normal;
            float depth = m.Depth;

            int contactCount = m.ContactPoints.Length;

            float e = Math.Min(r1.COR, r2.COR);
            Vector2[] contactList = m.ContactPoints;

            Vector2[] impulseList = new Vector2[contactCount];
            Vector2[] raList = new Vector2[contactCount];
            Vector2[] rbList = new Vector2[contactCount];

            for(int i = 0; i< contactCount; i++)
            {
                Vector2 ra = contactList[i] - r1.transform.Position;
                Vector2 rb = contactList[i] - r2.transform.Position;

                raList[i] = ra;
                rbList[i] = rb;

                Vector2 rap = new Vector2(ra.Y, ra.X);
                Vector2 rbp = new Vector2(rb.Y, rb.X);

                Vector2 angularLinearVelocityA = rap * r1.AngularVelocity;
                Vector2 angularLinearVelocityB = rbp * r2.AngularVelocity;

                Vector2 relVel = (r2.Velocity + angularLinearVelocityB) - (r1.Velocity + angularLinearVelocityA);

                float invMass1 = r1.InverseMass;
                float invMass2 = r2.InverseMass;

                float invMassSum = invMass1 + invMass2;

                if (invMassSum == 0f)
                {
                    continue;
                }

                float contactMagnitude = Vector2.Dot(relVel, normal);

                if (contactMagnitude > 0) 
                {
                    continue;
                }

                Console.WriteLine(r1.Inertia);

                float rapDotN = Vector2.Dot(rap, normal);
                float rbpDotN = Vector2.Dot(rbp, normal);
                float n = (-(1f + e) * contactMagnitude);
                float d = invMassSum + (rapDotN * rapDotN) * r1.InverseInertia + (rbpDotN * rbpDotN) * r2.InverseInertia;

                float J = n / d;
                Console.WriteLine(n);
                J /= (float)contactCount;


                Vector2 impulse = normal * J;
                impulseList[i] = impulse;
                
            }

            for(int i = 0; i < contactCount; i++)
            {
                Vector2 impulse = impulseList[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];

                float rotImpualseA = Cross(ra, impulse) * r1.InverseInertia;
                float rotImpualseB = Cross(rb, impulse) * r2.InverseInertia;

                r1.SetVelocity((r1.Velocity - impulse) * r1.InverseMass);
                r2.SetVelocity((r2.Velocity + impulse) * r2.InverseMass);

                r1.AddTouque(-(rotImpualseA));
                r2.AddTouque((rotImpualseB));
            }
        }
        static float Cross(Vector2 a, Vector2 b)
        {
            // cz = ax * by − ay * bx
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
