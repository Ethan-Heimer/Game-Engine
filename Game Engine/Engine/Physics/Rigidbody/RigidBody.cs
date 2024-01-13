using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine.Engine.Physics
{
    public class RigidBody : Behavior
    {
        public float Mass = 1;
        public float InverseMass
        {
            get
            {
                if(Mass != 0)
                    return 1 / Mass;

                return 0;
            }
        }

        public float StaticFriction;
        public float DynamicFriction;

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
                return (1f / 12f) * Mass * (transform.Size.X * transform.Size.X + transform.Size.Y * transform.Size.Y);
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

        public float COR
        {
            get; private set;
        }

        bool fixedRotation = false;

        public bool HasInfinateMass
        {
            get
            {
                return Mass == 0f;
            }
        }

        public bool Gravity;

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
            PhysicsSystem.AddRigidBody(this, Gravity);
        }

        //could become Magic Function FixedUpdate
        public void PhysicsUpdate(float dt)
        {
            if (Mass == 0)
                return;

            //update position
            //Console.WriteLine(Velocity);
            transform.Position += Velocity * dt;
            transform.Rotation += AngularVelocity * dt;
        
        }

       
        public void SetCoefficientOfRestitution(float coefficient) => COR = coefficient;    

        public void AddForce(Vector2 force) => Velocity += force;
        public void SetVelocity(Vector2 vel) => Velocity = vel;
        
        public void AddTouque(float force) => AngularVelocity += force; 
        public void SetAngularVelocity(float vel) => AngularVelocity = vel;
    }
}
