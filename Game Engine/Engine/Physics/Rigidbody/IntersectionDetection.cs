using GameEngine.Engine.Physics;
using GameEngine.Engine.Physics.Primitives;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Shapes;

namespace GameEngine.Engine.Physics.Rigidbody
{
    public static class IntersectionDetection
    {
        public static bool PointOnLine(Vector2 point, Line line)
        {
            float dy = line.End.Y - line.Start.Y;
            float dx = line.End.X - line.Start.X;
            float m = dy / dx;

            float b = line.End.Y - (m * line.End.X);

            return point.Y == m * point.X * b;
        }

        public static bool PointInCircle(Vector2 point, Circle circle)
        {
            Vector2 centerToPoint = point - circle.Center;
            return centerToPoint.LengthSquared() < circle.RadiusSquared;
        }

        public static bool PointInBox(Vector2 point, Box2D box)
        {
            Vector2 localPoint = Transform.RotateVector(point, box.transform.WorldRotation, box.transform.WorldPosition);

            Vector2 min = box.LocalMin;
            Vector2 max = box.LocalMax;

            return localPoint.X <= max.X && min.X <= localPoint.X && localPoint.Y <= max.Y && min.Y <= max.Y;
        }

        public static bool LineInCircle(Line line, Circle circle)
        {
            if (PointInCircle(line.Start, circle) || PointInCircle(line.End, circle))
            {
                return true;
            }

            //line segment to test
            Vector2 ab = line.End - line.Start;

            Vector2 circleCenter = circle.Center;
            Vector2 centerToStart = circleCenter - line.Start;

            //project centerToStart onto ab and get the percent of ab reflected onto
            float t = Vector2.Dot(centerToStart, ab)/Vector2.Dot(ab, ab);

            if (t < 0 || t > 1)
                return false;

            //find the closest point by multiplying the line by the pecent
            Vector2 closestPoint = line.Start * t;

            return PointInCircle(closestPoint, circle);
        }

        //breakes with perfectly horizontal and verticla lines
        public static bool LineInBox(Line line, Box2D box) 
        {
            float theta = -box.transform.WorldRotation;
            Vector2 center = box.transform.WorldPosition;

            Vector2 localStart = Transform.RotateVector(line.Start, theta, center);
            Vector2 localEnd = Transform.RotateVector(line.End, theta, center);

            if(PointInBox(localStart, box) || PointInBox(localEnd, box))
                return true;

            Vector2 unitVector = localStart - localEnd;
            unitVector.Normalize();

            unitVector.X = (unitVector.X != 0) ? 1f/unitVector.X : 0f;
            unitVector.Y = (unitVector.Y != 0) ? 1f/unitVector.Y : 0f;

            Vector2 min = box.LocalMin;
            Vector2 max = box.LocalMax;

            min = (min - localStart) * unitVector;
            max = (max - localStart) * unitVector;

            float tmin = Math.Max(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y));
            float tmax = Math.Min(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y));

            if(tmax < 0 || tmin > tmax)
                return false;

            float t = (tmin < 0f) ? tmax : tmin;

            return t > 0 && t * t < line.LengthSquared;
            
        }

        public static bool RayCast(Ray2D ray, Circle circle, ref RayCastResult result)
        {
            if (result != null)
                result.Reset();

            Vector2 circleOrigin = circle.Center - ray.Origin;
            float radiusSquared = circle.RadiusSquared;
            float circleOriginLengthSquared = circleOrigin.LengthSquared();

            //project vector into direction of the ray
            float a = Vector2.Dot(circleOrigin, ray.Direction);
            float bsq = circleOriginLengthSquared - (a * a);

            if(radiusSquared - bsq < 0f) 
                return false;

            float f = (float)Math.Sqrt(radiusSquared - bsq);
            float t = 0;
            if (circleOriginLengthSquared < radiusSquared)
            {
                //ray start inside circle
                t = a + f;
            }
            else
                t = a - f;

            if(result != null) 
            {
                Vector2 point = (ray.Origin + ray.Direction) * t;
                Vector2 normal = point - circle.Center;
                normal.Normalize();

                result.Init(point, normal, t, true);
            }

            return true;
        }

        //implemented diffrently from the video, watch part #9 at 14:58 id solution does not work
        public static bool RayCast(Box2D box, Ray2D ray, ref RayCastResult result)
        {
            float theta = -box.transform.WorldRotation;
            Vector2 localOrigin = Transform.RotateVector(ray.Origin, theta, box.transform.WorldPosition);
            Vector2 localDirection = Transform.RotateVector(ray.Direction, theta, box.transform.WorldPosition);


            if (result != null)
                result.Reset();

            Vector2 unitVector = localDirection;
            unitVector.Normalize();

            unitVector.X = (unitVector.X != 0) ? 1f / unitVector.X : 0f;
            unitVector.Y = (unitVector.Y != 0) ? 1f / unitVector.Y : 0f;

            Vector2 min = box.LocalMin;
            Vector2 max = box.LocalMax;

            min = (min - localOrigin) * unitVector;
            max = (max - localOrigin) * unitVector;

            float tmin = Math.Max(Math.Min(min.X, max.X), Math.Min(min.Y, max.Y));
            float tmax = Math.Min(Math.Max(min.X, max.X), Math.Max(min.Y, max.Y));

            if (tmax < 0 || tmin > tmax)
                return false;

            float t = (tmin < 0f) ? tmax : tmin;
            //bool hit = t > 0 && t * t < ray.Max; //for when rays have a maximumm distance;
            bool hit = t > 0;

            if (!hit)
                return false;

            if (result != null)
            {
                Vector2 point = (ray.Origin + ray.Direction) * t; //use unrotated axis for calculations
                Vector2 normal = ray.Origin - point;
                normal.Normalize();

                result.Init(point, normal, t, true);
            }

            return true;
        }

        //PRoblem Might be here
        public static bool BoxInBox(Box2D boxOne, Box2D boxTwo)
        {
            Vector2[] axisToTest =
            {
                Transform.RotateVector(new Vector2(0, 1), boxOne.transform.WorldRotation, new Vector2()),
                Transform.RotateVector(new Vector2(1, 0), boxOne.transform.WorldRotation, new Vector2()),

                Transform.RotateVector(new Vector2(0, 1), boxTwo.transform.WorldRotation, new Vector2()),
                Transform.RotateVector(new Vector2(1, 0), boxTwo.transform.WorldRotation, new Vector2()),
            };

            for (int i =0; i < axisToTest.Length; i++) 
            {
                if(!OverlappingAxis(boxOne, boxTwo, axisToTest[i]))
                    return false;
            }

            return true;

        }

        static bool OverlappingAxis(Box2D box1, Box2D box2, Vector2 axis)
        {
            Vector2 intervalOne = GetInterval(box1, axis);
            Vector2 intervalTwo = GetInterval(box2, axis);

            return ((intervalTwo.X <= intervalOne.Y) && (intervalOne.X <= intervalTwo.Y));
        }

        static Vector2 GetInterval(Box2D box, Vector2 axis)
        {
            Vector2 result = new Vector2();

            Vector2[] verticies = box.transform.GetVerticies();

            result.X = Vector2.Dot(axis, verticies[0]);
            result.Y = result.X;

            for(int i = 0; i < 4; i++) 
            {
                float projection = Vector2.Dot(axis, verticies[i]);

                if(projection < result.X)
                {
                    result.X = projection;
                }

                if(projection > result.Y) 
                {
                    result.Y = projection;
                }
            }

            return result;
        }
    }
}
