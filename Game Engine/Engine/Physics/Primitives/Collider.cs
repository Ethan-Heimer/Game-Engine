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
    }
}
