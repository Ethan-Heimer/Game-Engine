using GameEngine.Engine.Physics.Rigidbody;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Engine.Physics
{
    public static class Collisions
    {
        
        public static CollisionManifold FindCollisionFeatures(Collider a, Collider b)
        {
            //depending on colliders, find method to run
            return null;
        }


        //code from Box2d lite and translated from c++ into c# 
        public static CollisionManifold FindCollisionFeatures(Box2D a, Box2D b)
        {
            Vector2 halfA = a.transform.Size * .5f;
            Vector2 halfB= b.transform.Size * .5f;

            Vector2 posA = a.transform.Position;
            Vector2 posB = b.transform.Position;

            Matrix rotA = Matrix.CreateRotationZ(a.transform.Rotation * (float)(Math.PI / 180));
            Matrix rotB = Matrix.CreateRotationZ(a.transform.Rotation * (float)(Math.PI / 180));

            //transpose means flipping the matrix
            Matrix rotAT = Matrix.Transpose(rotA);
            Matrix rotBT = Matrix.Transpose(rotB);

            Vector2 dp = posB - posA;
            Vector2 dA = Vector2.Transform(dp, rotAT);  
            Vector2 dB = Vector2.Transform(dp, rotAT);

            //rotates anything from bs local space into as local space
            Matrix c = rotAT * rotBT;
            Matrix absC = AbsMatrix(c);

            //rotates anything from as local space into bs local space
            Matrix absCT = Matrix.Transpose(absC);

            //Box a faces
            //This does a dot product by using the transformation matrix and matrix multiplication rules. Very condensed...
            //So its just projecting along all the faces
            //SAT

            //i dont have hope that is math works right, code found at 12:20
            Vector2 faceA = AbsVector2(dA) - halfA - Vector2.Transform(halfB, absC);
            if (faceA.X > 0 || faceA.Y > 0)
                return new CollisionManifold();

            //same thing but for face B
            Vector2 faceB = AbsVector2(dB) - ((Vector2.Transform(halfA, absCT) * halfB));
            if (faceB.X > 0 || faceB.Y > 0)
                return new CollisionManifold();

            //find the best axis
            Axis axis = Axis.FACE_A_X;
            float separation = faceA.X;
            Vector2 normal = dA.X > 0f ? new Vector2(rotA.M11, rotA.M21) : -new Vector2(rotA.M11, rotA.M21);

            const float relativeTol = .95f;
            const float absoluteTol = .01f;

            if(faceA.Y > relativeTol * separation + absoluteTol * halfA.Y)
            {
                axis = Axis.FACE_A_Y;
                separation = faceA.Y;
                normal = dA.X > 0f ? new Vector2(rotA.M12, rotA.M22) : -new Vector2(rotA.M12, rotA.M22);
            }

            //b faces
            if (faceB.X > relativeTol * separation + absoluteTol * halfB.X)
            {
                axis = Axis.FACE_B_X;
                separation = faceB.X;
                normal = dB.X > 0f ? new Vector2(rotB.M11, rotB.M21) : -new Vector2(rotB.M11, rotB.M21);
            }

            if (faceB.Y > relativeTol * separation + absoluteTol * halfB.Y)
            {
                axis = Axis.FACE_B_Y;
                separation = faceB.Y;
                normal = dB.Y > 0f ? new Vector2(rotB.M12, rotB.M22) : -new Vector2(rotB.M12, rotB.M22);
            }

            //---------------------------------------------
            //Setup clipping plane data based on the seperating axis
            

            return null;
        }

        //create method to do this in fna dll
        static Matrix AbsMatrix(Matrix m)
        {
            Matrix matrix = new Matrix();

            matrix.M11 = Math.Abs(m.M11);
            matrix.M12 = Math.Abs(m.M12);
            matrix.M13 = Math.Abs(m.M13);
            matrix.M14 = Math.Abs(m.M14); 
            
            matrix.M21 = Math.Abs(m.M21);
            matrix.M22 = Math.Abs(m.M22);
            matrix.M23 = Math.Abs(m.M23);
            matrix.M24 = Math.Abs(m.M24); 

            matrix.M31 = Math.Abs(m.M31);
            matrix.M32 = Math.Abs(m.M32);
            matrix.M33 = Math.Abs(m.M33);
            matrix.M34 = Math.Abs(m.M34);
            
            matrix.M31 = Math.Abs(m.M31);
            matrix.M32 = Math.Abs(m.M32);
            matrix.M33 = Math.Abs(m.M33);
            matrix.M34 = Math.Abs(m.M34);

            return matrix;
        }

        static Vector2 AbsVector2(Vector2 v) => new Vector2(Math.Abs(v.X), Math.Abs(v.Y));
        
        
    }

    public enum Axis
    {
        FACE_A_X,
        FACE_A_Y,

        FACE_B_X,
        FACE_B_Y
    }
}
