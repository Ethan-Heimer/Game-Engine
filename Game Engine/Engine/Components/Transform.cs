using GameEngine.Debugging;
using GameEngine.Engine.ComponentModel;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                var parentPos = gameObject.parent != null ? gameObject.parent.Transform.Position : Vector2.Zero;
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

        public Vector2 Center
        {
            get
            {
                return new Vector2(-Bounds.Width / 2 + Bounds.X, -Bounds.Height / 2 + Bounds.Height);
            }
        }

        public void Update()
        {
            var pos = Matrix.CreateTranslation(new Vector3(Position, 0));
            var rot = Matrix.CreateRotationZ((float)GetRotationInRad());
            var scale = Matrix.CreateScale(Scale);

            TransformMatrix =  (scale * rot * pos) * (gameObject.parent != null ? gameObject.parent.Transform.TransformMatrix : Matrix.Identity);

            Renderer.RenderRect(transform.Bounds, new Color(0, 255, 150) * .1f);
        }

        public double GetRotationInRad()
        {
            return Rotation * (Math.PI / 180);
        }
    }

    public enum OriginType
    {
        Center,
        Zero
    }
}
