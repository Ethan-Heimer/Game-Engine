﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine.Physics.Rigidbody
{
    public class CollisionManifold
    {
        public bool IsColliding
        {
            get; private set;
        }

        public Vector2 Normal
        {
            get; private set;
        }

        List<Vector2> contactPoints = new List<Vector2>();
        public Vector2[] ContactPoints
        {
            get
            {
                return contactPoints.ToArray();
            }
        }

        public float Depth
        {
            get; private set;
        }

        public int ContactCount
        {
            get; private set;
        }

        public CollisionManifold()
        {
            Depth = 0; 
            IsColliding = false;
        }

        public CollisionManifold(Vector2 normal, int ContactCount, Vector2[] contactPoints, float depth)
        {
            Normal = normal;

            if(contactPoints != null)
                this.contactPoints = contactPoints.ToList();
            else
                this.contactPoints = new List<Vector2>();
            Depth = depth;
            
            IsColliding = true;
            this.ContactCount = ContactCount;
        }

        public void AddContactPoint(Vector2 point) => contactPoints.Add(point); 
    }
}
