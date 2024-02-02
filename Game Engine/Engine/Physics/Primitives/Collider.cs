using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Engine.Physics
{
    public class Collider : Behavior
    {
        public Vector2 Size;

        public Vector2 halfSize
        {
            get
            {
                return Size / 2;
            }
        }

        RigidBody _ridgidBody;

        public bool IsColliding;
        public Vector2[] CollsionNormals
        {
            get
            {
                return collisionNormals.ToArray();
            }
        }
        
        List<Vector2> collisionNormals = new List<Vector2>();

        public RigidBody RigidBody
        {
            get
            {
                if (_ridgidBody == null)
                    _ridgidBody = gameObject.GetComponent<RigidBody>();

                return _ridgidBody; 
            }
        }

        public void AddNormal(Vector2 normal)
        {
            collisionNormals.Add(normal);
        }

        public void ClearNormals() => collisionNormals.Clear(); 
        
        public void OnDraw()
        {
            Renderer.DrawCircle(transform.GetVerticies()[0], 10, Color.Red);
            Renderer.DrawCircle(transform.GetVerticies()[1], 10, Color.Red);
            Renderer.DrawCircle(transform.GetVerticies()[2], 10, Color.Red);
            Renderer.DrawCircle(transform.GetVerticies()[3], 10, Color.Red);
        }

        public Vector2[] GetVerticies()
        {
            Vector2[] verticies = new Vector2[]
           {
                new Vector2(-Size.X/2 + transform.WorldPosition.X, -Size.Y/2 + transform.WorldPosition.Y),
                new Vector2(Size.X/2 + transform.WorldPosition.X, -Size.Y/2 + transform.WorldPosition.Y),
                new Vector2(Size.X / 2 + transform.WorldPosition.X, Size.Y / 2 + transform.WorldPosition.Y),
                new Vector2(-Size.X/2 + transform.WorldPosition.X, Size.Y/2 + transform.WorldPosition.Y),
           };

            for (int i = 0; i < verticies.Length; i++)
            {
                verticies[i] = Transform.RotateVector(verticies[i], transform.WorldRotation, transform.WorldPosition);
            }

            return verticies;
        }

        public void WhileInEditor()
        {
            Vector2[] verticies = GetVerticies();

            for(int i = 0; i < verticies.Length; i++)
            {
                Renderer.DrawLine(verticies[i], verticies[(i + 1) % verticies.Length], Color.Green);
            }
        }
    }
}
