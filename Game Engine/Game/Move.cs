using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Move : Behavior
    {
        public int Speed;
        RigidBody body;

        public void Start()
        {
            body = gameObject.GetComponent<RigidBody>();
        }
       
        public void Update() 
        {
            if(InputManager.IsKeyDown(Keys.A)) 
            {
                body.SetVelocity(new Vector2(-1 * Speed, 0));
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                body.SetVelocity(new Vector2(1 * Speed, 0));
            }

            if(InputManager.IsKeyDown(Keys.W))
            {
                body.SetVelocity(new Vector2(0, -1 * Speed));
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                body.SetVelocity(new Vector2(0, 1 * Speed));
            }
        }
    }
}
