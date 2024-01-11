using GameEngine.Engine.Physics.Rigidbody;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine.Engine.Physics
{
    public static class Collisions
    {

        //code from Box2d lite and translated from c++ into c# 
        public static CollisionManifold FindCollisionFeatures(Collider a, Collider b)
        {
            CollisionManifold manifold = new CollisionManifold();

            Vector2 halfA = a.transform.Size * .5f; //should be scaled 
            Vector2 halfB= b.transform.Size * .5f;

            Vector2 posA = a.transform.WorldPosition;
            Vector2 posB = b.transform.WorldPosition;

            Matrix rotA = Matrix.CreateRotationZ(a.transform.WorldRotation);
            Matrix rotB = Matrix.CreateRotationZ(b.transform.WorldRotation);

            Console.WriteLine(b.transform.WorldRotation);

            //transpose means flipping the matrix
            Matrix rotAT = Matrix.Transpose(rotA);
            Matrix rotBT = Matrix.Transpose(rotB);

            Vector2 dp = posB - posA;
            Vector2 dA = Vector2.Transform(dp, rotAT);  
            Vector2 dB = Vector2.Transform(dp, rotBT);

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
            {
                Console.WriteLine("Face A Returned");
                return new CollisionManifold();
            }

            //same thing but for face B
            Vector2 faceB = AbsVector2(dB) - ((Vector2.Transform(halfA, absCT) * halfB));
            if (faceB.X > 0 || faceB.Y > 0)
            {
                Console.WriteLine("Face B returned");
                return new CollisionManifold();
            }

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
            Vector2 frontNormal = Vector2.Zero, sideNormal = Vector2.Zero;
            float front = 0, negSide = 0, posSide = 0;
            ClipVertext[] incidentEdge = new ClipVertext[2] { new ClipVertext(), new ClipVertext() };
            Edge negEdge = Edge.NO_EDGE, posEdge = Edge.NO_EDGE;

            switch (axis)
            {
                case Axis.FACE_A_X:
                    frontNormal = normal;
                    front = Vector2.Dot(posA, frontNormal) + halfA.X;
                    sideNormal = new Vector2(rotA.M11, rotA.M21);

                    float side = Vector2.Dot(posA, sideNormal);

                    negSide = -side + halfA.Y;
                    posSide = side = halfA.Y;

                    negEdge = Edge.Edge3;
                    posEdge = Edge.Edge1;

                    ComputeInciednetEdges(ref incidentEdge, halfB, posB, rotB, frontNormal);
                    break;

                case Axis.FACE_A_Y:
                    frontNormal = normal;
                    front = Vector2.Dot(posA, frontNormal) + halfA.Y;
                    sideNormal = new Vector2(rotA.M11, rotA.M21);

                    float side2 = Vector2.Dot(posA, sideNormal);

                    negSide = -side2 + halfA.X;
                    posSide = side2 + halfA.X;

                    negEdge = Edge.Edge2;
                    posEdge = Edge.Edge4;

                    ComputeInciednetEdges(ref incidentEdge, halfB, posB, rotB, frontNormal);
                    break;

                case Axis.FACE_B_X:
                    frontNormal = -normal;
                    front = Vector2.Dot(posB, frontNormal) + halfB.X;
                    sideNormal = new Vector2(rotB.M12, rotB.M22);

                    float side3 = Vector2.Dot(posB, sideNormal);

                    negSide = -side3 + halfB.Y;
                    posSide = side3 + halfB.Y;
                    negEdge = Edge.Edge3;
                    posEdge = Edge.Edge1;
                    ComputeInciednetEdges(ref incidentEdge, halfA, posA, rotA, frontNormal);
                    break;

                case Axis.FACE_B_Y:
                    frontNormal = -normal;
                    front = Vector2.Dot(posB, frontNormal) + halfB.Y;
                    sideNormal = new Vector2(rotB.M11, rotB.M22);

                    float side4 = Vector2.Dot(posB, sideNormal);
                    negSide = -side4 + halfB.X;
                    posSide = side4 + halfB.X;
                    negEdge = Edge.Edge2;
                    posEdge = Edge.Edge4;
                    ComputeInciednetEdges(ref incidentEdge, halfA, posA, rotA, frontNormal);
                    
                    break;
            }

            //at this point we should have the edges the boxes are colliding on
            ClipVertext[] clipPoints1 = new ClipVertext[2]
            {
                new ClipVertext(),
                new ClipVertext()
            }; 
            
            ClipVertext[] clipPoints2 = new ClipVertext[2]
            {
                new ClipVertext(),
                new ClipVertext()
            };

            int np; //number of points

            //clip box to line 1
            np = ClipSegmentToLine(ref clipPoints1, ref incidentEdge, -sideNormal, negSide, negEdge);

            //seperating axis theorem
            if (np < 2)
                return manifold;

            np = ClipSegmentToLine(ref clipPoints2, ref clipPoints1, sideNormal, posSide, posEdge);


            //SAT
            if (np < 2)
                return manifold;

            // Now clipPoints2 contains the clipping points.
            // Due to roundoff, it is possible that clipping removes all points.
            
            //implament loop part
            manifold = new CollisionManifold(normal, null, separation);

            for(int i = 0; i < 2; i++)
            {
                //seperation is probobly not the same as the depth, so here is a possable bug
                float seperation = Vector2.Dot(frontNormal, clipPoints2[i].v) - front;

                if(seperation < 0)
                {
                    Vector2 point = clipPoints2[i].v - separation * frontNormal;
                    manifold.AddContactPoint(point);
                }
            }

            return manifold;
        }

        static void ComputeInciednetEdges(ref ClipVertext[] c, Vector2 h, Vector2 pos, Matrix rotation, Vector2 normal)
        {
            Matrix rotT = Matrix.Transpose(rotation);
            Vector2 n = -Vector2.Transform(normal, rotT);
            Vector2 nAbs = new Vector2(Math.Abs(n.X), Math.Abs(n.Y));

            if(nAbs.X > nAbs.Y)
            {
                if(Math.Sign(n.X) > 0f)
                {
                    c[0].v = new Vector2(h.X, -h.Y);
                    c[0].fp.e.InEdge2 = Edge.Edge3;
                    c[0].fp.e.OutEdge2 = Edge.Edge4;

                    c[1].v = new Vector2(h.X, h.Y);
                    c[1].fp.e.InEdge2 = Edge.Edge4;
                    c[1].fp.e.OutEdge2 = Edge.Edge1;
                }
                else
                {
                    c[0].v = new Vector2(-h.X, h.Y);
                    c[0].fp.e.InEdge2 = Edge.Edge1;
                    c[0].fp.e.OutEdge2 = Edge.Edge2;

                    c[1].v = new Vector2(-h.X, -h.Y);
                    c[1].fp.e.InEdge2 = Edge.Edge2;
                    c[1].fp.e.OutEdge2 = Edge.Edge3;
                }
            }
            else
            {
                if (Math.Sign(n.Y) > 0.0f)
                {
                    c[0].v = new Vector2(h.X, h.Y);
                    c[0].fp.e.InEdge2 = Edge.Edge4;
                    c[0].fp.e.OutEdge2 = Edge.Edge4;

                    c[1].v = new Vector2(-h.X, h.Y);
                    c[1].fp.e.InEdge2 = Edge.Edge1;
                    c[1].fp.e.OutEdge2 = Edge.Edge2;
                }
                else
                {
                    c[0].v = new Vector2(-h.X, -h.Y);
                    c[0].fp.e.InEdge2 = Edge.Edge2;
                    c[0].fp.e.OutEdge2 = Edge.Edge3;

                    c[1].v = new Vector2(h.X, -h.Y);
                    c[1].fp.e.InEdge2 = Edge.Edge3;
                    c[1].fp.e.OutEdge2 = Edge.Edge4;
                }
            }

            c[0].v = pos + Vector2.Transform(c[0].v, rotation);
            c[1].v = pos + Vector2.Transform(c[1].v, rotation);
        }

        static int ClipSegmentToLine(ref ClipVertext[] vOut, ref ClipVertext[] vIn, Vector2 Normal, float offset, Edge clipEdge)
        {
            //start out with no points
            int numOut = 0;

            //calculate the distance from the end points to the line
            float distance0 = Vector2.Dot(Normal, vIn[0].v) - offset;
            float distance1 = Vector2.Dot(Normal, vIn[1].v) - offset;

            //if the points are behind the plane
            if (distance0 <= 0.0f) vOut[numOut++] = vIn[0];
            if (distance1 <= 0.0f) vOut[numOut++] = vIn[1];

            //If the point are on diffrn=ent sides of the plane
            if(distance0 * distance1 < 0f)
            {
                float interp = distance0 / (distance0 - distance1);
                vOut[numOut].v = vIn[0].v = interp * (vIn[1].v - vIn[0].v);

                if(distance0 > 0f)
                {
                    vOut[numOut].fp = vIn[0].fp;
                    vOut[numOut].fp.e.InEdge1 = clipEdge;
                    vOut[numOut].fp.e.InEdge2 = Edge.NO_EDGE;
                }
                else
                {
                    vOut[numOut].fp = vIn[1].fp;
                    vOut[numOut].fp.e.OutEdge1 = clipEdge;
                    vOut[numOut].fp.e.OutEdge2 = Edge.NO_EDGE;
                }
                ++numOut;
            }

            return numOut;
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
        
        static void Flip(ref FeaturePair fp)
        {
           
             Swap(ref fp.e.InEdge1, ref fp.e.InEdge2);
             Swap(ref fp.e.OutEdge1, ref fp.e.OutEdge2);
        } 

        static void Swap(ref Edge a, ref Edge b) 
        {
            var c = a; 
            a = b; b = c;
        }

        static Vector2 Sign(Vector2 v) => new Vector2(Math.Sign(v.X), Math.Sign(v.Y));
        
    }

    public struct FeaturePair
    {
        public struct Edges
        {
            public Edge InEdge1;
            public Edge OutEdge1;

            public Edge InEdge2;
            public Edge OutEdge2;
        }

        public Edges e;
        public int value;
    }

    public class ClipVertext
    {
        public ClipVertext()
        {
            fp.value = 0;
        }

        public Vector2 v;
        public FeaturePair fp;
    }

    public enum Axis
    {
        FACE_A_X,
        FACE_A_Y,

        FACE_B_X,
        FACE_B_Y
    }
    public enum Edge
    {
        NO_EDGE = 0,
        Edge1,
        Edge2,
        Edge3,
        Edge4,
    }
}
