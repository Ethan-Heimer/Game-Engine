using GameEngine.Debugging;
using GameEngine.Editor.UI.Inspector;
using GameEngine.Engine.ComponentModel;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine
{
    [ExecuteAlways]
    public class Transform : Behavior
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale = 1;

        public OriginType originType = OriginType.Center;

        public Matrix TransformMatrix;

        public Vector2 WorldPosition
        {
            get
            {
                return new Vector2(TransformMatrix.M41, TransformMatrix.M42);
            }
        }

        public float WorldScale
        {
            get
            {
                return TransformMatrix.M33;
            }
        }

        public float WorldRotation
        {
            get 
            {
                return (float)Math.Atan2(TransformMatrix.M12, TransformMatrix.M11);
            }
        }

        Vector2 OriginMultiplyer
        {
            get
            {

                switch (originType)
                {
                    case OriginType.Center:
                        return Vector2.One * .5f;

                    case OriginType.Zero:
                        return Vector2.Zero;

                }

                return Vector2.Zero;
            }
        }

        public Vector2 Origin
        {
            get
            {
                var width = gameObject.renderer.Bounds.Width;
                var height = gameObject.renderer.Bounds.Height;

                return new Vector2(width * OriginMultiplyer.X, height * OriginMultiplyer.Y);
            }
        }

        public Vector2 WorldOrigin
        {
            get
            {
                var parentPos = gameObject.Parent != null ? gameObject.Parent.Transform.Position : Vector2.Zero;
                return Origin - parentPos;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                if (gameObject.renderer == null)
                    return Rectangle.Empty;

                var rot = WorldRotation * (180/Math.PI);
                var pos = WorldPosition;
                var scale = WorldScale;

                Vector2 rotVector = new Vector2(Math.Abs((float)Math.Cos(rot * (Math.PI / 180))), Math.Abs((float)Math.Sin(rot * (Math.PI / 180))));

                var width = (gameObject.renderer.Bounds.Width * Math.Abs(scale)) * rotVector.X + (gameObject.renderer.Bounds.Height * Math.Abs(scale)) * rotVector.Y;
                var height = (gameObject.renderer.Bounds.Height * Math.Abs(scale)) * rotVector.X + (gameObject.renderer.Bounds.Width * Math.Abs(scale)) * rotVector.Y;

                Rectangle bounds = new Rectangle
                {
                    Width = (int)width,
                    Height = (int)height,

                    X = (int)((pos.X) - width * OriginMultiplyer.X),
                    Y = (int)((pos.Y) - height * OriginMultiplyer.Y),
                };

                return bounds;
            }
        }

        public Vector2 Size
        {
            get
            {
                if (gameObject.renderer == null)
                    return Vector2.Zero;

                return new Vector2(gameObject.renderer.Bounds.Width * Math.Abs(WorldScale), gameObject.renderer.Bounds.Height * Math.Abs(WorldScale));
            }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(-Bounds.Width / 2 + Bounds.X, -Bounds.Height / 2 + Bounds.Height);
            }
        }

        public Vector2[] GetVerticies()
        {
            Vector2[] verticies = new Vector2[]
            {
                new Vector2(-Size.X/2 + WorldPosition.X, -Size.Y/2 + WorldPosition.Y),
                new Vector2(Size.X/2 + WorldPosition.X, -Size.Y/2 + WorldPosition.Y),
                new Vector2(-Size.X/2 + WorldPosition.X, Size.Y/2 + WorldPosition.Y),
                new Vector2(Size.X / 2 + WorldPosition.X, Size.Y / 2 + WorldPosition.Y)
            };

            for(int i = 0; i < verticies.Length; i++)
            {
               verticies[i] = RotateVector(verticies[i], WorldRotation, WorldPosition);
            }
            
            return verticies;
        }

        public void Update()
        {
            var pos = Matrix.CreateTranslation(new Vector3(Position, 0));
            var rot = Matrix.CreateRotationZ((float)GetRotationInRad());
            var scale = Matrix.CreateScale(Scale);

            TransformMatrix =  (scale * rot * pos) * (gameObject.Parent != null ? gameObject.Parent.Transform.TransformMatrix : Matrix.Identity);
        }

        public double GetRotationInRad()
        {
            return Rotation * (Math.PI / 180);
        }

        public double GetWorldRotationInRad()
        {
            return WorldRotation * (Math.PI / 180);
        }

        public static Vector2 RotateVector(Vector2 vector, float degree, Vector2 origin)
        {
            float cos = (float)Math.Cos(degree);
            float sin = (float)Math.Sin(degree);

            float x = vector.X - origin.X;
            float y = vector.Y - origin.Y;

            float xPrime = x * cos - y * sin;
            float yPrime = x * sin + y * cos;

            return new Vector2(xPrime + origin.X, yPrime + origin.Y);
        }
    }

    public enum OriginType
    {
        Center,
        Zero
    }
}
