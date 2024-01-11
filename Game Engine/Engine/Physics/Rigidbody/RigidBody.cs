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
        public Vector2 Velocity
        {
            get; private set;
        }

        Vector2 forceAccum;
        
        float angularVelocity;

        float linearDamping;

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

            //calculate linear velocity
            Vector2 acceleration = forceAccum * InverseMass;

            Velocity = Velocity + (acceleration * dt);

            //update position
            transform.Position += Velocity * dt;

            ClearAccululatiors();
        }

        void ClearAccululatiors()
        {
            forceAccum = Vector2.Zero;  
        }

        public void AddForce(Vector2 force) 
        {
            this.forceAccum += force;
        }

        public void SetVelocity(Vector2 vel) => Velocity = vel;
        public void SetCoefficientOfRestitution(float coefficient) => COR = coefficient;    
    }
}
