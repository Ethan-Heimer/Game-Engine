using GameEngine.Debugging;
using GameEngine.Engine.Events;
using GameEngine.Engine.Physics.Rigidbody;
using GameEngine.Engine.Settings;
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


namespace GameEngine.Engine.Physics
{
    [ContainsSettings]
    public static class PhysicsSystem
    {
        [EngineSettings("Physics")]
        static float fixedUpdate;

        [EngineSettings("Physics")]
        static Vector2 gravityDirection;

        [EngineSettings("Physics")]
        static float gravityIntensity;

        [EngineSettings("Physics")]
        static float impulseIterations = 3;

        static List<(int, int)> contactPairs = new List<(int, int)> ();
        static List<CollisionManifold> collisions = new List<CollisionManifold>();

        public static void Init(float fixedUpdateDeltaTime) 
        {
            fixedUpdate = fixedUpdateDeltaTime;

            EngineEventManager.AddEventListener<WhileInPlayMode>((e) => Update(fixedUpdateDeltaTime));
        }

        public static void Update(float dt) 
        {
            FixedUpdate(gravityDirection, gravityIntensity);
        }

        public static void FixedUpdate(Vector2 gravityDirection, float gravityIntensity)
        {
            var rigidBodies = PhysicsWorld.GetRigidBodies();

            contactPairs.Clear();
            collisions.Clear();

            //find collision (look int quad tree, much faster than this)
            BroadPhase(rigidBodies);
            NarrowPhase(rigidBodies);

            //FixedUpdate magic function can be ran here
            foreach (var o in rigidBodies)
            {
                o.PhysicsUpdate(fixedUpdate, gravityDirection, gravityIntensity);
            }
        }


        private static void BroadPhase(RigidBody[] rigidBodies)
        {
            //find collision (look int quad tree, much faster than this)
            int size = rigidBodies.Length;

            foreach(var o in rigidBodies)
            {
                o.Collider.IsColliding = false;
                o.Collider.ClearNormals();
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++)
                {
                    if (i == j)
                        continue;

                    CollisionManifold result = new CollisionManifold();

                    RigidBody r1 = rigidBodies[i];
                    RigidBody r2 = rigidBodies[j];

                    Collider c1 = r1.Collider;
                    Collider c2 = r2.Collider;

                    if (r1.Static && r2.Static)
                    {
                        continue;
                    }

                    if (c1 != null && c2 != null && !(r1.HasInfinateMass && r1.HasInfinateMass))
                    {
                        result = Collisions.FindCollisionFeatures(c1, c2);
                    }

                    if (result != null && result.IsColliding)
                    {
                        contactPairs.Add((i, j));
                        collisions.Add(result);

                        c1.IsColliding = true;
                        c2.IsColliding = true;

                        c1.AddNormal(result.Normal); 
                        c2.AddNormal(result.Normal);
                    }
                }
            }
        }
        private static void NarrowPhase(RigidBody[] rigidBodies)
        {
            //resolve collisions with iterative impulse resolution
            //itterate a certain amount of times to get an approx. solution
            for (int i = 0; i < impulseIterations; i++)
            {
                for (int j = 0; j < contactPairs.Count; j++)
                {
                    RigidBody r1 = rigidBodies[contactPairs[j].Item1];
                    RigidBody r2 = rigidBodies[contactPairs[j].Item2];

                    if (r1.Static)
                    {
                        r2.transform.Position += (collisions[j].Normal * collisions[j].Depth / 2);
                    }
                    else if (r2.Static)
                    {
                        r1.transform.Position += (-collisions[j].Normal * collisions[j].Depth / 2);
                    }
                    else
                    {
                        r1.transform.Position += (-collisions[j].Normal * collisions[j].Depth / 2);
                        r2.transform.Position += (collisions[j].Normal * collisions[j].Depth / 2);
                    }

                    ApplyImpulseWithRotationAndFriction(r1, r2, collisions[j]);
                }
            }
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
            float J = (n / invMassSum);

            Vector2 impulse = relNormal * J;

            r1.SetVelocity((r1.Velocity + impulse) * invMass1 * -1);
            r2.SetVelocity((r2.Velocity + impulse) * invMass2);   
        }

        public static void ApplyImpulseWithRotation(RigidBody bodyA, RigidBody bodyB, CollisionManifold contact)
        {
            Vector2 normal = contact.Normal;
            Vector2 contact1 = contact.ContactPoints[0];
            Vector2 contact2 = contact.ContactPoints[1];
            int contactCount = contact.ContactCount;

            float e = Math.Min(bodyA.COR, bodyB.COR);

            Vector2[] contactList = new Vector2[] { contact1, contact2 };
            Vector2[] impulseList = new Vector2[contactCount];
           
            Vector2[] raList = new Vector2[contactCount];
            Vector2[] rbList = new Vector2[contactCount];

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 ra = contactList[i] - bodyA.transform.WorldPosition;
                Vector2 rb = contactList[i] - bodyB.transform.WorldPosition;

                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                Vector2 relativeVelocity =
                    (bodyB.Velocity + angularLinearVelocityB) -
                    (bodyA.Velocity + angularLinearVelocityA);

                float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);

                if (contactVelocityMag > 0f)
                {
                    continue;
                }

                float raPerpDotN = Vector2.Dot(raPerp, normal);
                float rbPerpDotN = Vector2.Dot(rbPerp, normal);

                float denom = bodyA.InverseMass + bodyB.InverseMass +
                    (raPerpDotN * raPerpDotN) * bodyA.InverseInertia +
                    (rbPerpDotN * rbPerpDotN) * bodyB.InverseInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= (float)contactCount;

                Vector2 impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 impulse = impulseList[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];

                float rotImpulseA = Cross(ra, impulse) * bodyA.InverseInertia;
                float rotImpulseB = Cross(rb, impulse) * bodyB.InverseInertia;

                bodyA.AddForce(-impulse * bodyA.InverseMass);
                bodyA.AddTouque(rotImpulseA);
                
                bodyB.AddForce(impulse * bodyB.InverseMass);
                bodyB.AddTouque(rotImpulseB);
            }
        }

        public static void ApplyImpulseWithRotationAndFriction(RigidBody bodyA, RigidBody bodyB, CollisionManifold contact)
        {
            Vector2 normal = contact.Normal;
            Vector2 contact1 = contact.ContactPoints[0];
            Vector2 contact2 = contact.ContactPoints[1];
            int contactCount = contact.ContactCount;

            float e = Math.Min(bodyA.COR, bodyB.COR);

            float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * .5f;
            float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * .5f;

            Vector2[] contactList = new Vector2[] { contact1, contact2 };
            float[] jList = new float[contactCount];

            Vector2[] impulseList = new Vector2[contactCount];
            Vector2[] impulseFrictionList = new Vector2[contactCount];

            Vector2[] raList = new Vector2[contactCount];
            Vector2[] rbList = new Vector2[contactCount];

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 ra = contactList[i] - bodyA.transform.WorldPosition;
                Vector2 rb = contactList[i] - bodyB.transform.WorldPosition;

                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                Vector2 relativeVelocity =
                    (bodyB.Velocity + angularLinearVelocityB) -
                    (bodyA.Velocity + angularLinearVelocityA);

                float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);

                if (contactVelocityMag > 0f)
                {
                    continue;
                }

                float raPerpDotN = Vector2.Dot(raPerp, normal);
                float rbPerpDotN = Vector2.Dot(rbPerp, normal);

                float denom = bodyA.InverseMass + bodyB.InverseMass +
                    (raPerpDotN * raPerpDotN) * bodyA.InverseInertia +
                    (rbPerpDotN * rbPerpDotN) * bodyB.InverseInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= (float)contactCount;

                jList[i] = j;

                Vector2 impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 impulse = impulseList[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];

                float rotImpulseA = Cross(ra, impulse) * bodyA.InverseInertia;
                float rotImpulseB = Cross(rb, impulse) * bodyB.InverseInertia;

                bodyA.AddForce(-impulse * bodyA.InverseMass);
                bodyA.SetAngularVelocity(rotImpulseA);

                bodyB.AddForce(impulse * bodyB.InverseMass);
                bodyB.SetAngularVelocity(rotImpulseB);

                string nameA = bodyA.gameObject.Name;
                string nameB = bodyB.gameObject.Name;
            }

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 ra = contactList[i] - bodyA.transform.WorldPosition;
                Vector2 rb = contactList[i] - bodyB.transform.WorldPosition;

                raList[i] = ra;
                rbList[i] = rb;

                Vector2 raPerp = new Vector2(-ra.Y, ra.X);
                Vector2 rbPerp = new Vector2(-rb.Y, rb.X);

                Vector2 angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                Vector2 angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                Vector2 relativeVelocity =
                    (bodyB.Velocity + angularLinearVelocityB) -
                    (bodyA.Velocity + angularLinearVelocityA);

                Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

                if(tangent == Vector2.Zero)
                {
                    continue;
                }

                tangent.Normalize();
                float contactVelocityMag = Vector2.Dot(relativeVelocity, tangent);

                float raPerpDotT = Vector2.Dot(raPerp, tangent);
                float rbPerpDotT = Vector2.Dot(rbPerp, tangent);

                float denom = bodyA.InverseMass + bodyB.InverseMass +
                    (raPerpDotT * raPerpDotT) * bodyA.InverseInertia +
                    (rbPerpDotT * rbPerpDotT) * bodyB.InverseInertia;

                float jt = -contactVelocityMag;
                jt /= denom;
                jt /= (float)contactCount;

                Vector2 impulseFriction = Vector2.Zero;
                float j = jList[i];

                if(Math.Abs(jt) <= j * sf)
                {
                   impulseFriction = jt * tangent;
                }
                else
                {
                    impulseFriction = -j * tangent * df;
                }

                impulseFrictionList[i] = impulseFriction;
            }

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 frictionImpulse = impulseFrictionList[i];
                Vector2 ra = raList[i];
                Vector2 rb = rbList[i];

                bodyA.AddForce(-frictionImpulse * bodyA.InverseMass);
                //bodyA.AddTouque(Cross(ra, frictionImpulse) * bodyA.InverseInertia);

                bodyB.AddForce(frictionImpulse * bodyB.InverseMass);
                //bodyB.AddTouque(Cross(rb, frictionImpulse) * bodyB.InverseInertia);
            }


        }

      

        static float Cross(Vector2 a, Vector2 b)
        {
            // cz = ax * by − ay * bx
            return (float)(a.X * b.Y - a.Y * b.X);
        }
    }
}
