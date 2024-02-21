using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Engine.Components;
using GameEngine.Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class Move : Behavior
    {
        public int Acceleration;
        public int MaxSpeed;
        RigidBody body;
        AnimationRenderer animationRenderer;

        Direction playerDirection;

        Vector2 prevDirection;
        public void Start()
        {
            body = gameObject.GetComponent<RigidBody>();
            animationRenderer = gameObject.GetComponent<AnimationRenderer>();
        }
       
        public void Update() 
        {
            Vector2 direction = Vector2.Zero;

            if(InputManager.IsKeyDown(Keys.A)) 
            {
                direction.X = -1;
            }
            if (InputManager.IsKeyDown(Keys.D))
            {
                direction.X = 1;
            }

            if(InputManager.IsKeyDown(Keys.W))
            {
                direction.Y = -1;
            }
            if (InputManager.IsKeyDown(Keys.S))
            {
                direction.Y = 1;
            }

            body.AddForce(direction * Acceleration);

            if(!(-MaxSpeed < body.Velocity.X && body.Velocity.X < MaxSpeed))
            {
                body.SetVelocity(new Vector2(MaxSpeed * Math.Sign(body.Velocity.X), body.Velocity.Y));
            }

            if (!(-MaxSpeed < body.Velocity.Y && body.Velocity.Y < MaxSpeed))
            {
                body.SetVelocity(new Vector2(body.Velocity.X, MaxSpeed * Math.Sign(body.Velocity.Y)));
            }

            if(prevDirection != direction)
            {
                if(direction == Vector2.Zero)
                {
                    if (prevDirection == new Vector2(1, 0))
                        playerDirection = Direction.RightIdle;
                    else
                        playerDirection = Direction.LeftIdle;
                }
                else if(direction == new Vector2(1, 0))
                {
                    playerDirection = Direction.Right;
                }
                else if(direction == new Vector2(-1, 0))
                {
                    playerDirection = Direction.Left;
                }

                if(animationRenderer != null)
                animationRenderer.SawpYLevel(((int)playerDirection));
            }

            prevDirection = direction;
        }
    }

    enum Direction
    {
        RightIdle = 0,
        Right = 1,
        LeftIdle = 2,
        Left = 3
    }
}
