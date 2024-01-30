using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Engine.Physics
{
    public class RigidBody : Behavior
    {
        public float Mass = 1;

        public float StaticFriction;
        public float DynamicFriction;

        public bool Static = false; 
        public bool FixedRotation = false;

        public bool Gravity;
        public Vector2 OffsetGravityDirection;
        public float OffsetGravityMagnitude;

        public float drag;
        public float angularDrag;

        Vector2 forceAccumulator; 

        public float InverseMass
        {
            get
            {
                if(Mass != 0)
                    return 1 / Mass;

                return 0;
            }
        }
        public Vector2 Velocity
        {
            get; private set;
        }
        public float AngularVelocity 
        {
            get; private set;
        }

        public float Inertia
        {
            get 
            {
                return ((1f / 12f) * Mass * (transform.Size.X * transform.Size.X + transform.Size.Y * transform.Size.Y));
            }

        }

        public float InverseInertia
        {
            get
            {
                if(Mass == 0)
                    return 0;
                
                return 1 / Inertia;
            }
        }

        public float COR = 0f;
        


        public bool HasInfinateMass
        {
            get
            {
                return Mass == 0f;
            }
        }


        Collider _collider;
        public Collider Collider
        {
            get
            {
                if(_collider == null)
                    _collider = gameObject.GetComponent<Collider>();

                return _collider;
            }
        }

        public void Start()
        {
            //Register into simulation
            PhysicsWorld.AddRigidBody(this);
        }

        public void OnDisable()
        {
            PhysicsWorld.RemoveRigidBody(this);
        }

        //could become Magic Function FixedUpdate
        public void PhysicsUpdate(float dt, Vector2 gravity, float intensity)
        {
            if (Static)
            {
                Velocity = Vector2.Zero;
                AngularVelocity = 0;
                return;
            }

            bool applyGravity = true;
            
            foreach(Vector2 o in Collider.CollsionNormals)
            {
                if(o == -OffsetGravityDirection)
                    applyGravity = false;
            }

            Vector2 acceleration = forceAccumulator * InverseMass;

            Velocity += acceleration;

            if(Gravity && applyGravity)
            {
                Velocity += OffsetGravityDirection + gravity * (OffsetGravityMagnitude * intensity) * dt;
            }
            else
            {
                if( -drag < Velocity.X && Velocity.X < drag)
                {
                    Velocity = new Vector2(0, Velocity.Y);
                }

                if (-drag < Velocity.Y && Velocity.Y < drag)
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }

                Velocity -= new Vector2(Math.Sign(Velocity.X) * drag, Math.Sign(Velocity.Y) * drag); 
                //AngularVelocity -= Math.Sign(AngularVelocity) * drag; 
            }

            transform.Position += Velocity * dt;

            if (!FixedRotation)
            {
                AngularVelocity = AngularVelocity > 0 ? AngularVelocity -= angularDrag : 0;
                transform.Rotation += AngularVelocity * dt * (180f/(float)Math.PI);
            }
            else
            {
                AngularVelocity = 0;
            }
            forceAccumulator = Vector2.Zero;
            
        
        }

       
        public void SetCoefficientOfRestitution(float coefficient) => COR = coefficient;

        public void AddForce(Vector2 force)
        {
            if (Static)
                return;

            forceAccumulator += force;
        }
        public void SetVelocity(Vector2 vel)
        {
            if (Static)
                return;

            Velocity = vel;
        }

        public void AddTouque(float force)
        {
            if (Static)
                return;
            AngularVelocity += force;
        }

        public void SetAngularVelocity(float vel)
        {
            if (Static)
                return;

            AngularVelocity = vel;
        }
    }
}
